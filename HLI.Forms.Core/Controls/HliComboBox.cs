// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.Core.HliComboBox.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using HLI.Forms.Core.Extensions;
using HLI.Forms.Core.Services;

using Xamarin.Forms;

[assembly: InternalsVisibleTo("XFTest.NetStandard")]

namespace HLI.Forms.Core.Controls
{
    /// <inheritdoc />
    /// <summary>
    ///     Custom combo pox with <see cref="Placeholder" />, <see cref="ItemsSource" /> and optional
    ///     <see cref="ItemTemplate" />.
    /// </summary>
    /// <seealso cref="HliAutoComplete" />
    /// <seealso cref="HliBindablePicker" />
    /// <example>
    ///     <code lang="XAML"><![CDATA[
    /// <hli:HliComboBox DisplayMemberPath="Name" ItemsSource="{Binding Models}" SelectedItem="{Binding SelectedItem, Mode=TwoWay}" 
    ///     ]]></code>
    /// </example>
    /// <remarks>"Drop down" in this view referes to content displayed as a modal using Xamarin Navigation</remarks>
    public class HliComboBox : Grid
    {
        #region Static Fields

        /// <summary>
        ///     See <see cref="Placeholder" />
        /// </summary>
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
            nameof(Placeholder),
            typeof(string),
            typeof(HliComboBox),
            "Select",
            propertyChanged: OnPlaceholderChanged);

        /// <summary>
        ///     See <see cref="IsDropdownVisible" />
        /// </summary>
        public static readonly BindableProperty IsDropdownVisibleProperty = BindableProperty.Create(
            nameof(IsDropdownVisible),
            typeof(bool),
            typeof(HliComboBox),
            false,
            propertyChanged: OnIsDropdownVisibleChanged);

