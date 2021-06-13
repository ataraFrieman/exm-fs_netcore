using Quze.Models.Entities;
using Quze.Organization.Web.Pages.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.Organization.Web.ViewModels
{
    public class OperationsResponseVM
    {
            public List<AppointmentOppVM> OperationsList;
            public List<ConflictVM> ConflictList;
            public ServiceQueue ServiceQueue;
        
    }
}
