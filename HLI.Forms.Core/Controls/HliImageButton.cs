// // // --------------------------------------------------------------------------------------------------------------------
// // // <copyright file="HLI.Forms.HliImageButton.cs" company="Sogeti Sverige AB">
// // //   Copyright © Sogeti Sverige AB, 2016
// // // </copyright>
// // // --------------------------------------------------------------------------------------------------------------------

using Xamarin.Forms;

namespace HLI.Forms.Core.Controls
{
    /// <summary>
    ///     Display a button with <see cref="Image"/> content optionally using an <see cref="ImageConverter"/>
    /// </summary>
    public class HliImageButton : HliLinkButton
    {
        #region Static Fields

        /// <summary>
        ///     See <see cref="Image" />
        /// </summary>
        public static readonly BindableProperty ImageProperty = BindableProperty.Create(
            nameof(Image),
            typeof(FileImageSource),
            typeof(HliImageButton),
            null,
            BindingMode.OneWay,
            propertyChanged: OnImageChanged);

        /// <summary>
        ///     See <see cref="ImageConverter" />
        /// </summary>
        public static readonly BindableProperty ImageConverterProperty = BindableProperty.Create(
            nameof(ImageConverter),
            typeof(IValueConverter),
            typeof(HliImageButton),
            null,
            propertyChanged: OnImageConverterChanged);

        #endregion

        #region Fields

        // TODO: Cached Image
        private readonly Image imageView = new Image();

        #endregion

        #region Constructors and Destructors

        public HliImageButton()
        {
            this.Content = this.imageView;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the image.
        /// </summary>
        public FileImageSource Image
        {
            get => (FileImageSource)this.GetValue(ImageProperty);

            set => this.SetValue(ImageProperty, value);
        }

        /// <summary>
        ///     Gets or sets the image converter.
        /// </summary>
        public IValueConverter ImageConverter
        {
            get => (IValueConverter)this.GetValue(ImageConverterProperty);

            set => this.SetValue(ImageConverterProperty, value);
        }

        #endregion

        #region Methods

        private static void OnImageChanged(BindableObject bindable, object o, object newValue)
        {
            ((HliImageButton)bindable).BindImage();
        }

        private static void OnImageConverterChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((HliImageButton)bindable).BindImage();
        }

        /// <summary>
        ///     Rebind the image using the <see cref="ImageConverter" />
        /// </summary>
        private void BindImage()
        {
            if (this.ImageConverter != null)
            {
                this.imageView.Source = this.ImageConverter.Convert(this.Image, typeof(FileImageSource), null, null) as FileImageSource;
            }
            else
            {
                this.imageView.Source = this.Image;
            }
        }

        #endregion
    }
}