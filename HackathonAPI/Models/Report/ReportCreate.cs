﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HackathonAPI.Models.Report
{
    public class ReportCreate
    {
        [Required(ErrorMessage = "{0} is required")]
        public string title { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string description { get; set; }
        public string creator { get; set; }
        public List<string> images { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public int group { get; set; }
        public string category { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public float? lat { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public float? lng { get; set; }

        public bool AreCoordsValid()
        {
            return (lat >= -90 && lat <= 90 && lng >= -180 && lng <= 180);
        }
    }
}