using Microsoft.EntityFrameworkCore;
using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;
using Quze.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quze.DAL.Stores
{
    public class FellowStore : StoreBase<Fellow>
    {

        public FellowStore(QuzeContext ctx) : base(ctx)
        {

        }
        public List<Fellow> GetFellowsByOrganization(int OrganizationId)
        {
            List<Fellow> fellowsList = new List<Fellow>();
            var fellowsListQuery = ctx.Fellows.Where(f => f.OrganizationId == OrganizationId&&!f.IsDeleted);//.OrderBy(f => f.TimeUpdated);
            fellowsList = fellowsListQuery.ToList();
            return fellowsList;
        }
        public Fellow AddFellow(OperationRecord item, int organizationId, bool validateExist = false)
        {
            var fellowName = item.FellowName.Split(" ");
            if (validateExist)
            {
                Fellow validateFellow = ctx.Fellows.Where(f => f.IdentityNumber == item.FellowCode && f.OrganizationId == organizationId).FirstOrDefault();
                if (validateFellow != null)
                {
                    validateFellow.FirstName = fellowName.Length > 0 ? fellowName[0] : "";
                    validateFellow.LastName = fellowName.Length > 1 ? fellowName[1] : "";
                    validateFellow.Gender = item.FellowGender;
                    validateFellow.BirthDate = DateTime.Now.AddYears(0 - item.FellowAge);
                    validateFellow.Age = item.FellowAge;
                    validateFellow.Weight = item.FellowWeight;
                    validateFellow.Height = item.FellowHeight;
                    validateFellow.diabetis = item.IsFellowDiabetic;
                    validateFellow.hypertension = item.IsHBP;
                    return validateFellow;
                }
            }
            Fellow fellow = new Fellow()
            {
                IdentityNumber=item.FellowCode,
                FirstName = fellowName.Length > 0 ? fellowName[0] : "",
                LastName = fellowName.Length > 1 ? fellowName[1] : "",
                Gender = item.FellowGender,
                BirthDate = DateTime.Now.AddYears(0 - item.FellowAge),
                Age = item.FellowAge,
                Weight = item.FellowWeight,
                Height = item.FellowHeight,
                diabetis = item.IsFellowDiabetic,
                hypertension = item.IsHBP,
                OrganizationId=organizationId,
                //Height= item.
            };
            return AddFellow(fellow, validateExist);
        }
        public Fellow AddFellow(Fellow fellow, bool validateExist = false)
        {
            ctx.Fellows.Add(fellow);
            return fellow;
        }
        public void DeleteFellow(Fellow fellow)
        {
            var Deleted = fellow;
            Deleted.IsDeleted = true;
            ctx.Entry(fellow).CurrentValues.SetValues(Deleted);
            ctx.Entry(fellow).State = EntityState.Modified;
            ctx.SaveChanges();
        }
        public void UpdateFellow(Fellow fellow)
        {
            var Updated = ctx.Fellows.FirstOrDefault(f => f.Id == fellow.Id);
            Updated.FirstName = fellow.FirstName;
            Updated.LastName = fellow.LastName;
            Updated.IdentityNumber = fellow.IdentityNumber;
            ctx.Entry(Updated).State = EntityState.Modified;
            ctx.SaveChanges();
        }
    }
}
