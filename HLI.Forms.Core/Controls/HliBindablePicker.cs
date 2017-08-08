// // // --------------------------------------------------------------------------------------------------------------------
// // // <copyright file="HLI.Forms.HliComboBox.cs" company="Sogeti Sverige AB">
// // //   Copyright © Sogeti Sverige AB, 2016
// // // </copyright>
// // // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using HLI.Core.Extensions;
using HLI.Forms.Core.Extensions;

using Xamarin.Forms;

namespace HLI.Forms.Core.Controls
{
    /// <remarks>
    ///     <para>Based on bindable picker written by Simon Villiard</para>
    ///     <para>https://forums.xamarin.com/discussion/30801/xamarin-forms-bindable-picker</para>
    /// </remarks>
    /// <summary>
    ///     <para>Allows binding a <see cref="Picker" /> to an <see cref="ItemsSource" /> of objects.</para>
    ///     <para>Customize using <see cref="DisplayMemberpath" /> with <see cref="SelectedValuePath" />.</para>
    ///     <para>Get/set the whole selected object using <see cref="SelectedItem"/></para>
    /// </summary>
    public class HliBindablePicker : Picker
    {
        #region Static Fields

        /// <summary>
        ///     Source of business objects
        /// </summary>
        public new static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(HliBindablePicker),
            null,
            BindingMode.OneWay,
            propertyChanged: OnItemsSourcePropertyChanged);

        /// <summary>
        ///     The display memberpath property.
        /// </summary>
        public static readonly BindableProperty DisplayMemberpathProperty = BindableProperty.Create(
            nameof(DisplayMemberpath),
            typeof(string),
            typeof(HliBindablePicker),
            null,
            BindingMode.OneWay,
            propertyChanged: (bindable, oldValue, newValue) =>
                {
                    if (oldValue != newValue)
                    {
                        BindablePropertyChanged(bindable, ComboBoxProperty.DisplayMemberPath);
                    }
                });

