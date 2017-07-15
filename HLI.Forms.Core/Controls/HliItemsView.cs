// // // --------------------------------------------------------------------------------------------------------------------
// // // <copyright file="HLI.Forms.HliItemsView.cs" company="Sogeti Sverige AB">
// // //   Copyright © Sogeti Sverige AB, 2016
// // // </copyright>
// // // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Specialized;

using HLI.Forms.Core.Extensions;

using Xamarin.Forms;

namespace HLI.Forms.Core.Controls
{
    /// <summary>
    ///     <para>A simple listview for a smaller collection when `ListView` is not required.</para>
    ///     <para>Binds <see cref="ItemsSource"/> using <see cref="ItemTemplate"/></para>
    /// </summary>
    /// <remarks>Will not perform well with long list</remarks>
    public class HliItemsView : ScrollView
    {
        #region Static Fields

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
            nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(HliItemsView),
            null,
            propertyChanged: (bindable, value, newValue) => bindable.AsType<HliItemsView>().Populate());

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(HliItemsView),
            null,
            BindingMode.OneWay,
            propertyChanged: OnItemsSourceChanged);

        #endregion

        #region Constructors and Destructors

        public HliItemsView()
        {
            this.LoadAppResources();
        }

        #endregion

        #region Public Properties

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)this.GetValue(ItemsSourceProperty);

            set => this.SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        ///     Template used for items. Must be a <see cref="View" /> or a <see cref="ViewCell" />
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)this.GetValue(ItemTemplateProperty);

            set => this.SetValue(ItemTemplateProperty, value);
        }

        #endregion

        #region Methods

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var obsColl = newValue as INotifyCollectionChanged;
            if (obsColl != null)
            {
                obsColl.CollectionChanged += (sender, args) => bindable.AsType<HliItemsView>().Populate();
            }

            bindable.AsType<HliItemsView>().Populate();
        }

        private void Populate()
        {
            // Clean
            this.Content = null;

            // Only populate once both properties are recieved
            if (this.ItemsSource == null || this.ItemTemplate == null)
            {
                return;
            }

            // Create a stack to populate with items
            var list = new StackLayout
                           {
                               Orientation = this.Orientation == ScrollOrientation.Horizontal ? StackOrientation.Horizontal : StackOrientation.Vertical
                           };

            foreach (var viewModel in this.ItemsSource)
            {
                var content = this.ItemTemplate.CreateContent();
                if (!(content is View) && !(content is ViewCell))
                {
                    throw new Exception($"Invalid visual object {nameof(content)}");
                }

                var view = content is View ? content as View : ((ViewCell)content).View;
                view.BindingContext = viewModel;

                list.Children.Add(view);
            }

            // Set stack as conent to this ScrollView
            this.Content = list;
        }

        #endregion
    }
}