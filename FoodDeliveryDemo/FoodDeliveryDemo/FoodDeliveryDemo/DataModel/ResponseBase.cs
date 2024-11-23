using System.Runtime.Serialization;

namespace TrackerAdminApi.DataModel
{
    [DataContract]
    public class ResponseBase<T>
    {
        public ResponseBase()
        {
            IsSuccessful = true;

        }

        [DataMember]
        public object? Data { get; set; }

        [DataMember]
        public bool IsSuccessful { get; set; }

        public int Code { get; set; }

        public string? Message { get; set; }


    }
}
