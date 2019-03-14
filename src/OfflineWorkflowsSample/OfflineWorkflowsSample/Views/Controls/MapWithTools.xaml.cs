﻿using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Esri.ArcGISRuntime.Mapping;
using OfflineWorkflowsSample;
using OfflineWorkflowSample.ViewModels;

namespace OfflineWorkflowSample
{
    public sealed partial class MapWithTools : UserControl
    {
        // Dependency properties enable specifying custom bindable properties, in this case map and item.
        public static readonly DependencyProperty MapProperty =
            DependencyProperty.Register(
                nameof(Map), typeof(Map),
                typeof(MapWithTools), null
            );

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register(
                nameof(Item), typeof(PortalItemViewModel),
                typeof(MapWithTools), null
            );

        public MapWithTools()
        {
            InitializeComponent();
            DataContext = this;
        }

        private MainViewModel ViewModel => (MainViewModel) Application.Current.Resources[nameof(MainViewModel)];

        public Map Map
        {
            get => (Map) GetValue(MapProperty);
            set => SetValue(MapProperty, value);
        }

        public PortalItemViewModel Item
        {
            get => (PortalItemViewModel) GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }

        #region Pivot item hack

        // Pivot item hack needed to prevent UWP layout cycle, which results in a crash.

        private List<object> _pivotContents;

        private void MenuButtonClicked(object sender, RoutedEventArgs e)
        {
            MapLegendSplitView.IsPaneOpen = !MapLegendSplitView.IsPaneOpen;
        }

        private void MapLegendSplitView_OnPaneClosing(SplitView sender, SplitViewPaneClosingEventArgs args)
        {
            if (OuterPivot.Items.Any())
            {
                _pivotContents = OuterPivot.Items.ToList();
                OuterPivot.Items.Clear();
            }
        }

        private void MapLegendSplitView_OnPaneOpening(SplitView sender, object args)
        {
            if (_pivotContents != null)
            {
                OuterPivot.Items.Clear();
                foreach (var item in _pivotContents)
                    OuterPivot.Items.Add(item);
                _pivotContents = null;
            }
        }

        #endregion
    }
}