// // // --------------------------------------------------------------------------------------------------------------------
// // // <copyright file="Sogeti.Forms.Core.AutoComplete.cs" company="Sogeti Sverige AB">
// // //   Copyright © Sogeti Sverige AB, 2016
// // // </copyright>
// // // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Runtime.CompilerServices;


using HLI.Forms.Core.Extensions;
using HLI.Forms.Core.Models;
using HLI.Forms.Core.Services;

using Xamarin.Forms;

[assembly: InternalsVisibleTo("XFTest.NetStandard")]

namespace HLI.Forms.Core.Controls
{
    /// <inheritdoc />
    /// <summary>
    /// <para>A "drop down" / <see cref="T:Xamarin.Forms.Picker" /> like view that allows the user to filter values in a list.</para>
    /// <para>Can be used with <see cref="!:AutoCompleteModel" /> to facilitate binding</para>
    /// </summary>
    public class HliAutoComplete : HliComboBox
    {
        #region Static Fields

        /// <summary>
        ///     See <see cref="DisplayMode" />
        /// </summary>
        public static readonly BindableProperty DisplayModeProperty = BindableProperty.Create(
            nameof(DisplayMode),
            typeof(AutoCompleteDisplayMode),
            typeof(HliAutoComplete),
            AutoCompleteDisplayMode.FilteredItems);

        /// <summary>
        ///     See <see cref="SearchText" />
        /// </summary>
        public static readonly BindableProperty SearchTextProperty = BindableProperty.Create(
            nameof(SearchText),
            typeof(string),
            typeof(HliAutoComplete),
            propertyChanged: OnSearchTextPropertyChanged);

        /// <summary>
        ///     See <see cref="FilteredItems" />
        /// </summary>
        public static readonly BindableProperty FilteredItemsProperty = BindableProperty.Create(
            nameof(FilteredItems),
            typeof(FilterableObservableCollection<object>),
            typeof(HliAutoComplete),
            new FilterableObservableCollection<object>());

        /// <summary>
        ///     See <see cref="IsSelectedItemVisible" />
        /// </summary>
        public static readonly BindableProperty IsSelectedItemVisibleProperty = BindableProperty.Create(
            nameof(IsSelectedItemVisible),
            typeof(bool),
            typeof(HliAutoComplete),
            false,
            propertyChanged: OnIsSelectedItemVisibleChanged);

        #endregion

        #region Fields

        /// <summary>
        ///     <see cref="SearchBar" /> displayed with the <see cref="HliComboBox.DropDownListView" />
        /// </summary>
        private readonly SearchBar dropDownSearchBar = new SearchBar { HorizontalOptions = LayoutOptions.FillAndExpand, Margin = 5 };

        /// <summary>
        ///     Mock <see cref="SearchBar" /> displayed as selected item
        /// </summary>
        private readonly SearchBar selectedSearchBar = new SearchBar { HorizontalOptions = LayoutOptions.FillAndExpand };

        #endregion

        #region Constructors and Destructors

        public HliAutoComplete()
        {
            this.HasDropDownArrow = false;
            this.HasContentBorder = false;

            this.dropDownSearchBar.Unfocused += this.OnUnfocused;

            this.dropDownSearchBar.SetBinding<HliAutoComplete>(SearchBar.TextProperty, ac => ac.SearchText, BindingMode.TwoWay);
            this.dropDownSearchBar.BindingContext = this;

            this.selectedSearchBar.SetBinding<HliAutoComplete>(SearchBar.TextProperty, ac => ac.SearchText, BindingMode.OneWay);
            this.selectedSearchBar.BindingContext = this;
            this.selectedSearchBar.SearchButtonPressed += this.OnSelectedItemTapped;
            this.selectedSearchBar.Focused += this.OnSelectedItemTapped;
        }

        #endregion

        #region Enums

        /// <summary>
        ///     Determines how the <see cref="HliAutoComplete" /> suggests values
        /// </summary>
        public enum AutoCompleteDisplayMode
        {
            /// <summary>
            ///     Only the items partially matching the user's search text are displayed
            /// </summary>
            FilteredItems,

            /// <summary>
            ///     All items are displayed when the user starts typing.
            /// </summary>
            AllItems
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Determines how the <see cref="HliAutoComplete" /> suggests values. Default is
        ///     <see cref="AutoCompleteDisplayMode.FilteredItems">FilteredItems</see>
        /// </summary>
        public AutoCompleteDisplayMode DisplayMode
        {
            get => (AutoCompleteDisplayMode)this.GetValue(DisplayModeProperty);

            set => this.SetValue(DisplayModeProperty, value);
        }

        /// <summary>
        ///     The <see cref="HliComboBox.ItemsSource" /> with user's filter applied. Default value is an empty collection.
        /// </summary>
        public FilterableObservableCollection<object> FilteredItems
        {
            get => (FilterableObservableCollection<object>)this.GetValue(FilteredItemsProperty);

            set => this.SetValue(FilteredItemsProperty, value);
        }

