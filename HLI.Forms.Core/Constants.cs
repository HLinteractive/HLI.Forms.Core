// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.Constants.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2016
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

// Cause design issues
//[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace HLI.Forms.Core
{
    /// <summary>
    /// Constant values in the application
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Keys used when navigating (Prism)
        /// </summary>
        public static class NavigationParameterKeys
        {
            public const string Model = "model";

            public const string Id = "id";
        }

        /// <summary>
        /// Keys used by <see cref="HliFeedbackView"/>
        /// </summary>
        public static class FeedbackKeys
        {
            public const string Message = "Message";

            public const string Error = "Error";
        }
    }
}