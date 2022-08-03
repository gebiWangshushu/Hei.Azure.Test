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

        /// <summary>
        /// 当启用beta版本的时候接口有效
        /// </summary>
        /// <returns></returns>
        [FeatureGate(MyFeatureFlags.Beta)]
        [HttpGet]
        public async Task<IActionResult> Beta()
        {
            var beta = await _featureManager.IsEnabledAsync(nameof(MyFeatureFlags.Beta));
            return Success("Beta", beta);
        }

        /// <summary>
        /// 当启用v1版本的时候接口有效
        /// </summary>
        /// <returns></returns>
        [FeatureGate(MyFeatureFlags.V1)]
        [HttpGet]
        public async Task<IActionResult> V1()
        {
            return Success("V1");
        }

        /// <summary>
        /// 当启用v2版本的时候接口有效
        /// </summary>
        /// <returns></returns>
        [FeatureGate(MyFeatureFlags.V2)]
        [HttpGet]
        public async Task<IActionResult> V2()
        {
            return Success("V2");
        }

        /// <summary>
        /// 启用百分率的功能开关
        /// </summary>
        /// <returns></returns>
        [FeatureGate(MyFeatureFlags.PercentageFlag)]
        [HttpGet]
        public async Task<IActionResult> PercentageFlag()
        {
            return Success("PercentageFlag");
        }

        /// <summary>
        /// 启用时间窗口的功能开关
        /// </summary>
        /// <returns></returns>
        [FeatureGate(MyFeatureFlags.TimeWindowFlag)]
        [HttpGet]
        public async Task<IActionResult> TimeWindowFlag()
        {
            return Success("TimeWindowFlag");
        }

        /// <summary>
        /// 自定义功能开关
        /// </summary>
        /// <returns></returns>
        [FeatureGate(MyFeatureFlags.CustomFeatureFlag)]
        [HttpGet]
        public async Task<IActionResult> CustomFeatureFlag(string platform)
        {
            return Success("CustomFeatureFlag");
        }
    }
}