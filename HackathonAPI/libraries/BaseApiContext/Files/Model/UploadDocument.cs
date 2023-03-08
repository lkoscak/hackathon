using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApiContext.Files.Model
{
    public class UploadDocument
    {
        public int udComId { get; set; }
        public int udEntId { get; set; }
        public string? udFilePath { get; set; }
        public string? udRelativePath { get; set; }
        public string? udDescription { get; set; }
        public string? udUrl { get; set; }
        public bool udActive { get; set; }
        public int udType { get; set; }
        public int udRefId { get; set; }
        public int udCreatorId { get; set; }
        public long udCreated { get; set; }

        //parameteres from Document form from Fleet portal
        public int usrId { get; set; }
        public int folderId { get; set; }
        public string? guid { get; set; }
        public string? originalFileName { get; set; }
        public int fileSize { get; set; }
        public string? fileExtension { get; set; }
    }
}
