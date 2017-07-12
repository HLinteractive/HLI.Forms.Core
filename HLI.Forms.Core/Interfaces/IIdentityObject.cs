// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HLI.Forms.IIdentityObject.cs" company="HL Interactive">
//   Copyright © HL Interactive, Stockholm, Sweden, 2015
// </copyright>
// <summary>
//   The IdentityObject interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace HLI.Forms.Core.Interfaces
{
    /// <summary>
    /// The IdentityObject interface.
    /// </summary>
    public interface IIdentityObject
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        [DataMember]
        DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        [DataMember]
        int CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets the updated.
        /// </summary>
        [DataMember]
        DateTime? Updated { get; set; }

        /// <summary>
        /// Gets or sets the updated by.
        /// </summary>
        [DataMember]
        int? UpdatedBy { get; set; }

        #endregion
    }
}