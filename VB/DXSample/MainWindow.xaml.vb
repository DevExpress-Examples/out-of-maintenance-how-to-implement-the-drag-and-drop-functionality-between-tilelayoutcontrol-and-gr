Imports System
Imports System.Linq
Imports System.Windows
Imports DevExpress.Xpf.Grid
Imports System.Windows.Input
Imports System.Collections.Generic
Imports DevExpress.Xpf.Core.Native
Imports DevExpress.Xpf.LayoutControl
Imports System.Collections.ObjectModel

Namespace DXSample
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window

		Private handler As Integer = GridControl.InvalidRowHandle
		Private point As Point

		Public Sub New()
			InitializeComponent()
			gridControl1.ItemsSource = DataHelper.GetData1()
			tileLayoutControl1.ItemsSource = DataHelper.GetData2()
		End Sub

		Private Sub SetToZero()
			handler = GridControl.InvalidRowHandle
			point = New Point(-1000, -1000)
		End Sub
		#Region "GridToTilelayout"

		Private Sub gridControl1_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)


			Dim grid As GridControl = TryCast(sender, GridControl)
			Dim view As GridViewBase = TryCast(grid.View, GridViewBase)
			Dim mousePos As Point = e.GetPosition(Nothing)

			If e.LeftButton <> MouseButtonState.Pressed Then
				Return
			End If
			If handler = GridControl.InvalidRowHandle Then
				Return
			End If

			Dim rect As New Rect((point.X - SystemParameters.MinimumHorizontalDragDistance), (point.Y - CInt(Math.Truncate(SystemParameters.MinimumVerticalDragDistance))), SystemParameters.MinimumHorizontalDragDistance * 2, SystemParameters.MinimumVerticalDragDistance * 2)

			If Not rect.Contains(mousePos) Then
				Dim dragData As New DataObject("GridToLC", (TryCast(view.GetRowElementByRowHandle(handler).DataContext, RowData)).Row)
				SetToZero()
				DragDrop.DoDragDrop(grid, dragData, DragDropEffects.Copy)
			End If
		End Sub

		Private Sub tileLayoutControl1_Drop(ByVal sender As Object, ByVal e As DragEventArgs)
			If e.Data.GetDataPresent("GridToLC") Then
				Dim obj As ExampleObject = TryCast(e.Data.GetData("GridToLC"), ExampleObject)
				Dim tileLayout As TileLayoutControl = TryCast(sender, TileLayoutControl)
				TryCast(tileLayout.ItemsSource, ObservableCollection(Of ExampleObject)).Add(obj)
			End If
		End Sub

		Private Sub gridControl1_PreviewMouseDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
			If e.LeftButton = MouseButtonState.Pressed Then
				point = e.GetPosition(Nothing)
				Dim grid As GridControl = TryCast(sender, GridControl)
				Dim info As GridViewHitInfoBase

				If grid.View.GetType() Is GetType(TableView) Then
					Dim view As TableView = TryCast(grid.View, TableView)
					info = view.CalcHitInfo(TryCast(e.OriginalSource, DependencyObject))
				Else
					Dim view As CardView = TryCast(grid.View, CardView)
					info = view.CalcHitInfo(TryCast(e.OriginalSource, DependencyObject))
				End If

				If info.RowHandle <> GridControl.InvalidRowHandle Then
					handler = info.RowHandle
					e.Handled = True
				End If
			End If

		End Sub

		Private Sub tileLayoutControl1_DragEnter(ByVal sender As Object, ByVal e As DragEventArgs)
			If (Not e.Data.GetDataPresent("GridToLC")) OrElse sender Is e.Source Then
				e.Effects = DragDropEffects.None
			End If
		End Sub
		#End Region
		#Region "TileLayoutToGrid"

		Private Sub tileLayoutControl1_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
			If e.LeftButton <> MouseButtonState.Pressed Then
				Return
			End If
			Dim tileLayoutControl As TileLayoutControl = TryCast(sender, TileLayoutControl)
			Dim mousePos As Point = e.GetPosition(tileLayoutControl1)

			Dim rect As New Rect((point.X - SystemParameters.MinimumHorizontalDragDistance), (point.Y - CInt(Math.Truncate(SystemParameters.MinimumVerticalDragDistance))), SystemParameters.MinimumHorizontalDragDistance * 2, SystemParameters.MinimumVerticalDragDistance * 2)

			If Not rect.Contains(mousePos) Then
				Dim hitTest As IInputElement = tileLayoutControl.InputHitTest(mousePos)
				Dim tile As Tile = TryCast(LayoutHelper.FindParentObject(Of Tile)(CType(hitTest, DependencyObject)), Tile)
				If tile IsNot Nothing Then
					Dim dragData As New DataObject("LCToGrid", tile.Content)
					SetToZero()
					DragDrop.DoDragDrop(tileLayoutControl, dragData, DragDropEffects.Copy)
				End If
			End If
		End Sub

		Private Sub tileLayoutControl1_PreviewMouseDown(ByVal sender As Object, ByVal e As MouseButtonEventArgs)
			If e.LeftButton = MouseButtonState.Pressed Then
				point = e.GetPosition(tileLayoutControl1)
			End If
		End Sub

		Private Sub gridControl1_DragEnter(ByVal sender As Object, ByVal e As DragEventArgs)
			If (Not e.Data.GetDataPresent("LCToGrid")) OrElse sender Is TryCast(e.Source, Tile) Then
				e.Effects = DragDropEffects.None
			End If
		End Sub

		Private Sub gridControl1_Drop(ByVal sender As Object, ByVal e As DragEventArgs)
			If e.Data.GetDataPresent("LCToGrid") Then
				Dim obj As ExampleObject = TryCast(e.Data.GetData("LCToGrid"), ExampleObject)
				Dim grid As GridControl = TryCast(sender, GridControl)
				TryCast(grid.ItemsSource, ObservableCollection(Of ExampleObject)).Add(obj)
			End If
		End Sub
		#End Region



	End Class
End Namespace
