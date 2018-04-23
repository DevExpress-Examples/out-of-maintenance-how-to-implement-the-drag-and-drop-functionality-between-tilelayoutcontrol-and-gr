using System;
using System.Linq;
using System.Windows;
using DevExpress.Xpf.Grid;
using System.Windows.Input;
using System.Collections.Generic;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.LayoutControl;
using System.Collections.ObjectModel;

namespace DXSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int handler = GridControl.InvalidRowHandle;
        Point point;

        public MainWindow()
        {
            InitializeComponent();
            gridControl1.ItemsSource = DataHelper.GetData1();
            tileLayoutControl1.ItemsSource = DataHelper.GetData2();
        }

        private void SetToZero()
        {
            handler = GridControl.InvalidRowHandle;
            point = new Point(-1000, -1000);
        }
        #region GridToTilelayout

        private void gridControl1_MouseMove(object sender, MouseEventArgs e)
        {


            GridControl grid = sender as GridControl;
            GridViewBase view = grid.View as GridViewBase;
            Point mousePos = e.GetPosition(null);

            if (e.LeftButton != MouseButtonState.Pressed) return;
            if (handler == GridControl.InvalidRowHandle) return;
            
            Rect rect = new Rect(
                (point.X - SystemParameters.MinimumHorizontalDragDistance),
                (point.Y - (int)SystemParameters.MinimumVerticalDragDistance),
                SystemParameters.MinimumHorizontalDragDistance * 2,
                SystemParameters.MinimumVerticalDragDistance * 2);
            
            if (!rect.Contains(mousePos))
            {
                DataObject dragData = new DataObject("GridToLC", (view.GetRowElementByRowHandle(handler).DataContext as RowData).Row);
                SetToZero();
                DragDrop.DoDragDrop(grid, dragData, DragDropEffects.Copy);
            }
        }

        private void tileLayoutControl1_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("GridToLC"))
            {
                ExampleObject obj = e.Data.GetData("GridToLC") as ExampleObject;
                TileLayoutControl tileLayout = sender as TileLayoutControl;
                (tileLayout.ItemsSource as ObservableCollection<ExampleObject>).Add(obj);
            }
        }

        private void gridControl1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                point = e.GetPosition(null);
                GridControl grid = sender as GridControl;
                GridViewHitInfoBase info;

                if (grid.View.GetType() == typeof(TableView))
                {
                    TableView view = grid.View as TableView;
                    info = view.CalcHitInfo(e.OriginalSource as DependencyObject);
                }
                else
                {
                    CardView view = grid.View as CardView;
                    info = view.CalcHitInfo(e.OriginalSource as DependencyObject);
                }

                if (info.RowHandle != GridControl.InvalidRowHandle)
                {
                    handler = info.RowHandle;
                    e.Handled = true;
                }
            }
           
        }

        private void tileLayoutControl1_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("GridToLC") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }
        #endregion
        #region TileLayoutToGrid

        private void tileLayoutControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            TileLayoutControl tileLayoutControl = sender as TileLayoutControl;
            Point mousePos = e.GetPosition(tileLayoutControl1);

            Rect rect = new Rect(
                (point.X - SystemParameters.MinimumHorizontalDragDistance),
                (point.Y - (int)SystemParameters.MinimumVerticalDragDistance),
                SystemParameters.MinimumHorizontalDragDistance * 2,
                SystemParameters.MinimumVerticalDragDistance * 2);

            if (!rect.Contains(mousePos))
            {
                IInputElement hitTest = tileLayoutControl.InputHitTest(mousePos);
                Tile tile = LayoutHelper.FindParentObject<Tile>((DependencyObject)hitTest) as Tile;
                if (tile != null)
                {
                    DataObject dragData = new DataObject("LCToGrid", tile.Content);
                    SetToZero();
                    DragDrop.DoDragDrop(tileLayoutControl, dragData, DragDropEffects.Copy);
                }
            }
        }

        private void tileLayoutControl1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                point = e.GetPosition(tileLayoutControl1);
            }
        }

        private void gridControl1_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("LCToGrid") || sender == e.Source as Tile)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void gridControl1_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("LCToGrid"))
            {
                ExampleObject obj = e.Data.GetData("LCToGrid") as ExampleObject;
                GridControl grid = sender as GridControl;
                (grid.ItemsSource as ObservableCollection<ExampleObject>).Add(obj);
            }
        }
        #endregion



    }
}
