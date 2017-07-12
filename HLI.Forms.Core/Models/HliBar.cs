// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.HliBar.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2016
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System;

namespace HLI.Forms.Core.Models
{
    public class HliBar
    {
        #region Public Properties

        /// <summary>
        ///     The duration in <c>long</c> ticks
        /// </summary>
        public long Duration { get; set; }

        /// <summary>
        ///     The duration as <see cref="TimeSpan" />
        /// </summary>
        public TimeSpan DurationSpan => new TimeSpan(this.Duration);

        public double Quantity { get; set; }
        
        public object TimeLogType { get; set; }

        public string Title { get; set; }

        #endregion
    }
}