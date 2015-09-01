using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportMeetupGroup.Meetup
{
    public class MeetupUser
    {
        public string bio { get; set; }
        public Birthdate birthday { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string gender { get; set; }
        public facebook_connection facebook_connection { get; set; }
        public string hometown { get; set; }
        public long id { get; set; }
        public long joined { get; set; }
        public string lang { get; set; }
        public Nullable<decimal> lat { get; set; }
        public Nullable<decimal> lon { get; set; }
        public string link { get; set; }
        public int? membership_count { get; set; }
        public bool? messagable { get; set; }
        public string messaging_pref { get; set; }
        public string name { get; set; }
        public MeetupPhoto photo { get; set; }
        public string status { get; set; }
        public long? visited { get; set; }
        public string reachable { get; set; }
        public List<MeetupTopic> topics { get; set; }
        public List<MeetupPhoto> photos { get; set; }
    }
}
