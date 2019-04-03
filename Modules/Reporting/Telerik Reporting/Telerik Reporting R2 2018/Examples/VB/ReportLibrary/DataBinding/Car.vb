
Imports System.Collections


Public Class Car
    Private m_manufacturer As String
    Private m_model As String
    Private m_year As Integer
    Private m_imageUrl As String
    Private m_availableColor As ArrayList

    Public Sub New(manufacturer As String, model As String, year As Integer, imageUrl As String, availableColor As String())
        Me.m_manufacturer = manufacturer
        Me.m_model = model
        Me.m_year = year
        Me.m_imageUrl = imageUrl
        Me.AvailableColor = New ArrayList(availableColor)
    End Sub

    Public Property Model() As String
        Get
            Return Me.m_model
        End Get
        Set(value As String)
            Me.m_model = Value
        End Set
    End Property

    Public Property Manufacturer() As String
        Get
            Return Me.m_manufacturer
        End Get
        Set(value As String)
            Me.m_manufacturer = Value
        End Set
    End Property

    Public Property Year() As Integer
        Get
            Return Me.m_year
        End Get
        Set(value As Integer)
            Me.m_year = Value
        End Set
    End Property

    Public Property ImageUrl() As String
        Get
            Return Me.m_imageUrl
        End Get
        Set(value As String)
            Me.m_imageUrl = Value
        End Set
    End Property

    Public Property AvailableColor() As ArrayList
        Get
            Return Me.m_availableColor
        End Get
        Set(value As ArrayList)
            Me.m_availableColor = Value
        End Set
    End Property
End Class

