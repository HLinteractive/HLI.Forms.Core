// // // --------------------------------------------------------------------------------------------------------------------
// // // <copyright file="HLI.Forms.HliWrapLayout.cs" company="Sogeti Sverige AB">
// // //   Copyright © Sogeti Sverige AB, 2016
// // // </copyright>
// // // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

namespace HLI.Forms.Core.Controls
{
    /// <summary>
    ///     Wraps children
    /// </summary>
    /// <remarks>https://github.com/conceptdev/xamarin-forms-samples/blob/master/Evolve13/Evolve13/Controls/HliWrapLayout.cs</remarks>
    public class HliWrapLayout : Layout<View>
    {
        #region Static Fields

        /// <summary>
        ///     See <see cref="Spacing" />
        /// </summary>
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create(
            nameof(Spacing),
            typeof(double),
            typeof(HliWrapLayout),
            5.0d,
            propertyChanged: (bindable, oldvalue, newvalue) => ((HliWrapLayout)bindable).layoutCache.Clear());

        #endregion

        #region Fields

        readonly Dictionary<View, SizeRequest> layoutCache = new Dictionary<View, SizeRequest>();

        #endregion

        #region Constructors and Destructors

        public HliWrapLayout()
        {
            this.VerticalOptions = this.HorizontalOptions = LayoutOptions.StartAndExpand;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Spacing added between elements (both directions). Default value is 5
        /// </summary>
        /// <value>The spacing.</value>
        public double Spacing
        {
            get => (double)this.GetValue(SpacingProperty);

            set => this.SetValue(SpacingProperty, value);
        }

        #endregion

        #region Methods

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            double lastX, lastY;
            var layout = this.NaiveLayout(width, height, out lastX, out lastY);

            foreach (var t in layout)
            {
                var offset = (int)((width - t.Last().Item2.Right) / 2);
                foreach (var dingus in t)
                {
                    var location = new Rectangle(dingus.Item2.X + x + offset, dingus.Item2.Y + y, dingus.Item2.Width, dingus.Item2.Height);
                    LayoutChildIntoBoundingRegion(dingus.Item1, location);
                }
            }
        }

        protected override void OnChildMeasureInvalidated()
        {
            base.OnChildMeasureInvalidated();
            this.layoutCache.Clear();
        }

        #region Overrides of VisualElement

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            double lastX;
            double lastY;
            var layout = this.NaiveLayout(widthConstraint, heightConstraint, out lastX, out lastY);

            return new SizeRequest(new Size(lastX, lastY));
        }

        #endregion

        private List<List<Tuple<View, Rectangle>>> NaiveLayout(double width, double height, out double lastX, out double lastY)
        {
            double startX = 0;
            double startY = 0;
            var right = width;
            double nextY = 0;

            lastX = 0;
            lastY = 0;

            var result = new List<List<Tuple<View, Rectangle>>>();
            var currentList = new List<Tuple<View, Rectangle>>();

            foreach (var child in this.Children)
            {
                SizeRequest sizeRequest;
                if (!this.layoutCache.TryGetValue(child, out sizeRequest))
                {
                    this.layoutCache[child] = sizeRequest = child.Measure(double.PositiveInfinity, double.PositiveInfinity);
                }

                var paddedWidth = sizeRequest.Request.Width + this.Spacing;
                var paddedHeight = sizeRequest.Request.Height + this.Spacing;

                if (startX + paddedWidth > right)
                {
                    startX = 0;
                    startY += nextY;

                    if (currentList.Count > 0)
                    {
                        result.Add(currentList);
                        currentList = new List<Tuple<View, Rectangle>>();
                    }
                }

                currentList.Add(
                    new Tuple<View, Rectangle>(child, new Rectangle(startX, startY, sizeRequest.Request.Width, sizeRequest.Request.Height)));

                lastX = Math.Max(lastX, startX + paddedWidth);
                lastY = Math.Max(lastY, startY + paddedHeight);

                nextY = Math.Max(nextY, paddedHeight);
                startX += paddedWidth;
            }

            result.Add(currentList);
            return result;
        }

        #endregion
    }
}