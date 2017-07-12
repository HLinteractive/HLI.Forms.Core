// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.ObjectExtensions.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2016
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

namespace HLI.Forms.Core.Extensions
{
    /// <summary>
    ///     Extends the <c>Object</c> classs
    /// </summary>
    public static class ObjectExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Return <see cref="object" /> as the specied type
        /// </summary>
        /// <typeparam name="T">
        ///     Type of <see cref="object" />
        /// </typeparam>
        /// <param name="obj">
        ///     this
        /// </param>
        /// <returns>
        ///     Object as type of T or <c>null</c>
        /// </returns>
        public static T AsType<T>(this object obj) where T : class
        {
            return obj as T;
        }
        
        #endregion
    }
}