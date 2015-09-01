using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExportMeetupGroup.Models;
using ExportMeetupGroup.Common;
using System.Net;
using System.IO;
using ExportMeetupGroup.Meetup;
using CarlosAg.ExcelXmlWriter;

namespace ExportMeetupGroup.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        [HttpGet]
        public ActionResult Index()
        {
            HomeModel model = new HomeModel();
            if (SessionManagement.model != null)
            {
                model = SessionManagement.model;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(HomeModel model)
        {
            if (!string.IsNullOrEmpty(model.MeetupGroupUrl))
            {
                if (SessionManagement.model == null || SessionManagement.model.MeetupGroupUrl != model.MeetupGroupUrl)
                {
                    string url = getJsonUrl(model.MeetupGroupUrl);
                    MeetupData data = readJson(url);
                    Workbook excel = getExcel(data);
                    model.excel = excel;
                    SessionManagement.model = model;
                }
                else
                {
                    model = SessionManagement.model;
                }
            }
            
            return View(model);
        }

        [HttpPost]
        public ActionResult ExportToExcel()
        {
            if (SessionManagement.model.excel != null)
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("content-disposition", @"attachment;filename=" + SessionManagement.model.MeetupGroupUrl + ".xls");
                SessionManagement.model.excel.Save(Response.OutputStream);
                Response.End();
            }
            return RedirectToAction("Index", SessionManagement.model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            SessionManagement.access_token = model.access_token;
            return RedirectToAction("Index");
        }

        public string getJsonUrl(string MeetupGroupUrl)
        {
            string url = "https://api.meetup.com/2/members";
            url += "?offset=1";
            url += "&format=json";
            url += "&group_urlname=" + MeetupGroupUrl;
            url += "&photo-host=public";
            url += "&page=100";
            url += "&order=name";
            url += "&sig=true";
            url += "&access_token=" + SessionManagement.access_token;
            return url;
        }

        public string MakeRequest(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));

                    Stream stream = response.GetResponseStream();
                    //stream.Position = 0;
                    var sr = new StreamReader(stream);
                    string data = sr.ReadToEnd();

                    return data;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private Workbook getExcel(MeetupData data)
        {
            Workbook book = new Workbook();
            setUserSheet(book, data.results);

            return book;
        }

        private void setUserSheet(Workbook book ,List<MeetupUser> list)
        {
            List<MeetupTopic> topic_list = new List<MeetupTopic>();
            List<MeetupUserTopic> user_topic_list = new List<MeetupUserTopic>();

            Worksheet sheet = book.Worksheets.Add("Meetup User");
            WorksheetTable table = sheet.Table;

            WorksheetRow hRow = table.Rows.Add();
            hRow.Cells.Add("id");
            hRow.Cells.Add("name");
            hRow.Cells.Add("bio");
            hRow.Cells.Add("birthday");
            hRow.Cells.Add("country");
            hRow.Cells.Add("city");
            hRow.Cells.Add("state");
            hRow.Cells.Add("facebook_connection");
            hRow.Cells.Add("gender");
            hRow.Cells.Add("hometown");
            hRow.Cells.Add("joined");
            hRow.Cells.Add("lang");
            hRow.Cells.Add("lat");
            hRow.Cells.Add("lon");
            hRow.Cells.Add("link");
            hRow.Cells.Add("membership_count");
            hRow.Cells.Add("messagable");
            hRow.Cells.Add("messaging_pref");
            hRow.Cells.Add("photo_id");
            hRow.Cells.Add("highres_link");
            hRow.Cells.Add("photo_link");
            hRow.Cells.Add("thumb_link");
            hRow.Cells.Add("status");
            hRow.Cells.Add("visited");
            hRow.Cells.Add("reachable");

            foreach (MeetupUser item in list)
            {
                WorksheetRow row = table.Rows.Add();

                row.Cells.Add(Helper.toString(item.id));
                row.Cells.Add(Helper.toString(item.name));
                row.Cells.Add(Helper.toString(item.bio));
                string birthday = "";
                if (item.birthday != null)
                {
                    if (item.birthday.day > 0)
                    {
                        birthday += item.birthday.day;
                    }
                    if (item.birthday.month > 0)
                    {
                        if (birthday != "")
                            birthday += "/";
                        birthday += item.birthday.month;
                    }
                    if (item.birthday.year > 0)
                    {
                        if (birthday != "")
                            birthday += "/";
                        birthday += item.birthday.year;
                    }
                }
                row.Cells.Add(Helper.toString(birthday));
                row.Cells.Add(Helper.toString(item.country));
                row.Cells.Add(Helper.toString(item.city));
                row.Cells.Add(Helper.toString(item.state));
                if (item.facebook_connection != null)
                {
                    row.Cells.Add(Helper.toString(item.facebook_connection.status));
                }
                else
                {
                    row.Cells.Add("");
                }
                row.Cells.Add(Helper.toString(item.gender));
                row.Cells.Add(Helper.toString(item.hometown));
                row.Cells.Add(Helper.toDate(item.joined));
                row.Cells.Add(Helper.toString(item.lang));
                row.Cells.Add(Helper.toString(item.lat));
                row.Cells.Add(Helper.toString(item.lon));
                row.Cells.Add(Helper.toString(item.link));
                row.Cells.Add(Helper.toString(item.membership_count));
                row.Cells.Add(Helper.toString(item.messagable));
                row.Cells.Add(Helper.toString(item.messaging_pref));
                if (item.photo != null)
                {
                    row.Cells.Add(Helper.toString(item.photo.photo_id));
                    row.Cells.Add(Helper.toString(item.photo.highres_link));
                    row.Cells.Add(Helper.toString(item.photo.photo_link));
                    row.Cells.Add(Helper.toString(item.photo.thumb_link));
                }
                else
                {
                    row.Cells.Add("");
                    row.Cells.Add("");
                    row.Cells.Add("");
                    row.Cells.Add("");
                }
                row.Cells.Add(Helper.toString(item.status));
                row.Cells.Add(Helper.toDate(item.visited));
                row.Cells.Add(Helper.toString(item.reachable));

                foreach (MeetupTopic topic in item.topics)
                {
                    if (!topic_list.Any(t => t.id == topic.id))
                    {
                        topic_list.Add(topic);
                    }
                    user_topic_list.Add(new MeetupUserTopic()
                    {
                        userid = item.id,
                        topicid = topic.id
                    });
                }
            }

            setTopicSheet(book,topic_list);
            setUserTopicSheet(book,user_topic_list);
        }

        private void setTopicSheet(Workbook book, List<MeetupTopic> list)
        {
            Worksheet sheet = book.Worksheets.Add("Meetup Topics");
            WorksheetTable table = sheet.Table;

            WorksheetRow hRow = table.Rows.Add();

            hRow.Cells.Add("id");
            hRow.Cells.Add("name");
            hRow.Cells.Add("urlkey");

            foreach (MeetupTopic topic in list)
            {
                WorksheetRow row = table.Rows.Add();

                row.Cells.Add(Helper.toString(topic.id));
                row.Cells.Add(Helper.toString(topic.name));
                row.Cells.Add(Helper.toString(topic.urlkey));
            }
        }

        private void setUserTopicSheet(Workbook book, List<MeetupUserTopic> list)
        {
            Worksheet sheet = book.Worksheets.Add("Meetup User Topics");
            WorksheetTable table = sheet.Table;

            WorksheetRow hRow = table.Rows.Add();
            hRow.Cells.Add("user id");
            hRow.Cells.Add("topic id");

            foreach (MeetupUserTopic user_topic in list)
            {
                WorksheetRow row = table.Rows.Add();

                row.Cells.Add(Helper.toString(user_topic.userid));
                row.Cells.Add(Helper.toString(user_topic.topicid));
            }
        }

        public MeetupData readJson(string url)
        {
            string data = MakeRequest(url);
            if (data != null)
            {
                MeetupData meetupData = Newtonsoft.Json.JsonConvert.DeserializeObject<MeetupData>(data);
                if (meetupData.meta.next != "")
                {
                    MeetupData pData = readJson(meetupData.meta.next);
                    meetupData.results.AddRange(pData.results);
                }
                return meetupData;
            }
            else
            {
                return null;
            }
        }
    }
}
