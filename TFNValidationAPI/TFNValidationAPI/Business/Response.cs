
namespace TFNValidationAPI.Business
{
    public class Response
    {
        public int Status;
        public string Message;

        public Response(int status, string message)
        {
            this.Status = status;
            this.Message = message;
        }
    }
}
