// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.Core.HliOrientatedView.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using Xamarin.Forms;

namespace HLI.Forms.Core.Controls
{
    /// <summary>
    ///     A view that respons to orientation changes and either display the <see cref="PortraitContent" /> or
    ///     <see cref="LandscapeContent" />
    /// </summary>
    //[ContentProperty("PortraitContent")]
    public class HliOrientatedView : ContentView
    {
        #region Static Fields

        /// <summary>
        ///     See <see cref="LandscapeContent" />
        /// </summary>
        public static readonly BindableProperty LandscapeContentProperty = BindableProperty.Create(
            nameof(LandscapeContent),
            typeof(View),
            typeof(HliOrientatedView),
            null,
            propertyChanged: LoadContent);

        /// <summary>
        ///     See <see cref="PortraitContent" />
        /// </summary>
        public static readonly BindableProperty PortraitContentProperty = BindableProperty.Create(
            nameof(PortraitContent),
            typeof(View),
            typeof(HliOrientatedView),
            null,
            propertyChanged: LoadContent);

        #endregion

        #region Public Properties

        public bool IsLandscape => this.Width > this.Height;

        /// <summary>
        ///     The content shown when device is in landscape mode.
        /// </summary>
        public View LandscapeContent
        {
            get => (View)this.GetValue(LandscapeContentProperty);

            set => this.SetValue(LandscapeContentProperty, value);
        }

        /// <summary>
        ///     The content shown when device is in portrait mode.
        /// </summary>
        /// <remarks>Sets <see cref="LandscapeContent" /> to the same value if currently <c>null</c></remarks>
        public View PortraitContent
        {
            get => (View)this.GetValue(PortraitContentProperty);

            set => this.SetValue(PortraitContentProperty, value);
        }

        #endregion

        #region Methods

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            // Listen to orientation changes
            this.LoadContent();
        }

        private static void LoadContent(BindableObject bindable, object oldValue, object newValue)
        {
            ((HliOrientatedView)bindable).LoadContent();
        }

        private void LoadContent()
        {
            if ((int)this.Width == -1 || (int)this.Height == -1)
            {
                return;
            }

            if (this.IsLandscape && this.Content != this.LandscapeContent)
            {
                // Set landscape content
                this.SetContentVisibility();
                this.Content = this.LandscapeContent;
            }
            else if (this.IsLandscape == false && this.Content != this.PortraitContent)
            {
                // Set portrait content
                this.SetContentVisibility();
                this.Content = this.PortraitContent;
            }
        }

        private void SetContentVisibility()
        {
            if (this.LandscapeContent != null)
            {
                this.LandscapeContent.IsVisible = this.IsLandscape;
            }

            if (this.PortraitContent != null)
            {
                this.PortraitContent.IsVisible = this.IsLandscape == false;
            }
        }

        #endregion
    }
}