﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TFNValidationAPI.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace TFNValidationAPI.Business
{
    public class Algorithm : IAlgorithm
    {
        private readonly IMemoryCache _cache;
        private readonly IGlobalSettings _settings;
        private readonly LinkStrategy _linkStrategy;
        public Algorithm(IMemoryCache cache, IGlobalSettings settings)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _linkStrategy = new LinkStrategy(cache, settings);
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
                
                bool isLinked = _linkStrategy.CheckForLinkedAttempt(numberStr);
                if (isLinked)
                    return new Response(100, "Sorry! Looks like you are trying to guess the algorithm.");
                int ret = await Evaluate(numberStr, numberStr.Length);

                if (ret > 0)
                    return new Response(ret, "Valid TFN");
                else
                    return new Response(ret, "Invalid TFN");
                
            }
            catch (Exception ex) {
                return new Response(-1, ex.Message);
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
                
                int[] weightFactor;
                if (len == 8)
                {
                    weightFactor = eightDigitWeighFactor;
                }
                else if (len == 9)
                {
                    weightFactor = nineDigitWeighFactor;
                }
                else
                {
                    return 0;
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

    public class LinkStrategy
    {
        private readonly IMemoryCache _cache;
        private readonly IGlobalSettings _settings;
        private readonly Utils _utils;
        public LinkStrategy(IMemoryCache cache, IGlobalSettings settings)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _utils = new Utils();
        }

        /* CheckForLinkedAttempt() - Checks if there are links between tfn attempts
        * Inputs: String numberStr - the tfn number in string
        */
        public bool CheckForLinkedAttempt(string numberStr)
        {
            try
            {
                bool isTfnLinked = false;
                string prevTfn = _cache.Get("tfn")?.ToString();
                string getPrevAttemptedTfn = _cache.Get("datetime")?.ToString();

                /* do not check the first tfn validation attempt */
                if (prevTfn != null)
                {
                    isTfnLinked = _utils.TfnLinkedMethod(numberStr, prevTfn);
                    int currentCountInCache = Convert.ToInt32(_cache.Get("linkedCount") ?? 0);
                    if (isTfnLinked)
                        _cache.Set("linkedCount", currentCountInCache + 1);
                }

                _cache.Set("tfn", numberStr);

                // set datetime in cache only when its the first attempt or tfn are not linked
                if (!isTfnLinked || prevTfn == null)
                    _cache.Set("datetime", DateTime.Now);

                int count = Convert.ToInt32(_cache.Get("linkedCount") ?? 0);

                // if the linked count is >=2 check the diff between the datetime at first attempt and now. 
                // if less than 30 seconds, the api should an appropriate message
                if (count >= 2)
                {
                    DateTime now = DateTime.Now;
                    DateTime prevTime = DateTime.Parse(getPrevAttemptedTfn);

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
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class Utils
    {
        public Utils()
        {

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

            return substrings.Any(newTfn.Contains);
        }
    }
}
