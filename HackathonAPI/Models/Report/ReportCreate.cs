using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackathonAPI.Models.Report
{
    public class ReportCreate
    {
        public string title { get; set; }
        public string description { get; set; }
        public string creator { get; set; }
        public List<string> images { get; set; }
        public int group { get; set; }
        public string category { get; set; }
        public float lat { get; set; }
        public float lng { get; set; }
    }
}