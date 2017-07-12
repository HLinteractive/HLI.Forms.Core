// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.StringExtensions.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2016
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;

using Xamarin.Forms;

namespace HLI.Forms.Core.Extensions
{
    public static class StringExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///     <para>Convertes a resource path to it's platform equalant</para>
        ///     <para>Allows using folders on Windows while the removing the folder path on iOS and Android</para>
        /// </summary>
        /// <param name="source">this</param>
        /// <returns>Platform safe path</returns>
        public static string ToPlatformPath(this string source)
        {
            var result = source;
            if (new[] { Device.Android, Device.iOS }.Contains(Device.RuntimePlatform))
            {
                // Remove everything but the file name on iOS and Android since they don't support hierarchical directories
                result = result.Substring(result.LastIndexOf(@"\", StringComparison.Ordinal) + 1);
                //Debug.WriteLine("ImageToPlatformConverter {0} > {1}", stringOrImageSource, result);
            }

            return result;
        }

        #endregion
    }
}