using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace app.Responses
{
    [DataContract]
    public class CreateGuestResponse
    {
        [DataMember(Name = "id")]
        public uint Id { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "login")]
        public string Login { get; set; }

        [DataMember(Name = "nickname")]
        public string Nickname { get; set; }

        [DataMember(Name = "security")]
        public List<string> Security { get; set; }

        [DataMember(Name = "lang")]
        private string Lang { get; set; }

        [DataMember(Name = "community")]
        public string Community { get; set; }

        [DataMember(Name = "added_date")]
        private string AddedDate { get; set; }

        [DataMember(Name = "added_ip")]
        public string AddedIp { get; set; }

        [DataMember(Name = "login_date")]
        public string LoginDate { get; set; }

        [DataMember(Name = "login_ip")]
        public string LoginIp { get; set; }

        [IgnoreDataMember]
        public string XPassword { get; set; }


        [IgnoreDataMember]
        public DateTime AddedDateSer =>
            DateTime.ParseExact(AddedDate, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
    }
}