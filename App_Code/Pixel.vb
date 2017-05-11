Option Strict On

Public Class Pixel

	Public ReadOnly Property Red As Integer
	Public ReadOnly Property Green As Integer
	Public ReadOnly Property Blue As Integer

	Sub New(red As Integer, green As Integer, blue As Integer)
		Me.Red = red
		Me.Green = green
		Me.Blue = blue
	End Sub

	Sub New(value As Integer)
		Me.New(value, value, value)
	End Sub

	Public Sub Negative(maxPixelValue As Integer)
		setAll(Function(value) maxPixelValue - value)
	End Sub

	Public Sub Brighten()
		setAll(Function(value) value + 10)
	End Sub

	Public Sub Darken()
		setAll(Function(value) value - 10)
	End Sub

	Public Sub IncreaseContrast(maxPixelValue As Integer)
		Dim mutator =
			Function(value As Integer) As Integer
				Dim halfMax As Double = maxPixelValue / 2
				Return CInt((value - halfMax) * 1.1 + halfMax)
			End Function

		setAll(mutator)
	End Sub

	Public Sub DecreaseContrast(maxPixelValue As Integer)
		Dim mutator =
			Function(value As Integer) As Integer
				Dim halfMax As Double = maxPixelValue / 2
				Return CInt((value - halfMax) / 1.1 + halfMax)
			End Function

		setAll(mutator)
	End Sub

	Private Sub setAll(mutator As Func(Of Integer, Integer))
		_Red = mutator(Red)
		_Green = mutator(Green)
		_Blue = mutator(Blue)
	End Sub
End Class
