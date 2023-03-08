using BaseApiContext.Files.Model;
using BaseApiContext.Files.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Core.BusinessLogicManager;
using WebApi.Core.Context;

namespace BaseApiContext.Files
{
    public class DocumentManager : BLManager
    {
        IContextManager contextManager;
        private MultipartFormDataStreamProvider result;
        IDocumentRepository documentRepository;
        public DocumentManager(IContextManager contextManager) : base(contextManager)
        {
            this.contextManager = contextManager;
            documentRepository = contextManager.resolveDI<IDocumentRepository>();
        }

        public async Task<UploadedDocumentInfo> saveUploadDocument(HttpRequestMessage request, List<string>? allowedExtensions = null, int maxFileSize = 3000)
        {
            if (allowedExtensions == null) allowedExtensions = getAllowedExtensions();
            string root = ContextManager.configManager.getDocumentImagesPath();
            string hostDomain = ContextManager.configManager.getFleetWebBaseURL();
            string documentFolder = ContextManager.configManager.getDocumentFolder();
            ContextManager.loggerManager.info("Document save folder => " + root);

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
                ContextManager.loggerManager.info("Creating directory => " + root);
            }

            var provider = new MultipartFormDataStreamProvider(root);

            result = await request.Content.ReadAsMultipartAsync(provider);

            UploadDocument? uploadDocument = getDocumentInfo(provider);

            string path = result.FileData.First().LocalFileName;
            ContextManager.loggerManager.info("Path: " + path);

            if (uploadDocument == null)
            {
                ContextManager.loggerManager.info("Document object not found, deleting file");
                File.Delete(path);
                throw new Exception("Error parsing document data!");
            }
            else
            {
                return await saveDocument(allowedExtensions, hostDomain, documentFolder, uploadDocument, path, maxFileSize);
            }
        }
        public async Task<UploadedDocumentInfo> saveUploadDocumentWithType(HttpRequestMessage request, UploadDocument uploadDocument, List<string>? allowedExtensions = null, int maxFileSize = 3000)
        {
            if (allowedExtensions == null) allowedExtensions = getAllowedExtensions();
            string root = ContextManager.configManager.getDocumentImagesPath();
            string hostDomain = ContextManager.configManager.getFleetWebBaseURL();
            string documentFolder = ContextManager.configManager.getDocumentFolder();
            ContextManager.loggerManager.info("Document save folder => " + root);
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
                ContextManager.loggerManager.info("Creating directory => " + root);
            }
            var provider = new MultipartFormDataStreamProvider(root);
            result = await request.Content.ReadAsMultipartAsync(provider);
            string path = result.FileData.First().LocalFileName;
            ContextManager.loggerManager.info("Path: " + path);

