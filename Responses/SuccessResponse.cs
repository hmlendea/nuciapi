namespace NuciAPI.Responses
{
    public class SuccessResponse : Response
    {
        public override bool IsSuccess => true;

        public SuccessResponse() : base(SuccessResponseMessages.Default) { }

        public SuccessResponse(string message) : base(message) { }
    }
}
