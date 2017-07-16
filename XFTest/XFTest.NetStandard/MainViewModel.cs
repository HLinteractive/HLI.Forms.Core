// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="XFTest.NetStandard.MainViewModel.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XFTest.NetStandard
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Fields

        private Planet selectedItem;

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public List<Planet> Models => new List<Planet>
                                          {
                                              new Planet { Moons = 2, Name = "Tatoinne" },
                                              new Planet { Moons = 1, Name = "Hoth" },
                                              new Planet { Name = "Mustafar", Moons = 6 },
                                              new Planet { Name = "Dantoinne", Moons = 3 },
                                              new Planet { Name = "Kashyyyk", Moons = 5 },
                                              new Planet { Name = "Alderaan", Moons = 2 },
                                              new Planet { Name = "Naboo", Moons = 4 },
                                              new Planet { Name = "Bespin", Moons = 2 },
                                              new Planet { Name = "Yavin", Moons = 5 },
                                              new Planet { Name = "Endor", Moons = 3 }
                                          };

        public Planet SelectedItem
        {
            get => this.selectedItem;
            set
            {
                this.selectedItem = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region Methods

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}