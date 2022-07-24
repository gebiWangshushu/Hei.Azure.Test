﻿using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Passport.Infrastructure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hei.Azure.Test.Controllers
{
    [Route("api/azure/config/[action]")]
    public class AzureConfigController : PassportApiController
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly Settings _settings;

        public AzureConfigController(IConfiguration configuration, IAzureStorageApi azureStorageApi, IOptionsSnapshot<Settings> settings)
        {
            _configuration = configuration;
            _settings = settings.Value;
        }

        /// <summary>
        /// 读取配置string
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(string key)
        {
            var result = _configuration[key];

            return Success("get config success", result);
        }

        /// <summary>
        /// 读取配置对象
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetObject(string key)
        {
            var result = _configuration.GetSection(key).Get<AzureStorageConfig>();

            return Success("get config success", result);
        }

        [HttpGet]
        public IActionResult GetSentinel()
        {
            return Success("get sentinel success", _settings);
        }
    }
}