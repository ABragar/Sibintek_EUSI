Imports Telerik.Windows.Controls
Imports System.IO
Imports System.Windows.Markup

Partial Public Class MainPage
    Inherits UserControl

    Shared ReadOnly dictionaries() As String = {
        "/VB.SilverlightDemo;component/Themes/{0}/System.Windows.xaml",
        "/VB.SilverlightDemo;component/Themes/{0}/Telerik.Windows.Controls.xaml",
        "/VB.SilverlightDemo;component/Themes/{0}/Telerik.Windows.Controls.Input.xaml",
        "/VB.SilverlightDemo;component/Themes/{0}/Telerik.Windows.Controls.Navigation.xaml",
        "/VB.SilverlightDemo;component/Themes/{0}/Telerik.ReportViewer.Silverlight.xaml"
     }

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub ThemeSelector_SelectionChanged(sender As Object, e As Telerik.Windows.Controls.SelectionChangedEventArgs)
        Dim selectedItem = DirectCast(DirectCast(sender, RadComboBox).SelectedItem, RadComboBoxItem)
        Dim themeNameParts = DirectCast(selectedItem.Content, String).Split("_"c)
        Dim themeName = themeNameParts(0)
        Dim themeVariation = String.Empty

        MergeResourceDictionaries(themeName)

        If themeNameParts.Length > 1 Then
            Select Case themeName
                Case "VisualStudio2013"
                    VisualStudio2013Palette.LoadPreset(DirectCast(GetPresetEnum(GetType(VisualStudio2013Palette.ColorVariation), themeNameParts(1)), VisualStudio2013Palette.ColorVariation))
                    Exit Select
                Case "Office2013"
                    Office2013Palette.LoadPreset(DirectCast(GetPresetEnum(GetType(Office2013Palette.ColorVariation), themeNameParts(1)), Office2013Palette.ColorVariation))
                    Exit Select
            End Select

            themeVariation = themeNameParts(1)
        End If

        Me.SetLayoutBackground(themeName, themeVariation)
    End Sub

    Private Sub SetLayoutBackground(themeName As String, themeVariation As String)
        Dim grid = TryCast(Me.LayoutRoot, Grid)
        If grid IsNot Nothing Then
            grid.Background = New SolidColorBrush(GetLayoutBackground(themeName, themeVariation, grid))
        End If
    End Sub

    Private Function GetPresetEnum(enumeration As Type, colorVariation As String) As Object
        Return [Enum].Parse(enumeration, colorVariation, False)
    End Function

    Private Shared Function GetLayoutBackground(themeName As String, themeVariation As String, grid As Panel) As Color
        Select Case themeName
            Case "VisualStudio2013", "Office2013"
                If themeVariation = "White" Then
                    Return Color.FromArgb(&HFF, &HFF, &HFF, &HFF)
                Else
                    Return Color.FromArgb(&HFF, &HD6, &HD6, &HD6)
                End If
            Case "ExpressionDark"
                Return Color.FromArgb(&HFF, &H0, &H0, &H0)
            Case "OfficeBlack"
                Return Color.FromArgb(&HFF, &HCA, &HCA, &HCA)
            Case "OfficeBlue"
                Return Color.FromArgb(&HFF, &HCD, &HE5, &HFE)
            Case "OfficeSilver"
                Return Color.FromArgb(&HFF, &HDC, &HDD, &HE7)
            Case Else
                Return Color.FromArgb(&HFF, &HFF, &HFF, &HFF)
        End Select
    End Function

    Shared Sub MergeResourceDictionaries(theme As String)
        Dim mergedDictionaries = Application.Current.Resources.MergedDictionaries
        mergedDictionaries.Clear()
        For Each dictionary As String In dictionaries
            Dim entry = String.Format(dictionary, theme)
            Dim resourceInfo = App.GetResourceStream(New Uri(entry, UriKind.RelativeOrAbsolute))
            Dim resourceReader = New StreamReader(resourceInfo.Stream)
            Dim xaml As String = resourceReader.ReadToEnd()
            Dim resourceTheme = TryCast(XamlReader.Load(xaml), ResourceDictionary)
            mergedDictionaries.Add(resourceTheme)
        Next
    End Sub

End Class