using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarlosAg.ExcelXmlWriter;

namespace ExportMeetupGroup.Models
{
    public class HomeModel
    {
        public string MeetupGroupUrl { get; set; }

        public Workbook excel { get; set; }
    }
}