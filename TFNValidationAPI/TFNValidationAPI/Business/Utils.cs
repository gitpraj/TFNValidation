using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFNValidationAPI.Interface;

namespace TFNValidationAPI.Business
{
    public class Utils
    {
        private readonly IAlgorithm _algorithm;
        public Utils(
            IAlgorithm algorithm)
        {
            _algorithm = algorithm;
        }

        /* RunAsync() - to run the validation asynchronously */
        public async Task<Response> RunAsync(string id)
        {
            return await _algorithm.Validate(id);
        }
    }
}
