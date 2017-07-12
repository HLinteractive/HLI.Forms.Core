// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.AppService.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2016
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

using HLI.Forms.Core.Models;
using Xamarin.Forms;

using static HLI.Forms.Core.Constants;

namespace HLI.Forms.Core.Services
{
    /// <summary>
    ///     Common helpers for the <see cref="Application" />
    /// </summary>
    public static class AppService
    {
        #region Constants

        private const string DebugSeparator = " ********** ";

        #endregion

        #region Public Methods and Operators

        public static void ReportToHockeyApp(string message)
        {
            try
            {
#if !DEBUG
                var service = DependencyService.Get<IAnalyticsService>();
                service?.RegisterEvent(message);
#endif
            }
            catch (Exception ex)
            {
                Debug.WriteLine(DebugSeparator + DebugSeparator);
                Debug.WriteLine("HOCKEYAPP NOT AVAILABLE:");
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        ///     Write debug trace for Exception
        /// </summary>
        /// <param name="ex">Exception to output</param>
        /// <param name="isFeedback">True to output in any available <see cref="HliFeedbackView" /></param>
        /// <param name="caller">The calling function. Populated automatically by <see cref="CallerMemberNameAttribute" /></param>
        public static void WriteDebug(Exception ex, bool isFeedback = true, [CallerMemberName] string caller = null)
        {
            if (ex is SerializationException)
            {
                // Ignore since there's a workaround and they span the output
                return;
            }

            Debug.WriteLine($"{DebugSeparator}EXCEPTION in {caller}{DebugSeparator}");
            Debug.WriteLine(ex.Message);
            Debug.WriteLine("DETAILS" + DebugSeparator);
            Debug.WriteLine(ex);
            if (isFeedback)
            {
                MessagingCenter.Send(new HliFeedbackMessage(HliFeedbackMessage.FeedbackType.Error, ex.Message), FeedbackKeys.Message);
            }

            ReportToHockeyApp($"{caller} {ex}");
        }

        /// <summary>
        ///     Writes title with separator and message below
        /// </summary>
        /// <param name="title">Title of message</param>
        /// <param name="message">The message</param>
        /// <param name="isFeedback">True to output in any available <see cref="HliFeedbackView" /></param>
        public static void WriteDebug(string title, string message, bool isFeedback = true)
        {
            Debug.WriteLine(DebugSeparator + title.ToUpper() + DebugSeparator);
            Debug.WriteLine(message);
            Debug.WriteLine(DebugSeparator + DebugSeparator);
            var formattableString = $"{title} {message}";
            ReportToHockeyApp(formattableString);
            if (isFeedback)
            {
                MessagingCenter.Send(new HliFeedbackMessage(HliFeedbackMessage.FeedbackType.Error, formattableString), FeedbackKeys.Message);
            }
        }

        /// <summary>
        ///     Writes message below debug separator line
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="upperCase">Write as uppercase (default: false)</param>
        /// <param name="caller">The calling function. Populated automatically by <see cref="CallerMemberNameAttribute" /></param>
        public static void WriteDebug(string message, bool upperCase = false, [CallerMemberName] string caller = null)
        {
            Debug.WriteLine($"{DebugSeparator} {caller} {DebugSeparator}");
            Debug.WriteLine(upperCase ? message.ToUpper() : message);
            ReportToHockeyApp($"{caller} {message}");
        }

        /// <summary>
        ///     Writes message to any available <see cref="HliFeedbackView" />
        /// </summary>
        /// <param name="message">The message</param>
        public static void WriteFeedback(string message)
        {
            MessagingCenter.Send(new HliFeedbackMessage(HliFeedbackMessage.FeedbackType.Message, message), FeedbackKeys.Message);
        }

#endregion
    }
}