// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.Core.ViewExtensions.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Input;

using Xamarin.Forms;

namespace HLI.Forms.Core.Extensions
{
    /// <summary>
    ///     The view extensions.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class ViewExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Adds a new tap gesture recognizer to the View
        /// </summary>
        /// <param name="view">
        ///     This
        /// </param>
        /// <param name="tappedEventHandler">
        ///     Event that's triggered on tapped
        /// </param>
        public static void AddTapGestureRecognizer(this View view, EventHandler tappedEventHandler)
        {
            var gestureRecognizer = CreateGestureRecognizer(tappedEventHandler);
            if (view.GestureRecognizers.Contains(gestureRecognizer)) return;

            view.GestureRecognizers.Add(gestureRecognizer);
        }

        /// <summary>
        ///     Adds a tap gesture regocnier that executes supplied command when tapped
        /// </summary>
        /// <param name="view">this</param>
        /// <param name="command">Command to execute</param>
        /// <param name="commandParameter">Optional parameter</param>
        public static void AddTappedCommand(this View view, ICommand command, object commandParameter = null)
        {
            var gestureRecognizer = new TapGestureRecognizer { Command = command, CommandParameter = commandParameter };

            if (view.GestureRecognizers.Contains(gestureRecognizer)) return;

            view.GestureRecognizers.Add(gestureRecognizer);
        }

        /// <summary>
        ///     Creates a new tapped recognizer for the TapCommand.
        /// </summary>
        /// <param name="tappedEventHandler">
        ///     Event that's triggered on tapped
        /// </param>
        /// <returns>
        ///     New TapGestureRecognizer
        /// </returns>
        public static TapGestureRecognizer CreateGestureRecognizer(EventHandler tappedEventHandler)
        {
            var gestureRecognizer = new TapGestureRecognizer();
            gestureRecognizer.Tapped -= tappedEventHandler;
            gestureRecognizer.Tapped += tappedEventHandler;
            return gestureRecognizer;
        }

        /// <summary>
        ///     Finds the specified <paramref name="viewToFind" /> in this view's children (Grid/StackLayout/ContentView)
        /// </summary>
        /// <param name="view">this</param>
        /// <param name="viewToFind">The child view to find</param>
        /// <returns><c>True</c> if this view has the specified child</returns>
        public static bool HasChildView(this View view, View viewToFind)
        {
            var grid = view as Grid;
            var sl = view as StackLayout;
            var cv = view as ContentView;
            if (grid == null && sl == null) return cv != null && cv.Content == viewToFind;

            if (grid != null) return grid.Children.Any(v => v == viewToFind) || grid.Children.Any(v => HasChildView(v, viewToFind));

            return sl.Children.Any(v => v == viewToFind) || sl.Children.Any(v => HasChildView(v, viewToFind));
        }

        /// <summary>
        ///     Ensures view inherits Application resources such as styles
        /// </summary>
        /// <param name="view">this</param>
        public static void LoadAppResources(this View view)
        {
            if (Application.Current != null) view.Resources = view.Resources ?? Application.Current.Resources;
        }

        #endregion
    }
}