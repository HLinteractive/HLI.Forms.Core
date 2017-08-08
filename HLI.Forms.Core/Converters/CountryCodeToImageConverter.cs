// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.Core.CountryCodeToImageConverter.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;

using HLI.Forms.Core.Services;

using Xamarin.Forms;

namespace HLI.Forms.Core.Converters
{
    /// <inheritdoc />
    /// <summary>
    ///     Converts a two-letter ISO country code to the equalant flag <see cref="T:Xamarin.Forms.ImageSource" /> using
    ///     <a href="http://www.geognos.com/">http://www.geognos.com/</a> API
    /// </summary>
    public class CountryCodeToImageConverter : IValueConverter
    {
        #region Constants

        /// <summary>
        ///     Geognos World Countries API. Granted by registering app
        /// </summary>
        const string FlagImageUri = "http://www.geognos.com/api/en/countries/flag/{0}.png";

        #endregion

        #region Implementation of IValueConverter

        /// <inheritdoc />
        /// <summary>
        ///     See <see cref="T:HLI.Forms.Core.Converters.CountryCodeToImageConverter" />
        /// </summary>
        /// <param name="value">Two-letter ISO country code</param>
        /// <param name="targetType">Not used</param>
        /// <param name="parameter">Not used</param>
        /// <param name="culture">Not used</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                // Defaults to EU
                var twoLetterIso = string.IsNullOrWhiteSpace(value?.ToString()) ? "EU" : value.ToString();

                var uri = string.Format(FlagImageUri, twoLetterIso);
                return ImageSource.FromUri(new Uri(uri));
            }
            catch (Exception ex)
            {
                AppService.WriteDebug(ex);
                return null;
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Converts back to country code string
        /// </summary>
        /// <returns>Country code string</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var imageSource = value as UriImageSource;
            if (imageSource == null)
            {
                return value;
            }

            try
            {
                // Try to extract the country code
                return imageSource.Uri.AbsoluteUri.Substring(imageSource.Uri.AbsoluteUri.LastIndexOf('/') + 1).Replace(".png", string.Empty);
            }
            catch (Exception ex)
            {
                AppService.WriteDebug(ex);
                return value;
            }
        }

        #endregion
    }
}