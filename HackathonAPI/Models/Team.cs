using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackathonAPI.Models
{
    public class Team
    {
        public int id { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public string icon { get; set; }
    }
}