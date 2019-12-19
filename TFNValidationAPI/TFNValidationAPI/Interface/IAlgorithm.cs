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
        /// Methods for validating TFN.
        /// </summary>
        Task<Response> Validate(string number);
        Task<int> Evaluate(string numberStr, int len);
        bool CheckForLinkedAttempt(string numberStr);
        bool TfnLinkedMethod(string newTfn, string prevTfn);
    }
}
