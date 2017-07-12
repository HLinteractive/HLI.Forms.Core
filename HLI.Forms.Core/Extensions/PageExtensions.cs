// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.PageExtensions.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2016
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using Xamarin.Forms;

namespace HLI.Forms.Core.Extensions
{
    /// <summary>
    /// Extends the Page class
    /// </summary>
    public static class PageExtensions
    {
        /// <summary>
        /// <para>Allows the page to pull from the Global styles</para>
        /// <para>Source https://forums.xamarin.com/discussion/30951/xamarin-forms-global-code-styles-not-working</para>
        /// </summary>
        /// <param name="page">this</param>
        public static void LoadResourcesFromApp(this Page page)
        {
            page.Resources = page.Resources ?? Application.Current.Resources;
        }
    }
}