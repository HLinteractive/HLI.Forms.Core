// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.Core.ViewBehaviors.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Windows.Input;

using HLI.Forms.Core.Extensions;

using Xamarin.Forms;

namespace HLI.Forms.Core.Behaviors
{
    /// <summary>
    ///     Extends <see cref="View" /> with custom behaviors.
    /// </summary>
    /// <example>
    ///     <code>
    ///     &lt;ContentPage xmlns:behaviors="clr-namespace:HLI.Forms.Core.Behaviors;assembly=HLI.Forms.Core"&gt;
    ///     &lt;Button Text="Animated" behaviors:ViewBehaviors.IsAnimated="True" /&gt;
    /// </code>
    /// </example>
    public class ViewBehaviors
    {
        #region Static Fields

        /// <summary>
        ///     See <see cref="SetItemTappedCommand(ListView, ICommand)" />
        /// </summary>
        public static readonly BindableProperty TappedCommandProperty = BindableProperty.CreateAttached(
            "TappedCommand",
            typeof(ICommand),
            typeof(ViewBehaviors),
            null,
            propertyChanged: (bindable, value, newValue) => AddTappedCommand(bindable));

        /// <summary>
        ///     See <see cref="SetTappedCommandParameter(BindableObject, object)" />
        /// </summary>
        public static readonly BindableProperty TappedCommandParameterProperty = BindableProperty.CreateAttached(
            "TappedCommandParameter",
            typeof(object),
            typeof(ViewBehaviors),
            default(object),
            propertyChanged: (bindable, value, newValue) => AddTappedCommand(bindable));

        private static ICommand command;

        /// <summary>
        ///     See <see cref="SetItemTappedCommand(ListView, ICommand)" />
        /// </summary>
        public static readonly BindableProperty ItemTappedCommandProperty = BindableProperty.CreateAttached(
            "ItemTappedCommand",
            typeof(ICommand),
            typeof(ViewBehaviors),
            null,
            propertyChanged: ItemTappedChanged);

        /// <summary>
        ///     See <see cref="SetIsAnimated(BindableObject, bool)" />
        /// </summary>
        public static readonly BindableProperty IsAnimatedProperty = BindableProperty.CreateAttached(
            "IsAnimated",
            typeof(bool),
            typeof(ViewBehaviors),
            default(bool),
            propertyChanged: IsAnimatedChanged);

        /// <summary>
        ///     See <see cref="SetIsFocused(BindableObject, bool?)" />
        /// </summary>
        public static readonly BindableProperty IsFocusedProperty = BindableProperty.CreateAttached(
            "IsFocused",
            typeof(bool?),
            typeof(ViewBehaviors),
            null,
            propertyChanged: IsFocusedPropertyChanged);

        #endregion

        #region Public Methods and Operators

        public static bool GetIsAnimated(BindableObject view)
        {
            return (bool)view.GetValue(IsAnimatedProperty);
        }

        public static bool? GetIsFocused(BindableObject view)
        {
            return (bool?)view.GetValue(IsFocusedProperty);
        }

        public static ICommand GetItemTappedCommand(BindableObject view)
        {
            return (ICommand)view.GetValue(ItemTappedCommandProperty);
        }

        public static ICommand GetTappedCommand(BindableObject view)
        {
            return (ICommand)view.GetValue(TappedCommandProperty);
        }

        public static object GetTappedCommandParameter(BindableObject view)
        {
            return view.GetValue(TappedCommandParameterProperty);
        }

        /// <summary>
        ///     Determines if the <see cref="View" /> has a simple zoom animation when tapped. Bindable Property.
        /// </summary>
        /// <param name="view">The view</param>
        /// <param name="value"><c>True</c> to animate on tapped</param>
        public static void SetIsAnimated(BindableObject view, bool value)
        {
            view.SetValue(IsAnimatedProperty, value);
        }

        /// <summary>
        ///     Sets or binds the View's "focused" state. Bindable Property. See <see cref="ViewBehaviors" /> for example
        /// </summary>
        public static void SetIsFocused(BindableObject bindable, bool? value)
        {
            bindable.SetValue(IsFocusedProperty, value);
        }

        /// <summary>
        ///     Sets the <see cref="ListView.ItemTapped" /> <see cref="Command" />. Bindable Property. See
        ///     <see cref="ViewBehaviors" /> for example
        /// </summary>
        /// <param name="value">
        ///     <see cref="ICommand" />
        /// </param>
        /// <param name="view">The View</param>
        public static void SetItemTappedCommand(ListView view, ICommand value)
        {
            view.SetValue(ItemTappedCommandProperty, value);
        }

        #endregion

        #region Methods

        private static async void AddTappedCommand(BindableObject bindable)
        {
            var view = bindable.AsType<View>();
            command = GetTappedCommand(bindable);
            if (view != null && command != null) view.AddTappedCommand(command, GetTappedCommandParameter(bindable));
        }

        private static void IsAnimatedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // Add animation to the view
            var view = bindable.AsType<View>();
            view?.GestureRecognizers.Add(
                new TapGestureRecognizer
                    {
                        Command = new Command(
                            async () =>
                                {
                                    // Scale up and down
                                    await view.ScaleTo(1.2, 50, Easing.CubicOut);
                                    await view.ScaleTo(1, 50, Easing.CubicIn);
                                })
                    });
        }

        private static void IsFocusedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable.AsType<View>();

            view.Focused -= ViewOnFocused;
            view.Focused += ViewOnFocused;
            view.Unfocused -= ViewOnFocused;
            view.Unfocused += ViewOnFocused;

            if (oldValue == newValue || newValue == null) return;

            bool isFocused;
            if (bool.TryParse(newValue.ToString(), out isFocused) == false) return;

            if (isFocused) view.Focus();
            else view.Unfocus();
        }

        private static void ItemTappedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var listView = bindable.AsType<ListView>();
            listView.ItemTapped += async (sender, args) =>
                {
                    (newValue as ICommand)?.Execute(null);

                    await Task.Delay(TimeSpan.FromSeconds(2));

                    // Clear selection
                    listView.SelectedItem = null;
                };
        }

        private static void ViewOnFocused(object sender, FocusEventArgs focusEventArgs)
        {
            SetIsFocused(sender as BindableObject, focusEventArgs.IsFocused);
        }

        #endregion

        // ReSharper disable UnusedMember.Global - behavior

        /// <summary>
        ///     Sets the tapped command on <see cref="View" />
        /// </summary>
        /// <param name="view">View to set</param>
        /// <param name="value">
        ///     <see cref="ICommand" />
        /// </param>
        public static void SetTappedCommand(BindableObject view, ICommand value)
        {
            view.SetValue(TappedCommandProperty, value);
        }

        /// <summary>
        ///     Sets the tapped <see cref="Command" /> parameter on <see cref="View" />. Bindable Property. See
        ///     <see cref="ViewBehaviors" /> for example
        /// </summary>
        /// <param name="view">View to set</param>
        /// <param name="value">Command parameter to use</param>
        public static void SetTappedCommandParameter(BindableObject view, object value)
        {
            view.SetValue(TappedCommandParameterProperty, value);
        }

        //// ReSharper restore UnusedMember.Global
    }
}