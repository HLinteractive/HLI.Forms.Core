// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.HliFeedbackMessage.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2016
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

namespace HLI.Forms.Core.Models
{
    public class HliFeedbackMessage
    {
        public enum FeedbackType
        {
            Message,
            Error,
            Debug
        }

        public HliFeedbackMessage()
        {
        }

        public HliFeedbackMessage(FeedbackType type, string message)
        {
            this.Type = type;
            this.Message = message;
        }

        public string Message { get; }

        public FeedbackType Type { get; }
    }
}