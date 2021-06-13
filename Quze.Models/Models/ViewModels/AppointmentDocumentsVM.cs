using System;

namespace Quze.Models.Models.ViewModels
{
    public class AppointmentDocumentVM : BaseVM
    {
        public int AppointmentId { get; set; }
        public int RequiredDocumentId { get; set; }
        public RequiredDocumentVM RequiredDocument { get; set; }
        public DateTime? TimeApproved { get; set; }
        public string ApprovedFrom { get; set; }

    }
}