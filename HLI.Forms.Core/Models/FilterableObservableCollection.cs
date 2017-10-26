// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.Core.FilterableObservableCollection.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace HLI.Forms.Core.Models
{
    /// <summary>
    ///     An <see cref="ICollection{T}" /> that can be filtered
    /// </summary>
    /// <typeparam name="T">Type of objects in the collection (no restrictions)</typeparam>
    public class FilterableObservableCollection<T> : ObservableCollection<T>, IFilterableObservableCollection
    {
        #region Fields

        /// <summary>
        ///     When <c>true</c>, filtering is suspended. Default is <c>false</c>.
        /// </summary>
        public bool SuspendFiltering = false;

        /// <summary>
        ///     The last used filter
        /// </summary>
        private Func<T, bool> currentFilter;

        private Func<T, object> currentSorting;

        private bool sortDescending;

        #endregion

        #region Constructors and Destructors

        public FilterableObservableCollection()
        {
        }

        public FilterableObservableCollection(IList<T> items)
            : base(items)
        {
            this.SourceCollection = new List<T>(items);

            // Check if  source is an observable collection and listen to changes
            var observable = items as ObservableCollection<T>;
            if (observable != null)
            {
                // Set source collection and listen to changes
                this.SourceCollection = observable;
                observable.CollectionChanged -= this.SourceCollectionChanged;
                observable.CollectionChanged += this.SourceCollectionChanged;
            }
            else
            {
                // Set source collection to the new items, that also have been populated to base
                this.SourceCollection = new List<T>(items);
            }
        }

        #endregion

        #region Public Properties

        public IList<T> SourceCollection { get; } = new List<T>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Clears any filters and restores source collection
        /// </summary>
        public void ClearFilterAndSort()
        {
            this.currentFilter = null;
            this.currentSorting = null;
            this.Clear();
            foreach (var item in this.SourceCollection)
            {
                this.Add(item);
            }
        }

        /// <summary>
        ///     Filter the collection on specified criteria
        /// </summary>
        /// <param name="filter">An expression that determines which items are returned by the collection</param>
        public void Filter(Func<T, bool> filter)
        {
            this.currentFilter = filter;
            this.ApplyFilterAndSort();
        }

        public void FilterAndSort(Func<T, bool> filter, Func<T, object> sorting, bool descending = false)
        {
            this.sortDescending = descending;
            this.currentFilter = filter;
            this.currentSorting = sorting;
            this.ApplyFilterAndSort();
        }

        /// <summary>
        ///     Refresh the collection based on current <see cref="Filter" /> and <see cref="currentSorting" />
        /// </summary>
        public void Refresh()
        {
            this.ApplyFilterAndSort();
        }

        /// <summary>
        ///     Applies the provided sorting
        /// </summary>
        /// <param name="sorting">Determines how objects are sorted</param>
        /// <param name="descending">Determines if items are sorted descending</param>
        public void Sort(Func<T, object> sorting, bool descending = false)
        {
            this.sortDescending = descending;
            this.currentSorting = sorting;
            this.ApplyFilterAndSort();
        }

        #endregion

        #region Methods

        #region Overrides of ObservableCollection<T>

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            if (this.SourceCollection?.Contains(item) == false)
            {
                this.SourceCollection.Add(item);
            }
        }

        #endregion

        /// <summary>
        ///     Applies filtering and sorting on <see cref="Collection{T}.Items" />
        /// </summary>
        private void ApplyFilterAndSort()
        {
            if (this.SuspendFiltering)
            {
                return;
            }

            // Clear this collection
            this.Clear();

            // Sort if needed
            var sortedCollection = this.GetSortedCollection();

            // Check if any filter is supplied
            if (this.currentFilter == null)
            {
                // Filter is empty - reset collection to source
                foreach (var item in sortedCollection)
                {
                    this.Add(item);
                }

                return;
            }

            // Filter collection
            foreach (var item in sortedCollection.Where(this.currentFilter))
            {
                this.Add(item);
            }
        }

        private IList<T> GetSortedCollection()
        {
            if (this.currentSorting == null)
            {
                return this.SourceCollection;
            }

            return this.sortDescending
                       ? this.SourceCollection.OrderByDescending(this.currentSorting).ToList()
                       : this.SourceCollection.OrderBy(this.currentSorting).ToList();
        }

        /// <summary>
        ///     Occurs when the source collection changes
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="args">Change args</param>
        private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            this.Refresh();
        }

        #endregion
    }
}