        /// <summary>
        ///     Gets or sets the selected item (business object)
        /// </summary>
        public new static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
            nameof(SelectedItem),
            typeof(object),
            typeof(HliBindablePicker),
            null,
            BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
                {
                    if (oldValue != newValue)
                    {
                        BindablePropertyChanged(bindable, ComboBoxProperty.SelectedItem);
                    }
                });

        /// <summary>
        ///     Name of the property that will populate <see cref="SelectedValue" />
        /// </summary>
        public static readonly BindableProperty SelectedValuePathProperty = BindableProperty.Create(
            nameof(SelectedValuePath),
            typeof(string),
            typeof(HliBindablePicker));

        /// <summary>
        ///     The selected value, specified by <see cref="SelectedValuePath" />
        /// </summary>
        public static readonly BindableProperty SelectedValueProperty = BindableProperty.Create(
            nameof(SelectedValue),
            typeof(object),
            typeof(HliBindablePicker),
            null,
            BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
                {
                    if (oldValue != newValue)
                    {
                        BindablePropertyChanged(bindable, ComboBoxProperty.SelectedValue);
                    }
                });

        #endregion

        #region Fields

        /// <summary>
        ///     The source dictionary.
        /// </summary>
        private Dictionary<object, object> sourceDictionary;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HliBindablePicker" /> class.
        /// </summary>
        public HliBindablePicker()
        {
            this.LoadAppResources();
            this.SelectedIndexChanged += this.OnSelectedIndexChanged;
        }

        #endregion

        #region Enums

        /// <summary>
        ///     Properties in this class
        /// </summary>
        private enum ComboBoxProperty
        {
            /// <summary>
            ///     The selected index.
            /// </summary>
            SelectedIndex,

            /// <summary>
            ///     The selected item.
            /// </summary>
            SelectedItem,

            /// <summary>
            ///     The selected value.
            /// </summary>
            SelectedValue,

            /// <summary>
            ///     The display member path.
            /// </summary>
            DisplayMemberPath,

            // ReSharper disable once UnusedMember.Local - kept for consistency
            /// <summary>
            ///     The selected value path.
            /// </summary>
            SelectedValuePath
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     <para>Name of the property to display in the combo box selection drop down.</para>
        ///     <para>For an IDictionary <see cref="ItemsSource" />, <see cref="DisplayMemberpath" /> defaults to "Quantity"</para>
        /// </summary>
        public string DisplayMemberpath
        {
            get => (string)this.GetValue(DisplayMemberpathProperty);

            set => this.SetValue(DisplayMemberpathProperty, value);
        }

        /// <summary>
        ///     Gets or sets the items source. Should be an implementation of IEnumerable, rather than the interface itself.
        /// </summary>
        /// <value>
        ///     The items source.
        /// </value>
        public new IEnumerable ItemsSource
        {
            get => (IEnumerable)this.GetValue(ItemsSourceProperty);

            set => this.SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        ///     Gets or sets the selected item.
        /// </summary>
        /// <value>
        ///     The selected item.
        /// </value>
        public new object SelectedItem
        {
            get => this.GetValue(SelectedItemProperty);

            set => this.SetValue(SelectedItemProperty, value);
        }

        /// <summary>
        ///     The selected value, specified by <see cref="SelectedValuePath" />
        /// </summary>
        public object SelectedValue
        {
            get => this.GetValue(SelectedValueProperty);

            set => this.SetValue(SelectedValueProperty, value);
        }

        /// <summary>
        ///     <para>Name of the property that will populate <see cref="SelectedValue" /></para>
        ///     <para>For an IDictionary <see cref="ItemsSource" />, <see cref="DisplayMemberpath" /> defaults to "Key"</para>
        /// </summary>
        public string SelectedValuePath
        {
            get => (string)this.GetValue(SelectedValuePathProperty);

            set => this.SetValue(SelectedValuePathProperty, value);
        }

        #endregion

        #region Methods

        private static void BindablePropertyChanged(BindableObject bindable, ComboBoxProperty comboBoxProperty)
        {
            bindable.AsType<HliBindablePicker>().SyncSelecteItemWithSelectedIndex(comboBoxProperty);
        }

        /// <summary>
        ///     Called when [items source property changed].
        /// </summary>
        private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object o)
        {
            bindable.AsType<HliBindablePicker>().Rebind();
        }

        /// <summary>
        ///     Add the provided KeyValuePair to <see cref="Picker.Items" /> and <see cref="sourceDictionary" />
        /// </summary>
        private void AddToItemsAndSelectionValues(object key, object value)
        {
            this.Items.Add(this.ValueIsDisplayMember() ? value.ToString() : key.ToString());
            this.sourceDictionary.Add(this.ValueIsDisplayMember() ? key : value, new KeyValuePair<object, object>(key, value));
        }

        private void ItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            var newItems = args.NewItems;
            this.OnItemsSourceNewItems(newItems);

            if (args.OldItems == null)
            {
                return;
            }

            foreach (var oldItem in args.OldItems)
            {
                this.Items.Remove((oldItem ?? string.Empty).ToString());
            }
        }

        private int ItemsSourceCount()
        {
            if (this.ItemsSource is IEnumerable<object>)
            {
                return ((IEnumerable<object>)this.ItemsSource).Count();
            }

            if (this.ItemsSource is IDictionary)
            {
                return ((IDictionary)this.ItemsSource).Count;
            }

            return 0;
        }

        /// <summary>
        ///     The items source query or null.
        /// </summary>
        /// <returns>
        ///     The <see cref="IQueryable" />.
        /// </returns>
        private IQueryable ItemsSourceQueryOrNull()
        {
            if (this.ItemsSource is IDictionary)
            {
                return ((IDictionary)this.ItemsSource).AsQueryable();
            }

            return this.ItemsSource?.AsQueryable();
        }

        /// <summary>
        ///     If <see cref="DisplayMemberpath" /> is set, extract value of property from <see cref="newItems" /> and populate
        ///     <see cref="Picker.Items" />
        /// </summary>
        /// <param name="newItems">
        ///     List of items with <see cref="DisplayMemberpath" /> property
        /// </param>
        private void OnItemsSourceNewItems(IEnumerable newItems)
        {
            if (newItems == null || string.IsNullOrWhiteSpace(this.DisplayMemberpath))
            {
                return;
            }

            // Clear old values
            this.sourceDictionary = new Dictionary<object, object>();
            this.Items.Clear();

            if (newItems is IDictionary)
            {
                // If Dictionary, default Key/Quantity display/selected value member
                if (string.IsNullOrWhiteSpace(this.DisplayMemberpath))
                {
                    this.DisplayMemberpath = "Quantity";
                }

                if (string.IsNullOrWhiteSpace(this.SelectedValuePath))
                {
                    this.SelectedValuePath = "Key";
                }

                this.PopulateItemsAndSelectionValuesFromDictionary();
                return;
            }

            foreach (var newItem in newItems.Cast<object>().Where(newItem => newItem != null))
            {
                // Display value to Items
                var displayValue = newItem.GetValueForProperty(this.DisplayMemberpath);
                this.Items.Add(displayValue.ToString());

                // Selection Quantity + item object to sourceDictionary
                if (string.IsNullOrWhiteSpace(this.SelectedValuePath) == false)
                {
                    var valueForProperty = newItem.GetValueForProperty(this.SelectedValuePath);
                    if (this.sourceDictionary.ContainsKey(valueForProperty) == false)
                    {
                        this.sourceDictionary.Add(valueForProperty, newItem);
                    }
                }
            }
        }

        /// <summary>
        ///     The on selected index changed.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="eventArgs">
        ///     The event args.
        /// </param>
        private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            this.SyncSelecteItemWithSelectedIndex(ComboBoxProperty.SelectedIndex);
        }

        /// <summary>
        ///     Parse type of ItemsSource as Dictionary and populate <see cref="Picker.Items" /> and
        ///     <see cref="sourceDictionary" />.
        /// </summary>
        private void PopulateItemsAndSelectionValuesFromDictionary()
        {
            var dict = this.ItemsSource as IDictionary;
            if (dict != null)
            {
                foreach (var keyvalue in dict.Keys)
                {
                    this.AddToItemsAndSelectionValues(keyvalue, dict[keyvalue]);
                }
            }
        }

        /// <summary>
        ///     Listen to changes in <see cref="ItemsSource" /> and call <see cref="OnItemsSourceNewItems" />
        /// </summary>
        private void Rebind()
        {
            // Check if ItemsSource supports notification
            var notifyCollection = this.ItemsSource as INotifyCollectionChanged;
            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged -= this.ItemsSourceCollectionChanged;
                notifyCollection.CollectionChanged += this.ItemsSourceCollectionChanged;
            }

            this.OnItemsSourceNewItems(this.ItemsSource);
        }

        /// <summary>
        ///     The selected item as key value pair.
        /// </summary>
        /// <returns>
        ///     The <see cref="KeyValuePair{TKey,TValue}" />.
        /// </returns>
        private KeyValuePair<object, object> SelectedItemAsKeyValuePair()
        {
            if (this.sourceDictionary != null && this.SelectedItem != null)
            {
                // Safe match
                return this.sourceDictionary.FirstOrDefault(pair => pair.Value.ToString() == this.SelectedItem.ToString());
            }

            return new KeyValuePair<object, object>();
        }

        /// <summary>
        ///     Sets <see cref="Picker.SelectedIndex" /> from <see cref="SelectedItem" />
        /// </summary>
        private void SetSelectedIndex()
        {
            // SelectedIndex from SelectedItem
            var keyValuePair = this.SelectedItemAsKeyValuePair();
            if (keyValuePair.Key != null)
            {
                var key = keyValuePair.Key; ////this.ValueIsDisplayMember() ? keyValuePair.Key : keyValuePair.Value;
                this.SelectedIndex = this.sourceDictionary.Keys.IndexOf(key);
            }
            else if (this.SelectedItem != null && this.ItemsSource != null)
            {
                if (this.ItemsSource is IEnumerable<object>)
                {
                    this.SelectedIndex = ((IEnumerable<object>)this.ItemsSource).ToArray().IndexOf(this.SelectedItem);
                }
                else if (this.ItemsSource.IndexOf(this.SelectedItem) != -1)
                {
                    this.SelectedIndex = this.ItemsSource.IndexOf(this.SelectedItem);
                }
            }
        }

        /// <summary>
        ///     Sets <see cref="SelectedItem" /> from <see cref="Picker.SelectedIndex" /> or <see cref="SelectedValue" />
        /// </summary>
        private void SetSelectedItem()
        {
            // SelectedItem from SelectedIndex
            if (this.ItemsSourceQueryOrNull() != null && this.SelectedIndex != -1 && this.SelectedIndex < this.ItemsSourceCount())
            {
                if (this.ItemsSource is IEnumerable<object>)
                {
                    this.SelectedItem = ((IEnumerable<object>)this.ItemsSource).ToArray()[this.SelectedIndex];
                }
                else if (this.ItemsSource is IDictionary)
                {
                    this.SelectedItem = this.sourceDictionary.ElementAt(this.SelectedIndex).Value;
                }
            }

            // SelectedItem from SelectedValue
            if (this.SelectedValue != null && this.sourceDictionary != null && this.sourceDictionary.ContainsKey(this.SelectedValue))
            {
                this.SelectedItem = this.sourceDictionary[this.SelectedValue];
            }
        }

        /// <summary>
        ///     Syncs <see cref="Picker.SelectedIndex" />, <see cref="SelectedItem" /> and <see cref="SelectedValue" />.
        /// </summary>
        /// <param name="propertyChanged">
        ///     The property Changed.
        /// </param>
        private void SyncSelecteItemWithSelectedIndex(ComboBoxProperty propertyChanged)
        {
            if (propertyChanged != ComboBoxProperty.SelectedIndex)
            {
                this.SetSelectedIndex();
            }

            if (propertyChanged != ComboBoxProperty.SelectedItem)
            {
                this.SetSelectedItem();
            }

            if (propertyChanged != ComboBoxProperty.SelectedValue)
            {
                // SelectedValue from SelectedItem
                if (this.SelectedItem != null && this.SelectedValuePath != null)
                {
                    this.SelectedValue = this.SelectedItem.GetValueForProperty(this.SelectedValuePath);
                }

                this.SetSelectedIndex();
            }
        }

        /// <summary>
        ///     Determines if "value" is <see cref="DisplayMemberpath" />
        /// </summary>
        /// <returns>
        ///     <c>True</c> if DisplayMemberpath is "value"
        /// </returns>
        private bool ValueIsDisplayMember()
        {
            return this.DisplayMemberpath?.ToLower() == "value";
        }

        #endregion
    }
}