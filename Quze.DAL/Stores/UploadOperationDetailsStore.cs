using System;
using System.Collections.Generic;
using System.Text;
using Quze.Models.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Quze.DAL.Stores
{
    public class UploadOperationDetailsStore : StoreBase<UploadOperationDetails>
    {

        public UploadOperationDetailsStore(QuzeContext ctx) : base(ctx)
        {

        }

        public void startUpload(int serviceQId)
        {
            UploadOperationDetails uploadOperationDetails = new UploadOperationDetails();
            var serviceQ = ctx.ServiceQueues.Where(o => o.Id== serviceQId).FirstOrDefault();
            uploadOperationDetails.serviceQId = serviceQ.Id;
            uploadOperationDetails.uploadBeginTime = DateTime.Now;
            uploadOperationDetails.isStart =true;
            ctx.uploadOperationDetails.Add(uploadOperationDetails);
            ctx.SaveChanges();
        }

        public void endUpload(int serviceQ)
        {
            var uploadQ = ctx.uploadOperationDetails.Where(o => o.serviceQId == serviceQ).FirstOrDefault();
            uploadQ.isEnd = true;
            uploadQ.uploadEndTime = DateTime.Now;
            ctx.SaveChanges();
        }
        public void errorUpload(int serviceQ)
        {
            var uploadQ = ctx.uploadOperationDetails.Where(o => o.serviceQId == serviceQ).FirstOrDefault();
            uploadQ.isError = true;
            ctx.SaveChanges();
        }

    }
}


