Imports System.Data

Public Class UserAccount
    Public Property Id As String
    Public Property Username As String
    Public Property Password As String
    Public Property Role As String
    Public Property FullName As String
    Public Property IsSuspended As Boolean = False
End Class

Public Class TrainRecord
    Public Property Id As String
    Public Property TrainNumber As String
    Public Property Route As String
    Public Property DepartureStation As String
    Public Property ArrivalStation As String
    Public Property DepartureTime As String
    Public Property Capacity As Integer
    Public Property EconomyTariff As Integer
    Public Property FirstClassTariff As Integer
    Public Property VIPTariff As Integer
    Public Property Status As String
End Class

Public Class BookingRecord
    Public Property Id As String
    Public Property TicketRef As String
    Public Property TrainNumber As String
    Public Property Route As String
    Public Property PassengerName As String
    Public Property TravelClass As String
    Public Property TravelDate As String
    Public Property Fare As Integer
    Public Property MemberUsername As String
    Public Property Status As String = "Confirmed"
End Class

Public Class DataService
    Private Shared _instance As DataService
    Public Shared ReadOnly Property Instance As DataService
        Get
            If _instance Is Nothing Then
                _instance = New DataService()
            End If
            Return _instance
        End Get
    End Property

    Public Property CurrentUser As UserAccount
    Public Property Users As New List(Of UserAccount)
    Public Property Trains As New List(Of TrainRecord)
    Public Property Bookings As New List(Of BookingRecord)

    Public Sub New()
        SeedInitialData()
    End Sub

    Private Sub SeedInitialData()
        ' Users
        Users.Add(New UserAccount With {.Id = "u-admin", .Username = "admin", .Password = "admin123", .Role = "Admin", .FullName = "Chief Operations Admin"})
        Users.Add(New UserAccount With {.Id = "u-1", .Username = "john.kamau", .Password = "member123", .Role = "Member", .FullName = "John Kamau"})
        Users.Add(New UserAccount With {.Id = "u-2", .Username = "faith.mwangi", .Password = "member123", .Role = "Member", .FullName = "Faith Mwangi"})
        Users.Add(New UserAccount With {.Id = "u-3", .Username = "david.otieno", .Password = "member123", .Role = "Member", .FullName = "David Otieno"})
        Users.Add(New UserAccount With {.Id = "u-4", .Username = "sarah.w", .Password = "member123", .Role = "Member", .FullName = "Sarah Wanjiku"})

        ' Trains
        Trains.Add(New TrainRecord With {.Id = "t-101", .TrainNumber = "Express 101", .Route = "Nairobi - Mombasa", .DepartureStation = "Nairobi Terminus", .ArrivalStation = "Mombasa Terminus", .DepartureTime = "08:00", .Capacity = 560, .EconomyTariff = 1500, .FirstClassTariff = 3000, .VIPTariff = 6000, .Status = "On Time / Active"})
        Trains.Add(New TrainRecord With {.Id = "t-204", .TrainNumber = "Inter-County 204", .Route = "Nairobi - Kisumu", .DepartureStation = "Nairobi Central", .ArrivalStation = "Kisumu Terminus", .DepartureTime = "10:30", .Capacity = 420, .EconomyTariff = 1200, .FirstClassTariff = 2500, .VIPTariff = 5000, .Status = "On Time / Active"})
        Trains.Add(New TrainRecord With {.Id = "t-305", .TrainNumber = "Coastliner 305", .Route = "Mombasa - Nairobi", .DepartureStation = "Mombasa Terminus", .ArrivalStation = "Nairobi Terminus", .DepartureTime = "15:00", .Capacity = 560, .EconomyTariff = 1500, .FirstClassTariff = 3000, .VIPTariff = 6000, .Status = "On Time / Active"})
        Trains.Add(New TrainRecord With {.Id = "t-410", .TrainNumber = "Rift Valley Link 410", .Route = "Nakuru - Eldoret", .DepartureStation = "Nakuru Station", .ArrivalStation = "Eldoret Station", .DepartureTime = "14:15", .Capacity = 280, .EconomyTariff = 900, .FirstClassTariff = 1800, .VIPTariff = 3500, .Status = "Maintenance"})
        Trains.Add(New TrainRecord With {.Id = "t-502", .TrainNumber = "Night Star 502", .Route = "Nairobi - Mombasa", .DepartureStation = "Nairobi Terminus", .ArrivalStation = "Mombasa Terminus", .DepartureTime = "22:00", .Capacity = 700, .EconomyTariff = 1800, .FirstClassTariff = 3500, .VIPTariff = 7500, .Status = "Delayed"})

        ' Bookings
        Bookings.Add(New BookingRecord With {.Id = "b-1", .TicketRef = "PRS-9401", .TrainNumber = "Express 101", .Route = "Nairobi - Mombasa", .PassengerName = "John Kamau", .TravelClass = "First Class", .TravelDate = "2026-11-02", .Fare = 3000, .MemberUsername = "john.kamau"})
        Bookings.Add(New BookingRecord With {.Id = "b-2", .TicketRef = "PRS-9402", .TrainNumber = "Inter-County 204", .Route = "Nairobi - Kisumu", .PassengerName = "Faith Mwangi", .TravelClass = "VIP Sleeper", .TravelDate = "2026-11-03", .Fare = 5000, .MemberUsername = "faith.mwangi"})
        Bookings.Add(New BookingRecord With {.Id = "b-3", .TicketRef = "PRS-9403", .TrainNumber = "Coastliner 305", .Route = "Mombasa - Nairobi", .PassengerName = "David Otieno", .TravelClass = "Economy", .TravelDate = "2026-11-02", .Fare = 1500, .MemberUsername = "david.otieno"})
        Bookings.Add(New BookingRecord With {.Id = "b-4", .TicketRef = "PRS-9404", .TrainNumber = "Express 101", .Route = "Nairobi - Mombasa", .PassengerName = "Sarah Wanjiku", .TravelClass = "First Class", .TravelDate = "2026-11-04", .Fare = 3000, .MemberUsername = "sarah.w"})
        Bookings.Add(New BookingRecord With {.Id = "b-5", .TicketRef = "PRS-9405", .TrainNumber = "Night Star 502", .Route = "Nairobi - Mombasa", .PassengerName = "Peter Omondi", .TravelClass = "VIP Sleeper", .TravelDate = "2026-11-05", .Fare = 7500, .MemberUsername = "admin"})
    End Sub

    Public Function ValidateLogin(username As String, password As String, role As String, ByRef outUser As UserAccount, ByRef errorMessage As String) As Boolean
        Dim u = Users.Find(Function(x) x.Username.Equals(username, StringComparison.OrdinalIgnoreCase) AndAlso x.Password = password AndAlso x.Role.Equals(role, StringComparison.OrdinalIgnoreCase))
        If u Is Nothing Then
            errorMessage = "Invalid username or password for " & role & " account."
            Return False
        End If

        If u.IsSuspended Then
            errorMessage = "Your account has been suspended by Administration. Contact support."
            Return False
        End If

        outUser = u
        CurrentUser = u
        Return True
    End Function

    Public Function RegisterMember(fullName As String, username As String, password As String, ByRef errorMessage As String) As Boolean
        If Users.Exists(Function(x) x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)) Then
            errorMessage = "Username is already taken. Please choose another."
            Return False
        End If

        Dim newUser As New UserAccount With {
            .Id = "u-" & (Users.Count + 1),
            .Username = username,
            .Password = password,
            .Role = "Member",
            .FullName = fullName,
            .IsSuspended = False
        }
        Users.Add(newUser)
        Return True
    End Function

    Public Function GetBookingsDataTable(Optional memberFilter As String = Nothing) As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("Ticket Ref")
        dt.Columns.Add("Passenger Name")
        dt.Columns.Add("Route")
        dt.Columns.Add("Train No")
        dt.Columns.Add("Class")
        dt.Columns.Add("Travel Date")
        dt.Columns.Add("Fare (KES)")
        dt.Columns.Add("Booked By")
        dt.Columns.Add("Status")

        For Each b In Bookings
            If String.IsNullOrEmpty(memberFilter) OrElse b.MemberUsername.Equals(memberFilter, StringComparison.OrdinalIgnoreCase) Then
                dt.Rows.Add(b.TicketRef, b.PassengerName, b.Route, b.TrainNumber, b.TravelClass, b.TravelDate, b.Fare.ToString("N0"), b.MemberUsername, b.Status)
            End If
        Next
        Return dt
    End Function

    Public Function GetPassengersDataTable() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("Passenger Name")
        dt.Columns.Add("Total Trips")
        dt.Columns.Add("Total Spent (KES)")
        dt.Columns.Add("Preferred Class")
        dt.Columns.Add("Account Status")

        For Each u In Users.FindAll(Function(x) x.Role = "Member")
            Dim uBookings = Bookings.FindAll(Function(b) b.MemberUsername.Equals(u.Username, StringComparison.OrdinalIgnoreCase))
            Dim totalSpent As Integer = 0
            Dim topClass As String = "Economy"
            Dim classCounts As New Dictionary(Of String, Integer)

            For Each b In uBookings
                totalSpent += b.Fare
                If Not classCounts.ContainsKey(b.TravelClass) Then classCounts(b.TravelClass) = 0
                classCounts(b.TravelClass) += 1
            Next

            Dim maxCount = 0
            For Each kv In classCounts
                If kv.Value > maxCount Then
                    maxCount = kv.Value
                    topClass = kv.Key
                End If
            Next

            dt.Rows.Add(u.FullName, uBookings.Count, totalSpent.ToString("N0"), topClass, If(u.IsSuspended, "Suspended", "Active"))
        Next
        Return dt
    End Function

    Public Function GetTrainsDataTable() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("Train No")
        dt.Columns.Add("Route")
        dt.Columns.Add("Departure")
        dt.Columns.Add("Arrival")
        dt.Columns.Add("Time")
        dt.Columns.Add("Capacity")
        dt.Columns.Add("Economy (KES)")
        dt.Columns.Add("First (KES)")
        dt.Columns.Add("VIP (KES)")
        dt.Columns.Add("Status")

        For Each t In Trains
            dt.Rows.Add(t.TrainNumber, t.Route, t.DepartureStation, t.ArrivalStation, t.DepartureTime, t.Capacity, t.EconomyTariff.ToString("N0"), t.FirstClassTariff.ToString("N0"), t.VIPTariff.ToString("N0"), t.Status)
        Next
        Return dt
    End Function

    Public Function GetUsersDataTable() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("Username")
        dt.Columns.Add("Full Name")
        dt.Columns.Add("Role")
        dt.Columns.Add("Bookings Count")
        dt.Columns.Add("Status")

        For Each u In Users
            Dim count = Bookings.FindAll(Function(b) b.MemberUsername.Equals(u.Username, StringComparison.OrdinalIgnoreCase)).Count
            dt.Rows.Add(u.Username, u.FullName, u.Role, count, If(u.IsSuspended, "Suspended", "Active"))
        Next
        Return dt
    End Function
End Class
