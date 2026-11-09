<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainDashboard
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.btnBookings = New System.Windows.Forms.Button()
        Me.btnPassengers = New System.Windows.Forms.Button()
        Me.btnTrains = New System.Windows.Forms.Button()
        Me.btnReports = New System.Windows.Forms.Button()
        Me.btnLogout = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnBookings
        '
        Me.btnBookings.Location = New System.Drawing.Point(40, 50)
        Me.btnBookings.Name = "btnBookings"
        Me.btnBookings.Size = New System.Drawing.Size(180, 45)
        Me.btnBookings.TabIndex = 0
        Me.btnBookings.Text = "Manage Bookings"
        Me.btnBookings.UseVisualStyleBackColor = True
        '
        'btnPassengers
        '
        Me.btnPassengers.Location = New System.Drawing.Point(40, 110)
        Me.btnPassengers.Name = "btnPassengers"
        Me.btnPassengers.Size = New System.Drawing.Size(180, 45)
        Me.btnPassengers.TabIndex = 1
        Me.btnPassengers.Text = "Passenger Records"
        Me.btnPassengers.UseVisualStyleBackColor = True
        '
        'btnTrains
        '
        Me.btnTrains.Location = New System.Drawing.Point(40, 170)
        Me.btnTrains.Name = "btnTrains"
        Me.btnTrains.Size = New System.Drawing.Size(180, 45)
        Me.btnTrains.TabIndex = 2
        Me.btnTrains.Text = "Train Schedules"
        Me.btnTrains.UseVisualStyleBackColor = True
        '
        'btnReports
        '
        Me.btnReports.Location = New System.Drawing.Point(40, 230)
        Me.btnReports.Name = "btnReports"
        Me.btnReports.Size = New System.Drawing.Size(180, 45)
        Me.btnReports.TabIndex = 3
        Me.btnReports.Text = "System Reports"
        Me.btnReports.UseVisualStyleBackColor = True
        '
        'btnLogout
        '
        Me.btnLogout.Location = New System.Drawing.Point(40, 320)
        Me.btnLogout.Name = "btnLogout"
        Me.btnLogout.Size = New System.Drawing.Size(180, 45)
        Me.btnLogout.TabIndex = 4
        Me.btnLogout.Text = "Log Out"
        Me.btnLogout.UseVisualStyleBackColor = True
        '
        'MainDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(900, 600)
        Me.Controls.Add(Me.btnLogout)
        Me.Controls.Add(Me.btnReports)
        Me.Controls.Add(Me.btnTrains)
        Me.Controls.Add(Me.btnPassengers)
        Me.Controls.Add(Me.btnBookings)
        Me.Name = "MainDashboard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pamoja Rail Services - Main Dashboard"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnBookings As System.Windows.Forms.Button
    Friend WithEvents btnPassengers As System.Windows.Forms.Button
    Friend WithEvents btnTrains As System.Windows.Forms.Button
    Friend WithEvents btnReports As System.Windows.Forms.Button
    Friend WithEvents btnLogout As System.Windows.Forms.Button
End Class
