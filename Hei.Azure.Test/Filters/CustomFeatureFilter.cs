using Microsoft.FeatureManagement;

namespace Hei.Azure.Test
{
    [FilterAlias("CustomFeature")]
    public class CustomFeatureFilter : IFeatureFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomFeatureFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            var platform = _httpContextAccessor.HttpContext.Request.Query["platform"].ToString();
            var allowPlatform = context.Parameters.Get<CustomFeatureFilterSettings>();
            return Task.FromResult(allowPlatform.AllowedPlatforms.Any(c => c == platform));
        }
    }

    public class CustomFeatureFilterSettings
    {
        public string[] AllowedPlatforms { get; set; }
    }
}