        /// <summary>
        ///     See <see cref="ItemsSource" />
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(HliComboBox),
            propertyChanged: OnItemsSourceChanged);

        /// <summary>
        ///     See <see cref="SelectedItem" />
        /// </summary>
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
            nameof(SelectedItem),
            typeof(object),
            typeof(HliComboBox),
            null,
            propertyChanged: OnSelectedItemChanged);

        /// <summary>
        ///     See <see cref="ItemTemplate" />
        /// </summary>
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
            nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(HliComboBox),
            propertyChanged: OnItemTemplateChanged);

        /// <summary>
        ///     See <see cref="DisplayMemberPath" />
        /// </summary>
        public static readonly BindableProperty DisplayMemberPathProperty = BindableProperty.Create(
            nameof(DisplayMemberPath),
            typeof(string),
            typeof(HliComboBox),
            propertyChanged: OnDisplayMemberPathChanged);

        /// <summary>
        ///     See <see cref="RowHeight" />
        /// </summary>
        public static readonly BindableProperty RowHeightProperty = BindableProperty.Create(
            nameof(RowHeight),
            typeof(double),
            typeof(HliComboBox),
            30.0d,
            propertyChanged: OnRowHeightChanged);

        /// <summary>
        ///     See <see cref="SelectedItemHeight" />
        /// </summary>
        public static readonly BindableProperty SelectedItemHeightProperty = BindableProperty.Create(
            nameof(SelectedItemHeight),
            typeof(double),
            typeof(HliComboBox),
            30d);

        /// <summary>
        ///     See <see cref="SelectedMemberPath" />
        /// </summary>
        public static readonly BindableProperty SelectedMemberPathProperty = BindableProperty.Create(
            nameof(SelectedMemberPath),
            typeof(string),
            typeof(HliComboBox),
            propertyChanged: OnSelectedMemberPathChanged);

        /// <summary>
        ///     See <see cref="RowHeight" />
        /// </summary>
        public static readonly BindableProperty HasContentBorderProperty = BindableProperty.Create(
            nameof(HasContentBorder),
            typeof(bool),
            typeof(HliComboBox),
            true,
            propertyChanged: OnHasContentBorderChanged);

        /// <summary>
        ///     See <see cref="HasDropDownArrow" />
        /// </summary>
        public static readonly BindableProperty HasDropDownArrowProperty = BindableProperty.Create(
            nameof(HasDropDownArrow),
            typeof(bool),
            typeof(HliComboBox),
            true,
            propertyChanged: OnHasDropDownArrowChanged);

        /// <summary>
        ///     See <see cref="SelectedItemTemplate" />
        /// </summary>
        public static readonly BindableProperty SelectedItemTemplateProperty = BindableProperty.Create(
            nameof(SelectedItemTemplate),
            typeof(DataTemplate),
            typeof(HliComboBox),
            propertyChanged: OnSelectedItemTemplateChanged);

        /// <summary>
        ///     See <see cref="CloseButtonText" />
        /// </summary>
        public static readonly BindableProperty CloseButtonTextProperty = BindableProperty.Create(
            nameof(CloseButtonText),
            typeof(string),
            typeof(HliComboBox),
            propertyChanged: OnCloseButtonTextChanged);

        /// <summary>
        ///     See <see cref="CloseButtonText" />
        /// </summary>
        public static readonly BindableProperty CloseCommandProperty = BindableProperty.Create(
            nameof(CloseCommand),
            typeof(ICommand),
            typeof(HliComboBox));

        #endregion

        #region Fields

        /// <summary>
        ///     Closes the drop down
        /// </summary>
        protected readonly Button CloseButton = new Button { Text = "Close" };

        /// <summary>
        ///     The grid displayed around <see cref="DropDownListView" />
        /// </summary>
        protected readonly Grid DropDownGrid = new Grid { VerticalOptions = LayoutOptions.Start, Margin = new Thickness(0, -1, 0, 0), Padding = 2 };

        /// <summary>
        ///     THe listview displayed as a "dropdown" or "popup"
        /// </summary>
        protected readonly ListView DropDownListView = new ListView
                                                           {
                                                               BackgroundColor = Color.White,
                                                               HasUnevenRows = false,
                                                               SeparatorColor = Color.Transparent,
                                                               SeparatorVisibility = SeparatorVisibility.None,
                                                               IsPullToRefreshEnabled = false,
                                                               Margin = 4
                                                           };

        /// <summary>
        ///     Inner grid for selected item, with <see cref="SelectedItemTemplate" /> as child when <see cref="CreateChildren" />
        ///     has been called.
        /// </summary>
        protected readonly Grid SelectedUserContentGrid = new Grid();

        private readonly Label dropDownArrowLabel = new Label { Text = "▼", TextColor = Color.Gray, Margin = new Thickness(4) };

        /// <summary>
        ///     The border displayed around <see cref="userContentGrid" /> if <see cref="HasContentBorder" /> is true.
        /// </summary>
        private readonly Grid selectedBorderGrid = new Grid { BackgroundColor = Color.Gray, Padding = 2 };

        /// <summary>
        ///     Outer grid for selected item. Has two columns (*, Auto) and background: White.
        /// </summary>
        private readonly Grid selectedColumnsGrid = new Grid { BackgroundColor = Color.White };

        /// <summary>
        ///     The main grid where <see cref="SelectedItemTemplate" /> is displayed.
        /// </summary>
        private readonly Grid userContentGrid = new Grid();

        /// <summary>
        ///     Page that's shown as "drop down" (in reality a modal)
        /// </summary>
        private ContentPage dropDownPage;

        #endregion

        #region Constructors and Destructors

        public HliComboBox()
        {
            this.MinimumHeightRequest = this.SelectedItemHeight;
            this.userContentGrid.MinimumHeightRequest = this.SelectedItemHeight;
            this.CloseButton.Clicked += (sender, args) => this.IsDropdownVisible = false;

            // Rows 0 = "selected item" area
            this.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Rows 1: listview
            this.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            this.RowSpacing = 0;

            // Bind selected item
            this.DropDownListView.SetBinding<HliComboBox>(ListView.SelectedItemProperty, ac => ac.SelectedItem, BindingMode.TwoWay);

            this.DropDownListView.ItemSelected -= this.OnItemSelected;
            this.DropDownListView.ItemSelected += this.OnItemSelected;

            this.DropDownListView.BindingContext = this;
            this.DropDownGrid.BindingContext = this;

            // GRID 1: Outer selected border grid
            this.selectedBorderGrid.Padding = this.HasContentBorder ? 2 : 0;
            this.selectedBorderGrid.BackgroundColor = this.HasContentBorder ? Color.Gray : Color.Transparent;

            // GRID 2: "Columns" grid
            this.selectedColumnsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            this.selectedColumnsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Column 0: User Content
            this.SelectedUserContentGrid.AddTapGestureRecognizer(this.OnSelectedItemTapped);
            this.selectedColumnsGrid.Children.Add(this.SelectedUserContentGrid);

            // Column 1: Drop down arrow
            this.dropDownArrowLabel.SetBinding<HliComboBox>(IsVisibleProperty, combobox => combobox.HasDropDownArrow);
            this.dropDownArrowLabel.BindingContext = this;
            this.selectedColumnsGrid.Children.Add(this.dropDownArrowLabel);
            this.dropDownArrowLabel.AddTapGestureRecognizer(this.OnSelectedItemTapped);
            SetColumn(this.dropDownArrowLabel, 1);

            this.selectedBorderGrid.Children.Add(this.selectedColumnsGrid);
            this.selectedBorderGrid.AddTapGestureRecognizer(this.OnSelectedItemTapped);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     The text displayed in the "Close" button when selecting items. Default value is "Close". This is a bindable
        ///     property.
        /// </summary>
        public string CloseButtonText
        {
            get => this.GetValue(CloseButtonTextProperty).ToString();

            set => this.SetValue(CloseButtonTextProperty, value);
        }

        /// <summary>
        ///     Optional command executed when the user clicks on <see cref="CloseButton" />
        /// </summary>
        public ICommand CloseCommand
        {
            get => (ICommand)this.GetValue(CloseCommandProperty);

            set => this.SetValue(CloseCommandProperty, value);
        }

        /// <summary>
        ///     Creates <see cref="ItemTemplate" /> that binds to the specified path
        /// </summary>
        public string DisplayMemberPath
        {
            get => (string)this.GetValue(DisplayMemberPathProperty);

            set => this.SetValue(DisplayMemberPathProperty, value);
        }

        /// <summary>
        ///     Determines if a gray border is displayed around the main content (i.e like a combobox). Default is <c>True</c>
        /// </summary>
        public bool HasContentBorder
        {
            get => (bool)this.GetValue(HasContentBorderProperty);

            set => this.SetValue(HasContentBorderProperty, value);
        }

        /// <summary>
        ///     Determines if the view has an arrow to the far right indicating it's a drop down. Default is <c>True</c>
        /// </summary>
        public bool HasDropDownArrow
        {
            get => (bool)this.GetValue(HasDropDownArrowProperty);

            set => this.SetValue(HasDropDownArrowProperty, value);
        }

        /// <summary>
        ///     Determines if the drop down is visible. Default is <c>False</c>
        /// </summary>
        public bool IsDropdownVisible
        {
            get => (bool)this.GetValue(IsDropdownVisibleProperty);

            set => this.SetValue(IsDropdownVisibleProperty, value);
        }

        /// <summary>
        ///     An <see cref="IEnumerable" /> of objects.
        /// </summary>
        /// <seealso cref="ItemTemplate" />
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)this.GetValue(ItemsSourceProperty);

            set => this.SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        ///     The <see cref="ListView.ItemTemplate" />
        /// </summary>
        /// <seealso cref="ItemsSource" />
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)this.GetValue(ItemTemplateProperty);

            set => this.SetValue(ItemTemplateProperty, value);
        }

        /// <summary>
        ///     Placeholder when there is no <see cref="SelectedItem" />. Default value is "Select"
        /// </summary>
        public string Placeholder
        {
            get => (string)this.GetValue(PlaceholderProperty);

            set => this.SetValue(PlaceholderProperty, value);
        }

        /// <summary>
        ///     Specifiers the height of each item in the drop down. Default value is <c>30</c>
        /// </summary>
        public double RowHeight
        {
            get => (double)this.GetValue(RowHeightProperty);

            set => this.SetValue(RowHeightProperty, value);
        }

        /// <summary>
        ///     Selected item for the <see cref="DropDownListView" />. Default value is <c>null</c>. Bindable property.
        /// </summary>
        public object SelectedItem
        {
            get => this.GetValue(SelectedItemProperty);

            set => this.SetValue(SelectedItemProperty, value);
        }

        /// <summary>
        ///     The minimum height of the "selected" area. Default value is 30
        /// </summary>
        public double SelectedItemHeight
        {
            get => (double)this.GetValue(SelectedItemHeightProperty);

            set => this.SetValue(SelectedItemHeightProperty, value);
        }

        /// <summary>
        ///     Template for the "selected" area. BindingContext is <see cref="SelectedItem" />. Defaults to  <see cref="ItemTemplate" />. Bindable property.
        /// </summary>
        public DataTemplate SelectedItemTemplate
        {
            get => (DataTemplate)this.GetValue(SelectedItemTemplateProperty);

            set => this.SetValue(SelectedItemTemplateProperty, value);
        }

        /// <summary>
        ///     Creates <see cref="SelectedItemTemplate" /> from the specified path
        /// </summary>
        public string SelectedMemberPath
        {
            get => (string)this.GetValue(SelectedMemberPathProperty);

            set => this.SetValue(SelectedMemberPathProperty, value);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Determines if the dropdown can be displayed. Default value is <c>true</c>. Virtual.
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanShowDropDown()
        {
            return true;
        }

        /// <summary>
        ///     Uses <see cref="SelectedItemTemplate" /> to populate <see cref="SelectedUserContentGrid" />
        /// </summary>
        protected virtual void CreateSelectedItem()
        {
            // Use placeholder when no SelectedItem
            if (!string.IsNullOrWhiteSpace(this.Placeholder) && this.SelectedItem == null)
            {
                this.CreatePlaceholder();
                return;
            }

            // Use template and SelectedItem
            if (this.SelectedItemTemplate == default(DataTemplate) || this.SelectedItemTemplate == null)
            {
                return;
            }
            
            var cell = this.SelectedItemTemplate.CreateContent() as Cell;
            if (cell == null)
            {
                throw new Exception($"{nameof(this.SelectedItemTemplate)} expected to be a DataTemplate containing a Cell");
            }

            var view = cell.Parent as View;
            if (view == null)
            {
                return;
            }

            view.BindingContext = this.SelectedItem;
            view.HorizontalOptions = LayoutOptions.StartAndExpand;
            view.AddTapGestureRecognizer(this.OnSelectedItemTapped);

            this.SelectedUserContentGrid.Children.Clear();
            this.SelectedUserContentGrid.Children.Add(view);
        }

        /// <summary>
        ///     Occurs when an item is selected in the drop down. Closes the dropdown.
        /// </summary>
        protected virtual void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (this.ItemsSource == null || this.SelectedItem == null)
            {
                return;
            }

            this.IsDropdownVisible = false;
            this.CreateSelectedItem();
        }

        /// <summary>
        ///     Occurs when <see cref="Placeholder" /> changes. Populates the main area with the placeholder text.
        /// </summary>
        protected virtual void OnPlaceholderChanged()
        {
            this.CreatePlaceholder();
            this.CreateChildren();
        }

        /// <summary>
        ///     Calls <see cref="ShowDropDown" />
        /// </summary>
        protected void OnSelectedItemTapped(object sender, EventArgs eventArgs)
        {
            if (this.IsDropdownVisible)
            {
                this.CloseCommand?.Execute(null);
                this.HideDropDown();
            }
            else
            {
                this.ShowDropDown();
            }
        }

        /// <summary>
        ///     Hides the drop down
        /// </summary>
        protected void OnUnfocused(object sender, FocusEventArgs e)
        {
            this.IsDropdownVisible = false;
        }

        /// <summary>
        ///     Displays the <see cref="DropDownGrid" /> as a modal page
        /// </summary>
        protected virtual async void ShowOrHidePopup()
        {
            try
            {
                // The dropdown has been set to True - show dropdown as modal
                if (this.IsDropdownVisible)
                {
                    // Populate the page that's shown as "drop down"
                    this.dropDownPage = new ContentPage { Content = new StackLayout { Children = { this.DropDownGrid, this.CloseButton } } };

                    if (this.Navigation.ModalStack.Contains(this.dropDownPage) == false)
                    {
                        await this.Navigation.PushModalAsync(this.dropDownPage);
                    }
                }
                else if (this.Navigation.ModalStack.Contains(this.dropDownPage))
                {
                    await this.Navigation.PopModalAsync();
                    this.CloseCommand?.Execute(null);
                    this.HideDropDown();
                }
            }
            catch (Exception ex)
            {
                AppService.WriteDebug(ex);
            }
        }

        private static void OnCloseButtonTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue || string.IsNullOrWhiteSpace(newValue?.ToString()))
            {
                return;
            }

            bindable.AsType<HliComboBox>().CloseButton.Text = newValue.ToString();
        }

        private static void OnDisplayMemberPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue || string.IsNullOrWhiteSpace(newValue?.ToString()))
            {
                return;
            }

            bindable.AsType<HliComboBox>().CreateItemTemplateFromDisplayMemberPath();
        }

        private static void OnHasContentBorderChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue || newValue == null || newValue is bool == false)
            {
                return;
            }

            var hasBorder = (bool)newValue;
            var comboBox = bindable.AsType<HliComboBox>();

            comboBox.selectedBorderGrid.Padding = hasBorder ? 2 : 0;
            comboBox.selectedBorderGrid.BackgroundColor = hasBorder ? Color.Gray : Color.Transparent;
        }

        private static void OnHasDropDownArrowChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue || newValue == null || newValue is bool == false)
            {
                return;
            }

            bindable.AsType<HliComboBox>().CreateChildren();
        }

        private static void OnIsDropdownVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue || newValue is bool == false)
            {
                return;
            }

            bindable.AsType<HliComboBox>().ShowOrHidePopup();
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue || newValue == null || newValue is IEnumerable == false)
            {
                return;
            }

            bindable.AsType<HliComboBox>().CreateChildren();
        }

        private static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue || newValue == null || newValue is DataTemplate == false)
            {
                return;
            }

            // Set SelectedItemTemplate to the same value if not set
            var comboBox = bindable.AsType<HliComboBox>();
            // (comboBox.ItemTemplate == default(DataTemplate))
            {
                comboBox.ItemTemplate = (DataTemplate)newValue;
            }

            comboBox.CreateChildren();
        }

        private static void OnPlaceholderChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable.AsType<HliComboBox>();
            if (oldValue == newValue || newValue == null || newValue is string == false || view.SelectedItem != null)
            {
                return;
            }

            view.OnPlaceholderChanged();
        }

        private static void OnRowHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue || newValue == null || newValue is double == false)
            {
                return;
            }

            bindable.AsType<HliComboBox>().CreateChildren();
        }

        /// <summary>
        ///     Sets <see cref="IsDropdownVisible" /> to <c>True</c>
        /// </summary>
        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue || newValue == null)
            {
                return;
            }

            // Close drop down on selection
            bindable.AsType<HliComboBox>().IsDropdownVisible = false;
        }

        private static void OnSelectedItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue || newValue == null)
            {
                return;
            }

            bindable.AsType<HliComboBox>().CreateSelectedItem();
        }

        private static void OnSelectedMemberPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue == newValue || newValue == null || newValue is string == false)
            {
                return;
            }

            bindable.AsType<HliComboBox>().CreateDisplayTemplateFromMemberPath();
        }

        /// <summary>
        ///     Populates this children of this control
        /// </summary>
        private void CreateChildren()
        {
            this.Children.Clear();

            if (this.ItemsSource == null || this.ItemTemplate == default(DataTemplate))
            {
                return;
            }

            // User content Grid
            this.CreateSelectedItem();

            // Add outer most grid as child
            this.Children.Add(this.selectedBorderGrid);

            //// Drop Down Grid

            // Convert to objects
            var objects = this.ItemsSource.Cast<object>().ToList();

            // Populate ListView
            this.DropDownListView.ItemTemplate = this.ItemTemplate;
            this.DropDownListView.ItemsSource = objects;

            // The grid that's shown as popup on DisplayPopup
            this.DropDownGrid.Children.Add(this.DropDownListView);
        }

        private void CreateDisplayTemplateFromMemberPath()
        {
            var displayLabel = new Label { Margin = 4 };
            displayLabel.SetBinding(Label.TextProperty, new Binding(this.SelectedMemberPath));
            displayLabel.SetBinding<HliComboBox>(BindingContextProperty, view => view.SelectedItem);
            this.SelectedItemTemplate = new DataTemplate(() => displayLabel);
        }

        private void CreateItemTemplateFromDisplayMemberPath()
        {
            var label = new Label();
            label.SetBinding(Label.TextProperty, this.DisplayMemberPath);
            this.ItemTemplate = new DataTemplate(() => label);
        }

        private void CreatePlaceholder()
        {
            // Default content label displays the value of selected item as default
            var contentLabel = new Label { Text = this.Placeholder, Margin = new Thickness(3), TextColor = Color.Gray };
            contentLabel.AddTapGestureRecognizer(this.OnSelectedItemTapped);
            this.SelectedUserContentGrid.Children.Clear();
            this.SelectedUserContentGrid.Children.Add(contentLabel);
        }

        /// <summary>
        ///     Hides the dropdown / popup
        /// </summary>
        private void HideDropDown()
        {
            this.IsDropdownVisible = false;
        }

        /// <summary>
        ///     Opens the dropdown assuming <see cref="CanShowDropDown" /> returns <c>true</c>
        /// </summary>
        private void ShowDropDown()
        {
            this.IsDropdownVisible = this.CanShowDropDown();
        }

        #endregion
    }
}