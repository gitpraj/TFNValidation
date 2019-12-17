using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
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
            Utils ut = new Utils(_algorithm);
            Response ret = await ut.RunAsync(id);
            if (ret.status >= 0)
            {
                //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                //return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                return new JsonResult(new { message = ret.message }) { StatusCode = 200 };
            }
         else 
            {
                return new JsonResult(new { message = ret.message }) { StatusCode = 500 };
                //return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
