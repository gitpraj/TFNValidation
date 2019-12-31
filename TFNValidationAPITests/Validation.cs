using Microsoft.Extensions.Caching.Memory;
using System;
using Microsoft.Extensions.Configuration;
using TFNValidationAPI.Business;
using TFNValidationAPI.Interface;
using Xunit;
using System.Threading.Tasks;

namespace TFNValidationAPITests
{
    public class TFNValidationTests
    {
        private IMemoryCache cache;
        private IConfiguration configuration;
        private IGlobalSettings settings;
        private IAlgorithm alg;
        private Utils utils;

        public TFNValidationTests()
        {
            cache = new MemoryCache(new MemoryCacheOptions());

            configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            settings =  GlobalSettings.Create(configuration);
            utils = new Utils();
            alg = new Algorithm(cache, settings);
        }

        [InlineData("abc")]
        [InlineData("def")]
        [InlineData("praj")]
        [InlineData("lop")]
        [Theory]
        public async Task InvalidCharTfns(string tfn)
        {
            Response ret = await alg.Validate(tfn);
            Assert.Equal(0, ret.Status);
        }

        [InlineData("1234567")]
        [InlineData("1204567")]
        [InlineData("789456")]
        [InlineData("1234")]
        [Theory]
        public async Task InvalidNumberTfns(string tfn)
        {
            Response ret = await alg.Validate(tfn);
            Assert.Equal(0, ret.Status);
        }

        [InlineData("648188480")]
        [InlineData("648188499")]
        [InlineData("648188519")]
        [InlineData("648188527")]
        [InlineData("648188535")]
        [InlineData("37118629")]
        [InlineData("37118660")]
        [InlineData("37118705")]
        [InlineData("38593474")]
        [InlineData("38593519")]
        [InlineData("85655734")]
        [InlineData("85655797")]
        [InlineData("85655810")]
        [InlineData("37118655")]
        [InlineData("37118676")]
        [InlineData("38593469")]
        [InlineData("38593503")]
        [InlineData("38593524")]
        [InlineData("85655755")]
        [InlineData("85655805")]
        [Theory]
        public async Task ValidTfns(string tfn)
        {
            Response ret = await alg.Validate(tfn);
            Assert.Equal(1, ret.Status);
        }

        [InlineData("1234567", 7)]
        [InlineData("1204567", 7)]
        [InlineData("789456", 6)]
        [InlineData("1234", 4)]
        [Theory]
        public async Task InvalidValuation(string tfn, int len)
        {
            //Response ret = await alg.Validate(tfn);
            int ret = await alg.Evaluate(tfn, len);
            Assert.Equal(0, ret);
        }

        [InlineData("648188480",9)]
        [InlineData("648188499",9)]
        [InlineData("648188519",9)]
        [InlineData("648188527",9)]
        [InlineData("648188535",9)]
        [Theory]
        public async Task ValidValuation(string tfn, int len)
        {
            //Response ret = await alg.Validate(tfn);
            int ret = await alg.Evaluate(tfn, len);
            Assert.Equal(1, ret);
        }

        [InlineData("648188535", "648188534")]
        [InlineData("123456789", "123456781")]
        [InlineData("54678911", "12348911")]
        [Theory]
        public void ValidTfnLinkedMethod(string oldTfn, string newTfn)
        {
            bool ret = utils.TfnLinkedMethod(newTfn, oldTfn);
            Assert.True(ret);
        }

        [InlineData("648188535", "123456789")]
        [InlineData("12345678", "12356902")]
        [InlineData("34567891", "90871234")]
        [Theory]
        public void InvalidTfnLinkedMethod(string oldTfn, string newTfn)
        {
            bool ret = utils.TfnLinkedMethod(newTfn, oldTfn);
            Assert.False(ret);
        }
    }
}
