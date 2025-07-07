namespace NuciAPI.Responses
{
    /// <summary>
    /// Represents a successful response from the API.
    /// </summary>
    public class NuciApiSuccessResponse : NuciApiResponse
    {
        /// <summary>
        /// Indicates whether the request was successful.
        /// </summary>
        public override bool IsSuccessful => true;

        /// <summary>
        /// Default constructor for SuccessResponse.
        /// </summary>
        public NuciApiSuccessResponse() : base(NuciApiResponseMessages.SuccessMessages.Default) { }

        /// <summary>
        /// Initializes a new instance of the SuccessResponse class with a specific message.
        /// </summary>
        /// <param name="message"></param>
        public NuciApiSuccessResponse(string message) : base(message) { }

        /// <summary>
        /// Creates a new SuccessResponse instance from a specific message.
        /// </summary>
        /// <param name="message">The message to include in the response.</param>
        /// <returns></returns>
        public static NuciApiSuccessResponse FromMessage(string message) => new(message);

        /// <summary>
        /// Creates a new SuccessResponse instance with a default success message.
        /// </summary>
        public static NuciApiSuccessResponse Default => FromMessage(NuciApiResponseMessages.SuccessMessages.Default);
    }
}
