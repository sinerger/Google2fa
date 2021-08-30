using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Repositories;
using API.TFA;
using Google.Authenticator;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class GoogleTFAService : ITFAService
    {
        private ILeadTFARepository _repository;
        private readonly TwoFactorAuthenticator _twoFactorAuthenticator;
        private readonly TFAConfig _tfaConfig;
        private readonly TimeSpan _timeDrift;

        public GoogleTFAService(ILeadTFARepository repository, IOptions<TFAConfig> option)
        {
            _repository = repository;
            _twoFactorAuthenticator = new TwoFactorAuthenticator();
            _tfaConfig = option.Value;
            _timeDrift = _tfaConfig.GetTimeDrift();
        }

        public async Task<bool> IsLeadTFAExistAsync(Guid leadID)
        {
            if (leadID != Guid.Empty)
            {
                var result = await _repository.GetIsExistTFAByLeadIDAsync(leadID);

                return result;
            }

            throw new ArgumentException("Guid leadID is empty");
        }

        public async Task<TFAModel> GetTFAModelAsync(Guid leadID)
        {
            if (leadID != Guid.Empty)
            {
                var key = await GetTFAKeyByLeadIDAsync(leadID);

                SetupCode setupInfo = _twoFactorAuthenticator
                    .GenerateSetupCode(_tfaConfig.ApplicationName, _tfaConfig.AccountTitle,
                        key, _tfaConfig.SecretISBase32, _tfaConfig.SizeQRCode);

                var tfaModel = new TFAModel()
                {
                    QRCodeBase64 = setupInfo.QrCodeSetupImageUrl,
                    ManualEntryKey = setupInfo.ManualEntryKey
                };

                tfaModel.QRCodeBase64 = tfaModel.QRCodeBase64.Replace("data:image/png;base64,", "");

                return tfaModel;
            }

            throw new ArgumentException("Guid leadID is empty");
        }

        public async Task<bool> ConfirmPinAsync(Guid leadID, string pin)
        {
            if (leadID != Guid.Empty && pin != null)
            {
                var key = await GetTFAKeyByLeadIDAsync(leadID);
                bool isCorrectPIN = _twoFactorAuthenticator.ValidateTwoFactorPIN(key, pin, _timeDrift);

                return isCorrectPIN;
            }
            else if (leadID == Guid.Empty)
            {
                throw new ArgumentException("Guid leadID is empty");
            }

            throw new ArgumentNullException("String pin is null");
        }

        public async Task<bool> ConfirmConnectTFAToLeadAsync(Guid leadID, string pin)
        {
            if (leadID != Guid.Empty && pin != null)
            {
                var result = false;

                if (await ConfirmPinAsync(leadID, pin))
                {
                    await _repository.SetExistTFAByLeadIDAsync(leadID);

                    result = true;
                }

                return result;
            }
            else if (leadID == Guid.Empty)
            {
                throw new ArgumentException("Guid leadID is empty");
            }

            throw new ArgumentNullException("String pin is null");
        }

        private async Task<string> GetTFAKeyByLeadIDAsync(Guid leadID)
        {
            if (leadID != Guid.Empty)
            {
                var key = await _repository.GetTFAKeyByLeadIDAsync(leadID);

                if (key == null || key == string.Empty)
                {
                    key = leadID + _tfaConfig.SecretString;
                    await AddTFAKeyToLeadAsync(leadID, key);
                }

                return key;
            }

            throw new ArgumentException("Guid leadID is empty");
        }

        private async Task AddTFAKeyToLeadAsync(Guid leadID, string key)
        {
            if (leadID != Guid.Empty && key != null)
            {
                await _repository.AddTFAKeyToLeadAsync(leadID, key);
            }
            else if (leadID == Guid.Empty)
            {
                throw new ArgumentException("Guid leadID is empty");
            }
            else
            {
                throw new ArgumentNullException("String key is null");
            }
        }
    }
}
