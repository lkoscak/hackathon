using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackathonAPI.Models.Report
{
    public class Report
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime created;
        public string creator { get; set; }
        public List<string> images { get; set; }
        public int status { get; set; }
        public int group { get; set; }
        public int team { get; set; }
        public string category { get; set; }
        public float lat { get; set; }
        public float lng { get; set; }
    }
}