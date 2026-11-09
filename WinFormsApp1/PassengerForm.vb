Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient

Public Class PassengerForm
    Inherits Form

    Private txtPassengerName As TextBox
    Private txtTrainNo As TextBox
    Private cmbClass As ComboBox
    Private btnSave As Button
    Private btnClear As Button
    Private dgvPassengers As DataGridView
    Private btnClose As Button

    Private connectionString As String = "server=localhost;database=pamoja_rail;uid=root;pwd=;"

    Public Sub New()
        Me.Text = "Pamoja Rail Services - Passenger Directory"
        Me.Size = New Size(1000, 650)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.DoubleBuffered = True
        Me.BackColor = Color.FromArgb(248, 250, 252)

        BuildUI()
    End Sub

    Private Sub BuildUI()
        Me.Controls.Clear()

        ' Title Header
        Dim lblTitle As New Label() With {
            .Text = "Passenger Traveler Directory",
            .Font = New Font("Segoe UI", 16, FontStyle.Bold),
            .ForeColor = Color.FromArgb(230, 0, 0),
            .Location = New Point(35, 20),
            .AutoSize = True,
            .BackColor = Color.Transparent
        }
        Me.Controls.Add(lblTitle)

        ' Left Form Panel
        Dim pnlForm As New Panel() With {
            .Location = New Point(35, 75),
            .Size = New Size(300, 520),
            .BackColor = Color.White
        }

        Dim topBar As New Panel() With {.Dock = DockStyle.Top, .Height = 4, .BackColor = Color.FromArgb(230, 0, 0)}
        pnlForm.Controls.Add(topBar)

        Dim lblFormTitle As New Label() With {
            .Text = "Register / Update Passenger",
            .Font = New Font("Segoe UI", 12, FontStyle.Bold),
            .ForeColor = Color.FromArgb(230, 0, 0),
            .Location = New Point(20, 16),
            .AutoSize = True
        }
        pnlForm.Controls.Add(lblFormTitle)

        ' Full Name
        Dim lblName As New Label() With {.Text = "PASSENGER FULL NAME", .ForeColor = Color.FromArgb(100, 116, 139), .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold), .Location = New Point(20, 60), .AutoSize = True}
        txtPassengerName = New TextBox() With {.Location = New Point(20, 83), .Size = New Size(260, 30), .Font = New Font("Segoe UI", 10)}
        pnlForm.Controls.AddRange(New Control() {lblName, txtPassengerName})

        ' Preferred Train / Route
        Dim lblTrain As New Label() With {.Text = "PREFERRED ROUTE / TRAIN", .ForeColor = Color.FromArgb(100, 116, 139), .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold), .Location = New Point(20, 130), .AutoSize = True}
        txtTrainNo = New TextBox() With {.Location = New Point(20, 153), .Size = New Size(260, 30), .Font = New Font("Segoe UI", 10), .Text = "Express 101"}
        pnlForm.Controls.AddRange(New Control() {lblTrain, txtTrainNo})

        ' Class
        Dim lblClass As New Label() With {.Text = "DEFAULT TRAVEL CLASS", .ForeColor = Color.FromArgb(100, 116, 139), .Font = New Font("Segoe UI", 8.5F, FontStyle.Bold), .Location = New Point(20, 200), .AutoSize = True}
        cmbClass = New ComboBox() With {.Location = New Point(20, 223), .Size = New Size(260, 30), .Font = New Font("Segoe UI", 10), .DropDownStyle = ComboBoxStyle.DropDownList}
        cmbClass.Items.AddRange(New String() {"Economy", "First Class", "VIP Sleeper"})
        cmbClass.SelectedIndex = 0
        pnlForm.Controls.AddRange(New Control() {lblClass, cmbClass})

        ' Save Button
        btnSave = New Button() With {
            .Text = "Save Passenger",
            .Location = New Point(20, 285),
            .Size = New Size(125, 42),
            .BackColor = Color.FromArgb(230, 0, 0),
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnSave.FlatAppearance.BorderSize = 0
        AddHandler btnSave.Click, AddressOf SavePassenger
        pnlForm.Controls.Add(btnSave)

        ' Clear Button
        btnClear = New Button() With {
            .Text = "Clear",
            .Location = New Point(155, 285),
            .Size = New Size(125, 42),
            .BackColor = Color.FromArgb(241, 245, 249),
            .ForeColor = Color.FromArgb(100, 116, 139),
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnClear.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225)
        AddHandler btnClear.Click, Sub()
            txtPassengerName.Clear()
            txtTrainNo.Text = "Express 101"
            cmbClass.SelectedIndex = 0
        End Sub
        pnlForm.Controls.Add(btnClear)

        ' Return Button
        btnClose = New Button() With {
            .Text = "Return to Dashboard",
            .Location = New Point(20, 455),
            .Size = New Size(260, 42),
            .BackColor = Color.FromArgb(241, 245, 249),
            .ForeColor = Color.FromArgb(100, 116, 139),
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnClose.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225)
        AddHandler btnClose.Click, Sub() Me.Close()
        pnlForm.Controls.Add(btnClose)

        Me.Controls.Add(pnlForm)

        ' DataGridView Table Card
        dgvPassengers = New DataGridView() With {
            .Location = New Point(355, 75),
            .Size = New Size(605, 520),
            .BackgroundColor = Color.White,
            .BorderStyle = BorderStyle.None,
            .ForeColor = Color.FromArgb(15, 23, 42),
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .RowHeadersVisible = False,
            .Font = New Font("Segoe UI", 9.5F)
        }
        dgvPassengers.EnableHeadersVisualStyles = False
        dgvPassengers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 0, 0)
        dgvPassengers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvPassengers.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
        dgvPassengers.ColumnHeadersHeight = 36
        Me.Controls.Add(dgvPassengers)

        LoadPassengers()
    End Sub

    Private Sub SavePassenger(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtPassengerName.Text) Then
            MessageBox.Show("Please enter passenger full name.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim pName = txtPassengerName.Text.Trim()
        Dim uname = pName.ToLower().Replace(" ", ".")
        Dim err As String = Nothing
        DataService.Instance.RegisterMember(pName, uname, "member123", err)

        MessageBox.Show("Passenger '" & pName & "' registered in directory.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        txtPassengerName.Clear()
        LoadPassengers()
    End Sub

    Private Sub LoadPassengers()
        dgvPassengers.DataSource = DataService.Instance.GetPassengersDataTable()
    End Sub
End Class