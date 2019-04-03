Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Reflection
Imports System.Text

Public Class ReportInfo
    Private m_name As String
    Private m_description As String
    Private m_reportType As Type
    Private m_index As Integer

    Public Property Name() As String
        Get
            Return Me.m_name
        End Get
        Set(value As String)
            Me.m_name = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return Me.m_description
        End Get
        Set(value As String)
            Me.m_description = value
        End Set
    End Property

    Public ReadOnly Property AssemblyQualifiedName() As String
        Get
            Return Me.m_reportType.AssemblyQualifiedName
        End Get
    End Property

    Public Property ReportType() As Type
        Get
            Return Me.m_reportType
        End Get
        Set(value As Type)
            Me.m_reportType = value
        End Set
    End Property

    Public Property Index() As Integer
        Get
            Return Me.m_index
        End Get
        Set(value As Integer)
            Me.m_index = value
        End Set
    End Property

    Public Sub New(name As String, description As String, reportType As Type, index As Integer)
        Me.m_name = name
        Me.m_description = description
        Me.m_reportType = reportType
        Me.m_index = index
    End Sub
End Class

Class ReportComparer
    Implements IComparer(Of Type)

    Public Function Compare(t1 As Type, t2 As Type) As Integer Implements IComparer(Of Type).Compare
        Return t1.Name.CompareTo(t2.Name)
    End Function

End Class

Public Class ReportManager
    Private reportAssembly As Assembly

    Public Sub New()
        Me.New(GetType(ReportManager).Assembly)
    End Sub

    Public Sub New(reportAssembly As Assembly)
        Me.reportAssembly = reportAssembly
    End Sub

    Public Function GetReports() As IEnumerable(Of ReportInfo)
        Dim infos As New List(Of ReportInfo)
        If Me.reportAssembly IsNot Nothing Then
            Dim types As New List(Of Type)()
            For Each t As Type In Me.reportAssembly.GetTypes()
                If IsValidReportType(t) Then
                    types.Add(t)
                End If
            Next

            types.Sort(New ReportComparer)

            For i As Integer = 0 To types.Count - 1
                infos.Add(CreateReportInfo(types(i), i))
            Next
        End If
        Return infos
    End Function

    Private Shared Function IsValidReportType(t As Type) As Boolean
        If GetType(Telerik.Reporting.IReportDocument).IsAssignableFrom(t) AndAlso Not t.IsAbstract Then
            Dim attributes = t.GetCustomAttributes(GetType(BrowsableAttribute), False)
            If attributes.Length > 0 Then
                Return DirectCast(attributes(0), BrowsableAttribute).Browsable
            Else
                Return True
            End If
        End If
        Return False
    End Function

    Private Shared Function CreateReportInfo(t As Type, index As Integer) As ReportInfo
        Dim description As String = String.Empty
        Dim attributes As Object() = Nothing

        attributes = t.GetCustomAttributes(GetType(DescriptionAttribute), False)
        If attributes.Length > 0 Then
            description = DirectCast(attributes(0), DescriptionAttribute).Description
        End If

        Dim reportInfo = New ReportInfo(FormatName(t.Name), FormatDescription(description), t, index)

        Return reportInfo
    End Function

    Private Shared Function FormatDescription(text As String) As String
        If Not text.EndsWith(".") Then
            text += "."
        End If
        Return text
    End Function

    Private Shared Function FormatName(name As String) As String
        Dim sb As New StringBuilder()
        For i As Integer = 0 To name.Length - 1
            Dim c As Char = name(i)
            If i = 0 Then
                c = [Char].ToUpper(c)
            ElseIf [Char].IsUpper(c) Then
                sb.Append(" ")
            End If
            sb.Append(c)
        Next
        Return sb.ToString()
    End Function
End Class