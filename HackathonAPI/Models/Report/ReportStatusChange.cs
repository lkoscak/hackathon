using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HackathonAPI.Models.Report
{
    public class ReportStatusChange
    {
        [Required(ErrorMessage = "{0} is required")]
        public int status { get; set; }
        public int? id { get; set; }
    }
}