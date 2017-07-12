// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HLI.Forms.BytesToImageSourceConverter.cs" company="HL Interactive">
//   Copyright © HL Interactive, Stockholm, Sweden, 2015
// </copyright>
// <summary>
//   Allows binding of an image from byte array
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;

using Xamarin.Forms;

namespace HLI.Forms.Core.Converters
{
    /// <summary>
    ///     Allows binding of an image from byte array
    /// </summary>
    public class BytesToImageSourceConverter : IValueConverter
    {
        #region Public Methods and Operators
        
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
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("This converter only works for one way binding");
        }

        #endregion
    }
}