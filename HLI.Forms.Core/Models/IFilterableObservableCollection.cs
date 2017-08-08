// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.Core.IFilterableObservableCollection.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System.Collections;

namespace HLI.Forms.Core.Models
{
    /// <summary>
    ///     An <see cref="ICollection" /> that can be filtered
    /// </summary>
    public interface IFilterableObservableCollection : ICollection

    {
        #region Public Methods and Operators

        /// <summary>
        ///     Clears any filters and restores source collection
        /// </summary>
        void ClearFilterAndSort();

        /// <summary>
        ///     Refresh the collection based on current <see cref="FilterableObservableCollection{T}.Filter" />
        /// </summary>
        void Refresh();

        #endregion
    }
}