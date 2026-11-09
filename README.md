# 🚆 Pamoja Rail Services – Standalone Desktop Application

A complete commercial railway operations, fleet scheduling, and passenger ticket reservation desktop system.

**100% Cross-Platform Native Desktop Application** — Runs on **Linux, Windows, and macOS** with standard Python 3. No external packages, web browsers, or servers required.

---

## 🚀 How to Run on Any Laptop (VS Code)

### Step 1: Open in VS Code
Open the project folder in VS Code (`File` $\rightarrow$ `Open Folder...` $\rightarrow$ `vscode`).

### Step 2: Run the Application
In your VS Code terminal (`Ctrl` + `` ` ``), run:
```bash
python3 main.py
```
*(On Windows you can also run `python main.py`, or press **`F5`** / **`Ctrl` + `Shift` + `B`** in VS Code)*.

👉 The native desktop application window **"Pamoja Rail Services - Control Center"** will pop up immediately!

*(Optional: To run in text-only terminal mode, use `python3 main.py --cli`)*.

---

## 🔑 Default Login Credentials

* **Administrator Portal**:
  * Role: **`Administrator`**
  * Username: `admin`
  * Password: `admin123`
* **Passenger Member Portal**:
  * Role: **`Member Portal`**
  * Username: `john.kamau`
  * Password: `member123`
  * *(Or click "Register Member Account" to create a new passenger account)*

---

## 🧭 System Features

1. **Operations Dashboard**: Real-time KPIs for Total Bookings, Active Fleet, Registered Members, and Alerts.
2. **Ticket Reservations**: Issue tickets across Kenya (Nairobi, Mombasa, Kisumu, Nakuru, Eldoret) with automated class tariffs (Economy, First Class, VIP Sleeper) and reference numbers (`PRS-940x`).
3. **Passenger Directory**: Unified manifest with trip totals, cumulative spending (KES), and preferred travel class.
4. **Train Fleet & Schedules**: Fleet timetable management, departure times, capacity limits, and operational status toggles (*On Time / Active*, *Delayed*, *Maintenance*, *Cancelled*).
5. **System Reports**: Financial summaries and booking distribution analytics.
6. **User Oversight (Admin Only)**: Suspend, reinstate, or delete passenger member accounts.
7. **Persistent Storage**: All updates and reservations persist in `pamoja_data.json`.

---

## 📁 Clean Repository Structure
```text
vscode/
├── main.py              # Main Native Desktop Application
├── pamoja_data.json     # Local persistent data file
├── README.md            # Setup & usage instructions
└── .vscode/             # VS Code task runner & F5 launch configurations
    ├── tasks.json       # Run task (Ctrl+Shift+B)
    ├── launch.json      # F5 Run/Debug configuration
    └── extensions.json # Recommended Python extension
```