            if (uploadDocument == null)
            {
                ContextManager.loggerManager.info("Document object not found, deleting file");
                File.Delete(path);
                throw new Exception("Error parsing document data!");
            }
            else
            {
                return await saveDocument(allowedExtensions, hostDomain, documentFolder, uploadDocument, path, maxFileSize);
            }
        }

        public async Task<UploadedDocumentInfo> saveFileAsDocument(byte[] fileData, UploadDocument uploadDocument)
        {
            string extension = Path.GetExtension(uploadDocument.originalFileName);
            string root = ContextManager.configManager.getDocumentImagesPath();
            string hostDomain = ContextManager.configManager.getFleetWebBaseURL();
            string documentFolder = ContextManager.configManager.getDocumentFolder();
            uploadDocument.udComId = contextManager.session.CompanyId;
            uploadDocument.udCreatorId = contextManager.session.UserId;
            string stringdate = DateTime.Now.ToString("yyyyMMddHHmmss");
            uploadDocument.udCreated = long.Parse(stringdate);


            if (getAllowedExtensions().Contains(extension.ToLower()))
            {
                DateTime dateTime = DateTime.Now;
                string dayString = dateTime.ToString("yyyyMMdd");
                string directoryName = root + @"\" + ContextManager.session.CompanyId + @"\" + dayString;
                DirectoryInfo directory = new DirectoryInfo(directoryName);
                if (!directory.Exists) directory = Directory.CreateDirectory(directoryName);
                string fileName = Guid.NewGuid().ToString();
                
                uploadDocument.guid = fileName;
                string newFileName = directory.FullName + @"\" + fileName + extension;
                ContextManager.loggerManager.info("NewFileName: " + newFileName);
                File.WriteAllBytes(newFileName, fileData);

                uploadDocument.udFilePath = newFileName;
                uploadDocument.udRelativePath = @"\" + ContextManager.session.CompanyId + @"\" + dayString + @"\" + fileName + extension;

                uploadDocument.udUrl = hostDomain + "/" + documentFolder + uploadDocument.udRelativePath.Replace(@"\", "/");

                UploadedDocumentInfo result = await saveDocumentData(uploadDocument);
                if (result.status == 1)
                {
                    ContextManager.loggerManager.info("Document saved successfully");
                    return result;
                }
                else
                {
                    ContextManager.loggerManager.info("Document data saving failed!");
                    File.Delete(newFileName);
                    throw new Exception("Document data not saved!");
                }
            }
            return null;
        }
        
        #region internal functions
        private async Task<UploadedDocumentInfo> saveDocument(List<string> allowedExtensions, string hostDomain, string documentFolder, UploadDocument uploadDocument, string path, int maxFileSize = 3000)
        {
            var originalFileName = GetDeserializedFileName(result.FileData.First());
            uploadDocument.originalFileName = originalFileName;
            ContextManager.loggerManager.info("originalFileName: " + originalFileName);
            var uploadedFileInfo = new FileInfo(result.FileData.First().LocalFileName);
            string extension = Path.GetExtension(originalFileName);
            if (uploadedFileInfo.Length > 1000 * maxFileSize)
            {
                ContextManager.loggerManager.info("File size is too big => " + uploadedFileInfo.Length);
                File.Delete(path);
                throw new Exception("File size is too big!");
            }


            if (allowedExtensions.Contains(extension.ToLower()))
            {
                DateTime dateTime = DateTime.Now;
                string dayString = dateTime.ToString("yyyyMMdd");
                string directoryName = uploadedFileInfo.DirectoryName + @"\" + ContextManager.session.CompanyId + @"\" + dayString;

                DirectoryInfo directory = new DirectoryInfo(directoryName);
                if (!directory.Exists) directory = Directory.CreateDirectory(directoryName);

                string fileName = Guid.NewGuid().ToString();
                uploadDocument.guid = fileName;

                string newFileName = directory.FullName + @"\" + fileName + extension;
                ContextManager.loggerManager.info("NewFileName: " + newFileName);
                File.Move(path, newFileName);

                uploadDocument.udFilePath = newFileName;
                uploadDocument.udRelativePath = @"\" + ContextManager.session.CompanyId + @"\" + dayString + @"\" + fileName + extension;

                uploadDocument.udUrl = hostDomain + "/" + documentFolder + uploadDocument.udRelativePath.Replace(@"\", "/");
                
                UploadedDocumentInfo result = await saveDocumentData(uploadDocument);
                if (result.status == 1)
                {
                    ContextManager.loggerManager.info("Document saved successfully");
                    return result;
                }
                else
                {
                    ContextManager.loggerManager.info("Document data saving failed!");
                    File.Delete(newFileName);
                    throw new Exception("Document data not saved!");
                }
            }
            else
            {
                ContextManager.loggerManager.info("Extension " + extension + " not supported, deleting file!");
                File.Delete(path);
                throw new Exception("File extension not supported!");
            }
        }

        private List<string> getAllowedExtensions()
        {
            List<string> allowedExtensions = new List<string>();
            allowedExtensions.AddRange(new List<string>() { ".png", ".jpg", ".jpeg", ".pdf", ".xlsx", ".docx" });
            return allowedExtensions;
        }

        private UploadDocument? getDocumentInfo(MultipartFormDataStreamProvider provider)
        {
            UploadDocument? uploadDocument = null;
            foreach (var key in provider.FormData.AllKeys)
            {
                foreach (var val in provider.FormData.GetValues(key))
                {
                    ContextManager.loggerManager.info(string.Format("ProviderValue => {0}: {1}", key, val));
                    if (key.Equals("document"))
                    {
                        uploadDocument = JsonConvert.DeserializeObject<UploadDocument>(val);
                        ContextManager.loggerManager.info("Document found: entId: " + uploadDocument.udEntId + ", description: " + uploadDocument.udDescription);
                    }
                }
            }

            return uploadDocument;
        }

        private async Task<UploadedDocumentInfo> saveDocumentData(UploadDocument uploadDocument)
        {
            UploadedDocumentInfo result = null;
            bool usingDedicatedTransaction = true;
            if (contextManager.dbManager.GetTransaction() != null) usingDedicatedTransaction = false;
            if(usingDedicatedTransaction) contextManager.dbManager.BeginTransaction(this);
            try
            { 
                result = await documentRepository.UploadDocument(uploadDocument);
                if (result.status == 1)
                {
                    contextManager.loggerManager.info("Commit transaction!");
                    if(usingDedicatedTransaction) contextManager.dbManager.Commit(this);
                }
                else
                {
                    contextManager.loggerManager.info("Rollback transaction!");
                    if(usingDedicatedTransaction) contextManager.dbManager.Rollback(this);
                }
            }
            catch (Exception ex)
            {

                contextManager.loggerManager.error(ex);
                if(usingDedicatedTransaction) contextManager.dbManager.Rollback(this);

            }
            return result;
        }

        private string GetDeserializedFileName(MultipartFileData fileData)
        {
            var fileName = GetFileName(fileData);
            return JsonConvert.DeserializeObject(fileName).ToString();
        }

        public string GetFileName(MultipartFileData fileData)
        {
            return fileData.Headers.ContentDisposition.FileName;
        }

        public async Task<bool> checkIfDocumentCanBeAdded(UploadDocument uploadDocument)
        {
            return await documentRepository.CheckIfCanAddDocument(uploadDocument);
        }
        #endregion
    }
}
