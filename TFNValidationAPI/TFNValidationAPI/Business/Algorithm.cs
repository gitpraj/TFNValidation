using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFNValidationAPI.Interface;

namespace TFNValidationAPI.Business
{
    public class Algorithm : IAlgorithm
    {
        private readonly IConfiguration _config;
        public Algorithm(IConfiguration config)
        {
            _config = config;
        }

        /* Validate() - validate the TFN
         * Inputs: String numberStr - the tfn number in string 
         */
        public async Task<Response> Validate(string numberStr)
        {
            //await Task.FromResult(true);
            /* TFN to be of length 8 or 9 */
            try
            {
                if (numberStr.Length < 8 || numberStr.Length > 9)
                {
                    return new Response(0,"TFN must be of length 8 or 9");
                }
                else
                {
                    int ret = await Evaluate(numberStr, numberStr.Length);
                    if (ret > 0)
                        return new Response(ret, "Valid TFN");
                    else
                        return new Response(ret, "Invalid TFN");
                }
            }
            catch (Exception ex) {
                return new Response(-1, ex?.Message);
            }
        }

        /* Evaluate() - algorithm to validate the TFN
         * Inputs: String numberStr - the tfn number in string 
                   int len - length of the tfn number 
         */
        public async Task<int> Evaluate(string numberStr, int len)
        {
            try
            {
                int[] nineDigitWeighFactor = _config.GetSection("AppSettings:NineDigitWeighFactor")?.Get<int[]>() ?? new int[0];
                int[] eightDigitWeighFactor = _config.GetSection("AppSettings:EightDigitWeighFactor")?.Get<int[]>() ?? new int[0];
                int[] weightFactor = new int[] { };
                if (len == 8)
                {
                    weightFactor = eightDigitWeighFactor;
                }
                else if (len == 9)
                {
                    weightFactor = nineDigitWeighFactor;
                }

                int sum = 0;
                for (int i = 0; i < numberStr.Length; i++)
                {
                    int part1 = int.Parse(numberStr[i].ToString());
                    int part2 = weightFactor[i];
                    sum = await Task.FromResult(sum + part1 * part2);
                }

                int rem = sum % 11;

                if (rem == 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
