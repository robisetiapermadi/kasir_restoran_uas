
Public Class formInvoice
    Private Sub doneButton_Click(sender As Object, e As EventArgs) Handles doneButton.Click
        Dim oForm As New formUtama

        invoice.Items().Clear()
        oForm.resetOrderList()

        Me.Hide()

    End Sub
End Class