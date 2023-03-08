using BaseApiContext.Files.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApiContext.Files
{
    public interface IDocumentManager
    {
        public Task<UploadedDocumentInfo> saveUploadDocument(HttpRequestMessage request, List<string>? allowedExtensions = null);
        
    }
}
