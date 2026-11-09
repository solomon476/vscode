# Pamoja Rail Services – Final Project

Pamoja Rail Services is a comprehensive railway operations and ticket reservation system featuring **separate Administrator and Passenger Member portals**, modern **Pure Red, Crisp White & Imperial Gold** aesthetics, and role-based access control.

This repository includes both the **complete VB.NET Windows Forms solution** for Visual Studio and the **standalone web deployment**.

---

## 🚆 Key Features & Architecture

### 👑 Administrator Portal
* **Operations Dashboard**: Real-time KPI metrics (Total Bookings, Registered Members, Active Fleet, On-Time Rate).
* **Booking Management**: Comprehensive ticket database with search and administrative reservation deletion.
* **Passenger Records**: Unified manifest of registered network passengers with trip count and expenditure totals.
* **Train Fleet & Routes**: Manage train routes, timetables, seat capacities, and status updates (On Time, Delayed, Maintenance, Cancelled).
* **System Reports & Financials**: Executive operations summaries, revenue statements, and timetable audits.
* **User Management**: Direct administrative oversight over passenger accounts (Suspend, Reinstate, or Delete member accounts).

### 🧍 Passenger Member Portal
* **My Travel Dashboard**: Personalized travel metrics and quick actions for registered travelers.
* **Book a Ticket**: Self-service ticket reservation for scheduled routes across Kenya with class selection (Economy, First Class, VIP Sleeper).
* **My Bookings**: Review personal confirmed travel reservations.
* **Train Schedules**: Live departure timetables and fare tariffs.
* **Role Restrictions**: Members are restricted from administrative reports and user management.

---

## 🎨 Visual Design
* **Primary Color**: Pure Vivid Red (`#E60000` / `Color.FromArgb(230, 0, 0)`)
* **Backgrounds & Cards**: Crisp Porcelain White (`#FFFFFF` / `#F8FAFC`)
* **Accents**: Imperial Gold (`#F59E0B` / `Color.FromArgb(245, 158, 11)`)
* **Login Experience**: Crisp luminous white glass backdrop with a bold Pure Red portal card, pure white controls, and zero credential exposure.

---

## 💻 How to Open & Run in VB.NET (Visual Studio)

1. Clone or download this repository.
2. Open **`WinFormsApp1.sln`** in **Microsoft Visual Studio 2022** (or later).
3. Set the build configuration to **Any CPU** or **x64**.
4. Press **F5** or click **Start** to run the project.
   - The startup form is **`SignUpForm`**.
   - Select **Administrator** or **Member Portal** from the role selector.
   - Enter credentials to enter the respective dashboard.

### Pre-Seeded Default Accounts (Local DataService)
* **Administrator**:
  * Username: `admin`
  * Password: `admin123`
* **Passenger Members**:
  * `john.kamau` / `member123`
  * `faith.mwangi` / `member123`
  * `david.otieno` / `member123`
  * `sarah.w` / `member123`
  * Or click **Register Member Account** to create a new passenger account instantly.

*(Note: The VB.NET forms include a resilient `DataService` that operates both with local in-memory records and can optionally connect to MySQL on `localhost:3306` database `pamoja_rail`)*.

---

## 🌐 Web Version
Open **`index.html`** in any modern web browser on laptop or mobile. It features:
* Inverted Pure Red login card on a crisp white backdrop.
* Full mobile navigation drawer (`☰`) for smartphones and tablets.
* Dynamic local storage persistence (`localStorage`).

---

## 📁 Repository Structure
```
├── WinFormsApp1.sln                 # Visual Studio Solution File
├── WinFormsApp1/                    # VB.NET Project Files
│   ├── WinFormsApp1.vbproj          # .NET 8 Windows Forms Project
│   ├── DataService.vb               # Central Data, Auth & Mock Records Service
│   ├── SignUpForm.vb                # Inverted Pure Red & White Login Screen
│   ├── MainDashboard.vb             # Dual-Portal Main Operations Dashboard
│   ├── UserManagementForm.vb        # Admin Member Account Management
│   ├── BookingForm.vb               # Ticket Reservation Form
│   ├── PassengerForm.vb             # Passenger Records Directory Form
│   ├── TrainForm.vb                 # Train Fleet & Timetable Form
│   ├── ReportForm.vb                # System Analytics & Report Viewer
│   └── download.jpg                 # Railway Background Asset
├── index.html                       # Responsive Pure Red Web Application
├── rail_bg.jpg                      # High-Resolution Background Graphic
├── .gitignore                       # Clean Git configuration
└── README.md                        # Documentation & Setup Guide
```
