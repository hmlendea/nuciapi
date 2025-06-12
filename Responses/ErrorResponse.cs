using System;

namespace NuciAPI.Responses
{
    public sealed class ErrorResponse : Response
    {
        public override bool Success => false;

        public ErrorResponse() : base(ErrorResponseMessages.Default) { }

        public ErrorResponse(string message) : base(message) { }

        public ErrorResponse(Exception exception) : base(exception.Message) { }

        public static ErrorResponse FromMessage(string message) => new(message);

        public static ErrorResponse FromException(Exception exception) => new(exception);
    }
}
