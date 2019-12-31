using Microsoft.Extensions.Configuration;
using TFNValidationAPI.Interface;

namespace TFNValidationAPI.Business
{
    public class GlobalSettings : IGlobalSettings
    {
        public int[] EightDigitWeighFactor { get; private set; }
        public int[] NineDigitWeighFactor { get; private set; }

        public static GlobalSettings Create(IConfiguration configuration)
        {
            return new GlobalSettings
            {
                EightDigitWeighFactor = configuration.GetSection("AppSettings:EightDigitWeighFactor")?.Get<int[]>() ?? new int[0],
                NineDigitWeighFactor = configuration.GetSection("AppSettings:NineDigitWeighFactor")?.Get<int[]>() ?? new int[0]
            };
        }
    }
}
