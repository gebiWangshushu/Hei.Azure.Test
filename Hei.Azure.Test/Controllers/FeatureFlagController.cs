using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using Passport.Infrastructure;

namespace Hei.Azure.Test.Controllers
{
    [Route("api/azure/featureflag/[action]")]
    public class FeatureFlagController : PassportApiController
    {
        private readonly IFeatureManager _featureManager;

        public FeatureFlagController(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        [FeatureGate(MyFeatureFlags.Beta)]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}