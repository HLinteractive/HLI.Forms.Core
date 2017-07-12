// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.ExceptionExtensions.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2016
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

using HLI.Forms.Core.Services;

namespace HLI.Forms.Core.Extensions
{
    public static class ExceptionExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Write debug trace for Exception
        /// </summary>
        /// <param name="ex">Exception to output</param>
        /// <param name="isFeedback">True to output in any available <see cref="HliFeedbackView"/></param>
        /// <param name="caller">The calling function. Populated automatically by <see cref="CallerMemberNameAttribute" /></param>
        public static void WriteDebug(this Exception ex, bool isFeedback = true, [CallerMemberName] string caller = null)
            => AppService.WriteDebug(ex, isFeedback, caller);

        #endregion
    }
}