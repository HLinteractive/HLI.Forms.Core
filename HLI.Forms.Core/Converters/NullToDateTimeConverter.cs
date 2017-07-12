// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.NullToDateTimeConverter.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2016
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;

using Xamarin.Forms;

namespace HLI.Forms.Core.Converters
{
    /// <summary>
    ///     Prevents DateTime? from being breaking Xamarin Forms by returning DateTime.Now instead of <c>null</c>
    /// </summary>
    public class NullToDateTimeConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ?? DateTime.Now;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}