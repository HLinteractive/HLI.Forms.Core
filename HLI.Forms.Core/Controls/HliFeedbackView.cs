// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.Core.HliFeedbackView.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using HLI.Forms.Core.Models;
using HLI.Forms.Core.Resources;

using Xamarin.Forms;

namespace HLI.Forms.Core.Controls
{
    /// <summary>
    ///     Displays feedback to the user when a <see cref="HliFeedbackMessage" /> is published through
    ///     <see cref="MessagingCenter" /> using hte key <see cref="Constants.FeedbackKeys.Message" />
    /// </summary>
    public class HliFeedbackView : Label
    {
        #region Constructors and Destructors

        public HliFeedbackView()
        {
            this.FontAttributes = FontAttributes.Bold;
            this.IsVisible = false;

            MessagingCenter.Subscribe<HliFeedbackMessage>(this, Constants.FeedbackKeys.Message, this.OnMessage);
        }

        #endregion

        #region Methods

        protected override void OnParentSet()
        {
            base.OnParentSet();
            this.IsVisible = false;
        }

        private void OnMessage(HliFeedbackMessage message)
        {
            this.IsVisible = false;
            if (string.IsNullOrWhiteSpace(message?.Message))
            {
                return;
            }

            this.Text = message.Message;
            switch (message.Type)
            {
                case HliFeedbackMessage.FeedbackType.Message:
                    this.TextColor = Colors.IntraNodesColor;
                    break;
                case HliFeedbackMessage.FeedbackType.Error:
                    this.TextColor = Colors.ContractNodesColor;
                    break;
                case HliFeedbackMessage.FeedbackType.Debug:
                    this.TextColor = Colors.AdminNodesColor;
                    break;
            }
#if DEBUG
            if (message.Type == HliFeedbackMessage.FeedbackType.Debug)
            {
                this.IsVisible = true;
            }
#else
                this.IsVisible = true;
#endif
        }

        #endregion
    }
}