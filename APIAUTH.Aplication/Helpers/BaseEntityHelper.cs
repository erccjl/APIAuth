using APIAUTH.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAUTH.Aplication.Helpers
{
    public static class BaseEntityHelper
    {
        public static void SetCreated<T>(T entity)
            where T : BaseEntity
        {
            entity.CreatedDate = DateTime.Now;
            entity.State = Domain.Enums.BaseState.Activo;
        }

        public static void SetUpdated<T>(T entity)
            where T : BaseEntity
        {
            entity.UpdatedDate = DateTime.Now;
        }

        public static void SetActive<T>(T entity)
            where T : BaseEntity
        {
            entity.State = Domain.Enums.BaseState.Activo;
            entity.UpdatedDate = DateTime.Now;
        }

        public static void SetInactive<T>(T entity)
            where T : BaseEntity
        {
            entity.State = Domain.Enums.BaseState.Inactivo;
            entity.UpdatedDate = DateTime.Now;
        }
    }
}
