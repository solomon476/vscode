Imports System
Imports System.Windows.Forms

Public Module Program
    <STAThread()>
    Public Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New SignUpForm())
    End Sub
End Module
