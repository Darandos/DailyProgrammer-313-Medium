Option Strict On

' TODO Change signature of MapPixel
'		If parameters other than x and y are in scope, we don't need to pass them

Public Module Transformer
    Public Function RotateClockwise(pixels As Integer(,)) As Integer(,)
        Dim mapPixelRotateClockwise As MapPixel =
            Function(lPixels As Integer(,), y As Integer, x As Integer, ySize As Integer, xSize As Integer,
                    maxPixelValue As Integer) As Integer
                Return lPixels(xSize - x - 1, y)
            End Function

        Dim newXSize As Integer = pixels.GetLength(0)
        Dim newYSize As Integer = pixels.GetLength(1)

        Return Transform(pixels, mapPixelRotateClockwise, newXSize, newYSize)
    End Function

    Public Function RotateCounterClockwise(pixels As Integer(,)) As Integer(,)
        Dim mapPixelRotateCounterClockwise As MapPixel =
            Function(lPixels As Integer(,), y As Integer, x As Integer, ySize As Integer, xSize As Integer,
                     maxPixelValue As Integer) As Integer
                Return lPixels(x, ySize - y - 1)
            End Function

        Dim newXSize As Integer = pixels.GetLength(0)
        Dim newYSize As Integer = pixels.GetLength(1)

        Return Transform(pixels, mapPixelRotateCounterClockwise, newXSize, newYSize)
    End Function

    Public Function FlipHorizontal(pixels As Integer(,)) As Integer(,)
        Dim mapPixelFlipHorizontal As MapPixel =
            Function(lPixels As Integer(,), y As Integer, x As Integer, ySize As Integer, xSize As Integer,
                     maxPixelValue As Integer) As Integer
                Return lPixels(y, xSize - x - 1)
            End Function

        Return Transform(pixels, mapPixelFlipHorizontal)
    End Function

    Public Function FlipVertical(pixels As Integer(,)) As Integer(,)
        Dim mapPixelFlipVertical As MapPixel =
            Function(lPixels As Integer(,), y As Integer, x As Integer, ySize As Integer, xSize As Integer,
                     maxPixelValue As Integer) As Integer
                Return lPixels(ySize - y - 1, x)
            End Function

        Return Transform(pixels, mapPixelFlipVertical)
    End Function

    Public Function Enlarge(pixels As Integer(,)) As Integer(,)
        Dim mapPixelEnlarge As MapPixel =
            Function(lPixels As Integer(,), y As Integer, x As Integer, ySize As Integer, xSize As Integer,
                     maxPixelValue As Integer) As Integer
                Return lPixels(y \ 2, x \ 2)
            End Function

		Dim newXSize As Integer = pixels.GetLength(1) * 2
		Dim newYSize As Integer = pixels.GetLength(0) * 2

		Return Transform(pixels, mapPixelEnlarge, newXSize, newYSize)
    End Function

    Public Function Shrink(pixels As Integer(,)) As Integer(,)
        Dim mapPixelShrink As MapPixel =
            Function(lPixels As Integer(,), y As Integer, x As Integer, ySize As Integer, xSize As Integer,
                     maxPixelValue As Integer) As Integer
                Dim sum As Integer = lPixels(y * 2, x * 2)
                sum += lPixels(y * 2 + 1, x * 2)
                sum += lPixels(y * 2, x * 2 + 1)
                sum += lPixels(y * 2 + 1, x * 2 + 1)

                Return sum \ 4
            End Function

		Dim newXSize As Integer = pixels.GetLength(1) \ 2
		Dim newYSize As Integer = pixels.GetLength(0) \ 2

		Return Transform(pixels, mapPixelShrink, newXSize, newYSize)
    End Function

    Public Function Negative(pixels As Integer(,), maxPixelValue As Integer) As Integer(,)
        Dim mapPixelNegative As MapPixel =
            Function(lPixels As Integer(,), y As Integer, x As Integer, ySize As Integer, xSize As Integer,
                     lMaxPixelValue As Integer) As Integer
                If lMaxPixelValue = 0 Then
                    Throw New ArgumentNullException("lMaxPixelValue")
                End If
                Return lMaxPixelValue - lPixels(y, x)
            End Function

        Return Transform(pixels, mapPixelNegative, maxPixelValue:=maxPixelValue)
    End Function

    Public Function Brighten(pixels As Integer(,)) As Integer(,)
        Dim mapPixelBrighten As MapPixel =
            Function(lPixels As Integer(,), y As Integer, x As Integer, ySize As Integer, xSize As Integer,
                     lMaxPixelValue As Integer) As Integer
                Return lPixels(y, x) + 10
            End Function

        Return Transform(pixels, mapPixelBrighten)
    End Function

    Public Function Darken(pixels As Integer(,)) As Integer(,)
        Dim mapPixelDarken As MapPixel =
            Function(lPixels As Integer(,), y As Integer, x As Integer, ySize As Integer, xSize As Integer,
                     lMaxPixelValue As Integer) As Integer
                Return lPixels(y, x) - 10
            End Function

        Return Transform(pixels, mapPixelDarken)
    End Function

    Public Function IncreaseContrast(pixels As Integer(,), maxPixelValue As Integer) As Integer(,)
		Dim mapPixelIncreaseContrast As MapPixel =
			Function(lPixels As Integer(,), y As Integer, x As Integer, ySize As Integer, xSize As Integer,
					 lMaxPixelValue As Integer) As Integer
				If lMaxPixelValue = 0 Then
					Throw New ArgumentNullException("lMaxPixelValue")
				End If

				Dim halfMax As Double = maxPixelValue / 2
				Dim newPixel As Integer = CInt((lPixels(y, x) - halfMax) * 1.1 + halfMax)

				Return newPixel
			End Function

		Return Transform(pixels, mapPixelIncreaseContrast, maxPixelValue:=maxPixelValue)
	End Function

    Public Function DecreaseContrast(pixels As Integer(,), maxPixelValue As Integer) As Integer(,)
        Dim mapPixelIncreaseContrast As MapPixel =
            Function(lPixels As Integer(,), y As Integer, x As Integer, ySize As Integer, xSize As Integer,
                     lMaxPixelValue As Integer) As Integer
                If lMaxPixelValue = 0 Then
                    Throw New ArgumentNullException("lMaxPixelValue")
                End If

                Dim halfMax As Double = maxPixelValue / 2
                Dim newPixel As Integer = CInt((lPixels(y, x) - halfMax) / 1.1 + halfMax)

                Return newPixel
            End Function

		Return Transform(pixels, mapPixelIncreaseContrast, maxPixelValue:=maxPixelValue)
	End Function

    Private Function Transform(pixels As Integer(,),
                               callback As MapPixel,
                               Optional xSize As Integer = 0,
                               Optional ySize As Integer = 0,
                               Optional maxPixelValue As Integer = 0) As Integer(,)
        xSize = If(xSize = 0, pixels.GetLength(1), xSize)
        ySize = If(ySize = 0, pixels.GetLength(0), ySize)

        Dim newPixels(ySize - 1, xSize - 1) As Integer

        For y As Integer = 0 To ySize - 1
            For x As Integer = 0 To xSize - 1
                newPixels(y, x) = callback(pixels, y, x, ySize, xSize, maxPixelValue)
            Next
        Next

        Return newPixels
    End Function

    Private Delegate Function MapPixel(pixels As Integer(,),
                                       y As Integer,
                                       x As Integer,
                                       ySize As Integer,
                                       xSize As Integer,
                                       maxPixelValue As Integer) As Integer
End Module