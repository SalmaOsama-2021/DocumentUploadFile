
using DocumentUploadFile.DataAccess;
using DocumentUploadFile.DataAccess.Repository_Interface.Documents;
using DocumentUploadFile.Models.Api.Response;
using DocumentUploadFile.Models.Domain;
using HRMS.Models.API.Request.General;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentUploadFile.DataAccess.Repository.DocumentsRepository
{
    public class DocumentRepository: IDocumentRepository
    {
        private readonly ApplicationDbContext context;
        private readonly  IFileUploadHelper _fileUploadHelper;
        public DocumentRepository(ApplicationDbContext db) 
        {
            this.context = db;
        }

        public async Task<int> AddDocument(documentsDto request)
        {
            try
            {
                var listfromDocument = new List<Document_files>();

                if (request.document_files != null && request.document_files.Count() > 0)
                {
                    request.document_files.ForEach(async x =>
                    {
                        listfromDocument.Add(new Document_files
                        {
                           
                            isDeleted = false,
                            isEnabled = true,
                            Document_ID = x.Document_ID,
                            File_path = x.fileSource != null ? await UploadFiles(Base64ToImage(x), "Attachment/Documnts/", 0) : "",



                        });
                    });

                }
                var allDocuments = new documents
                {
                   isEnabled = true,
                    isDeleted = false,
                   Due_date=request.due_date,
                   Created=request.created,
                   Date=request.date,
                   Name=request .name,
                   PriorityId=request.priorityId,
                    Document_files = listfromDocument
                };

                await context.documents.AddAsync(allDocuments);
                await context.SaveChangesAsync();
                return allDocuments.ID;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private IFormFile Base64ToImage(Document_filesDto attach)
        {
            byte[] bytes = Convert.FromBase64String(attach.File_path.Substring(attach.File_path.LastIndexOf(',') + 1));
            MemoryStream stream = new MemoryStream(bytes);
            IFormFile file = new FormFile(stream, 0, bytes.Length, attach.fileName, attach.fileName);
            return file;
        }
        async Task<string> UploadFiles(IFormFile formFile, string path, int userId)
        {
            var imagePath = await _fileUploadHelper.Upload(formFile, path + userId.ToString());
            return imagePath;
        }
        public async Task<int> deleteDocument(int DocumentId)
        {
            try
            {
                var Documents = await context.documents.FindAsync(DocumentId);
                Documents.isDeleted = true;
                await context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<documents>> getDocumentDatatable(int documentId)
        {
            var documents = await context.documents.Include(x => x.Document_files.Where(i => !i.isDeleted.Value))
                .Where(x => x.isDeleted != true ).OrderByDescending(x => x.ID)
                .ToListAsync();
   
            return documents;
        }

        public async Task<documents> getDocumentById(int documentId)
        {
            var documents = await context.documents.Include(x => x.Document_files.Where(i => !i.isDeleted.Value)).Where(x => x.ID ==documentId
            && !x.isDeleted.Value).FirstOrDefaultAsync();

            return documents;
        }

        public async Task<int> UpdateDocument(documentsDto request)
        {
            try
            {
                var AllDocument = await context.documents.Where(x => x.ID == request.iD).Include(x => x.Document_files).FirstOrDefaultAsync();
                AllDocument.Name = request.name;
                AllDocument.Due_date = request.due_date;
                AllDocument.Date = request.date;
                AllDocument.Created = request.created;
                AllDocument.Due_date = request.due_date;
                AllDocument.PriorityId = request.priorityId;


                foreach (var item in request.document_files)
                {
                    var Document_files = await context.Document_files.Where(x => x.ID == item.ID).FirstOrDefaultAsync();
                    if (item.ID != 0)
                    {
                        // update 
                        Document_files.File_path = item.fileSource != null ? await UploadFiles(Base64ToImage(item), "Attachment/Documnts/", 0) : "";
                        Document_files.Document_ID = item.Document_ID;
                        Document_files.ID = item.ID;

                    }
                    else
                    {
                        // add
                        var Document_filesobj = new Document_files { File_path = item.fileSource != null ? await UploadFiles(Base64ToImage(item), "Attachment/Documnts/", 0) : "", Document_ID = item.Document_ID,isEnabled = true,ID=item.ID };
                        await context.Document_files.AddRangeAsync(Document_filesobj);
                    }
                }
                await context.SaveChangesAsync();


                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<Priority>> getAllPriority()
        {
            var Priorities = await context.Priority
                .Where(x => x.isDeleted != true)
                .ToListAsync();

            return Priorities;
        }

       
    }
}
