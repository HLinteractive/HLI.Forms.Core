// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.Core.Constants.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using HLI.Forms.Core.Controls;

namespace HLI.Forms.Core
{
    /// <summary>
    ///     Constant values in the application
    /// </summary>
    public static class Constants
    {
        /// <summary>
        ///     Keys used by <see cref="HliFeedbackView" />
        /// </summary>
        public static class FeedbackKeys
        {
            #region Constants

            /// <summary>
            ///     Key used to send error feedback
            /// </summary>
            public const string Error = "Error";

            /// <summary>
            ///     Key used to send message to feedback
            /// </summary>
            public const string Message = "Message";

            #endregion
        }

        /// <summary>
        ///     Keys used when navigating (Prism)
        /// </summary>
        public static class NavigationParameterKeys
        {
            #region Constants

            /// <summary>
            ///     Key used to send a id using Prism navigation
            /// </summary>
            public const string Id = "id";

            /// <summary>
            ///     Key used to send a model using Prism navigation
            /// </summary>
            public const string Model = "model";

            #endregion
        }
    }
}