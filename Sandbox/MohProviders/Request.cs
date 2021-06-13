using System;
using System.Collections.Generic;
using System.Text;

namespace MohProviders
{
    public class Request<T>
    {

        /// <summary>
        /// The version of the API that this request compliance with, if avoided the system will assume last version
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// The value was supplied to the user wen registerd
        /// </summary>
        public string AuthonticationToken { get; set; }
        /// <summary>
        /// A value indicating 
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// A number or code that identifies the request for tracking, this number will be returned in the 
        ///  Clients should avoid sending the same number or code multiple times
        /// </summary>
        public string referenceId { get; set; }
        /// <summary>
        /// Create,Update,Read,Delete
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        /// Indicates if to allow partial success updates
        /// </summary>
        public bool? AllowPartialSuccess { get; set; }

        public int? EntityId { get; set; }
        public List<T> Entities { get; set; }

        public T Entity { get; set; }
    }

}
