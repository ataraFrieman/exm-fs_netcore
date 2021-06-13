using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Quze.Models.Entities
{
    [Table("UploadOperationDetails‏")]
   public class UploadOperationDetails‏: EntityBase
    {
        public DateTime uploadBeginTime‏ { get; set; }
        public DateTime uploadEndTime‏ { get; set; }
        public bool isStart‏ { get; set; }
        public bool isEnd‏ { get; set; }
        public bool isError { get; set; }
        public int serviceQId { get; set; }

    }
}
