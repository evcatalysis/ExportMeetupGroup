using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ExportMeetupGroup.Common
{
    public class Helper
    {
        public static string Meetup_client_id
        {
            get
            {
                return ConfigurationManager.AppSettings["Meetup_client_id"].ToString();
            }
        }

        public static string Meetup_client_secret
        {
            get
            {
                return ConfigurationManager.AppSettings["Meetup_client_secret"].ToString();
            }
        }

        public static string Meetup_redirect_uri
        {
            get
            {
                return ConfigurationManager.AppSettings["Meetup_redirect_uri"].ToString();
            }
        }

        public static string toString(object obj)
        {
            if (obj != null)
                return obj.ToString();
            else
                return "";
        }

        public static string toDate(object obj)
        {
            if (obj != null)
            {
                double javaTimeStamp = Convert.ToDouble(obj);
                // Java timestamp is millisecods past epoch
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(Math.Round(javaTimeStamp / 1000)).ToLocalTime();
                return dtDateTime.ToString();
            }
            else
            {
                return "";
            }
        }
    }
}