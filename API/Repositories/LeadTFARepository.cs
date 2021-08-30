using API.Extensions;
using Insight.Database;
using System;
using System.Data;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class LeadTFARepository : ILeadTFARepository
    {
        private readonly ILeadTFARepository _leadTFAKeyRepository;
        public IDbConnection DBConnection { get; }

        public LeadTFARepository(IDbConnection dbConnection)
        {
            DBConnection = dbConnection;
            _leadTFAKeyRepository = DBConnection.As<ILeadTFARepository>();
        }

        public async Task AddTFAKeyToLeadAsync(Guid leadID, string TFAKey)
        {
            if (leadID != Guid.Empty && TFAKey != null)
            {
                await DBConnection.QueryAsync(nameof(AddTFAKeyToLeadAsync).GetStoredProcedureName(), new { leadID, TFAKey });
            }
            else if (leadID == Guid.Empty)
            {
                throw new ArgumentException("Guid leadID is empty");
            }

            throw new ArgumentNullException("String key is null");
        }

        public async Task<string> GetTFAKeyByLeadIDAsync(Guid leadID)
        {
            if (leadID != Guid.Empty)
            {
                var key = await _leadTFAKeyRepository.GetTFAKeyByLeadIDAsync(leadID);

                return key;
            }

            throw new ArgumentException("Guid leadID is empty");
        }

        public async Task<bool> GetIsExistTFAByLeadIDAsync(Guid leadID)
        {
            if (leadID != Guid.Empty)
            {
                var isExist = await _leadTFAKeyRepository.GetIsExistTFAByLeadIDAsync(leadID);

                return isExist;
            }

            throw new ArgumentException("Guid leadID is empty");
        }

        public async Task SetExistTFAByLeadIDAsync(Guid leadID)
        {
            if (leadID != Guid.Empty)
            {
                await _leadTFAKeyRepository.SetExistTFAByLeadIDAsync(leadID);
            }
            else
            {
                throw new ArgumentException("Guid leadID is empty");
            }
        }
    }
}
