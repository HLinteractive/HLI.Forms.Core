// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.Core.AppExtensions.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using HLI.Forms.Core.Controls;
using HLI.Forms.Core.Services;

using Xamarin.Forms;

namespace HLI.Forms.Core.Extensions
{
    public static class AppExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Tries to find the key in application settings
        /// </summary>
        /// <returns>Setting value or <c>null</c></returns>
        public static object GetSetting(this Application app, string key)
        {
            try
            {
                if (!app.Properties.ContainsKey(key))
                {
                    return false;
                }

                return Application.Current.Properties[key];
            }
            catch (Exception ex)
            {
                AppService.WriteDebug(ex);
                return null;
            }
        }

        /// <summary>
        ///     Saves key / value to app settings
        /// </summary>
        public static async Task SaveSetting(this Application app, string key, object value)
        {
            try
            {
                if (app.Properties.ContainsKey(key))
                {
                    app.Properties.Remove(key);
                }

                app.Properties.Add(key, value);
                await app.SavePropertiesAsync();
            }
            catch (Exception ex)
            {
                AppService.WriteDebug(ex);
            }
        }

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