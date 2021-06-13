using Quze.Models.Entities;
using System;

namespace Quze.Models.Logic
{
    public class DomainEntityFactory
    {
        public static void FillCreatedTimeFields<T>(T entity,int createdUserId) where T:EntityBase
        {
            if(entity==null )return;
            entity.CreatedUserId = entity.LastUpdateUserId = createdUserId;
            entity.TimeCreated = entity.TimeUpdated = DateTime.Now;
        }
    }
}
