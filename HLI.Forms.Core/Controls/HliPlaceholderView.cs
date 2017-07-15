// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.HliPlaceholderView.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2016
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using HLI.Forms.Core.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace HLI.Forms.Core.Controls
{
    /// <summary>
    ///     A view that displays <see cref="PlaceholderView" /> when unfocused and <see cref="ModalPage" /> when the user is
    ///     editing.
    /// </summary>
    public class HliPlaceholderView : ContentView
    {
        #region Fields

        /// <summary>
        ///     Invoked when the modal is closed by user
        /// </summary>
        public Action OnModalClosed;

        private readonly ContentView placeHolderView = new ContentView();

        private View closeButton;

        /// <summary>
        ///     Determines if the model is currently open
        /// </summary>
        private bool isModalOpen;

        #endregion

        #region Constructors and Destructors

        public HliPlaceholderView()
        {
            this.LoadAppResources();
            this.Content = this.placeHolderView;
            this.CloseButton = new Button { Text = "Select" };
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     The close button (view) used to dismiss the modal. Default is a Button with the text "Select".
        /// </summary>
        public View CloseButton
        {
            get => this.closeButton;

            set
            {
                if (value == null || value == this.closeButton)
                {
                    return;
                }

                value.HorizontalOptions = LayoutOptions.End;
                value.Margin = new Thickness(5);
                value.VerticalOptions = LayoutOptions.End;

                this.closeButton = value;

                // Attach the close/open event
                this.closeButton.AddTapGestureRecognizer(this.OnCloseOpenTapped);
            }
        }

        /// <summary>
        ///     The <see cref="ContentPage" /> displayed as modal. Remarks: there is no default padding  / margin.
        /// </summary>
        public ContentPage ModalPage { get; set; } = new ContentPage();

        /// <summary>
        ///     The view displayed as placeholder
        /// </summary>
        public View PlaceholderView
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

        #region Public Methods and Operators

        /// <summary>
        ///     Closes the modal if open
        /// </summary>
        /// <returns>Awaitable Task</returns>
        public async Task CloseModal()
        {
            if (this.Navigation.ModalStack.Any())
            {
                await this.Navigation.PopModalAsync();
                this.isModalOpen = false;
                this.OnModalClosed?.Invoke();
            }
        }

        /// <summary>
        ///     Opens the modal if not already open
        /// </summary>
        /// <returns>Awaitable Task</returns>
        public async Task OpenModal()
        {
            if (this.Navigation.ModalStack.Any())
            {
                return;
            }

            await this.Navigation.PushModalAsync(this.CreateModalPage());
            this.isModalOpen = true;
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
                    entry.IsEnabled = false;
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
        ///     Generates the page that is displayed as a modal popup.
        /// </summary>
        /// <returns>A new page</returns>
        private ContentPage CreateModalPage()
        {
            var page = this.ModalPage ?? new ContentPage();

            try
            {
                // Capture page content
                var pageContent = page.Content;
                page.LoadResourcesFromApp();
                page.Content = null;

                // Create new content with the close button
                var stack = new StackLayout { VerticalOptions = LayoutOptions.FillAndExpand };
                stack.Children.Add(pageContent);
                stack.Children.Add(this.closeButton);

                // Re-populate the page and inherit BindingContext
                page.Content = stack;
                page.BindingContext = this.BindingContext;
            }
            catch (Exception ex)
            {
                ex.WriteDebug();
            }

            return page;
        }

        /// <summary>
        ///     Occurs when the user taps the view to open modal or taps <see cref="CloseButton" />
        /// </summary>
        private async void OnCloseOpenTapped(object sender, EventArgs eventArgs)
        {
            // Is the modal open?
            if (this.isModalOpen)
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