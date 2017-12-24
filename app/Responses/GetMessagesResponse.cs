using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace app.Responses
{
    [DataContract]
    public class GetMessagesResponse
    {
        [DataMember(Name = "inbox")]
        public string Inbox { get; set; }

        [DataMember(Name = "_id")]
        public string Id { get; set; }

        [DataMember(Name = "subject")]
        public string Subject { get; set; }

        [DataMember(Name = "received")]
        public string Received { get; set; }

        [DataMember(Name = "originalInbox")]
        public string OriginalInbox { get; set; }

        [DataMember(Name = "links")]
        public List<string> Links { get; set; }

        [IgnoreDataMember]
        public DateTime ReceivedDate =>
            DateTime.ParseExact(Received, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
    }
}
