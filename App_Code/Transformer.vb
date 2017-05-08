Public Module Transformer
    Public Function RotateClockwise(pixels As Integer(,)) As Integer(,)
        Dim mapPixelRotateClockwise As MapPixel =
            Function(lPixels As Integer(,), y As Integer, x As Integer, ySize As Integer, xSize As Integer) _
            As Integer
                Return pixels(xSize - x - 1, y)
            End Function

        Dim newXSize As Integer = pixels.GetLength(0)
        Dim newYSize As Integer = pixels.GetLength(1)

        Return Transform(pixels, mapPixelRotateClockwise, newXSize, newYSize)
    End Function

    Public Function RotateCounterClockwise(pixels As Integer(,)) As Integer(,)
        Dim mapPixelRotateCounterClockwise As MapPixel =
            Function(lPixels As Integer(,), y As Integer, x As Integer, ySize As Integer, xSize As Integer) _
            As Integer
                Return pixels(x, ySize - y - 1)
            End Function

        Dim newXSize As Integer = pixels.GetLength(0)
        Dim newYSize As Integer = pixels.GetLength(1)

        Return Transform(pixels, mapPixelRotateCounterClockwise, newXSize, newYSize)
    End Function

    Public Function FlipHorizontal(pixels As Integer(,)) As Integer(,)
        Dim mapPixelFlipHorizontal As MapPixel =
            Function(lPixels As Integer(,), y As Integer, x As Integer, ySize As Integer, xSize As Integer) _
            As Integer
                Return pixels(y, xSize - x - 1)
            End Function

        Return Transform(pixels, mapPixelFlipHorizontal)
    End Function

    Public Function FlipVertical(pixels As Integer(,)) As Integer(,)
        Dim mapPixelFlipVertical As MapPixel =
            Function(lPixels As Integer(,), y As Integer, x As Integer, ySize As Integer, xSize As Integer) _
            As Integer
                Return pixels(ySize - y - 1, x)
            End Function

        Return Transform(pixels, mapPixelFlipVertical)
    End Function

    Private Function Transform(pixels As Integer(,),
                               callback As MapPixel,
                               Optional xSize As Integer = 0,
                               Optional ySize As Integer = 0) As Integer(,)
        xSize = If(xSize = 0, pixels.GetLength(1), xSize)
        ySize = If(ySize = 0, pixels.GetLength(0), ySize)

        Dim newPixels(ySize - 1, xSize - 1) As Integer

        For y As Integer = 0 To ySize - 1
            For x As Integer = 0 To xSize - 1
                newPixels(y, x) = callback(pixels, y, x, ySize, xSize)
            Next
        Next

        Return newPixels
    End Function

    Private Delegate Function MapPixel(pixels As Integer(,),
                                       y As Integer,
                                       x As Integer,
                                       ysize As Integer,
                                       xSize As Integer) As Integer
End Module