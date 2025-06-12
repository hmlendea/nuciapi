using System;

namespace NuciAPI.Responses
{
    public sealed class ErrorResponse : Response
    {
        public override bool Success => false;

        public ErrorResponse() : base(ErrorResponseMessages.Default) { }

        public ErrorResponse(string message) : base(message) { }

        public ErrorResponse(Exception exception) : base(exception.Message) { }
    }
}
