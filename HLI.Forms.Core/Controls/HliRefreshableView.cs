// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.Core.HliRefreshableView.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using HLI.Forms.Core.Extensions;
using HLI.Forms.Core.Resources;

using Xamarin.Forms;

namespace HLI.Forms.Core.Controls
{
    /// <inheritdoc />
    /// <summary>
    ///     <para>
    ///         A view with <see cref="T:Xamarin.Forms.ActivityIndicator" /> bound to properties "IsBusy" and "BusyReason" in
    ///         ViewModel.
    ///     </para>
    ///     <para>
    ///         Optionally set
    ///         <see cref="IsScrollable" /> and <see cref="Transparency" />.
    ///     </para>
    ///     <para>Generates the following XAML:</para>
    ///     <code>
    /// &lt;Grid&gt;
    ///     <!-- Only if IsScrollable = true -->
    ///     &lt;ScrollView&gt;
    ///         &lt;!-- Your content --&gt;
    ///     &lt;/ScrollView&gt;
    ///     &lt;ActivityIndicator IsRunning="True" IsVisible="{Binding IsBusy}" /&gt;
    /// &lt;/Grid&gt;
    /// </code>
    /// </summary>
    /// <example>
    ///     Wrapping a view in refreshable view:
    ///     <code> 
    ///         &lt;hli:HliRefreshableView IsScrollable="True"&gt;
    ///             &lt;StackLayout&gt;
    ///                 &lt;hli:HliBarChart
    ///                     BarScale="3"
    ///                     ItemsSource="{Binding Models}"
    ///                     LabelPath="Name"
    ///                     ValuePath="Moons" /&gt;
    ///             &lt;/StackLayout&gt;
    ///         &lt;/hli:HliRefreshableView&gt;
    /// </code>
    /// </example>
    [ContentProperty(nameof(ViewContent))]
    public class HliRefreshableView : ContentView
    {
        #region Static Fields

        /// <summary>
        ///     See <see cref="IsScrollable" />
        /// </summary>
        public static readonly BindableProperty IsScrollableProperty = BindableProperty.Create(
            nameof(IsScrollable),
            typeof(bool),
            typeof(HliRefreshableView),
            false,
            propertyChanged: (bindable, value, newValue) => ((HliRefreshableView)bindable).CreateContent());

        /// <summary>
        ///     See <see cref="Transparency" />
        /// </summary>
        public static readonly BindableProperty TransparencyProperty = BindableProperty.Create(
            nameof(Transparency),
            typeof(int),
            typeof(HliRefreshableView),
            200);

        #endregion

        #region Fields

        private View viewContent;

        #endregion

        #region Constructors and Destructors

        public HliRefreshableView()
        {
            this.LoadAppResources();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Determines if the view is scrollable. Default is <c>false</c>
        /// </summary>
        public bool IsScrollable
        {
            get => (bool)this.GetValue(IsScrollableProperty);

            set => this.SetValue(IsScrollableProperty, value);
        }

        /// <summary>
        ///     Determines the transparency of overlay. Accepts a value between 0 and 255. Default value is 200.
        /// </summary>
        public int Transparency
        {
            get => (int)this.GetValue(TransparencyProperty);

            set
            {
                if (value < 0 || value > 255)
                {
                    return;
                }

                this.SetValue(TransparencyProperty, value);
            }
        }

        /// <summary>
        ///     User content of this view
        /// </summary>
        public View ViewContent
        {
            get => this.viewContent;

            set
            {
                if (this.viewContent == value || value == null)
                {
                    return;
                }

                this.viewContent = value;
                this.CreateContent();
            }
        }

        #endregion

        #region Methods

        #region Overrides of ContentView

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (this.Content == null)
            {
                this.CreateContent();
            }

            if (this.Content != null)
            {
                this.Content.BindingContext = this.BindingContext;
            }
        }

        #endregion

        /// <summary>
        ///     Populates the content of this view from <see cref="ViewContent" />
        /// </summary>
        private void CreateContent()
        {
            if (this.ViewContent == null)
            {
                return;
            }

            // Reset content
            this.Content = null;

            var mainGrid = new Grid();

            Layout mainView;
            if (this.IsScrollable)
            {
                mainView = new ScrollView { Content = this.viewContent, Padding = 10 };
            }
            else
            {
                mainView = new ContentView { Content = this.viewContent, Padding = 10 };
            }

            // Panel that's visible when view model is busy
            var busyPanel = new Grid { BackgroundColor = Color.FromRgba(255, 255, 255, 200) };
            busyPanel.ColumnDefinitions.Add(new ColumnDefinition());
            busyPanel.ColumnDefinitions.Add(new ColumnDefinition());
            busyPanel.ColumnDefinitions.Add(new ColumnDefinition());
            busyPanel.RowDefinitions.Add(new RowDefinition());
            busyPanel.RowDefinitions.Add(new RowDefinition());
            busyPanel.RowDefinitions.Add(new RowDefinition());
            busyPanel.SetBinding(IsVisibleProperty, "IsBusy", BindingMode.OneWay);

            // Activity indicator
            var busyStack = new StackLayout();
            Grid.SetColumn(busyStack, 1);
            Grid.SetRow(busyStack, 1);
            var activityView = new ActivityIndicator
                                   {
                                       Color = Colors.TimeNodesColor,
                                       IsRunning = true,
                                       Scale = 0.7,
                                       WidthRequest = 100,
                                       HorizontalOptions = LayoutOptions.Center,
                                       VerticalOptions = LayoutOptions.Center
                                   };

            // Busy label displaying reason
            var busyLabel = new Label
                                {
                                    HorizontalTextAlignment = TextAlignment.Center,
                                    FontAttributes = FontAttributes.Bold,
                                    FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                                    HorizontalOptions = LayoutOptions.Center
                                };
            busyLabel.SetBinding(Label.TextProperty, "BusyReason", BindingMode.OneWay);

            busyStack.Children.Add(activityView);
            busyStack.Children.Add(busyLabel);
            busyPanel.Children.Add(busyStack);

            mainGrid.Children.Add(mainView);
            mainGrid.Children.Add(busyPanel);

            this.Content = mainGrid;
        }

        #endregion
    }
}