// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.Core.ImageToPlatformConverter.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;

using HLI.Forms.Core.Extensions;

using Xamarin.Forms;

namespace HLI.Forms.Core.Converters
{
    /// <summary>
    ///     Allows using image folders on Windows while the removing the folder path on iOS and Android
    /// </summary>
    public class ImageToPlatformConverter : IValueConverter
    {
        #region Public Methods and Operators

        /// <summary>
        ///     See <see cref="ImageToPlatformConverter" />
        /// </summary>
        /// <param name="stringOrImageSource">Image URI string or <see cref="ImageSource" /></param>
        /// <returns>
        ///     <see cref="FileImageSource" />
        /// </returns>
        public static FileImageSource ImageToPlatformSource(object stringOrImageSource)
        {
            if (stringOrImageSource == null
                || (stringOrImageSource is FileImageSource == false && string.IsNullOrWhiteSpace(stringOrImageSource.ToString())))
            {
                return null;
            }

            var result = stringOrImageSource is FileImageSource ? ((FileImageSource)stringOrImageSource).File : stringOrImageSource.ToString();

            result = result.ToPlatformPath();

            return new FileImageSource { File = result };
        }

        /// <summary>
        ///     See <see cref="ImageToPlatformConverter" />
        /// </summary>
        /// <param name="stringOrImageSource">Image URI string or <see cref="ImageSource" /></param>
        /// <returns></returns>
        public static string ImageToPlatformString(object stringOrImageSource)
        {
            return ImageToPlatformSource(stringOrImageSource).File;
        }

        /// <inheritdoc />
        /// <summary>
        ///     See <see cref="T:HLI.Forms.Core.Converters.ImageToPlatformConverter" />
        /// </summary>
        /// <param name="value">Image URI string or <see cref="T:Xamarin.Forms.ImageSource" /></param>
        /// <param name="targetType">Not used</param>
        /// <param name="parameter">Not used</param>
        /// <param name="culture">Not used</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ImageToPlatformSource(value);
        }

        /// <inheritdoc />
        /// <summary>
        ///     This converter only supports one way binding
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("This converter only supports one way binding");
        }

        #endregion
    }
}