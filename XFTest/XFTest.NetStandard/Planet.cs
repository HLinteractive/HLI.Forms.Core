// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="XFTest.NetStandard.Planet.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------
namespace XFTest.NetStandard
{
    public class Planet
    {
        public bool IsPlanetoid => true;

        public string Name { get; set; }

        public double Moons { get; set; }
    }
}