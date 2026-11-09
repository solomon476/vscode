# 🚆 Pamoja Rail Services – Native Desktop Edition (VS Code)

A complete, standalone commercial railway management and ticket reservation desktop system running **natively inside Visual Studio Code** on Linux, Windows, and macOS.

**100% Native Desktop Application — Zero Web Browser or Server Required.**

---

## ⚡ Quick Start: Running in VS Code

### 1. Open the Project in VS Code
```bash
git clone https://github.com/solomon476/vscode.git
cd vscode
code .
```

### 2. Run the Application Directly

#### Method A: Run with Python (Single Command)
In the VS Code terminal (`Ctrl` + `` ` ``), run:
```bash
python3 main.py
```
👉 The **native Pamoja Rail Services desktop application window** will pop up immediately on your screen!

#### Method B: One-Key Shortcut in VS Code
* Press **`Ctrl` + `Shift` + `B`** (or **`F5`**).
* The application window launches instantly.

#### Method C: Interactive Terminal Mode (CLI)
If you want to operate the train system completely inside the VS Code terminal text interface:
```bash
python3 main.py --cli
```

---

## 🔑 Login Credentials

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
* **Self-Registration**: Click **"New passenger? Register Member Account"** to create a profile instantly.

---

## 🧭 System Features & Capabilities

1. **📊 Operations Overview Dashboard**:
   * Live real-time KPIs: Total Bookings, Registered Members, On-Time Fleet, Delay Alerts.
2. **📋 Manage Bookings**:
   * Reservation ledger table, automated ticket reference generation (`PRS-940x`), class tariffs (Economy: KES 1,500, First: KES 3,000, VIP: KES 6,000), booking cancellation.
3. **👥 Passenger Directory**:
   * Complete manifest tracking total trips, cumulative expenditure, and preferred travel class.
4. **🚆 Train Fleet & Timetables**:
   * Fleet routes across Kenya (Nairobi, Mombasa, Kisumu, Nakuru, Eldoret), departure times, passenger capacities, and live status toggles (*On Time / Active*, *Delayed*, *Maintenance*, *Cancelled*).
5. **📈 System Reports**:
   * Revenue analytics, class breakdown, and route performance.
6. **🛡️ Member User Management (Admin Only)**:
   * Account suspension, reinstatement, and permanent removal.

---

## 📁 Repository Structure
```text
vscode/
├── main.py              # Main Native Desktop GUI & CLI Application
├── pamoja_data.json     # Local persistent data storage (auto-created)
├── README.md            # Documentation and instructions
├── .vscode/             # VS Code task runner & F5 launch configurations
│   ├── tasks.json       # Run Train Services (Ctrl+Shift+B)
│   ├── launch.json      # Debug & Launch configurations (F5)
│   └── extensions.json # Recommended VS Code extensions
├── WinFormsApp1/        # Complete VB.NET source files
└── WinFormsApp1.sln     # Solution file
```
