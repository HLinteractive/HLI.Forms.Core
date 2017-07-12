// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.ILocalize.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2016
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System.Globalization;

namespace HLI.Forms.Core.Interfaces
{
    /// <summary>
    ///     Because some platform-specific code is required to obtain the user's language preference,
    ///     we'll use a dependency service to expose that information in the Xamarin.Forms app and implement it for each
    ///     platform.
    /// </summary>
    public interface ILocalize
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Get the device's current culture. Not required for Win Phone.
        /// </summary>
        /// <returns>Current culture from device</returns>
        CultureInfo GetCurrentCultureInfo();

        /// <summary>
        /// Set culture on the device
        /// </summary>
        /// <param name="culture"></param>
        void SetCulture(CultureInfo culture);

        #endregion
    }
}