// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.Core.HliPlaceholderView.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using HLI.Forms.Core.Extensions;

using Xamarin.Forms;

namespace HLI.Forms.Core.Controls
{
    /// <summary>
    ///     <para>
    ///         A view that displays <see cref="UnfocusedView" /> when unfocused and <see cref="focusedPage" /> when the user
    ///         is
    ///         editing.
    ///     </para>
    ///     <para>The <see cref="focusedPage" /> has a <see cref="CloseView" /> you can customize.</para>
    ///     <para>There is also a <see cref="OnClosed" /> event and <see cref="ClosedCommand" /> you can subscribe to.</para>
    /// </summary>
    public class HliPlaceholderView : ContentView
    {
        #region Static Fields

        /// <summary>
        ///     See <see cref="ClosedCommand" />
        /// </summary>
        public static readonly BindableProperty ClosedCommandProperty = BindableProperty.Create(
            nameof(ClosedCommand),
            typeof(ICommand),
            typeof(HliPlaceholderView));

        #endregion

        #region Fields

        /// <summary>
        ///     Invoked when the modal is closed by user
        /// </summary>
        public Action OnClosed;

        private readonly ContentView placeHolderView = new ContentView();

        private  readonly ContentView focusedView = new ContentView();

        private View closeView;

        #endregion

        #region Constructors and Destructors

        public HliPlaceholderView()
        {
            this.LoadAppResources();
            this.Content = this.placeHolderView;
            this.CloseView = new Button { Text = "Ok" };
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the <see cref="View"/> used to dismiss the modal. Default is a Button with the text "Ok".
        /// </summary>
        public View CloseView
        {
            get => this.closeView;

            set
            {
                if (value == null || value == this.closeView)
                {
                    return;
                }

                value.HorizontalOptions = LayoutOptions.End;
                value.Margin = new Thickness(5);
                value.VerticalOptions = LayoutOptions.End;

                this.closeView = value;

                // Attach the close/open event
                this.closeView.AddTapGestureRecognizer(this.OnCloseOpenTapped);
            }
        }

        /// <summary>
        ///     Gets or sets the <see cref="Command"/> that is called when the modal is closed. Bindable property.
        /// </summary>
        public ICommand ClosedCommand
        {
            get => (ICommand)this.GetValue(ClosedCommandProperty);

            set => this.SetValue(ClosedCommandProperty, value);
        }

        /// <summary>
        ///     The <see cref="ContentPage" /> displayed modally.
        /// </summary>
        private ContentPage focusedPage;

        /// <summary>
        /// Gets or sets the <see cref="View"/> displayed modally
        /// </summary>
        public View FocusedView
        {
            get => this.focusedView.Content;
            set
            {
                if (this.focusedView.Content == value || value == null)
                {
                    return;
                }

                this.focusedView.Content = value;
            }
        }

        /// <summary>
        ///     The view displayed as placeholder
        /// </summary>
        public View UnfocusedView
        {
            get => this.placeHolderView.Content;

            set
            {
                if (this.placeHolderView.Content == value || value == null)
                {
                    return;
                }

                // Attach the close/open event
                this.AddCloseOpenListenerToView(value);
                this.placeHolderView.Content = value;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Determines if the model is currently open
        /// </summary>
        private bool IsModalOpen => this.Navigation.ModalStack.Contains(this.focusedPage);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Closes the modal if open
        /// </summary>
        /// <returns>Awaitable Task</returns>
        public async Task CloseModal()
        {
            if (!this.IsModalOpen)
            {
                return;
            }

            await this.Navigation.PopModalAsync();
            this.OnClosed?.Invoke();
            this.ClosedCommand?.Execute(null);
        }

        /// <summary>
        ///     Opens the modal if not already open
        /// </summary>
        /// <returns>Awaitable Task</returns>
        public async Task OpenModal()
        {
            if (this.IsModalOpen)
            {
                return;
            }

            this.CreateFocusedPage();
            await this.Navigation.PushModalAsync(this.focusedPage);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Recursively adds tapped listener to view and child view (if ContentView)
        /// </summary>
        /// <param name="view">The view</param>
        private void AddCloseOpenListenerToView(View view)
        {
            var layout = view as Layout<View>;
            var contentView = view as ContentView;

            if (layout != null)
            {
                foreach (var child in layout.Children)
                {
                    this.AddCloseOpenListenerToView(child);
                }
            }
            else if (contentView != null)
            {
                this.AddCloseOpenListenerToView(contentView.Content);
            }
            else
            {
                // Entry can't be enabled for tapped to work
                var entry = view as Entry;
                var button = view as Button;
                if (entry != null)
                {
                    entry.Focused -= this.OnCloseOpenTapped;
                    entry.Focused += this.OnCloseOpenTapped;
                }

                if (button != null)
                {
                    button.Clicked -= this.OnCloseOpenTapped;
                    button.Clicked += this.OnCloseOpenTapped;
                }
                else
                {
                    view.AddTapGestureRecognizer(this.OnCloseOpenTapped);
                }
            }
        }

        /// <summary>
        ///     Generates the page that is displayed as a modal popup: <see cref="focusedPage" />
        /// </summary>
        /// <returns>A new page</returns>
        private void CreateFocusedPage()
        {
            // Populate the page that's shown as "drop down"
            var page = new ContentPage();

            try
            {
                page.LoadResourcesFromApp();

                // Create new content with the close button
                var stack = new StackLayout { VerticalOptions = LayoutOptions.FillAndExpand };
                stack.Children.Add(this.focusedView);
                stack.Children.Add(this.closeView);

                // Re-populate the page and inherit BindingContext
                page.Content = stack;
                page.BindingContext = this.BindingContext;
                this.focusedPage = page;
            }
            catch (Exception ex)
            {
                ex.WriteDebug();
            }
        }

        /// <summary>
        ///     Occurs when the user taps the view to open modal or taps <see cref="CloseView" />
        /// </summary>
        private async void OnCloseOpenTapped(object sender, EventArgs eventArgs)
        {
            // Is the modal open?
            if (this.IsModalOpen)
            {
                // Close the model
                await this.CloseModal();
            }
            else
            {
                // Open the modal
                await this.OpenModal();
            }
        }

        #endregion
    }
}