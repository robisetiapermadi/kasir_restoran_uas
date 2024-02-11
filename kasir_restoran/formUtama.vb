﻿Imports System.Drawing.Text
Imports System.IO
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class formUtama

    Dim grandTotal As Integer

    Private Sub loadDaftarMenu()
        Dim daftarMenuText As String()
        Dim totalMenu As Integer




        Dim menuList_listview As ListViewItem

        daftarMenuText = System.IO.File.ReadAllLines("database\daftarmenu.csv")

        totalMenu = daftarMenuText.Length - 1

        menuList.Items().Clear()

        For i = 1 To totalMenu
            menuList_listview = menuList.Items.Add(Split(daftarMenuText(i), ",")(0))
            With menuList_listview
                .SubItems.Add(Split(daftarMenuText(i), ",")(1))
            End With
        Next

        hapusButton.Enabled = False

    End Sub

    Public Sub resetOrderList()
        orderList.Items().Clear()
    End Sub

    Private Sub printInvoice(grandTotal As Integer, kembalian As Integer, pembayaran As Integer)
        Dim oForm As New formInvoice
        oForm.Show()
        Dim objItemInvoice As ListViewItem
        Dim objItemOrder As ListViewItem

        Dim countOrder As Integer

        countOrder = orderList.Items.Count

        For i = 0 To countOrder - 1
            objItemOrder = orderList.Items(i)
            objItemInvoice = oForm.invoice.Items.Add(objItemOrder.Text)

            With objItemInvoice
                .SubItems.Add(objItemOrder.SubItems(1))
                .SubItems.Add(objItemOrder.SubItems(2))
                .SubItems.Add(objItemOrder.SubItems(3))
            End With
        Next

        oForm.kembalianLabel.Text = kembalian

        oForm.grandTotalLabel.Text = grandTotal

        oForm.pembayaranLabel.Text = pembayaran

    End Sub

    Private Function tambahKeKeranjang(menu As String, jumlah As Integer, harga As Integer) As Boolean
        Dim objItem As ListViewItem

        objItem = orderList.Items.Add(menu)
        With objItem
            .SubItems.Add(harga)
            .SubItems.Add(jumlah)
            .SubItems.Add(jumlah * harga)
        End With

        grandTotal = grandTotal + (jumlah * harga)

        grandTotalLabel.Text = grandTotal

        Return True
    End Function

    Private Function bayar(nominalBayar As Integer, total As Integer) As Integer
        Dim kembalian As Integer

        kembalian = nominalBayar - total

        Return kembalian
    End Function

    Private Sub formUtama_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadDaftarMenu()
    End Sub

    Private Sub addButton_Click(sender As Object, e As EventArgs) Handles addButton.Click
        Dim nama_menu As String
        Dim jumlah_menu As Integer
        Dim harga_menu As Integer

        Try
            nama_menu = menuList.Items(menuList.FocusedItem.Index).SubItems(0).Text
            harga_menu = menuList.Items(menuList.FocusedItem.Index).SubItems(1).Text
        Catch ex As NullReferenceException
            MsgBox("Tolong pilih menu yang akan diorder", MsgBoxStyle.Exclamation, "Warning")
        End Try
        jumlah_menu = jumlah_input.Value

        If jumlah_menu <= 0 Then
            MsgBox("Nilai Jumlah harus lebih dari 0", MsgBoxStyle.Exclamation, "Warning")
        ElseIf nama_menu <> Nothing Then
            tambahKeKeranjang(nama_menu, jumlah_menu, harga_menu)
            hapusButton.Enabled = True
        End If
    End Sub

    Private Sub hapusButton_Click(sender As Object, e As EventArgs) Handles hapusButton.Click
        Try
            grandTotal = grandTotal - Convert.ToInt32(orderList.Items(orderList.FocusedItem.Index).SubItems(3).Text)
            orderList.Items(orderList.FocusedItem.Index).Remove()
            grandTotalLabel.Text = grandTotal
            If orderList.Items.Count = 0 Then
                hapusButton.Enabled = False
            End If
        Catch ex As System.NullReferenceException
            MsgBox("Tolong pilih menu yang akan dihapus dari order list", MsgBoxStyle.Exclamation, "Warning")
        End Try


    End Sub

    Private Sub resetButton_Click(sender As Object, e As EventArgs) Handles resetButton.Click
        orderList.Items().Clear()
        grandTotalLabel.Text = 0
        grandTotal = 0
        hapusButton.Enabled = False
    End Sub

    Private Sub bayarButton_Click(sender As Object, e As EventArgs) Handles bayarButton.Click
        Dim nominalBayar As Integer
        Dim grandTotal As Integer
        Dim kembalian As Integer

        nominalBayar = 0

        If pembayaranInput.Text <> Nothing Then
            nominalBayar = Convert.ToInt32(pembayaranInput.Text)
        End If

        grandTotal = grandTotalLabel.Text

        If nominalBayar <= 0 Then
            MsgBox("Nilai Pembayaran harus lebih dari 0", MsgBoxStyle.Exclamation, "Warning")
        ElseIf nominalBayar < grandTotal Then
            MsgBox("Nilai Pembayaran tidak boleh kurang dari nilai Grand Total", MsgBoxStyle.Exclamation, "Warning")
        ElseIf grandTotal = 0 Then
            MsgBox("Masukan order-nya", MsgBoxStyle.Exclamation, "Warning")
        Else
            kembalian = bayar(nominalBayar, grandTotal)
            printInvoice(grandTotal, kembalian, nominalBayar)
        End If

    End Sub

End Class
