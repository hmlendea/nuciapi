namespace NuciAPI.Responses
{
    public class SuccessResponse : Response
    {
        public override bool Success => true;

        public SuccessResponse() : base(SuccessResponseMessages.Default) { }

        public SuccessResponse(string message) : base(message) { }

        public static SuccessResponse FromMessage(string message) => new(message);

    }
}
