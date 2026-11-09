Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Public Class UserManagementForm
    Inherits Form

    Private dgvUsers As DataGridView
    Private btnSuspend As Button
    Private btnDelete As Button
    Private btnRefresh As Button
    Private btnClose As Button

    Public Sub New()
        Me.Text = "Pamoja Rail Services - Member Account Management"
        Me.Size = New Size(950, 600)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.DoubleBuffered = True
        Me.BackColor = Color.FromArgb(248, 250, 252)

        BuildUI()
    End Sub

    Private Sub BuildUI()
        Me.Controls.Clear()

        ' Title
        Dim lblTitle As New Label() With {
            .Text = "Member Account Administration",
            .Font = New Font("Segoe UI", 16, FontStyle.Bold),
            .ForeColor = Color.FromArgb(230, 0, 0),
            .Location = New Point(35, 20),
            .AutoSize = True,
            .BackColor = Color.Transparent
        }
        Me.Controls.Add(lblTitle)

        Dim lblSub As New Label() With {
            .Text = "Oversee passenger accounts, booking activity, and manage permissions",
            .Font = New Font("Segoe UI", 10, FontStyle.Regular),
            .ForeColor = Color.FromArgb(100, 116, 139),
            .Location = New Point(35, 52),
            .AutoSize = True,
            .BackColor = Color.Transparent
        }
        Me.Controls.Add(lblSub)

        ' DataGridView
        dgvUsers = New DataGridView() With {
            .Location = New Point(35, 90),
            .Size = New Size(860, 390),
            .BackgroundColor = Color.White,
            .BorderStyle = BorderStyle.None,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .MultiSelect = False,
            .ReadOnly = True,
            .AllowUserToAddRows = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .RowHeadersVisible = False,
            .Font = New Font("Segoe UI", 10)
        }
        dgvUsers.EnableHeadersVisualStyles = False
        dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 0, 0)
        dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvUsers.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgvUsers.ColumnHeadersHeight = 38
        Me.Controls.Add(dgvUsers)

        ' Action Buttons
        btnSuspend = New Button() With {
            .Text = "Suspend / Reinstate Account",
            .Location = New Point(35, 500),
            .Size = New Size(240, 42),
            .BackColor = Color.FromArgb(245, 158, 11),
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat
        }
        btnSuspend.FlatAppearance.BorderSize = 0
        AddHandler btnSuspend.Click, AddressOf BtnSuspend_Click
        Me.Controls.Add(btnSuspend)

        btnDelete = New Button() With {
            .Text = "Delete Member Account",
            .Location = New Point(290, 500),
            .Size = New Size(200, 42),
            .BackColor = Color.FromArgb(230, 0, 0),
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat
        }
        btnDelete.FlatAppearance.BorderSize = 0
        AddHandler btnDelete.Click, AddressOf BtnDelete_Click
        Me.Controls.Add(btnDelete)

        btnRefresh = New Button() With {
            .Text = "Refresh",
            .Location = New Point(505, 500),
            .Size = New Size(120, 42),
            .BackColor = Color.FromArgb(241, 245, 249),
            .ForeColor = Color.FromArgb(15, 23, 42),
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat
        }
        btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225)
        AddHandler btnRefresh.Click, AddressOf LoadUsers
        Me.Controls.Add(btnRefresh)

        btnClose = New Button() With {
            .Text = "Close",
            .Location = New Point(795, 500),
            .Size = New Size(100, 42),
            .BackColor = Color.FromArgb(241, 245, 249),
            .ForeColor = Color.FromArgb(100, 116, 139),
            .Font = New Font("Segoe UI", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat
        }
        btnClose.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225)
        AddHandler btnClose.Click, Sub() Me.Close()
        Me.Controls.Add(btnClose)

        LoadUsers()
    End Sub

    Private Sub LoadUsers()
        dgvUsers.DataSource = DataService.Instance.GetUsersDataTable()
    End Sub

    Private Sub BtnSuspend_Click(sender As Object, e As EventArgs)
        If dgvUsers.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a user account row first.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim username As String = dgvUsers.SelectedRows(0).Cells("Username").Value.ToString()
        If username.Equals("admin", StringComparison.OrdinalIgnoreCase) Then
            MessageBox.Show("Cannot suspend the Primary System Administrator.", "Security", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim user = DataService.Instance.Users.Find(Function(u) u.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
        If user IsNot Nothing Then
            user.IsSuspended = Not user.IsSuspended
            Dim actionText = If(user.IsSuspended, "suspended", "reinstated")
            MessageBox.Show("User '" & username & "' has been " & actionText & ".", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadUsers()
        End If
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs)
        If dgvUsers.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a user account to delete.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim username As String = dgvUsers.SelectedRows(0).Cells("Username").Value.ToString()
        If username.Equals("admin", StringComparison.OrdinalIgnoreCase) Then
            MessageBox.Show("Cannot delete the Primary System Administrator.", "Security", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If MessageBox.Show("Are you sure you want to permanently delete user account '" & username & "'?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            DataService.Instance.Users.RemoveAll(Function(u) u.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
            DataService.Instance.Bookings.RemoveAll(Function(b) b.MemberUsername.Equals(username, StringComparison.OrdinalIgnoreCase))
            MessageBox.Show("Member account deleted successfully.", "Account Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadUsers()
        End If
    End Sub
End Class
