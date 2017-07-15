// // // --------------------------------------------------------------------------------------------------------------------
// // // <copyright file="HLI.Forms.HliLinkButton.cs" company="Sogeti Sverige AB">
// // //   Copyright © Sogeti Sverige AB, 2016
// // // </copyright>
// // // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Windows.Input;

using HLI.Forms.Core.Extensions;

using Xamarin.Forms;

namespace HLI.Forms.Core.Controls
{
    /// <summary>
    /// Displays text as a simple clickable link. Supports <see cref="Command"/> and <see cref="ClickedEvent"/>
    /// </summary>
    public class HliLinkButton : ContentView
    {
        #region Static Fields

        /// <summary>
        ///     See <see cref="CommandParameter" />
        /// </summary>
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            nameof(CommandParameter),
            typeof(object),
            typeof(HliLinkButton));

        /// <summary>
        ///     See <see cref="Command" />
        /// </summary>
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(HliLinkButton));

        /// <summary>
        ///     See <see cref="Text" />
        /// </summary>
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(HliLinkButton),
            null,
            BindingMode.TwoWay,
            propertyChanged: TextChanged);

        #endregion

        #region Constructors and Destructors

        public HliLinkButton()
        {
            this.LoadAppResources();
            this.AddTapGestureRecognizer(this.TappedEventHandler);
        }

        #endregion

        #region Public Events

        /// <summary>
        ///     Event that executes when button is clicked
        /// </summary>
        public event EventHandler ClickedEvent;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Command that executed then the button is clicked. Default is null.
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)this.GetValue(CommandProperty);

            set => this.SetValue(CommandProperty, value);
        }

        /// <summary>
        ///     Gets or sets the command parameter.
        /// </summary>
        public object CommandParameter
        {
            get => this.GetValue(CommandParameterProperty);

            set => this.SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        ///     Gets or sets the the button content from specifed <c>string</c>
        /// </summary>
        public string Text
        {
            get => (string)this.GetValue(TextProperty);

            set => this.SetValue(TextProperty, value);
        }

        #endregion

        #region Methods

        private static void TextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((HliLinkButton)bindable).Content = new Label { Text = (newValue ?? string.Empty).ToString() };
        }

        /// <summary>
        ///     Executes the <see cref="Command" /> and <see cref="ClickedEvent" /> if set
        /// </summary>
        /// <param name="sender">this</param>
        /// <param name="eventArgs">Arguments</param>
        private void TappedEventHandler(object sender, EventArgs eventArgs)
        {
            var cmd = this.Command;
            if (cmd != null && cmd.CanExecute(null))
            {
                cmd.Execute(this.CommandParameter);
            }

            var clicked = this.ClickedEvent;
            clicked?.Invoke(this, new EventArgs());
        }

        #endregion
    }
}