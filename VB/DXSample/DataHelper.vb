Imports System
Imports System.Collections.ObjectModel
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Drawing
Imports System.ComponentModel

Namespace DXSample

    Public Module DataHelper

        Public Function GetData1() As ObservableCollection(Of ExampleObject)
            Dim collection As ObservableCollection(Of ExampleObject) = New ObservableCollection(Of ExampleObject)() From {New ExampleObject With {.Name = "Basket", .ImageUri = "Images/Basket.png"}, New ExampleObject With {.Name = "Check", .ImageUri = "Images/Check.png"}, New ExampleObject With {.Name = "Customer", .ImageUri = "Images/Customer.png"}}
            Return collection
        End Function

        Public Function GetData2() As ObservableCollection(Of ExampleObject)
            Dim collection As ObservableCollection(Of ExampleObject) = New ObservableCollection(Of ExampleObject)() From {New ExampleObject With {.Name = "Folder", .ImageUri = "Images/Folder.png"}, New ExampleObject With {.Name = "Key", .ImageUri = "Images/Key.png"}, New ExampleObject With {.Name = "Home", .ImageUri = "Images/Home.png"}}
            Return collection
        End Function
    End Module

    Public Class ExampleObject
        Inherits DevExpress.Mvvm.BindableBase

        ' Fields...
        Private _ImageUri As String

        Private _Name As String

        Public Property Name As String
            Get
                Return _Name
            End Get

            Set(ByVal value As String)
                SetProperty(_Name, value, Function() Name)
            End Set
        End Property

        Public Property ImageUri As String
            Get
                Return _ImageUri
            End Get

            Set(ByVal value As String)
                SetProperty(_ImageUri, value, Function() ImageUri)
                RaisePropertiesChanged("ImageSource")
            End Set
        End Property

        Public ReadOnly Property ImageSource As ImageSource
            Get
                Return If(String.IsNullOrEmpty(ImageUri), Nothing, New BitmapImage(New Uri("/DXSample;component/" & ImageUri, UriKind.Relative)))
            End Get
        End Property
    End Class
End Namespace
