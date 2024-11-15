using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GammaItems.Source
{
    //public interface ICustomSorter : IComparer
    //{
    //    ListSortDirection SortDirection { get; set; }
    //}
    //class NaturalSorter : NaturalSortComparer, ICustomSorter
    //{
    //    public NaturalSorter() : base(StringComparison.OrdinalIgnoreCase) { }
    //    public ListSortDirection SortDirection { get; set; }

    //    public int Compare(object x, object y) =>
    //        SortDirection == ListSortDirection.Descending ?
    //            base.Compare((x as ItemDisplay).Name, (y as ItemDisplay).Name) :
    //            -base.Compare((x as ItemDisplay).Name, (y as ItemDisplay).Name);
    //}
    //public class CustomSortBehaviour
    //{
    //    public static readonly DependencyProperty CustomSorterProperty =
    //        DependencyProperty.RegisterAttached("CustomSorter", typeof(ICustomSorter), typeof(CustomSortBehaviour));

    //    public static ICustomSorter GetCustomSorter(DataGridColumn gridColumn)
    //    {
    //        return (ICustomSorter)gridColumn.GetValue(CustomSorterProperty);
    //    }

    //    public static void SetCustomSorter(DataGridColumn gridColumn, ICustomSorter value)
    //    {
    //        gridColumn.SetValue(CustomSorterProperty, value);
    //    }

    //    public static readonly DependencyProperty AllowCustomSortProperty =
    //        DependencyProperty.RegisterAttached("AllowCustomSort", typeof(bool),
    //        typeof(CustomSortBehaviour), new UIPropertyMetadata(false, OnAllowCustomSortChanged));

    //    public static bool GetAllowCustomSort(DataGrid grid)
    //    {
    //        return (bool)grid.GetValue(AllowCustomSortProperty);
    //    }

    //    public static void SetAllowCustomSort(DataGrid grid, bool value)
    //    {
    //        grid.SetValue(AllowCustomSortProperty, value);
    //    }

    //    private static void OnAllowCustomSortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        var existing = d as DataGrid;
    //        if (existing == null) return;

    //        var oldAllow = (bool)e.OldValue;
    //        var newAllow = (bool)e.NewValue;

    //        if (!oldAllow && newAllow)
    //        {
    //            existing.Sorting += HandleCustomSorting;
    //        }
    //        else
    //        {
    //            existing.Sorting -= HandleCustomSorting;
    //        }
    //    }

    //    private static void HandleCustomSorting(object sender, DataGridSortingEventArgs e)
    //    {
    //        var dataGrid = sender as DataGrid;
    //        if (dataGrid == null || !GetAllowCustomSort(dataGrid)) return;

    //        var listColView = dataGrid.ItemsSource as ListCollectionView;
    //        if (listColView == null)
    //            throw new Exception("The DataGrid's ItemsSource property must be of type, ListCollectionView");

    //        // Sanity check
    //        var sorter = GetCustomSorter(e.Column);
    //        if (sorter == null) return;

    //        // The guts.
    //        e.Handled = true;

    //        var direction = (e.Column.SortDirection != ListSortDirection.Ascending)
    //                            ? ListSortDirection.Ascending
    //                            : ListSortDirection.Descending;

    //        e.Column.SortDirection = sorter.SortDirection = direction;
    //        listColView.CustomSort = sorter;
    //    }
    //}
}
