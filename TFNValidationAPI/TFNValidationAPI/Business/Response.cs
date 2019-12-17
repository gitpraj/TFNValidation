using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFNValidationAPI.Business
{
    public class Response
    {
        public int status;
        public string message;

        public Response(int status, string message)
        {
            this.status = status;
            this.message = message;
        }
    }
}
