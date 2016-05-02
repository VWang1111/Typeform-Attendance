using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typeform_Attendance
{

    public class Rootobject
    {
        public int http_status { get; set; }
        public Stats stats { get; set; }
        public Question[] questions { get; set; }
        public Respons[] responses { get; set; }
    }

    public class Stats
    {
        public Responses responses { get; set; }
    }

    public class Responses
    {
        public int showing { get; set; }
        public int total { get; set; }
        public int completed { get; set; }
    }

    public class Question
    {
        public string id { get; set; }
        public string question { get; set; }
        public int field_id { get; set; }
    }

    public class Respons
    {
        public string completed { get; set; }
        public string token { get; set; }
        public Metadata metadata { get; set; }
        public object[] hidden { get; set; }
        public Answers answers { get; set; }
    }

    public class Metadata
    {
        public string browser { get; set; }
        public string platform { get; set; }
        public string date_land { get; set; }
        public string date_submit { get; set; }
        public string user_agent { get; set; }
        public string referer { get; set; }
        public string network_id { get; set; }
    }

    public class Answers
    {
        public string textfield_21284714 { get; set; }
        public string textfield_21284762 { get; set; }
        public string email_21284788 { get; set; }
    }

}
