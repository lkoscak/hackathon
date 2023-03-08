using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApi.Core.Files.Model
{
    public class ApiFileResult : IHttpActionResult
    {


        private readonly string contentType;
        private readonly string fileName;
        private readonly MemoryStream stream;
        private readonly bool attachment;

        public ApiFileResult(string filePath, string contentType, string fileName = null, bool attachment = true)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            this.contentType = contentType ?? MimeMapping.GetMimeMapping(Path.GetExtension(filePath));
            this.attachment = attachment;
            var fileStream = File.OpenRead(filePath);
            var fileBytes = new byte[fileStream.Length];
            fileStream.Read(fileBytes, 0, fileBytes.Length);
            stream = new MemoryStream(fileBytes);

            this.fileName = fileName ?? Path.GetFileName(filePath);
        }

        public ApiFileResult(MemoryStream stream, string contentType, string fileName, bool attachment = true)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (contentType == null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            this.stream = stream;
            this.contentType = contentType;
            this.fileName = fileName;
            this.attachment = attachment;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var bytes = stream.ToArray();
            var memoryStream = new MemoryStream(bytes);

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(memoryStream)
            };

            response.Content.Headers.ContentEncoding.Add("UTF-8");
            response.Headers.Add("charset", "UTF-8");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            response.Content.Headers.ContentLength = memoryStream.Length;
            if (this.attachment)
            {
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };
            }
            else
            {
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
                {
                    FileName = fileName
                };
            }

            return Task.FromResult(response);
        }
    }
}