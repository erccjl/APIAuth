using APIAUTH.Aplication.DTOs;
using APIAUTH.Aplication.Helpers;
using APIAUTH.Aplication.Interfaces;
using APIAUTH.Aplication.Valitations;
using APIAUTH.Domain.Entities;
using APIAUTH.Domain.Repository;
using AutoMapper;
using System.Net.Mail;

namespace APIAUTH.Aplication.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IMapper mapper, IUserRepository repository)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> Exists(int id)
        {
            return await _repository.Get(id) != null;
        }

        public async Task<UserDto> Save(CollaboratorDto dto)
        {
            var user = new User();

            try
            {
                MailAddress mail = new MailAddress(dto.Email);
                user.UserName = mail.User;
                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.DocumentNamber.ToString());
                user.IsGenericPassword = true;
                user.PasswordDate = DateTime.Now;
                user.StateUser = Domain.Enums.StateUser.Activo;
            }
            catch (FormatException ex)
            {
                //TODO: Agregar la respuesta de la excepcion
            }

            return _mapper.Map<UserDto>(user);
        }

        //TODO:
        /*
         * Alerta de vencimiento de contraseña (DEJAR PARA EL FINAL) con el campo PasswordDate
         */

        public async Task RecoverPassword(string email, string password)
        {
            //TODO: Ver que se puede hacer en este punto para mejorar la seguridad
            // Quizas mandar por mail para validar la identidad o verificar el mail de la persona con mas datos que solo el email
            var collaborator = _repository.GetByEmail(email);
            if (collaborator == null || collaborator.State == Domain.Enums.BaseState.Activo)
            {
                throw new UnauthorizedAccessException("User is non-existent");
            }
            var user = await _repository.Get(collaborator.UserId);

            user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            user.PasswordDate = DateTime.Now;
            user.IsGenericPassword = false;
            await _repository.Update(user);

        }

        public async Task<bool> ChangePassword(UserPasswordDto dto)
        {
            var collaborator =  _repository.GetByEmail(dto.Email);
            if (collaborator != null  && collaborator.User != null)
            {
                if (await _repository.ValidatePasswordAsync(collaborator.User, dto.CurrentPassword))
                {
                    var user = collaborator.User;
                    user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword); 
                    user.PasswordDate = DateTime.Now;
                    user.IsGenericPassword = false;
                    return await _repository.Update(user) != null;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}
