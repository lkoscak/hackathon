using BaseApiContext.Files.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApiContext.Files.Repository
{
    public interface IDocumentRepository
    {
        public Task<bool> CheckIfCanAddDocument(UploadDocument uploadDocument);
        public Task<UploadedDocumentInfo> UploadDocument(UploadDocument uploadDocument);
    }
}
