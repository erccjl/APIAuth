using APIAUTH.Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAUTH.Aplication.Interfaces
{
    public interface IGenericService<TDto>
         where TDto : BaseDto
    {
        Task<TDto> Get(int id);
        Task<bool> Exists(int id);
        Task Activate(int id);
        Task Inactivate(int id);
        Task<TDto> Save(TDto dto);
        Task<List<TDto>> GetAll();
        Task<(bool isValid, string message)> Validate(int? id, TDto dto);
    }
}
