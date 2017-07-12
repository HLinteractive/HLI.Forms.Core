// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HLI.Forms.HliCollectionViewSource.cs" company="HL Interactive">
//   Copyright © HL Interactive, Stockholm, Sweden, 2015
// </copyright>
// <summary>
//   A simple implementation of CollectionView / CollectionViewSource
//   for portable class libraries.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace HLI.Forms.Core.Models
{
    /// <summary>
    /// A simple implementation of CollectionView / CollectionViewSource
    ///     for portable class libraries.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    /// <remarks>
    /// Source: https://gist.github.com/jamie94bc/6262479
    /// </remarks>
    public class HliCollectionViewSource<T> : INotifyCollectionChanged, INotifyPropertyChanged, ICollection<T>
    {
        #region Fields

        /// <summary>
        ///     The sort descriptions.
        /// </summary>
        private readonly ObservableCollection<SortDescription<T>> sortDescriptions = new ObservableCollection<SortDescription<T>>();

        /// <summary>
        ///     The filter.
        /// </summary>
        private Predicate<T> filter;

        /// <summary>
        ///     The source.
        /// </summary>
        private ObservableCollection<T> source;

        /// <summary>
        ///     The view.
        /// </summary>
        private ObservableCollection<T> view = new ObservableCollection<T>();

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the filter.
        /// </summary>
        public Predicate<T> Filter
        {
            get
            {
                return this.filter;
            }

            set
            {
                if (this.filter == value)
                {
                    return;
                }

                this.filter = value;
                this.OnFilterChanged();
            }
        }

        /// <summary>
        ///     Gets the sort descriptions.
        /// </summary>
        public ICollection<SortDescription<T>> SortDescriptions => this.sortDescriptions;

        /// <summary>
        ///     Gets or sets the source.
        /// </summary>
        public ObservableCollection<T> Source
        {
            get
            {
                return this.source;
            }

            set
            {
                if (this.source == value)
                {
                    return;
                }

                if (this.source != null)
                {
                    this.source.CollectionChanged -= this.SourceOnCollectionChanged;
                }

                this.source = value;

                if (this.source != null)
                {
                    this.source.CollectionChanged += this.SourceOnCollectionChanged;
                }

                this.OnSourceChanged();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     The number of items after which a single
        ///     <see cref="NotifyCollectionChangedAction.Reset" /> event
        ///     will be fired as opposed to multiple events.
        /// </summary>
        /// <remarks>
        ///     This is due to portable class library's insufficient support
        ///     for <see cref="NotifyCollectionChangedEventArgs" />.
        /// </remarks>
        protected virtual int ResetThreshold => 5;

        // ReSharper disable once MemberCanBePrivate.Global
        /// <summary>
        ///     Gets the view.
        /// </summary>
        protected ObservableCollection<T> View
        {
            get
            {
                return this.view;
            }

            private set
            {
                this.view = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HliCollectionViewSource{T}"/> class.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        public HliCollectionViewSource(ObservableCollection<T> source)
        {
            this.Init(source);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HliCollectionViewSource{T}"/> class.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="sortDescriptions">
        /// The sort descriptions.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public HliCollectionViewSource(ObservableCollection<T> source, params SortDescription<T>[] sortDescriptions)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (sortDescriptions == null)
            {
                throw new ArgumentNullException("sortDescriptions");
            }

            foreach (var sortDescription in sortDescriptions)
            {
                this.SortDescriptions.Add(sortDescription);
            }

            this.Init(source);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HliCollectionViewSource{T}"/> class.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="sortDescriptions">
        /// The sort descriptions.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public HliCollectionViewSource(ObservableCollection<T> source, Predicate<T> filter, params SortDescription<T>[] sortDescriptions)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            this.filter = filter;

            foreach (var sortDescription in sortDescriptions)
            {
                this.SortDescriptions.Add(sortDescription);
            }

            this.Init(source);
        }

        /// <summary>
        /// The init.
        /// </summary>
        /// <param name="src">
        /// The src.
        /// </param>
        private void Init(ObservableCollection<T> src)
        {
            this.Source = src; // Source prop calls EnsureView()

            this.sortDescriptions.CollectionChanged += this.SortDescriptionsOnCollectionChanged;
        }

        #endregion

        #region OnChanged

        /// <summary>
        /// The source on collection changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var eventsToBroadcast = new List<NotifyCollectionChangedEventArgs>();

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    // ReSharper disable once LoopCanBeConvertedToQuery
                    foreach (var item in e.NewItems.Cast<T>().Where(this.MatchFilter))
                    {
                        eventsToBroadcast.Add(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, this.AddSorted(item)));
                    }

                    break;
                case NotifyCollectionChangedAction.Replace:
                    foreach (var old in e.OldItems.Cast<T>())
                    {
                        this.View.Remove(old);
                    }

                    // ReSharper disable once LoopCanBeConvertedToQuery
                    foreach (var item in e.NewItems.Cast<T>().Where(this.MatchFilter))
                    {
                        eventsToBroadcast.Add(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, this.AddSorted(item)));
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:

                    // ReSharper disable once LoopCanBeConvertedToQuery
                    foreach (var item in e.OldItems.Cast<T>())
                    {
                        var idx = this.View.IndexOf(item);

                        if (idx > -1 && this.View.Remove(item))
                        {
                            eventsToBroadcast.Add(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, idx));
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.EnsureView();
                    break;
            }

            if (eventsToBroadcast.Count > this.ResetThreshold)
            {
                this.NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            else
            {
                foreach (var evt in eventsToBroadcast)
                {
                    this.NotifyCollectionChanged(evt);
                }
            }

            // ReSharper disable once ExplicitCallerInfoArgument
            this.NotifyPropertyChanged("Count");
        }

        /// <summary>
        ///     The on source changed.
        /// </summary>
        private void OnSourceChanged()
        {
            this.EnsureView();
        }

        /// <summary>
        ///     The on filter changed.
        /// </summary>
        public void OnFilterChanged()
        {
            this.EnsureView();
        }

        /// <summary>
        /// The sort descriptions on collection changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SortDescriptionsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Unfortunately PCL's don't implement NotifyCollectionChangedEventArgs.Move so we have to reset the
            // list whenever this changes :(
            var sorted = this.View.ToList();
            sorted.Sort(this.SortComparison);
            this.View = new ObservableCollection<T>(sorted);

            this.NotifyCollectionChanged();
        }

        #endregion

        /// <summary>
        ///     The ensure view.
        /// </summary>
        private void EnsureView()
        {
            var eventsToBroadcast = new List<NotifyCollectionChangedEventArgs>();

            foreach (var item in this.Source)
            {
                if (this.filter != null && this.MatchFilter(item) == false)
                {
                    // Doesn't match - remove
                    this.View.Remove(item);
                }
                else if (this.View.Contains(item) == false)
                {
                    // Missing in View - add
                    eventsToBroadcast.Add(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, this.AddSorted(item)));
                }
            }

            eventsToBroadcast.AddRange(
                from item in this.View.Where(x => !this.Source.Contains(x))
                let indexOfItem = this.View.IndexOf(item)
                where this.View.Remove(item)
                select new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, indexOfItem));

            if (eventsToBroadcast.Count > this.ResetThreshold)
            {
                this.NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            else
            {
                foreach (var evt in eventsToBroadcast)
                {
                    this.NotifyCollectionChanged(evt);
                }
            }

            // ReSharper disable ExplicitCallerInfoArgument
            this.NotifyPropertyChanged("Count");
            this.NotifyPropertyChanged("View");
            this.NotifyCollectionChanged();
        }

        #region Utils

        /// <summary>
        /// The match filter.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool MatchFilter(T obj)
        {
            return this.Filter == null || this.Filter(obj);
        }

        /// <summary>
        ///     Gets the sort comparison.
        /// </summary>
        private Comparison<T> SortComparison
        {
            get
            {
                return (x, y) =>
                    {
                        // x > = 1, y > = -1
                        var move = 0;

                        foreach (var sd in this.SortDescriptions)
                        {
                            move = sd.Property(x).CompareTo(sd.Property(y));

                            if (sd.Order == SortOrder.Descending)
                            {
                                move *= -1;
                            }

                            if (move != 0)
                            {
                                return move;
                            }
                        }

                        return move;
                    };
            }
        }

        /// <summary>
        /// The add sorted.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int AddSorted(T item)
        {
            var sortedList = this.View.ToList();

            sortedList.Add(item);

            sortedList.Sort(this.SortComparison);

            var idx = sortedList.IndexOf(item);

            this.View.Insert(idx, item);

            return idx;
        }

        /// <summary>
        ///     The notify collection changed.
        /// </summary>
        private void NotifyCollectionChanged()
        {
            this.NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// The notify collection changed.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        private void NotifyCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged.Invoke(this, e);
            }
        }

        #endregion

        #region Implementation of INotifyCollectionChanged

        /// <summary>
        ///     Raise this event when the (filtered) view changes
        /// </summary>
        public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        ///     CollectionChanged event (per
        ///     <see cref="INotifyCollectionChanged">
        ///         ).
        ///     </see>
        /// </summary>
        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add
            {
                this.CollectionChanged += value;
            }

            remove
            {
                this.CollectionChanged -= value;
            }
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        /// <summary>
        ///     The property changed.
        /// </summary>
        protected virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     The property changed.
        /// </summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                this.PropertyChanged += value;
            }

            remove
            {
                this.PropertyChanged -= value;
            }
        }

        /// <summary>
        /// The notify property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Implementation of IEnumerable

        /// <summary>
        ///     The get enumerator.
        /// </summary>
        /// <returns>
        ///     The <see cref="IEnumerator" />.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.View.GetEnumerator();
        }

        /// <summary>
        ///     The get enumerator.
        /// </summary>
        /// <returns>
        ///     The <see cref="IEnumerator" />.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        public void Add(T item)
        {
            this.Source.Add(item);
        }

        /// <summary>
        ///     The clear.
        /// </summary>
        public void Clear()
        {
            if (this.View != null)
            {
                this.View.Clear();
            }
        }

        /// <summary>
        /// The contains.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Contains(T item)
        {
            return this.View.Contains(item);
        }

        /// <summary>
        /// The copy to.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <param name="arrayIndex">
        /// The array index.
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Gets the count.
        /// </summary>
        public int Count => this.View != null ? this.View.Count : 0;

        /// <summary>
        ///     Gets a value indicating whether is read only.
        /// </summary>
        public bool IsReadOnly => false;
    }

    /// <summary>
    /// The sort description.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class SortDescription<T>
    {
        #region Constructors and Destructors

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SortDescription{T}"/> class.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        /// <param name="order">
        /// The order.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public SortDescription(Func<T, IComparable> property, SortOrder order = SortOrder.Ascending)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            this.Property = property;
            this.Order = order;
        }

        #endregion

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the order.
        /// </summary>
        public SortOrder Order { get; private set; }

        /// <summary>
        ///     Gets the property.
        /// </summary>
        public Func<T, IComparable> Property { get; private set; }

        #endregion
    }

    /// <summary>
    ///     The sort order.
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        ///     The ascending.
        /// </summary>
        Ascending, 

        /// <summary>
        ///     The descending.
        /// </summary>
        Descending
    }
}