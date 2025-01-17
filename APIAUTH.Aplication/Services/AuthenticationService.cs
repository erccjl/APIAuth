using APIAUTH.Domain.Entities;
using APIAUTH.Domain.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using APIAUTH.Aplication.Interfaces;
using System.Text.Json;
using APIAUTH.Aplication.DTOs;
using APIAUTH.Domain.Enums;

namespace APIAUTH.Aplication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<(string idToken, string accessToken, string refreshToken)> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || !await _userRepository.ValidatePasswordAsync(user, password) || user.StateUser != StateUser.Activo)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var idToken = GenerateIdToken(user);
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            SaveRefreshTokenAsync(user.Id, refreshToken);

            return (idToken, accessToken, refreshToken);
        }

        private string GenerateIdToken(User user)
        {
            var collaborator = _userRepository.GetCollaboratorByIdUser(user.Id);
            var collaboratorType = Enum.Parse<CollaboratorTypeEnum>(collaborator.CollaboratorType.Description);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim("Name", $"{collaborator.LastName}, {collaborator.Name}"),
                new Claim("isInternal", (collaboratorType == CollaboratorTypeEnum.Internal).ToString()),
                new Claim("email", collaborator.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:IdTokenLifetimeMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateAccessToken(User user)
        {
            var collaborator = _userRepository.GetCollaboratorByIdUser(user.Id);
            var userRoles = _userRepository.GetRoles(user.Id);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (userRoles != null && userRoles.Any())
            {
                foreach (var role in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Role.Description));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenLifetimeMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<(string idToken, string accessToken, string refreshToken)> AuthenticateWithGoogleAsync(string idTokenGoogle)
        {
          
            string email = GetEmailFromIdToken(idTokenGoogle);

            var collaborator =  _userRepository.GetByEmail(email);

            if (collaborator == null || collaborator.State == BaseState.Inactivo)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var idToken = GenerateIdToken(collaborator.User);
            var accessToken = GenerateAccessToken(collaborator.User);
            var refreshToken = GenerateRefreshToken();

            SaveRefreshTokenAsync(collaborator.UserId, refreshToken);

            return (idToken, accessToken, refreshToken);
        }

        private string GetEmailFromIdToken(string idToken)
        {
            // Inicializa el manejador de JWT
            var handler = new JwtSecurityTokenHandler();

            // Lee el token para extraer los datos
            var jwtToken = handler.ReadJwtToken(idToken);

            // Accede a los claims del payload
            var emailClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "email");

            // Retorna el valor del email si existe
            return emailClaim?.Value ?? "No se encontró el email en el id_token";
        }

        private async Task<GoogleTokenResponse> GetAccessByGoogle(string authorizationCode)
        {
            //TODO: Deberia ir en variables de entorno
            var clientId = "226376114213-k219usnh08g1b4nc9cinkucnfatt54he.apps.googleusercontent.com"; // Reemplaza con tu Client ID de Google Cloud Console
            var clientSecret = "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"; // Reemplaza con tu Client Secret de Google Cloud Console
            var redirectUri = "http://localhost:3000"; // Reemplaza con la URI de redirección utilizada en el front-end
            var tokenUrl = "https://oauth2.googleapis.com/token";

            using var httpClient = new HttpClient();

            // Preparar la solicitud para obtener los tokens desde el servidor de Google
            var requestBody = new Dictionary<string, string>
                                {
                                    { "code", authorizationCode },
                                    { "client_id", clientId },
                                    { "client_secret", clientSecret },
                                    { "redirect_uri", redirectUri },
                                    { "grant_type", "authorization_code" }
                                };

            var content = new FormUrlEncodedContent(requestBody);
            var response = await httpClient.PostAsync(tokenUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al obtener el token de Google: {response.ReasonPhrase}, Detalles: {errorContent}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<GoogleTokenResponse>(responseString);

            return responseData;
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        public async Task<(string idToken, string accessToken, string refreshToken)> RefreshTokensAsync(string refreshToken)
        {
            var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);

            if (user == null || user.StateUser != StateUser.Activo)
            {
                throw new UnauthorizedAccessException("Invalid refresh token.");
            }

            if (user.RefreshTokenExpiryDate < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Refresh token has expired.");
            }

            var idToken = GenerateIdToken(user);
            var accessToken = GenerateAccessToken(user);

            var newRefreshToken = GenerateRefreshToken();
            SaveRefreshTokenAsync(user.Id, newRefreshToken);

            return (idToken, accessToken, refreshToken);
        }

        public async void SaveRefreshTokenAsync(int userId, string refreshToken)
        {
            var user = await _userRepository.Get(userId);
            user.RefreshToken=refreshToken;
            user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(30);

           await _userRepository.Update(user);
        }

    }
}
