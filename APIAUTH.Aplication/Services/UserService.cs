using APIAUTH.Aplication.DTOs;
using APIAUTH.Aplication.Helpers;
using APIAUTH.Aplication.Interfaces;
using APIAUTH.Aplication.Valitations;
using APIAUTH.Domain.Entities;
using APIAUTH.Domain.Repository;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAUTH.Aplication.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> repository, IMapper mapper, IUserRepository userRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task Activate(int id)
        {
            var user = await _repository.Get(id);
            BaseEntityHelper.SetActive(user);
            await _repository.Update(user);
        }

        public async Task<bool> Exists(int id)
        {
            return await _repository.Get(id) != null;
        }

        public async Task<UserDto> Get(int id)
        {
            var model = await _repository.Get(id);
            return _mapper.Map<UserDto>(model);
        }

        public async Task Inactivate(int id)
        {
            var user = await _repository.Get(id);
            BaseEntityHelper.SetInactive(user);
            await _repository.Update(user);
        }

        public async Task Save(UserDto dto)
        {
            if (dto.Id.Equals(0))
            {
                var newUser = _mapper.Map<User>(dto);
                newUser.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                newUser.PasswordDate = DateTime.Now;
                BaseEntityHelper.SetCreated(newUser);
                await _repository.Add(newUser);

                /*
                 * Encontrar una forma de hacer que la contraseña sea temporal y le pida al usuario que cambia la misma
                 */
            }
            else
            {
                var updateUser = _mapper.Map<User>(dto);
                BaseEntityHelper.SetUpdated(updateUser);
                await _repository.Update(updateUser);
            }

        }

        /*
         * Alerta de vencimiento de contraseña (DEJAR PARA EL FINAL)
         * Bloqueo del usuario (Nuevo estado me parece)
         * Desbloqueo del mismo (Se debe cambiar la contraseña)
         */

        public async Task<(bool isValid, string message)> Validate(int? id, UserDto dto)
        {
            var validations = new List<(bool isValid, string message)>();

            var validator = new UserValidator();
            var result = await validator.ValidateAsync(dto);
            validations.Add((result.IsValid, string.Join(Environment.NewLine, result.Errors.Select(x => $"Campo {x.PropertyName} invalido. Error: {x.ErrorMessage}"))));

            return (isValid: validations.All(x => x.isValid),
                    message: string.Join(Environment.NewLine, validations.Where(x => !x.isValid).Select(x => x.message)));
        }


        public async Task RecoverPassword(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || user.State == Domain.Enums.BaseState.Activo)
            {
                throw new UnauthorizedAccessException("User is non-existent");
            }

            var password = BCrypt.Net.BCrypt.HashPassword(user.UserName);
            user.PasswordDate = DateTime.Now;
            BaseEntityHelper.SetUpdated(user);
            await _repository.Update(user);
            /*
             * Encontrar una forma de hacer que la contraseña sea temporal y le pida al usuario que cambia la misma
             * Enviar un mail indicando cual es la nueva contraseña
             */
        }

        public async Task ChangePassword(string username, string newPassword)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            var password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.PasswordDate = DateTime.Now;
            BaseEntityHelper.SetUpdated(user);
            await _repository.Update(user);
        }


    }
}
