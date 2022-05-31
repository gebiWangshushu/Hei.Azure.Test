using Hei.Azure.Test.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Passport.Infrastructure;

namespace Hei.Azure.Test.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CosmosController : PassportApiController
    {
        private readonly UserContext _userContext;

        public CosmosController(UserContext userContext)
        {
            _userContext = userContext;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            _userContext.Set<UserModel>().Add(new UserModel
            {
                PartitionKey = "CI01",

                Name = "testname"
            }); ;

            var result = _userContext.SaveChanges();

            return Success("", result);
        }
    }
}