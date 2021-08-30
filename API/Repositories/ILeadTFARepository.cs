using System;
using System.Threading.Tasks;

namespace API.Repositories
{
    public interface ILeadTFARepository
    {
        Task AddTFAKeyToLeadAsync(Guid leadID, string key);
        Task<string> GetTFAKeyByLeadIDAsync(Guid leadID);
        Task<bool> GetIsExistTFAByLeadIDAsync(Guid leadID);
        Task SetExistTFAByLeadIDAsync(Guid leadID);
    }
}
