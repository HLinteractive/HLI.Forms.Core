// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="XFTest.NetStandard.MainViewModel.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

using HLI.Core.Models;

namespace XFTest.NetStandard
{
    public class MainViewModel : NotificationObject
    {
        #region Fields

        private string busyReason = "Loading...";

        private bool isBusy;

        private Planet selectedItem;

        #endregion

        #region Public Properties

        public string BusyReason
        {
            get => this.busyReason;

            set => this.SetProperty(ref this.busyReason, value);
        }

        public bool IsBusy
        {
            get => this.isBusy;

            set => this.SetProperty(ref this.isBusy, value);
        }

        public List<Planet> Models => new List<Planet>
                                          {
                                              new Planet
                                                  {
                                                      Moons = 2,
                                                      Name = "Jakku",
                                                      Class = Planet.PlanetClass.DesertPlanet,
                                                      Fauna = "Bloggin, Gnaw-jaw, Luggabeast etc",
                                                      Image =
                                                          "https://vignette2.wikia.nocookie.net/starwars/images/f/f4/Jakku_-_full_-_SW_Poe_Dameron_Flight_Log_.png/revision/latest/"
                                                  },
                                              new Planet
                                                  {
                                                      Moons = 3,
                                                      Name = "Hoth",
                                                      Class = Planet.PlanetClass.Terrestrial,
                                                      Fauna = "Wampa, Tauntaun, Rayboo",
                                                      Image =
                                                          "https://vignette4.wikia.nocookie.net/starwars/images/1/1d/Hoth_SWCT.png/revision/latest/"
                                                  },
                                              new Planet
                                                  {
                                                      Name = "Kashyyyk",
                                                      Moons = 3,
                                                      Class = Planet.PlanetClass.Terrestrial,
                                                      Fauna = "Arrawtha-dyr, Can-cells, Terentatek etc",
                                                      Image =
                                                          "https://vignette3.wikia.nocookie.net/swtor/images/0/01/Kashyyyk.jpg/revision/latest/"
                                                  },
                                              new Planet
                                                  {
                                                      Name = "Alderaan",
                                                      Class = Planet.PlanetClass.Terrestrial,
                                                      Moons = 0,
                                                      Fauna = "Alderaanian wolf-cat",
                                                      Image =
                                                          "https://vignette1.wikia.nocookie.net/starwars/images/4/4a/Alderaan.jpg/revision/latest/"
                                                  },
                                              new Planet
                                                  {
                                                      Name = "Naboo",
                                                      Moons = 2,
                                                      Image =
                                                          "https://vignette4.wikia.nocookie.net/starwars/images/5/50/Naboo.jpg/revision/latest/",
                                                      Class = Planet.PlanetClass.Terrestrial,
                                                      Fauna = "Colo claw fish, Falumpaset, Kaadu, Nuna, Opee sea killer etc"
                                                  },
                                              new Planet
                                                  {
                                                      Name = "Bespin",
                                                      Moons = 2,
                                                      Class = Planet.PlanetClass.GasGiant,
                                                      Fauna = "Air shrimp, Beldon, Crab glider, Rawwk, Velker",
                                                      Image =
                                                          "https://vignette2.wikia.nocookie.net/starwars/images/1/11/Bespin-SWCT.png/revision/latest/"
                                                  }
                                          };

        public Planet SelectedItem
        {
            get => this.selectedItem;
            set => this.SetProperty(ref this.selectedItem, value);
        }

        #endregion
    }
}