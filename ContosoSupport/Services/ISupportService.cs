using ContosoSupport.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContosoSupport.Services
{
    public interface ISupportService
    {
        void CreateAsync(SupportCase supportCase);
        Task<IEnumerable<SupportCase>> GetAsync(int? pageNumber = 1);
        Task<SupportCase> GetAsync(string id);
        Task<long> GetDocumentCountAsync();
        void RemoveAsync(string id);
        void RemoveAsync(SupportCase supportCase);
        void UpdateAsync(string id, SupportCase supportCase);
    }
}