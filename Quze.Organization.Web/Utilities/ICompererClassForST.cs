using Quze.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quze.Organization.Web.Utilites
{
    class ICompererClassForST : IComparer<ServiceType>
    {
        public int Compare(ServiceType x, ServiceType y)
        {
            return x.Id - y.Id;
        }
    }
}
