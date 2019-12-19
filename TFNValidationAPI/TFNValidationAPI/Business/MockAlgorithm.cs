using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFNValidationAPI.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace TFNValidationAPI.Business
{
    /* This MockAlgorithm class is just a mock algorithm which does not follow the actual algorithm. Mainly for testing purposes.  */
    public class MockAlgorithm : IAlgorithm
    {
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private readonly IGlobalSettings _settings;
        public MockAlgorithm(IConfiguration config, IMemoryCache cache, IGlobalSettings settings)
        {
            _config = config;
            _cache = cache;
            _settings = settings;
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
                if (numberStr.Length == 8 || numberStr.Length == 9)
                {
                    return new Response(0, "TFN must be of length 8 or 9");
                }
                else
                {
                    bool isLinked = CheckForLinkedAttempt(numberStr);
                    if (isLinked)
                        return new Response(100, "Sorry! Looks like you are trying to guess the algorithm.");
                    int ret = await Evaluate(numberStr, numberStr.Length);
                    if (ret > 0)
                        return new Response(ret, "Valid TFN");
                    else
                        return new Response(ret, "Invalid TFN");
                }
            }
            catch (Exception ex)
            {
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
                int[] eightDigitWeighFactor = _settings.EightDigitWeighFactor;
                int[] nineDigitWeighFactor = _settings.NineDigitWeighFactor;

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

        /* CheckForLinkedAttempt() - Checks if there are links between tfn attempts
         * Inputs: String numberStr - the tfn number in string
         */
        public bool CheckForLinkedAttempt(string numberStr)
        {
            bool isTfnLinked = false;
            string prevTfn = _cache.Get("tfn")?.ToString();
            string getPrevAttemptedTFN = _cache.Get("datetime")?.ToString();

            /* dont check the first tfn validation attempt */
            if (prevTfn != null)
            {
                isTfnLinked = TfnLinkedMethod(numberStr, prevTfn);
                int currentCountInCache = Convert.ToInt32(_cache.Get("linkedCount") ?? 0);
                if (isTfnLinked)
                    _cache.Set("linkedCount", currentCountInCache + 1);
            }

            _cache.Set("tfn", numberStr);

            // set datetime in cache only when its the first attempt or tfns are not linked
            if (!isTfnLinked || prevTfn == null)
                _cache.Set("datetime", DateTime.Now);

            int count = Convert.ToInt32(_cache.Get("linkedCount") ?? 0);

            // if the linked count is >=2 check the diff between the datetime at first attempt and now. 
            // if less than 30 seconds, the api should an appropriate message
            if (count >= 2)
            {
                DateTime now = DateTime.Now;
                DateTime prevTime = DateTime.Parse(getPrevAttemptedTFN);

                _cache.Remove("datetime");
                _cache.Remove("linkedCount");
                _cache.Remove("tfn");

                if ((now - prevTime).TotalSeconds <= 30)
                {
                    /* LINKED */
                    return true;
                }
            }
            return false;
        }


        /* TfnLinkedMethod() - Checks if there are links between the old tfn and the new tfn
         * Inputs: String newTfn - new tfn
         *         String prevTfn - prev tfn
         */
        public bool TfnLinkedMethod(string newTfn, string prevTfn)
        {
            //Create a collection of all of the 4-character substrings
            List<string> substrings = new List<string>();
            for (int i = 0; i < prevTfn.Length - 3; i++)
            {
                //Adds the 4-character substring
                substrings.Add(prevTfn.Substring(i, 4));
            }

            return substrings.Any(s => newTfn.Contains(s));
        }
    }
}
