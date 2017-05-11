Option Strict On

Public Module Transformer
	Public Function RotateClockwise(pixels As Pixel(,)) As Pixel(,)
		Dim newXSize As Integer = pixels.GetLength(0)
		Dim newYSize As Integer = pixels.GetLength(1)

		Dim mapPixelRotateClockwise As MapPixel =
			Function(y As Integer, x As Integer, ySize As Integer, xSize As Integer)
				Return pixels(xSize - x - 1, y)
			End Function

		Return Transform(pixels, mapPixelRotateClockwise, newXSize, newYSize)
	End Function

	Public Function RotateCounterClockwise(pixels As Pixel(,)) As Pixel(,)
		Dim newXSize As Integer = pixels.GetLength(0)
		Dim newYSize As Integer = pixels.GetLength(1)

		Dim mapPixelRotateCounterClockwise As MapPixel =
			Function(y As Integer, x As Integer, ySize As Integer, xSize As Integer)
				Return pixels(x, newYSize - y - 1)
			End Function

		Return Transform(pixels, mapPixelRotateCounterClockwise, newXSize, newYSize)
	End Function

	Public Function FlipHorizontal(pixels As Pixel(,)) As Pixel(,)
		Dim mapPixelFlipHorizontal As MapPixel =
			Function(y As Integer, x As Integer, ySize As Integer, xSize As Integer)
				Return pixels(y, xSize - x - 1)
			End Function

		Return Transform(pixels, mapPixelFlipHorizontal)
	End Function

	Public Function FlipVertical(pixels As Pixel(,)) As Pixel(,)
		Dim mapPixelFlipVertical As MapPixel =
			Function(y As Integer, x As Integer, ySize As Integer, xSize As Integer)
				Return pixels(ySize - y - 1, x)
			End Function

		Return Transform(pixels, mapPixelFlipVertical)
	End Function

	Public Function Enlarge(pixels As Pixel(,)) As Pixel(,)
		Dim mapPixelEnlarge As MapPixel =
			Function(y As Integer, x As Integer, ySize As Integer, xSize As Integer)
				Return pixels(y \ 2, x \ 2)
			End Function

		Dim newXSize As Integer = pixels.GetLength(1) * 2
		Dim newYSize As Integer = pixels.GetLength(0) * 2

		Return Transform(pixels, mapPixelEnlarge, newXSize, newYSize)
	End Function

	Public Function Shrink(pixels As Pixel(,)) As Pixel(,)
		Dim mapPixelShrink As MapPixel =
			Function(y As Integer, x As Integer, ySize As Integer, xSize As Integer)
				Return Pixel.Average(pixels(y * 2, x * 2), pixels(y * 2 + 1, x * 2),
									 pixels(y * 2, x * 2 + 1), pixels(y * 2 + 1, x * 2 + 1))
			End Function

		Dim newXSize As Integer = pixels.GetLength(1) \ 2
		Dim newYSize As Integer = pixels.GetLength(0) \ 2

		Return Transform(pixels, mapPixelShrink, newXSize, newYSize)
	End Function

	Private Function Transform(pixels As Pixel(,),
							   callback As MapPixel,
							   Optional nullableXSize As Integer? = Nothing,
							   Optional nullableYSize As Integer? = Nothing) As Pixel(,)
		Dim xSize As Integer = If(nullableXSize Is Nothing, pixels.GetLength(1), CInt(nullableXSize))
		Dim ySize As Integer = If(nullableYSize Is Nothing, pixels.GetLength(0), CInt(nullableYSize))

		Dim newPixels(ySize - 1, xSize - 1) As Pixel

		For y As Integer = 0 To ySize - 1
			For x As Integer = 0 To xSize - 1
				newPixels(y, x) = callback(y, x, ySize, xSize)
			Next
		Next

		Return newPixels
	End Function

	Private Delegate Function MapPixel(y As Integer, x As Integer, ySize As Integer, xSize As Integer) As Pixel
End Module