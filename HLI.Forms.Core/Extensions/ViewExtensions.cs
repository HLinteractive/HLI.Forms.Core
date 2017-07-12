// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HLI.Forms.ViewExtensions.cs" company="HL Interactive">
//   Copyright © HL Interactive, Stockholm, Sweden, 2015
// </copyright>
// <summary>
//   The view extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using Xamarin.Forms;

namespace HLI.Forms.Core.Extensions
{
    /// <summary>
    /// The view extensions.
    /// </summary>
    public static class ViewExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Adds a new tap gesture recognizer to the View
        /// </summary>
        /// <param name="view">
        /// This
        /// </param>
        /// <param name="tappedEventHandler">
        /// Event that's triggered on tapped
        /// </param>
        public static void AddTapGestureRecognizer(this View view, EventHandler tappedEventHandler)
        {
            var gestureRecognizer = CreateGestureRecognizer(tappedEventHandler);
            if (view.GestureRecognizers.Contains(gestureRecognizer))
            {
                return;
            }

            view.GestureRecognizers.Add(gestureRecognizer);
        }

        /// <summary>
        /// Creates a new tapped recognizer for the TapCommand.
        /// </summary>
        /// <param name="tappedEventHandler">
        /// Event that's triggered on tapped
        /// </param>
        /// <returns>
        /// New TapGestureRecognizer
        /// </returns>
        public static TapGestureRecognizer CreateGestureRecognizer(EventHandler tappedEventHandler)
        {
            var gestureRecognizer = new TapGestureRecognizer();
            gestureRecognizer.Tapped -= tappedEventHandler;
            gestureRecognizer.Tapped += tappedEventHandler;
            return gestureRecognizer;
        }

        /// <summary>
        /// Ensures view inherits Application resources such as styles
        /// </summary>
        /// <param name="view">this</param>
        public static void LoadAppResources(this View view)
        {
            if (Application.Current != null)
            {
                view.Resources = view.Resources ?? Application.Current.Resources;
            }
        }

        #endregion
    }
}