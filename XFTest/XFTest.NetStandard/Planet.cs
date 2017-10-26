// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="XFTest.NetStandard.Planet.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

namespace XFTest.NetStandard
{
    public class Planet
    {
        #region Constants

        private const string ThumbnailSuffix = "scale-to-width-down/50";

        #endregion

        #region Enums

        public enum PlanetClass
        {
            Terrestrial,

            GasGiant,

            DesertPlanet
        }

        #endregion

        #region Public Properties

        public PlanetClass Class { get; set; }

        public string Fauna { get; set; }

        public string Image { get; set; }

        public double Moons { get; set; }

        public string Name { get; set; }

        public string Thumbnail => this.Image + ThumbnailSuffix;

        #endregion
    }
}