        /// <summary>
        ///     Determines if the custom <see cref="HliComboBox.SelectedItemTemplate">SelectedItemTemplate</see> is used. Else a
        ///     searchbar is displayed. Default value is <c>False</c>.
        /// </summary>
        public bool IsSelectedItemVisible
        {
            get => (bool)this.GetValue(IsSelectedItemVisibleProperty);

            set => this.SetValue(IsSelectedItemVisibleProperty, value);
        }

        /// <summary>
        ///     Text the user has typed in the search box
        /// </summary>
        public string SearchText
        {
            get => (string)this.GetValue(SearchTextProperty);

            set => this.SetValue(SearchTextProperty, value);
        }

        #endregion

        #region Methods

        protected override void CreateSelectedItem()
        {
            if (this.IsSelectedItemVisible)
            {
                base.CreateSelectedItem();
                return;
            }

            // Custom logic to add searchbar as selected item
            this.SelectedUserContentGrid.Children.Clear();
            this.SelectedUserContentGrid.Children.Add(this.selectedSearchBar);
        }

        protected override void OnPlaceholderChanged()
        {
            this.selectedSearchBar.Placeholder = this.Placeholder;
            this.dropDownSearchBar.Placeholder = this.Placeholder;
            base.OnPlaceholderChanged();
        }

        protected override async void ShowOrHidePopup()
        {
            this.PopulateFilteredItems();
            this.DropDownListView.ItemsSource = this.FilteredItems;

            if (this.IsDropdownVisible)
            {
                var stackLayout = new StackLayout { Children = { this.dropDownSearchBar, this.DropDownGrid, this.CloseButton } };
                var popupPage = new ContentPage { Content = stackLayout };
                popupPage.Appearing += (sender, args) => this.dropDownSearchBar.Focus();
                await this.Navigation.PushModalAsync(popupPage);
            }
            else
            {
                await this.Navigation.PopModalAsync();
                this.CloseCommand?.Execute(null);
            }
        }
        
        private static void OnIsSelectedItemVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue || newValue == null || newValue is bool == false)
            {
                return;
            }

            var autoComplete = bindable.AsType<HliAutoComplete>();
            autoComplete.SelectedItemTemplate = (bool)newValue
                                                    ? new DataTemplate(() => autoComplete.selectedSearchBar)
                                                    : autoComplete.SelectedItemTemplate;
            autoComplete.Placeholder = null;
            autoComplete.CreateSelectedItem();
        }

        /// <summary>
        ///     Occurs when the <see cref="SearchTextProperty" /> changes
        /// </summary>
        private static void OnSearchTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue || newValue == null || newValue is string == false)
            {
                return;
            }

            bindable.AsType<HliAutoComplete>().FilterItemsSource();
        }

        private bool Filter(object item)
        {
            return item != null && this.dropDownSearchBar.Text != null
                   && item.ToString().ToLower().Trim().Contains(this.dropDownSearchBar.Text.ToLower().Trim());
        }

        /// <summary>
        ///     Occurs when the text changes in the search bar
        /// </summary>
        private void FilterItemsSource()
        {
            if (this.ItemsSource == null)
            {
                return;
            }

            try
            {
                this.PopulateFilteredItems();
                if (this.FilteredItems == null)
                {
                    return;
                }

                // Avoid view updates
                this.DropDownListView.ItemsSource = null;

                this.FilteredItems.ClearFilterAndSort();

                if (this.DisplayMode == AutoCompleteDisplayMode.FilteredItems)
                {
                    // Filter the listview based on user input
                    this.FilteredItems.FilterAndSort(this.Filter, this.ListViewDefaultSort);
                }
                else
                {
                    this.FilteredItems.SuspendFiltering = true;
                    var match = this.FilteredItems.Where(this.Filter).ToList();
                    foreach (var item in match)
                    {
                        this.FilteredItems.Remove(item);
                        this.FilteredItems.Insert(0, item);
                    }

                    this.FilteredItems.SuspendFiltering = false;
                }

                this.DropDownListView.ItemsSource = this.FilteredItems;

                //this.ShowOrHideDropDown();
            }
            catch (Exception ex)
            {
                AppService.WriteDebug(ex);
            }
        }

        private object ListViewDefaultSort(object item)
        {
            return item.ToString();
        }

        /// <summary>
        ///     If <see cref="HliComboBox.ItemsSource" /> is set, uses it to populate <see cref="FilteredItems" />
        /// </summary>
        private void PopulateFilteredItems()
        {
            if (this.ItemsSource == null)
            {
                return;
            }

            // Convert to objects
            var objects = this.ItemsSource.Cast<object>().ToList();

            // Populate with filtered collection
            this.FilteredItems = new FilterableObservableCollection<object>(objects);
        }

        #endregion
    }
}