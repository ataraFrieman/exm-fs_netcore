using Quze.Infrastruture.Extensions;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Quze.Models.ML
{
    [DataContract]
    public class BasicMLResponse
    {
        [DataMember(EmitDefaultValue = false)]
        public int? responseDuration { get; set; } = -1;

        [DataMember(EmitDefaultValue = false)]
        public int? responseDelayDuration { get; set; } = -1;

        [DataMember(EmitDefaultValue = false)]
        public float? responseHasDelay { get; set; } = (float)0.05;

        public float? responseNoShow { get; set; } = (float)0.05;

        [DataMember(EmitDefaultValue = false)]
        public List<string> Recommendation { get; set; } = new List<string>();

    }

    public class ResponseML : BasicMLResponse
    {
        [DataMember(EmitDefaultValue = false)]
        public int RequestId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? IsError { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Error> Errors { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public int? AsDelay { get; set; } = -1;

        [DataMember(EmitDefaultValue = false)]
        public float HasDelayProb { get; set; } = -1;


        [DataMember(EmitDefaultValue = false)]
        public int? IsNoShow { get; set; } = -1;

        [DataMember(EmitDefaultValue = false)]
        public float NSProb { get; set; } = -1;


        public void SetNoShowRecommendation()
        {
            if (responseNoShow > 0.8)
            {
                Recommendation.Add("High probability of no-show, stroke the appointment at the end of the day or parallel to another appointment");
            }
            else if (responseNoShow > 0.4)
            {
                Recommendation.Add("A reasonable possibility of no-show, it is recommended to verify with the fellow the arrival");
            }
        }

        public void SetAsDelayRecommendation()
        {
            if (responseHasDelay > 0.8)
            {
                Recommendation.Add("High probability of no-show, stroke the appointment at the end of the day or parallel to another appointment");
            }
            else if (responseHasDelay > 0.4)
            {
                Recommendation.Add("A reasonable possibility of no-show, it is recommended to verify with the fellow the arrival");
            }
            else
                Recommendation.Add("");


        }


        public void AddServiceError(string ServerErrorDescription)
        {
            if (Errors.IsNull())
            {
                Errors = new List<Error>();
            }

            Errors.Add(new Error() { ErrorDescription = ServerErrorDescription });
        }
    }

    public class Error
    {

        public string modelError { get; set; }

        public string ErrorType { get; set; }

        public string ErrorDescription { get; set; }
    }
}
