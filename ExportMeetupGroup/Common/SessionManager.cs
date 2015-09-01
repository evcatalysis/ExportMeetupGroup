using ExportMeetupGroup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExportMeetupGroup.Common
{
    public class SessionManagement
    {
        public static string access_token
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["access_token"] != null)
                {
                    return System.Web.HttpContext.Current.Session["access_token"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                System.Web.HttpContext.Current.Session["access_token"] = value;
            }

        }

        public static HomeModel model
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["HomeModel"] != null)
                {
                    return System.Web.HttpContext.Current.Session["HomeModel"] as HomeModel;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                System.Web.HttpContext.Current.Session["HomeModel"] = value;
            }

        }
    }
}
