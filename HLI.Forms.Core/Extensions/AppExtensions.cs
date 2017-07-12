// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.AppExtensions.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2016
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

using HLI.Forms.Core.Services;

namespace HLI.Forms.Core.Extensions
{
    public static class AppExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Write debug trace for Exception
        /// </summary>
        /// <param name="ex">Exception to output</param>
        /// <param name="isFeedback">True to output in any available <see cref="HliFeedbackView" /></param>
        /// <param name="caller">The calling function. Populated automatically by <see cref="CallerMemberNameAttribute" /></param>
        public static void WriteDebug(Exception ex, bool isFeedback = true, [CallerMemberName] string caller = null)
            // ReSharper disable once ExplicitCallerInfoArgument
            => AppService.WriteDebug(ex, isFeedback, caller);

        /// <summary>
        ///     Writes title with separator and message below
        /// </summary>
        /// <param name="title">Title of message</param>
        /// <param name="message">The message</param>
        /// <param name="isFeedback">True to output in any available <see cref="HliFeedbackView" /></param>
        public static void WriteDebug(string title, string message, bool isFeedback = true) => AppService.WriteDebug(title, message, isFeedback);

        /// <summary>
        ///     Writes message to any available <see cref="HliFeedbackView" />
        /// </summary>
        /// <param name="message">The message</param>
        public static void WriteFeedback(string message) => AppService.WriteFeedback(message);

        #endregion
    }
}