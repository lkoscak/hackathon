using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Core.Files.Model
{
    public class FileDownload
    {
        public int file_id { get; set; }
        public string file_type { get; set; }
        //public string frt_code { get; set; }
        //public int file_ref_id { get; set; }
        public string file_name { get; set; }
        public string file_guid { get; set; }
        public string file_download_path { get; set; }
        public int file_size { get; set; }
    }
}