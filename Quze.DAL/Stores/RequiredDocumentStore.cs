using Microsoft.EntityFrameworkCore;
using Quze.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Quze.DAL.Stores
{
    public class RequiredDocumentStore : StoreBase<RequiredDocument>
    {

        public RequiredDocumentStore(QuzeContext ctx) : base(ctx)
        {

        }

        public List<RequiredDocument> GetDocumentsByST(int id)
        {
            var tasks = ctx.RequiredDocuments
                //.Include(doc => doc.AlertRules)
               .Where(document => document.ServiceTypeID == id)
               .OrderBy(document => document.TimeUpdated)
               .Select(document => document
              ).ToList();

            return tasks;
        }



    }
}
