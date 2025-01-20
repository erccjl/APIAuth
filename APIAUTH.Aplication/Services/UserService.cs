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
                throw new Exception(ex.Message);
            }

            return _mapper.Map<UserDto>(user);
        }

        //TODO:
        /*
         * Alerta de vencimiento de contraseña (DEJAR PARA EL FINAL) con el campo PasswordDate
         */

        public async Task RecoverPassword(string email)
        {
            var collaborator = _repository.GetByEmail(email);
            if (collaborator == null || collaborator.State == Domain.Enums.BaseState.Activo)
            {
                throw new UnauthorizedAccessException("User is non-existent");
            }

            //TODO: Se debe bloquear al usuario y devolver el id del usuario administrador
        }

        public async Task<bool> ChangePassword(UserPasswordDto dto)
        {
            var collaborator = _repository.GetByEmail(dto.Email);
            if (collaborator != null && collaborator.User != null)
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
                    throw new UnauthorizedAccessException("Incorrect Password");
                }
            }

            throw new UnauthorizedAccessException("User is non-existent");
        }

        public async Task<UserDto> ActivePassword(CollaboratorDto? collaborator)
        {
            var user = collaborator.User;
            user.Password = BCrypt.Net.BCrypt.HashPassword(collaborator.DocumentNamber.ToString());
            user.IsGenericPassword = true;
            user.PasswordDate = DateTime.Now;

            return user;
        }
    }
}
