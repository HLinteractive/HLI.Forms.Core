// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HLI.Forms.SpeechService.cs" company="HL Interactive">
//   Copyright © HL Interactive, Stockholm, Sweden, 2015
// </copyright>
// <summary>
//   The speech service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using HLI.Forms.Core.Interfaces;

using Xamarin.Forms;

namespace HLI.Forms.Core.Services
{
    /// <summary>
    /// The speech service.
    /// </summary>
    public static class SpeechService
    {
        #region Public Methods and Operators

        #region Other

        /// <summary>
        /// Outputs a message through the speech service, implemented on each device
        /// </summary>
        /// <param name="message">
        /// Message to speak through the device's speech engine
        /// </param>
        public static void Speak(string message)
        {
            DependencyService.Get<ITextToSpeech>().Speak(message);
        }

        #endregion

        #endregion
    }
}