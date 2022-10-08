using AutoMapper;
using DocumentUploadFile.DataAccess.Repository.ResponseBuilders;
using DocumentUploadFile.DataAccess.Repository_Interface.Documents;
using DocumentUploadFile.Models.Api.PublicClass;
using DocumentUploadFile.Models.Api.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace DocumentUploadFile.Controllers
{
    public class DocumentUploadFile : Controller
    {
        private readonly IDocumentRepository _documentRepository;
        protected readonly IMapper _mapper;
        public DocumentUploadFile(IMapper mapper, IDocumentRepository documentRepository)
        {
            _mapper = mapper;
            _documentRepository = documentRepository;
        }
        [HttpPost]
        [Route("DocumentFileUpload/getAllDocument")]
        public async Task<ResponseBuilder> getAllDocument(int DocumentId)
        {
            ResponseBuilder Response = ResponseBuilder.Create(HttpStatusCode.OK, new { status = true }, null);
            try
            {
           
                var DocumentListData = await _documentRepository.getDocumentDatatable(DocumentId);
               
                var resultdata = _mapper.Map<List<documentsDto>>(DocumentListData);
                Response = ResponseBuilder.Create(HttpStatusCode.OK, resultdata,"Success");

                return Response;
            }
            catch (Exception ex)
            {
                Response = ResponseBuilder.Create(HttpStatusCode.InternalServerError, null,"Faild");
                return Response;


            }
           
        }
        [HttpPost]
        [Route("DocumentFileUpload/getAllPriority")]
        public async Task<ResponseBuilder> getAllPriority(GeneralDto request)
        {
            ResponseBuilder Response = ResponseBuilder.Create(HttpStatusCode.OK, new { status = true }, null);
            try
            {

                var PriorityListData = await _documentRepository.getAllPriority();

                
                Response = ResponseBuilder.Create(HttpStatusCode.OK, PriorityListData, "Success");

                return Response;
            }
            catch (Exception ex)
            {
                Response = ResponseBuilder.Create(HttpStatusCode.InternalServerError, null, "Faild");
                return Response;


            }

        }
        [HttpPost]
        [Route("DocumentFileUpload/AddDocuments")]
        public async Task<ResponseBuilder> AddDocuments()
        {
            var request = new documentsDto();
            ResponseBuilder Response = ResponseBuilder.Create(HttpStatusCode.OK, new { status = true }, null);
            try
            {
                request = JsonConvert.DeserializeObject<documentsDto>(Request.Form["requestBody"]);
                if (request.iD != 0)
                {
                    var isUpdated = await _documentRepository.UpdateDocument(request);
                    if (isUpdated > 0)
                    {
                        Response = ResponseBuilder.Create(HttpStatusCode.OK, null, "Success");
                    }
                    else Response = ResponseBuilder.Create(HttpStatusCode.InternalServerError, null, "Failed");
                }
                else
                {
                    var isAdded = await _documentRepository.AddDocument(request);
                    if (isAdded > 0)
                    {
                        Response = ResponseBuilder.Create(HttpStatusCode.OK, null, "Success");
                    }
                    else Response = ResponseBuilder.Create(HttpStatusCode.InternalServerError, null, "Failed");
                }
                return Response;

            }      
            catch (Exception ex)
            {
                Response = ResponseBuilder.Create(HttpStatusCode.InternalServerError, null, "Faild");
                return Response;


            }

        }
        [HttpPost]
        [Route("DocumentFileUpload/getByDocumentId")]
        public async Task<ResponseBuilder> getByDocumentId()
        {
            var request = new GeneralDto();
            ResponseBuilder Response = ResponseBuilder.Create(HttpStatusCode.OK, new { status = true }, null);
            try
            {
                request = JsonConvert.DeserializeObject<GeneralDto>(Request.Form["requestBody"]);
                var DocumentData = await _documentRepository.getDocumentById(request.id);

                var resultdata = _mapper.Map<documentsDto>(DocumentData);
                Response = ResponseBuilder.Create(HttpStatusCode.OK, resultdata, "Success");

                return Response;
            }
            catch (Exception ex)
            {
                Response = ResponseBuilder.Create(HttpStatusCode.InternalServerError, null, "Faild");
                return Response;


            }

        }
        [HttpPost]
        [Route("DocumentFileUpload/deleteDocument")]
        public async Task<ResponseBuilder> deleteDocument(int DocumentId)
        {
            var request = new GeneralDto();
            ResponseBuilder Response = ResponseBuilder.Create(HttpStatusCode.OK, new { status = true }, null);
            try
            {
                request = JsonConvert.DeserializeObject<GeneralDto>(Request.Form["requestBody"]);

                var isDeleted = await _documentRepository.deleteDocument(request.id);

                if (isDeleted > 0)
                {
                    Response = ResponseBuilder.Create(HttpStatusCode.OK, null,"Success");
                }
                else
                {
                    Response = ResponseBuilder.Create(HttpStatusCode.InternalServerError, null,"Faild");
                }
                return Response;
            }
            catch (Exception ex)
            {
                Response = ResponseBuilder.Create(HttpStatusCode.InternalServerError, null, "Faild");
                return Response;


            }

        }
    }
}
