// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.Core.BytesToImageSourceConverter.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;

using Xamarin.Forms;

namespace HLI.Forms.Core.Converters
{
    /// <inheritdoc />
    /// <summary>
    ///     Converts the supplied <see cref="byte" /> array to an <see cref="ImageSource" />
    /// </summary>
    public class BytesToImageSourceConverter : IValueConverter
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        /// <summary>
        ///     See <see cref="BytesToImageSourceConverter" />
        /// </summary>
        /// <param name="value"><see cref="byte" /> array</param>
        /// <param name="targetType">Not used</param>
        /// <param name="parameter">Not used</param>
        /// <param name="culture">Not used</param>
        /// <returns>
        ///     <see cref="ImageSource" />
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bytes = value as byte[];
            if (value == null)
            {
                return null;
            }

            // Source from Bytes[] 
            return ImageSource.FromStream(() => new MemoryStream(bytes));
        }

        /// <inheritdoc />
        /// <summary>
        ///     This converter only works for one way binding
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("This converter only works for one way binding");
        }

        #endregion
    }
}