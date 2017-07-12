// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.IAnalyticsService.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2016
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

namespace HLI.Forms.Core.Interfaces
{
    /// <summary>
    ///     Analytics &amp; insights implementation
    /// </summary>
    public interface IAnalyticsService
    {
        /// <summary> 
        ///     Track an event with metrics 
        /// </summary> 
        /// <param name="eventMessage">Event message</param> 
        void RegisterEvent(string eventMessage);

        /// <summary> 
        ///     Display feedback window to user 
        /// </summary> 
        void ShowFeedbackWindow();

        /// <summary> 
        /// Update the user's contact info 
        /// </summary> 
        /// <param name="user">The user name</param> 
        /// <param name="email">The user's contact info</param> 
        void UpdateContactInfo(string user, string email);

        /// <summary> 
        /// Track that the user has viewed a specific page 
        /// </summary> 
        /// <param name="page">Name of the page</param> 
        void TrackPageView(string page);

        /// <summary> 
        /// Track a specific metric 
        /// </summary> 
        /// <param name="name">Name of the metric</param> 
        /// <param name="key">Key of the metric</param> 
        /// <param name="value">Value of the metric (i.e number of items)</param> 
        void TrackMetric(string name, string key, double value);
    }
}