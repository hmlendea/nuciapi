namespace NuciAPI.Responses
{
    /// <summary>
    /// Represents a successful response from the API.
    /// </summary>
    public class SuccessResponse : NuciApiResponse
    {
        /// <summary>
        /// Indicates whether the response was successful.
        /// </summary>
        public override bool Success => true;

        /// <summary>
        /// Default constructor for SuccessResponse.
        /// </summary>
        public SuccessResponse() : base(SuccessResponseMessages.Default) { }

        /// <summary>
        /// Initializes a new instance of the SuccessResponse class with a specific message.
        /// </summary>
        /// <param name="message"></param>
        public SuccessResponse(string message) : base(message) { }

        /// <summary>
        /// Creates a new SuccessResponse instance from a specific message.
        /// </summary>
        /// <param name="message">The message to include in the response.</param>
        /// <returns></returns>
        public static SuccessResponse FromMessage(string message) => new(message);

        /// <summary>
        /// Creates a new SuccessResponse instance with a default success message.
        /// </summary>
        public static SuccessResponse Default => FromMessage(SuccessResponseMessages.Default);
    }
}
