Option Strict On

Imports System.IO

Public Class Index_aspx
    Inherits Page

    Protected Sub Submit_Click(sender As Object, e As EventArgs) Handles Submit.Click
        Dim pixels As Integer(,)
        Dim maxPixelValue As Integer
        Try
            pixels = ParsePgm(Pgm.Text, maxPixelValue)
        Catch ex As ArgumentException
            Results.InnerText = ex.Message
            Exit Sub
        End Try

        Dim reader As StringReader = New StringReader(SimplifyOperations(Operations.Value))
        Dim numTransformations As Integer = 0
        While reader.Peek <> -1
            Select Case Convert.ToChar(reader.Read())
                Case "R"c
                    pixels = RotateClockwise(pixels)
                Case "L"c
                    pixels = RotateCounterClockwise(pixels)
                Case "H"c
                    pixels = FlipHorizontal(pixels)
                Case "V"c
                    pixels = FlipVertical(pixels)
            End Select
            numTransformations += 1
        End While

        Dim output As New StringBuilder()
        output.AppendLine(String.Format("P2 {0} {1} {2}", pixels.GetLength(1), pixels.GetLength(0), maxPixelValue))
        For Each pixel As Integer In pixels
            output.AppendLine(CStr(pixel))
        Next
        Results.InnerText = output.ToString()
        NumTransformationsLabel.Text = CStr(numTransformations)
    End Sub

    Private Function ParsePgm(pgm As String, Optional ByRef max As Integer = 0) As Integer(,)
        Dim reader As New StringReader(pgm)

        Dim firstLine As String() = reader.ReadLine().Split(" "c)

        Dim xSize, ySize As Integer
        Try
            xSize = CInt(firstLine(1))
            ySize = CInt(firstLine(2))
            max = CInt(firstLine(3))
        Catch ex As IndexOutOfRangeException
            Throw New ArgumentException("Invalid PGM format", ex)
        End Try

        Dim result(ySize - 1, xSize - 1) As Integer
        For y As Integer = 0 To ySize - 1
            For x As Integer = 0 To xSize - 1
                result(y, x) = CInt(reader.ReadLine())
            Next
        Next

        Return result
    End Function

    Private Function SimplifyOperations(operations As String) As String
        Dim reader As StringReader = New StringReader(operations)
        Dim stateMachine As New BasicOperationState()

        While reader.Peek <> -1
            stateMachine.Transform(Convert.ToChar(reader.Read()))
        End While
        Return stateMachine.OperationString
    End Function

    Private Function RotateClockwise(pixels As Integer(,)) As Integer(,)
        Dim mapPixelRotateClockwise As MapPixel =
            Function(lPixels As Integer(,), y As Integer, x As Integer, ySize As Integer, xSize As Integer) _
            As Integer
                Return pixels(xSize - x - 1, y)
            End Function

        Dim newXSize As Integer = pixels.GetLength(0)
        Dim newYSize As Integer = pixels.GetLength(1)

        Return Transform(pixels, mapPixelRotateClockwise, newXSize, newYSize)
    End Function

    Private Function RotateCounterClockwise(pixels As Integer(,)) As Integer(,)
        Dim mapPixelRotateCounterClockwise As MapPixel =
            Function(lPixels As Integer(,), y As Integer, x As Integer, ySize As Integer, xSize As Integer) _
            As Integer
                Return pixels(x, ySize - y - 1)
            End Function

        Dim newXSize As Integer = pixels.GetLength(0)
        Dim newYSize As Integer = pixels.GetLength(1)

        Return Transform(pixels, mapPixelRotateCounterClockwise, newXSize, newYSize)
    End Function

    Private Function FlipHorizontal(pixels As Integer(,)) As Integer(,)
        Dim mapPixelFlipHorizontal As MapPixel =
            Function(lPixels As Integer(,), y As Integer, x As Integer, ySize As Integer, xSize As Integer) _
            As Integer
                Return pixels(y, xSize - x - 1)
            End Function

        Return Transform(pixels, mapPixelFlipHorizontal)
    End Function

    Private Function FlipVertical(pixels As Integer(,)) As Integer(,)
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
End Class
