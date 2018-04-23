Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Collections.ObjectModel
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Drawing
Imports System.ComponentModel

Namespace DXSample
	Public NotInheritable Class DataHelper

		Private Sub New()
		End Sub

		Public Shared Function GetData1() As ObservableCollection(Of ExampleObject)
			Dim collection As New ObservableCollection(Of ExampleObject)() From { _
				New ExampleObject With {.Name = "Basket", .ImageUri ="Images/Basket.png"}, _
				New ExampleObject With {.Name = "Check", .ImageUri ="Images/Check.png"}, _
				New ExampleObject With {.Name = "Customer", .ImageUri ="Images/Customer.png"} _
			}
			Return collection
		End Function
		Public Shared Function GetData2() As ObservableCollection(Of ExampleObject)
			Dim collection As New ObservableCollection(Of ExampleObject)() From { _
				New ExampleObject With {.Name = "Folder", .ImageUri ="Images/Folder.png"}, _
				New ExampleObject With {.Name = "Key", .ImageUri ="Images/Key.png"}, _
				New ExampleObject With {.Name = "Home", .ImageUri ="Images/Home.png"} _
			}
			Return collection
		End Function
	End Class
	Public Class ExampleObject
		Implements INotifyPropertyChanged

		' Fields...
		Private _ImageUri As String
		Private _Name As String

		Public Property Name() As String
			Get
				Return _Name
			End Get
			Set(ByVal value As String)
				If _Name = value Then
					Return
				End If
				_Name = value
				NotifyPropertyChanged("Name")
			End Set
		End Property

		Public Property ImageUri() As String
			Get
				Return _ImageUri
			End Get
			Set(ByVal value As String)
				If _ImageUri = value Then
					Return
				End If
				_ImageUri = value
				NotifyPropertyChanged("ImageUri")
				NotifyPropertyChanged("ImageSource")
			End Set
		End Property

		Public ReadOnly Property ImageSource() As ImageSource
			Get
				Return If(String.IsNullOrEmpty(ImageUri), Nothing, New BitmapImage(New Uri("/DXSample;component/" & ImageUri, UriKind.Relative)))
			End Get
		End Property

		Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
		Protected Sub NotifyPropertyChanged(ByVal info As String)
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(info))
		End Sub
	End Class
End Namespace
