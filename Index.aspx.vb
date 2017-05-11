Option Strict On

Imports System.IO

Public Class Index_aspx
	Inherits Page

	Protected Sub Submit_Click(sender As Object, e As EventArgs) Handles Submit.Click
		Dim pixels As Pixel(,)
		Dim maxPixelValue As Integer
		Dim isPpm As Boolean
		Try
			pixels = ParsePnm(Pgm.Text, maxPixelValue, isPpm)
		Catch ex As ArgumentException
			Results.InnerText = ex.Message
			Exit Sub
		End Try

		Dim reader As StringReader = New StringReader(SimplifyOperations(Operations.Value.ToUpper()))
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
				Case "E"c
					pixels = Enlarge(pixels)
				Case "S"c
					pixels = Shrink(pixels)
				Case "B"c
					For Each pixel In pixels
						pixel.Brighten()
					Next
				Case "D"c
					For Each pixel In pixels
						pixel.Darken()
					Next
				Case "C"c
					For Each pixel In pixels
						pixel.IncreaseContrast(maxPixelValue)
					Next
				Case "W"c
					For Each pixel In pixels
						pixel.DecreaseContrast(maxPixelValue)
					Next
				Case "N"c
					For Each pixel In pixels
						pixel.Negative(maxPixelValue)
					Next
			End Select
			numTransformations += 1
		End While

		Dim output As New StringBuilder()
		output.AppendLine($"{If(isPpm, "P3", "P2")} {pixels.GetLength(1)} {pixels.GetLength(0)} {maxPixelValue}")
		For Each pixel As Pixel In pixels
			For Each colour In {"Red", "Green", "Blue"}
				Dim subpixel As Integer = CInt(CallByName(pixel, colour, CallType.Get))
				If subpixel > maxPixelValue Then
					subpixel = maxPixelValue
				ElseIf subpixel < 0 Then
					subpixel = 0
				End If
				output.AppendLine(CStr(subpixel))
			Next
		Next
		Results.InnerText = output.ToString()
		NumTransformationsLabel.Text = CStr(numTransformations)
	End Sub

	Private Function ParsePnm(pnm As String, Optional ByRef max As Integer = 0,
							  Optional ByRef isPpm As Boolean = False) As Pixel(,)
		Dim reader As New StringReader(pnm)

		Dim firstLine As String() = reader.ReadLine().Split(" "c)

		Dim xSize, ySize As Integer
		Try
			isPpm = firstLine(0).Equals("P3")
			xSize = CInt(firstLine(1))
			ySize = CInt(firstLine(2))
			max = CInt(firstLine(3))
		Catch ex As IndexOutOfRangeException
			Throw New ArgumentException("Invalid PNM format", ex)
		End Try

		Dim result(ySize - 1, xSize - 1) As Pixel
		For y As Integer = 0 To ySize - 1
			For x As Integer = 0 To xSize - 1
				result(y, x) = If(isPpm,
					New Pixel(CInt(reader.ReadLine()), CInt(reader.ReadLine()), CInt(reader.ReadLine())),
					New Pixel(CInt(reader.ReadLine())))
			Next
		Next

		Return result
	End Function

	Private Function SimplifyOperations(operations As String) As String
		Dim bonus2operations As String = String.Empty

		For Each operationPair As Char() In New Char()() {({"E"c, "S"c}), ({"B"c, "D"c}), ({"C"c, "W"c})}
			Dim netOperations As String = SimplifyInverseOperations(operations, operationPair(0), operationPair(1))
			operations = operations.Replace(Convert.ToString(operationPair(0)), String.Empty)
			operations = operations.Replace(Convert.ToString(operationPair(1)), String.Empty)
			bonus2operations += netOperations
		Next

		Dim grossNegatives As Integer = New Regex("N").Matches(operations).Count
		operations = operations.Replace("N", String.Empty)
		If grossNegatives Mod 2 = 1 Then
			bonus2operations += "N"
		End If

		Dim reader As StringReader = New StringReader(operations)
		Dim stateMachine As New BasicOperationState()

		While reader.Peek <> -1
			stateMachine.Transform(Convert.ToChar(reader.Read()))
		End While
		Return stateMachine.OperationString + bonus2operations
	End Function

	Private Function SimplifyInverseOperations(operationsToPerform As String, operation1 As Char, operation2 As Char) As String
		Dim regex1 As New Regex(operation1)
		Dim timesToPerformOperation1 = regex1.Matches(operationsToPerform).Count

		Dim regex2 As New Regex(operation2)
		Dim timesToPerformOperation2 = regex2.Matches(operationsToPerform).Count

		Dim netOperation As Char = If(timesToPerformOperation1 - timesToPerformOperation2 > 0, operation1, operation2)
		Dim netTimesToPerform As Integer = Math.Abs(timesToPerformOperation1 - timesToPerformOperation2)

		Return New String(netOperation, netTimesToPerform)
	End Function
End Class
