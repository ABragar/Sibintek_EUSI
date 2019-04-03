
Imports System.Collections.Generic

Public Class Cars
    Inherits List(Of Car)
    Public Sub New()
        Dim car As Car
        car = New Car("Honda", "NSX GT", 2003, "http://www.telerik.com/images/reporting/cars/NSXGT_7.jpg", New String() {"Black", "Red", "White", "Orange"})
        Me.Add(car)

        car = New Car("Nissan", "Skyline R34 GT-R", 2005, "http://www.telerik.com/images/reporting/cars/EVLR34_1.jpg", New String() {"Black", "White"})
        Me.Add(car)

        car = New Car("BMW", "M3", 2007, "http://www.telerik.com/images/reporting/cars//m3_2.jpg", New String() {"Black", "Orange"})
        Me.Add(car)

        car = New Car("Mazda", "MX-5 SE", 2004, "http://www.telerik.com/images/reporting/cars/MX5_1.jpg", New String() {"Black"})
        Me.Add(car)

        car = New Car("Audi", "S4", 2006, "http://www.telerik.com/images/reporting/cars/S4_3.jpg", New String() {"Black", "Red", "White"})
        Me.Add(car)

        car = New Car("Renault", "R5 Turbo", 2006, "http://www.telerik.com/images/reporting/cars/R5_2.jpg", New String() {"Black", "Red", "White", "Orange"})
        Me.Add(car)

        car = New Car("Lotus", "Exige", 2007, "http://www.telerik.com/images/reporting/cars/Exige_1.jpg", New String() {"Black", "Red", "White"})
        Me.Add(car)
    End Sub
End Class
