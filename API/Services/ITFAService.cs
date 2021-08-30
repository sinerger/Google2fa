using API.TFA;
using System;
using System.Threading.Tasks;

namespace API.Services
{
    public interface ITFAService
    {
        Task<bool> IsLeadTFAExistAsync(Guid leadID);
        Task<TFAModel> GetTFAModelAsync(Guid leadID);
        Task<bool> ConfirmPinAsync(Guid leadID, string pin);
        Task<bool> ConfirmConnectTFAToLeadAsync(Guid leadID, string pin);
    }
}
