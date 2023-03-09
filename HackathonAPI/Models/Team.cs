using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackathonAPI.Models
{
    public class Team
    {
        public int team_id { get; set; }
        public string team_name { get; set; }
        public string team_color { get; set; }
        public string team_icon { get; set; }
    }
}