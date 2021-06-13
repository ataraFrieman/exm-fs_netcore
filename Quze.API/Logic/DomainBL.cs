using System;
using Quze.Models.Entities;

namespace Quze.Models.Logic
{
    public class DomainEntityFactory
    {
        public static void FillCreatedTimeFields<T>(T entity, int createdUserId) where T : EntityBase
        {
            if (entity == null) return;
            entity.CreatedUserId = entity.LastUpdateUserId = createdUserId;
            entity.TimeCreated = entity.TimeUpdated = DateTime.Now;
        }

        public static void FillUpdatedTimeFields<T>(T entity) where T : EntityBase
        {
            if (entity == null) return;
            entity.TimeUpdated = DateTime.Now;
        }
    }
}