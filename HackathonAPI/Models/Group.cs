using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackathonAPI.Models
{
    public class GroupModel
    {
        public int group_id { get; set; }
        public string group_name { get; set; }
        public string group_description { get; set; }
    }
}