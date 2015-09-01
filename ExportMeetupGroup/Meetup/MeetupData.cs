using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportMeetupGroup.Meetup
{
    public class MeetupData
    {
        public List<MeetupUser> results { get; set; }

        public Meetup_meta meta { get; set; }
    }
}
