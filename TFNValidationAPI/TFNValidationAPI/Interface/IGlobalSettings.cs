using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFNValidationAPI.Interface
{
    public interface IGlobalSettings
    {
        int[] EightDigitWeighFactor { get; }
        int[] NineDigitWeighFactor { get; }
    }
}
