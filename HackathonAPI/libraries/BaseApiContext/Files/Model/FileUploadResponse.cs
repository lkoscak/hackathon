using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Core.Files.Model
{
    public class FileUploadResponse
    {
        public int fileId;
        public string fileName;
        public string filePath;
        public string guid;
        //public string token { get; set; }
        //public bool mustChangePassword;
    }
}
