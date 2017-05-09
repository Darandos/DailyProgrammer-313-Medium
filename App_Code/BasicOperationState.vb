Option Strict On

Public Class BasicOperationState

    Private _currentState As State = State.Original
    Private _transformationTable(System.Enum.GetNames(GetType(State)).Length - 1) As Dictionary(Of Char, State)

    Public Property CurrentState() As State
        Get
            Return _currentState
        End Get
        Private Set(value As State)
            _currentState = value
        End Set
    End Property

    Public ReadOnly Property OperationString() As String
        Get
            Dim result As String = String.Empty
            Select Case _currentState
                Case State.Original
                    result = String.Empty
                Case State.R
                    result = "R"
                Case State.L
                    result = "L"
                Case State.H
                    result = "H"
                Case State.V
                    result = "V"
                Case State.UpsideDown
                    result = "RR"
                Case State.RH
                    result = "RH"
                Case State.RV
                    result = "RV"
            End Select

            Return result
        End Get
    End Property

    Sub New()
        For i As Integer = 0 To _transformationTable.Length - 1
            _transformationTable(i) = New Dictionary(Of Char, State)
        Next

        _transformationTable(State.Original).Item("R"c) = State.R
        _transformationTable(State.Original).Item("L"c) = State.L
        _transformationTable(State.Original).Item("H"c) = State.H
        _transformationTable(State.Original).Item("V"c) = State.V

        _transformationTable(State.R).Item("R"c) = State.UpsideDown
        _transformationTable(State.R).Item("L"c) = State.Original
        _transformationTable(State.R).Item("H"c) = State.RH
        _transformationTable(State.R).Item("V"c) = State.RV

        _transformationTable(State.L).Item("R"c) = State.Original
        _transformationTable(State.L).Item("L"c) = State.UpsideDown
        _transformationTable(State.L).Item("H"c) = State.RV
        _transformationTable(State.L).Item("V"c) = State.RH

        _transformationTable(State.H).Item("R"c) = State.RV
        _transformationTable(State.H).Item("L"c) = State.RH
        _transformationTable(State.H).Item("H"c) = State.Original
        _transformationTable(State.H).Item("V"c) = State.UpsideDown

        _transformationTable(State.V).Item("R"c) = State.RH
        _transformationTable(State.V).Item("L"c) = State.RV
        _transformationTable(State.V).Item("H"c) = State.UpsideDown
        _transformationTable(State.V).Item("V"c) = State.Original

        _transformationTable(State.UpsideDown).Item("R"c) = State.L
        _transformationTable(State.UpsideDown).Item("L"c) = State.R
        _transformationTable(State.UpsideDown).Item("H"c) = State.V
        _transformationTable(State.UpsideDown).Item("V"c) = State.H

        _transformationTable(State.RH).Item("R"c) = State.H
        _transformationTable(State.RH).Item("L"c) = State.V
        _transformationTable(State.RH).Item("H"c) = State.R
        _transformationTable(State.RH).Item("V"c) = State.L

        _transformationTable(State.RV).Item("R"c) = State.V
        _transformationTable(State.RV).Item("L"c) = State.H
        _transformationTable(State.RV).Item("H"c) = State.L
        _transformationTable(State.RV).Item("V"c) = State.R
    End Sub

    Public Sub Transform(operation As Char)
        Try
            _currentState = _transformationTable(_currentState).Item(operation)
        Catch ex As KeyNotFoundException
            Console.WriteLine("{0} is not a valid operation", operation)
        End Try
    End Sub

End Class
