using Google.Authenticator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Google2fa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TFAController : Controller
    {
        [HttpPost]
        public async Task<string> Registration()
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            var setupInfo = tfa.GenerateSetupCode("MyApp", "user@example.com", "SuperSecretKeyGoesHere", true, 300);

            string qrCodeImageUrl = setupInfo.QrCodeSetupImageUrl;
            string manualEntrySetupCode = setupInfo.ManualEntryKey;

            return manualEntrySetupCode;
        }
    }
}
