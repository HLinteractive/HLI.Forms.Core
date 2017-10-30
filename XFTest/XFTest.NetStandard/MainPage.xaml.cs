// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="XFTest.NetStandard.MainPage.xaml.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using Xamarin.Forms;

namespace XFTest.NetStandard
{
    public partial class MainPage
    {
        #region Constructors and Destructors

        public MainPage()
        {
            this.InitializeComponent();
            this.Resources = Application.Current.Resources;
        }

        #endregion
    }
}