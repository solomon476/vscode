# 🚆 Pamoja Rail Services – VS Code Edition

A complete, standalone commercial railway management and ticket reservation system configured specifically for development and execution inside **Visual Studio Code (VS Code)** on Linux, Windows, and macOS.

Featuring separate **Administrator** and **Passenger Member** portals, modern **Pure Red, Crisp White & Imperial Gold** aesthetics, and zero external database dependencies.

---

## ⚡ Quick Start: Running in VS Code

### 1. Open in VS Code
Open the repository folder in VS Code:
```bash
git clone https://github.com/solomon476/vscode.git
cd vscode
code .
```

### 2. Run the Application
Choose any of these seamless methods inside VS Code:

#### Option A: One-Key Shortcut (Recommended)
* Press **`Ctrl` + `Shift` + `B`** (or **`F5`**).
* VS Code will launch the local runner and automatically open the application in your default browser at `http://localhost:8000/index.html`.

#### Option B: Integrated Terminal
* Open the built-in VS Code terminal (`Ctrl` + `` ` ``).
* Run:
  ```bash
  python3 run_web.py
  ```

#### Option C: VS Code Live Server
* If you have the **Live Server** extension (`ritwickdey.LiveServer`) installed:
* Right-click **`index.html`** in the VS Code file explorer $\rightarrow$ **"Open with Live Server"**.

#### Option D: Direct Browser Launch
* From the VS Code terminal:
  ```bash
  xdg-open index.html
  ```

---

## 🔑 Login Credentials

The system includes pre-seeded accounts configured out of the box:

### 👑 Administrator Portal
* **Role**: Select **`Administrator`**
* **Username**: `admin`
* **Password**: `admin123`

### 🧍 Passenger Member Portal
* **Role**: Select **`Member Portal`**
* **Pre-seeded Accounts**:
  * `john.kamau` / `member123`
  * `faith.mwangi` / `member123`
  * `david.otieno` / `member123`
  * `sarah.w` / `member123`
* **Self-Registration**: Click **"New passenger? Register Member Account"** on the login screen to create a new member profile instantly.

---

## 🧭 System Capabilities & Modules

### 1. 📊 Operations Overview Dashboard
* Live KPIs: Total Bookings, Registered Members, Active Fleet, On-Time vs. Delayed/Maintenance trains.
* Personalized welcome banner and metrics for members (Total Trips, Total Spent, Upcoming Journeys).

### 2. 🎟️ Ticket Booking & Reservations
* Self-service and administrative ticket booking across major routes (Nairobi $\leftrightarrow$ Mombasa, Kisumu, Nakuru, Eldoret).
* Automated fare calculation by class:
  * **Economy**: KES 1,500
  * **First Class**: KES 3,000
  * **VIP Sleeper**: KES 6,000
* Automated ticket reference generation (e.g. `PRS-940x`).

### 3. 👥 Passenger Traveler Directory
* Comprehensive records manifest tracking total completed journeys, cumulative expenditure, and preferred travel classes.

### 4. 🚆 Train Fleet & Timetable Operations
* Real-time fleet management: update train numbers, routes, seat capacities, and operational statuses (*On Time / Active*, *Delayed*, *Maintenance*, *Cancelled*).

### 5. 📈 System Analytics & Operational Reports
* Real-time report generation:
  * **Executive Financial Ledger**: Financial overview and route revenues.
  * **Passenger Manifest Summary**: Passenger demographics and loyalty history.
  * **Train Fleet Audit**: Capacity utilization and delay tracking.

### 6. 🛡️ User Management (Admin Only)
* Administrative oversight to inspect, suspend, reinstate, or delete passenger accounts.

---

## 📁 Repository Structure
```text
vscode/
├── index.html           # Full Pamoja Rail standalone web application
├── rail_bg.jpg          # Background visual asset
├── run_web.py           # Single-command Python web runner & browser launcher
├── README.md            # VS Code setup & operating instructions
├── .vscode/             # Pre-configured VS Code settings
│   ├── tasks.json       # Build & run tasks (Ctrl+Shift+B)
│   ├── launch.json      # Debug configurations (F5)
│   └── extensions.json # Recommended VS Code extensions
├── WinFormsApp1/        # Complete VB.NET source files (for optional Windows builds)
└── WinFormsApp1.sln     # Solution file
```

---

## 🎨 Visual Identity
* **Primary Brand Red**: `#E60000` (Pure Vibrant Red)
* **Canvas & Cards**: `#FFFFFF` / `#F8FAFC` (Porcelain White)
* **Luxury Accents**: `#F59E0B` (Imperial Gold)
* **Mobile Ready**: Fully responsive with adaptive drawer navigation (`☰`) for laptops, tablets, and smartphones.
