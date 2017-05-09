Option Strict On

' TODO Change signature of MapPixel
'		If parameters other than x and y are in scope, we don't need to pass them

Public Module Transformer
	Public Function RotateClockwise(pixels As Integer(,)) As Integer(,)
		Dim newXSize As Integer = pixels.GetLength(0)
		Dim newYSize As Integer = pixels.GetLength(1)

		Dim mapPixelRotateClockwise As MapPixel =
			Function(y As Integer, x As Integer, ySize As Integer, xSize As Integer) As Integer
				Return pixels(xSize - x - 1, y)
			End Function

		Return Transform(pixels, mapPixelRotateClockwise, newXSize, newYSize)
	End Function

	Public Function RotateCounterClockwise(pixels As Integer(,)) As Integer(,)
		Dim newXSize As Integer = pixels.GetLength(0)
		Dim newYSize As Integer = pixels.GetLength(1)

		Dim mapPixelRotateCounterClockwise As MapPixel =
			Function(y As Integer, x As Integer, ySize As Integer, xSize As Integer) As Integer
				Return pixels(x, newYSize - y - 1)
			End Function

		Return Transform(pixels, mapPixelRotateCounterClockwise, newXSize, newYSize)
	End Function

	Public Function FlipHorizontal(pixels As Integer(,)) As Integer(,)
		Dim mapPixelFlipHorizontal As MapPixel =
			Function(y As Integer, x As Integer, ySize As Integer, xSize As Integer) As Integer
				Return pixels(y, xSize - x - 1)
			End Function

		Return Transform(pixels, mapPixelFlipHorizontal)
	End Function

	Public Function FlipVertical(pixels As Integer(,)) As Integer(,)
		Dim mapPixelFlipVertical As MapPixel =
			Function(y As Integer, x As Integer, ySize As Integer, xSize As Integer) As Integer
				Return pixels(ySize - y - 1, x)
			End Function

		Return Transform(pixels, mapPixelFlipVertical)
    End Function

    Public Function Enlarge(pixels As Integer(,)) As Integer(,)
		Dim mapPixelEnlarge As MapPixel =
			Function(y As Integer, x As Integer, ySize As Integer, xSize As Integer) As Integer
				Return pixels(y \ 2, x \ 2)
			End Function

		Dim newXSize As Integer = pixels.GetLength(1) * 2
		Dim newYSize As Integer = pixels.GetLength(0) * 2

		Return Transform(pixels, mapPixelEnlarge, newXSize, newYSize)
    End Function

    Public Function Shrink(pixels As Integer(,)) As Integer(,)
		Dim mapPixelShrink As MapPixel =
			Function(y As Integer, x As Integer, ySize As Integer, xSize As Integer) As Integer
				Dim sum As Integer = pixels(y * 2, x * 2)
				sum += pixels(y * 2 + 1, x * 2)
				sum += pixels(y * 2, x * 2 + 1)
				sum += pixels(y * 2 + 1, x * 2 + 1)

				Return sum \ 4
			End Function

		Dim newXSize As Integer = pixels.GetLength(1) \ 2
		Dim newYSize As Integer = pixels.GetLength(0) \ 2

		Return Transform(pixels, mapPixelShrink, newXSize, newYSize)
    End Function

    Public Function Negative(pixels As Integer(,), maxPixelValue As Integer) As Integer(,)
		Dim mapPixelNegative As MapPixel =
			Function(y As Integer, x As Integer, ySize As Integer, xSize As Integer) As Integer
				Return maxPixelValue - pixels(y, x)
			End Function

		Return Transform(pixels, mapPixelNegative)
	End Function

    Public Function Brighten(pixels As Integer(,)) As Integer(,)
		Dim mapPixelBrighten As MapPixel =
			Function(y As Integer, x As Integer, ySize As Integer, xSize As Integer)
				Return pixels(y, x) + 10
			End Function

		Return Transform(pixels, mapPixelBrighten)
    End Function

    Public Function Darken(pixels As Integer(,)) As Integer(,)
		Dim mapPixelDarken As MapPixel =
			Function(y As Integer, x As Integer, ySize As Integer, xSize As Integer) As Integer
				Return pixels(y, x) - 10
			End Function

		Return Transform(pixels, mapPixelDarken)
    End Function

    Public Function IncreaseContrast(pixels As Integer(,), maxPixelValue As Integer) As Integer(,)
		Dim mapPixelIncreaseContrast As MapPixel =
			Function(y As Integer, x As Integer, ySize As Integer, xSize As Integer) As Integer
				Dim halfMax As Double = maxPixelValue / 2
				Dim newPixel As Integer = CInt((pixels(y, x) - halfMax) * 1.1 + halfMax)

				Return newPixel
			End Function

		Return Transform(pixels, mapPixelIncreaseContrast)
	End Function

    Public Function DecreaseContrast(pixels As Integer(,), maxPixelValue As Integer) As Integer(,)
		Dim mapPixelIncreaseContrast As MapPixel =
			Function(y As Integer, x As Integer, ySize As Integer, xSize As Integer) As Integer
				Dim halfMax As Double = maxPixelValue / 2
				Dim newPixel As Integer = CInt((pixels(y, x) - halfMax) / 1.1 + halfMax)

				Return newPixel
			End Function

		Return Transform(pixels, mapPixelIncreaseContrast)
	End Function

	Private Function Transform(pixels As Integer(,),
							   callback As MapPixel,
							   Optional nullableXSize As Integer? = Nothing,
							   Optional nullableYSize As Integer? = Nothing) As Integer(,)
		Dim xSize As Integer = If(nullableXSize Is Nothing, pixels.GetLength(1), CInt(nullableXSize))
		Dim ySize As Integer = If(nullableYSize Is Nothing, pixels.GetLength(0), CInt(nullableYSize))

		Dim newPixels(ySize - 1, xSize - 1) As Integer

		For y As Integer = 0 To ySize - 1
			For x As Integer = 0 To xSize - 1
				newPixels(y, x) = callback(y, x, ySize, xSize)
			Next
		Next

		Return newPixels
	End Function

	Private Delegate Function MapPixel(y As Integer, x As Integer, ySize As Integer, xSize As Integer) As Integer
End Module