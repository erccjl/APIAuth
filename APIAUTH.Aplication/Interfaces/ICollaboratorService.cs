using APIAUTH.Aplication.DTOs;
using Microsoft.AspNetCore.Http;

namespace APIAUTH.Aplication.Interfaces
{
    public interface ICollaboratorService : IGenericService<CollaboratorDto>
    {
        Task<string> PutImage(IFormFile image);
    }
}
