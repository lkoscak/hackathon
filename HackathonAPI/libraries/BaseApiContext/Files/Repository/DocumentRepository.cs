using BaseApiContext.Files.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Core.Context;
using WebApi.Core.Repository;

namespace BaseApiContext.Files.Repository
{
    public class DocumentRepository : BaseRepository, IDocumentRepository
    {
        IContextManager contextManager;
        public DocumentRepository(IContextManager contextManager) : base(contextManager)
        {
            this.contextManager = contextManager;
        }
        
        public async Task<bool> CheckIfCanAddDocument(UploadDocument uploadDocument)
        {
            int documentCount = -1;
            try
            {
                using(SqlDataReader data = await DynamicQuery.GetDataAsync(Connection, Transaction, contextManager.sessionKey, 5504, uploadDocument.udRefId, uploadDocument.udType))
                {
                    if (data.HasRows)
                    {
                        while (data.Read())
                        {
                            if (data["document_count"] != DBNull.Value)
                            {
                                documentCount = Convert.ToInt32(data["document_count"]);
                            }
                        }
                    }
                }
                contextManager.loggerManager.info($"Document check for {uploadDocument.udRefId} and {uploadDocument.udType} returned {documentCount}");
                if (documentCount < 0) return false;
                if (uploadDocument.udType == (int)UploadDocumentType.SIGNED_TRAVEL_ORDER && documentCount >= 1) return false;
                if (uploadDocument.udType == (int)UploadDocumentType.TRAVEL_ORDER_ATTACHMENT && documentCount >= 5) return false;
                return true;
            }
            catch (Exception ex)
            {
                contextManager.loggerManager.error(ex);
                return false;
            }
            
        }
        
        public async Task<UploadedDocumentInfo> UploadDocument(UploadDocument uploadDocument)
        {
            UploadedDocumentInfo result = new UploadedDocumentInfo();

            string mailTo = null;
            string mailSubject = null;
            string mailContent = null;

            try
            {
                ContextManager.loggerManager.info("Uploading a document");

                if (uploadDocument.folderId > 0)
                {
                    ContextManager.loggerManager.info(uploadDocument.udUrl);

                    using (SqlCommand sqlCommand = new SqlCommand("dbo.Upload_document_file", Connection, Transaction))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add("@guid", SqlDbType.VarChar).Value = uploadDocument.guid;
                        sqlCommand.Parameters.Add("@fileName", SqlDbType.NVarChar).Value = uploadDocument.originalFileName.Substring(0, uploadDocument.originalFileName.LastIndexOf("."));
                        sqlCommand.Parameters.Add("@fileSize", SqlDbType.Int).Value = uploadDocument.fileSize;
                        sqlCommand.Parameters.Add("@fileExtension", SqlDbType.VarChar).Value = uploadDocument.fileExtension;
                        sqlCommand.Parameters.Add("@filePath", SqlDbType.NVarChar).Value = uploadDocument.udUrl;
                        sqlCommand.Parameters.Add("@creatorId", SqlDbType.Int).Value = ContextManager.session.UserId;
                        sqlCommand.Parameters.Add("@userId", SqlDbType.Int).Value = uploadDocument.usrId;
                        sqlCommand.Parameters.Add("@folderId", SqlDbType.Int).Value = uploadDocument.folderId;


                        using (SqlDataReader Data = await sqlCommand.ExecuteReaderAsync())
                        {
                            if (Data.HasRows)
                            {
                                if (await Data.ReadAsync())
                                {
                                    if (Data["fil_id"] != DBNull.Value) result.dbId = Convert.ToInt32(Data["fil_id"].ToString());
                                }
                            }
                        }
                    }
                }
                else
                {
                    using (SqlCommand sqlCommand = new SqlCommand("dbo.Upload_document", Connection, Transaction))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add("@comId", SqlDbType.Int).Value = uploadDocument.udComId;
                        sqlCommand.Parameters.Add("@entId", SqlDbType.Int).Value = uploadDocument.udEntId;
                        sqlCommand.Parameters.Add("@refId", SqlDbType.Int).Value = uploadDocument.udRefId;
                        sqlCommand.Parameters.Add("@typeId", SqlDbType.Int).Value = uploadDocument.udType;
                        sqlCommand.Parameters.Add("@filePath", SqlDbType.NVarChar).Value = uploadDocument.udFilePath;
                        sqlCommand.Parameters.Add("@relativePath", SqlDbType.NVarChar).Value = uploadDocument.udRelativePath;
                        sqlCommand.Parameters.Add("@description", SqlDbType.NVarChar).Value = uploadDocument.udDescription;
                        sqlCommand.Parameters.Add("@creatorId", SqlDbType.Int).Value = uploadDocument.udCreatorId;
                        sqlCommand.Parameters.Add("@created", SqlDbType.BigInt).Value = uploadDocument.udCreated;
                        sqlCommand.Parameters.Add("@hostDomain", SqlDbType.NVarChar).Value = contextManager.configManager.getFleetWebBaseURL();
                        sqlCommand.Parameters.Add("@originalFileName", SqlDbType.NVarChar).Value = uploadDocument.originalFileName;
                        sqlCommand.Parameters.Add("@url", SqlDbType.NVarChar).Value = uploadDocument.udUrl;

                        using (SqlDataReader Data = await sqlCommand.ExecuteReaderAsync())
                        {
                            if (Data.HasRows)
                            {
                                if (await Data.ReadAsync())
                                {
                                    if (Data["ud_id"] != DBNull.Value) result.dbId = Convert.ToInt32(Data["ud_id"].ToString());
                                    if (Data["forward_mails"] != DBNull.Value) mailTo = Data["forward_mails"].ToString();
                                    if (Data["mail_subject"] != DBNull.Value) mailSubject = Data["mail_subject"].ToString();
                                    if (Data["mail_content"] != DBNull.Value) mailContent = Data["mail_content"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ContextManager.loggerManager.error(e);
            }
            result.status = result.dbId > 0 ? 1 : 0;
            result.filePath = uploadDocument.udFilePath;
            //TODO after extract send mail logic to library
            //if (result.status != 0 && mailTo != null && mailTo != "" && mailSubject != null && mailContent != null)
            //{
            //    await SendMailWithAttachement(uploadDocument, mailTo, mailSubject, mailContent);
            //}

            return result;

        }
    }
}
