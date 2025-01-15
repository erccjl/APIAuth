using APIAUTH.Aplication.DTOs;
using APIAUTH.Aplication.Helpers;
using APIAUTH.Aplication.Interfaces;
using APIAUTH.Domain.Entities;
using APIAUTH.Domain.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace APIAUTH.Aplication.Services
{
    public class CollaboratorService : ICollaboratorService
    {
        private readonly IRepository<Collaborator> _repository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public CollaboratorService(IRepository<Collaborator> repository, IMapper mapper, IUserService userService)
        {
            _repository = repository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task Activate(int id)
        {
            var collaborator = await _repository.Get(id);
            BaseEntityHelper.SetActive(collaborator);
            await _repository.Update(collaborator);
        }

        public async Task<bool> Exists(int id)
        {
            return await _repository.Get(id) != null;
        }

        public async Task<CollaboratorDto> Get(int id)
        {
            var model = await _repository.Get(id);
            return _mapper.Map<CollaboratorDto>(model);
        }

        public async Task Inactivate(int id)
        {
            var collaborator = await _repository.Get(id);
            BaseEntityHelper.SetInactive(collaborator);
            await _repository.Update(collaborator);
        }

        public async Task<string> PutImage(IFormFile image)
        {
            var pathImage = await SavePicture(image);
            return String.Join(",", pathImage);
        }

        public async Task<string> SavePicture(IFormFile image)
        {
            var stringPath = "";

            if (image == null)
            {
                return stringPath;
            }
            try
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var imageName = $"{Path.GetFileNameWithoutExtension(image.FileName)}_{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                var imagePath = Path.Combine(uploadFolder, imageName);

                using (var stream = new FileStream(imagePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    await image.CopyToAsync(stream);
                }

                stringPath = Path.Combine("Images", imageName); //TODO: Modificar esta ruta
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar la imagen", ex);
            }

            return stringPath;
        }

        public async Task<CollaboratorDto> Save(CollaboratorDto dto)
        {
            Collaborator collaborator = new Collaborator();

            if (dto.Id.Equals(0))
            {
                dto.User = await _userService.Save(dto);
                var newCollaborator = _mapper.Map<Collaborator>(dto);
                BaseEntityHelper.SetCreated(newCollaborator);
                collaborator = await _repository.Add(newCollaborator);
            }
            else
            {
                var updatedCollaborator = _mapper.Map<Collaborator>(dto);
                BaseEntityHelper.SetUpdated(updatedCollaborator);
                collaborator = await _repository.Update(updatedCollaborator);
            }
            return _mapper.Map<CollaboratorDto>(collaborator);
        }

        public async Task<(bool isValid, string message)> Validate(int? id, CollaboratorDto dto)
        {
            var validations = new List<(bool isValid, string message)>();

            //TODO: Agregar las validaciones

            //var validator = new CollaboratorValidator();
            //var result = await validator.ValidateAsync(dto);
            //validations.Add((result.IsValid, string.Join(Environment.NewLine, result.Errors.Select(x => $"Campo {x.PropertyName} invalido. Error: {x.ErrorMessage}"))));

            return (isValid: validations.All(x => x.isValid),
                   message: string.Join(Environment.NewLine, validations.Where(x => !x.isValid).Select(x => x.message)));
        }
    }
}
