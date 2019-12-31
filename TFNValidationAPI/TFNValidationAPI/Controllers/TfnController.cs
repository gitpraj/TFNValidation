using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TFNValidationAPI.Business;
using TFNValidationAPI.Interface;

namespace TFNValidationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TfnController : ControllerBase
    {
        private readonly IAlgorithm _algorithm;
        public TfnController (IAlgorithm algorithm)
        {
            _algorithm = algorithm;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Response ret = await _algorithm.Validate(id);
            if (ret.Status >= 0)
            {
                return new JsonResult(new { message = ret.Message }) { StatusCode = 200 };
            }
            else 
            {
                return new JsonResult(new { message = ret.Message }) { StatusCode = 500 };
            }
        }
    }
}
