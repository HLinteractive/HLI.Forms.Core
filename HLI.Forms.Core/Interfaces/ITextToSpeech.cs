// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HLI.Forms.ITextToSpeech.cs" company="HL Interactive">
//   Copyright © HL Interactive, Stockholm, Sweden, 2015
// </copyright>
// <summary>
//   Implemented with DependencyService in each client.
//   Note: Classes implementing the Interface must have a parameterless constructor to work with the DependencyService.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace HLI.Forms.Core.Interfaces
{
    /// <summary>
    ///     Implemented with DependencyService in each client.
    ///     Note: Classes implementing the Interface must have a parameterless constructor to work with the DependencyService.
    /// </summary>
    public interface ITextToSpeech
    {
        #region Public Methods and Operators

        #region Other

        /// <summary>
        /// The speak.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        void Speak(string text);

        #endregion

        #endregion
    }
}