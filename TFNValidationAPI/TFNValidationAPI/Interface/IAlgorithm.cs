using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFNValidationAPI.Business;

namespace TFNValidationAPI.Interface
{
    public interface IAlgorithm
    {
        /// <summary>
        /// Returns true/false depending on the validation algorithm.
        /// </summary>
        Task<Response> Validate(string number);
    }
}
