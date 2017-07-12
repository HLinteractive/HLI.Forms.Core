// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.TimeNodes.Forms.ImageToPlatformConverter.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2016
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;

using HLI.Forms.Core.Extensions;

using Xamarin.Forms;

namespace HLI.Forms.Core.Converters
{
    /// <summary>
    ///     <para>Convertes a image uri to it's platform equalant</para>
    ///     <para>Allows using image folders on Windows while the removing the folder path on iOS and Android</para>
    /// </summary>
    public class ImageToPlatformConverter : IValueConverter
    {
        #region Public Methods and Operators

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

        public static string ImageToPlatformString(object stringOrImageSource)
        {
            return ImageToPlatformSource(stringOrImageSource).File;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ImageToPlatformSource(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("This converter only supports one way binding");
        }

        #endregion
    }
}