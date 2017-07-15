// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.OnOrientation.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2016
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using Xamarin.Forms;

namespace HLI.Forms.Core.Extensions
{
    /// <summary>
    ///     Returns value based on current orientation
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    public class OnOrientation<T> : BindableObject
    {
        #region Static Fields

        public static readonly BindableProperty ViewProperty = BindableProperty.Create(
            nameof(View),
            typeof(VisualElement),
            typeof(OnOrientation<T>),
            null);

        #endregion

        #region Public Properties

        public T Landscape { get; set; }

        public T Portrait { get; set; }

        /// <summary>
        ///     The parent view (<see cref="VisualElement" />) of this view
        /// </summary>
        public VisualElement View
        {
            get => (VisualElement)this.GetValue(ViewProperty);

            set => this.SetValue(ViewProperty, value);
        }

        #endregion

        #region Public Methods and Operators

        public static implicit operator T(OnOrientation<T> onOrientation)
        {
            if (onOrientation.View == null)
            {
                // Default to portrait
                return onOrientation.Portrait;
            }

            return onOrientation.View.Width > onOrientation.View.Height ? onOrientation.Landscape : onOrientation.Portrait;
        }

        #endregion
    }
}