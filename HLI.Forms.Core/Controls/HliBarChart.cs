// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="HLI.Forms.Core.HliBarChart.cs" company="HL Interactive">
// //   Copyright © HL Interactive, Stockholm, Sweden, 2017
// // </copyright>
// // --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using HLI.Core.Extensions;

using Xamarin.Forms;

namespace HLI.Forms.Core.Controls
{
    /// <summary>
    ///     Creates a simple bar chart from <see cref="ItemsSource" /> using <see cref="ValuePath" /> and
    ///     <see cref="LabelPath" /> to determine object members to display. Customize using <see cref="IsPercent" />
    /// </summary>
    public class HliBarChart : StackLayout
    {
        #region Static Fields

        /// <summary>
        ///     See <see cref="ItemsSource" />
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(HliBarChart),
            new List<object>(),
            propertyChanged: OnItemsSourceChanged);

        /// <summary>
        ///     See <see cref="BarScale" />
        /// </summary>
        public static readonly BindableProperty BarScaleProperty =
            BindableProperty.Create(nameof(BarScale), typeof(float), typeof(HliBarChart), 1.0f, propertyChanged: OnBarScaleChanged);

        /// <summary>
        ///     See <see cref="IsPercent" />
        /// </summary>
        public static readonly BindableProperty IsPercentProperty =
            BindableProperty.Create(nameof(IsPercent), typeof(bool), typeof(HliBarChart), false);

        /// <summary>
        ///     See <see cref="LabelPath" />
        /// </summary>
        public static readonly BindableProperty LabelPathProperty = BindableProperty.Create(nameof(LabelPath), typeof(string), typeof(HliBarChart));

        /// <summary>
        ///     See <see cref="ValuePath" />
        /// </summary>
        public static readonly BindableProperty ValuePathProperty = BindableProperty.Create(nameof(ValuePath), typeof(string), typeof(HliBarChart));

        private static readonly List<Color> Colors =
            new List<Color>
                {
                    Color.Aqua,
                    Color.Blue,
                    Color.Fuchsia,
                    Color.Green,
                    Color.Purple,
                    Color.Teal,
                    Color.Yellow,
                    Color.Olive,
                    Color.Lime,
                    Color.AliceBlue,
                    Color.AntiqueWhite,
                    Color.Beige,
                    Color.DarkMagenta,
                    Color.DarkOrange,
                    Color.DarkSeaGreen
                };

        #endregion

        #region Fields

        private int currentColor;

        #endregion

        #region Constructors and Destructors

        public HliBarChart()
        {
            this.Padding = new Thickness(0, 10);
            this.Orientation = StackOrientation.Horizontal;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Determines the scale of the chart's bars. Default is 1.0
        /// </summary>
        public float BarScale
        {
            get => (float)this.GetValue(BarScaleProperty);

            set => this.SetValue(BarScaleProperty, value);
        }

        /// <summary>
        ///     Determines if the value should be displayed as a percent
        /// </summary>
        public bool IsPercent
        {
            get => (bool)this.GetValue(IsPercentProperty);

            set => this.SetValue(IsPercentProperty, value);
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)this.GetValue(ItemsSourceProperty);

            set => this.SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        ///     LabelPath property of items, used as category axis
        /// </summary>
        public string LabelPath
        {
            get => (string)this.GetValue(LabelPathProperty);

            set => this.SetValue(LabelPathProperty, value);
        }

        /// <summary>
        ///     ValuePath property of bound items
        /// </summary>
        public string ValuePath
        {
            get => (string)this.GetValue(ValuePathProperty);

            set => this.SetValue(ValuePathProperty, value);
        }

        #endregion

        #region Methods

        private static void OnBarScaleChanged(BindableObject bindable, object o, object newValue)
        {
            ((HliBarChart)bindable).Load();
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object o, object newValue)
        {
            if (newValue != null)
            {
                ((HliBarChart)bindable).Load();
            }
        }

        private StackLayout CreateBar(object item)
        {
            var percent = this.GetPercent(item);

            var label = this.IsPercent
                            ? new Label { Text = percent.ToString("P0"), FontSize = 10 }
                            : new Label { Text = item.GetValueForProperty(this.ValuePath).ToString(), FontSize = 10 };

            var barView = new BoxView
                              {
                                  BackgroundColor = this.GetNextColor(),
                                  HeightRequest = percent * 100 * this.BarScale,
                                  VerticalOptions = LayoutOptions.Start
                              };
            var titleLabel = new Label { Text = item.GetValueForProperty(this.LabelPath).ToString(), FontSize = 10 };

            return new StackLayout
                       {
                           Orientation = StackOrientation.Vertical,
                           Children = { label, barView, titleLabel },
                           VerticalOptions = LayoutOptions.End
                       };
        }

        private BoxView CreateSpace()
        {
            return new BoxView { Color = Color.Transparent, WidthRequest = 10, HeightRequest = 1 };
        }

        private Color GetNextColor()
        {
            this.currentColor = this.currentColor + 1;
            if (this.currentColor > Colors.Count)
            {
                this.currentColor = 0;
            }

            var color = Colors[this.currentColor];
            return color;
        }

        private double GetPercent(object item)
        {
            var percent = double.Parse(item.GetValueForProperty(this.ValuePath).ToString()) / this.ItemsSource.OfType<object>()
                              .Sum(i => double.Parse(i.GetValueForProperty(this.ValuePath).ToString()));
            return percent;
        }

        private void Load()
        {
            if (this.ItemsSource == null)
            {
                return;
            }

            this.Children.Clear();
            foreach (var item in this.ItemsSource)
            {
                this.Children.Add(this.CreateSpace());
                this.Children.Add(this.CreateBar(item));
            }
        }

        #endregion
    }
}