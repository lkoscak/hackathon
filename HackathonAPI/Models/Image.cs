using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackathonAPI.Models
{
    public class Image
    {
        public string name { get; set; }
        public string type { get; set; }
        public byte [] content { get; set; }
    }
}