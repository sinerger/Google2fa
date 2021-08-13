using API.JWT;
using Google.Authenticator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TFAController : ControllerBase
    {
        //SuperSecretKeyGoesHere
        //f669ecd0-6b82-4f8b-b037-c77640d024ee
        private string secredKey = "SuperSecretKeyGoesHere";
        private ISessionService _sessionService;

        public TFAController()
        {
            _sessionService = new SessionService();
        }

        [HttpPost]
        public async Task<IActionResult> Registration()
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            SetupCode setupInfo = tfa.GenerateSetupCode("Test2FA", "user@example.com", secredKey, false, 3);

            string qrCodeImageUrl = setupInfo.QrCodeSetupImageUrl;
            string manualEntrySetupCode = setupInfo.ManualEntryKey;

            return Ok(qrCodeImageUrl);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetToken(Guid leadID)
        {
            var token = await _sessionService.CreateAuthTokenAsync(leadID);

            return Ok(token);
        }

        [HttpGet("pin")]
        [AllowAnonymous]
        //[ServiceFilter(typeof(TwoFactorAuthorizeAttribute))]
        [TypeFilter(typeof(TwoFactorAuthorizeAttribute))]
        //[TwoFactorAuthorize]
        public async Task<IActionResult> ConfirmPin()
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            TimeSpan ts = new TimeSpan(0,0,1);
            bool isCorrectPIN = tfa.ValidateTwoFactorPIN(secredKey, "252525", ts);

            return Ok(isCorrectPIN);
        }
    }
}
