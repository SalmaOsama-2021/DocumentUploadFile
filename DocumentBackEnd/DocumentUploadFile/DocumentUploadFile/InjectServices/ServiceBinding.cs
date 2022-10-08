



using DocumentUploadFile.DataAccess.Repository.DocumentsRepository;
using DocumentUploadFile.DataAccess.Repository_Interface.Documents;

namespace HRMS.InjectProviders
{
    public static class ServiceBinding
    {
        public static IServiceCollection InjectServices(this IServiceCollection services)
        {
            services.AddScoped<IDocumentRepository, DocumentRepository>();

            return services;
        }
    }
}
