using Microsoft.Extensions.Caching.Memory;
using System;
using Microsoft.Extensions.Configuration;
using TFNValidationAPI.Business;
using TFNValidationAPI.Interface;
using Xunit;
using System.Threading.Tasks;

namespace TFNValidationAPITests
{
    public class TFNMockValidationTests
    {
        private IMemoryCache cache;
        private IConfiguration configuration;
        private IGlobalSettings settings;
        private IAlgorithm alg;

        public TFNMockValidationTests()
        {
            cache = new MemoryCache(new MemoryCacheOptions());

            configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            settings = GlobalSettings.Create(configuration);
            alg = new MockAlgorithm(configuration, cache, settings);
        }

        [InlineData("abc")]
        [InlineData("def")]
        [InlineData("praj")]
        [InlineData("lop")]
        [Theory]
        public async Task InvalidCharTFNs(string tfn)
        {
            Response ret = await alg.Validate(tfn);
            Assert.Equal(0, ret.status);
        }

        [InlineData("1234567")]
        [InlineData("1204567")]
        [InlineData("789456")]
        [InlineData("1234")]
        [Theory]
        public async Task InvalidNumberTFNs(string tfn)
        {
            Response ret = await alg.Validate(tfn);
            Assert.Equal(0, ret.status);
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
        public async Task ValidTFNs(string tfn)
        {
            Response ret = await alg.Validate(tfn);
            Assert.Equal(1, ret.status);
        }
    }
}
