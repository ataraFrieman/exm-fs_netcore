using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quze.DAL.Stores
{
   public class DepartmentsStore: StoreBase<Departments>
    {
        public DepartmentsStore(QuzeContext ctx) : base(ctx)
        {

        }

        public Departments GetDepartmentById(int id)
        {

            Departments department = ctx.Departments.Find(id);
            return department;
        }
    }
}
