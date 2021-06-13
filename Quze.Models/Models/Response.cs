
using Quze.Infrastruture.Types;
using Quze.Infrastruture.Utilities;
using System.Collections.Generic;

namespace Quze.Models
{
    public class Response<T>
    {

        const int ErrorResultCode = -1;

        #region props
        public int ResultCode { get; set; }
        public T Entity { get; set; }
        public List<T> Entities { get; set; }
        public List<Error> Errors { get; }

        #endregion
        public Response()
        {
           Entities = new List<T>();
            Errors = new List<Error>();
            ResultCode = 0;//no errors
        }

        public Response(Request<T> request)
        {
            Entities = new List<T>();
            Errors = new List<Error>();
            ResultCode = 0;
            if (request != null)
            {
                Entity = request.Entity;
                Entities = request.Entities;
            }
        }
        

        public void AddError(Error error)
        {
            ResultCode = ErrorResultCode;
            Errors.Add(error);
        }

        public void AddError(int code, string description)
        {
            ResultCode = ErrorResultCode;
            var error = new Error(code, description);
            AddError(error);
        }

        public void AddError(ErrorCodes code, string description)
        {
            ResultCode = ErrorResultCode;
            AddError((int)code, description);
        }

        public void AddError(ErrorCodes code)
        {
            ResultCode = ErrorResultCode;
            AddError((int)code, string.Empty);
        }


        public void AddErrors(List<Error> errors)
        {
            ResultCode = ErrorResultCode;
            Errors.AddRange(errors);
        }
    }
}
