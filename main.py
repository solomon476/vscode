#!/usr/bin/env python3
"""
====================================================================
🚆 PAMOJA RAIL SERVICES - DESKTOP CONTROL CENTER
Native GUI Application (No Browser Required)
Runs natively in VS Code on Linux, Windows, and macOS.
====================================================================
"""

import os
import sys
import json
import tkinter as tk
from tkinter import ttk, messagebox

# ════════════════════════════════════════════════════════════════════
# 1. DATA SERVICE & PERSISTENCE
# ════════════════════════════════════════════════════════════════════

DATA_FILE = os.path.join(os.path.dirname(os.path.abspath(__file__)), "pamoja_data.json")

DEFAULT_USERS = [
    {"id": "admin-1", "username": "admin", "password": "admin123", "role": "Admin", "name": "Chief Operations Admin", "suspended": False},
    {"id": "m-1", "username": "john.kamau", "password": "member123", "role": "Member", "name": "John Kamau", "suspended": False},
    {"id": "m-2", "username": "faith.mwangi", "password": "member123", "role": "Member", "name": "Faith Mwangi", "suspended": False},
    {"id": "m-3", "username": "david.otieno", "password": "member123", "role": "Member", "name": "David Otieno", "suspended": False},
    {"id": "m-4", "username": "sarah.w", "password": "member123", "role": "Member", "name": "Sarah Wanjiku", "suspended": False}
]

DEFAULT_TRAINS = [
    {"id": "tr-101", "num": "Express 101", "route": "Nairobi - Mombasa", "dep": "Nairobi Terminus", "arr": "Mombasa Terminus", "time": "08:00", "cap": 560, "eco": 1500, "first": 3000, "vip": 6000, "status": "On Time / Active"},
    {"id": "tr-204", "num": "Inter-County 204", "route": "Nairobi - Kisumu", "dep": "Nairobi Central", "arr": "Kisumu Terminus", "time": "10:30", "cap": 420, "eco": 1200, "first": 2500, "vip": 5000, "status": "On Time / Active"},
    {"id": "tr-305", "num": "Coastliner 305", "route": "Mombasa - Nairobi", "dep": "Mombasa Terminus", "arr": "Nairobi Terminus", "time": "15:00", "cap": 560, "eco": 1500, "first": 3000, "vip": 6000, "status": "On Time / Active"},
    {"id": "tr-410", "num": "Rift Valley Link 410", "route": "Nakuru - Eldoret", "dep": "Nakuru Station", "arr": "Eldoret Station", "time": "14:15", "cap": 280, "eco": 900, "first": 1800, "vip": 3500, "status": "Maintenance"},
    {"id": "tr-502", "num": "Night Star 502", "route": "Nairobi - Mombasa", "dep": "Nairobi Terminus", "arr": "Mombasa Terminus", "time": "22:00", "cap": 700, "eco": 1800, "first": 3500, "vip": 7500, "status": "Delayed"}
]

DEFAULT_BOOKINGS = [
    {"id": "bk-9401", "ref": "PRS-9401", "train": "Express 101", "route": "Nairobi - Mombasa", "passenger": "John Kamau", "class": "First Class", "date": "2026-11-02", "fare": 3000, "member": "john.kamau", "status": "Confirmed"},
    {"id": "bk-9402", "ref": "PRS-9402", "train": "Inter-County 204", "route": "Nairobi - Kisumu", "passenger": "Faith Mwangi", "class": "VIP Sleeper", "date": "2026-11-03", "fare": 5000, "member": "faith.mwangi", "status": "Confirmed"},
    {"id": "bk-9403", "ref": "PRS-9403", "train": "Coastliner 305", "route": "Mombasa - Nairobi", "passenger": "David Otieno", "class": "Economy", "date": "2026-11-02", "fare": 1500, "member": "david.otieno", "status": "Confirmed"},
    {"id": "bk-9404", "ref": "PRS-9404", "train": "Express 101", "route": "Nairobi - Mombasa", "passenger": "Sarah Wanjiku", "class": "First Class", "date": "2026-11-04", "fare": 3000, "member": "sarah.w", "status": "Confirmed"},
    {"id": "bk-9405", "ref": "PRS-9405", "train": "Night Star 502", "route": "Nairobi - Mombasa", "passenger": "Peter Omondi", "class": "VIP Sleeper", "date": "2026-11-05", "fare": 7500, "member": "admin", "status": "Confirmed"}
]

class DataStore:
    def __init__(self):
        self.users = []
        self.trains = []
        self.bookings = []
        self.load()

    def load(self):
        if os.path.exists(DATA_FILE):
            try:
                with open(DATA_FILE, "r", encoding="utf-8") as f:
                    data = json.load(f)
                    self.users = data.get("users", DEFAULT_USERS)
                    self.trains = data.get("trains", DEFAULT_TRAINS)
                    self.bookings = data.get("bookings", DEFAULT_BOOKINGS)
                    return
            except Exception:
                pass
        self.users = list(DEFAULT_USERS)
        self.trains = list(DEFAULT_TRAINS)
        self.bookings = list(DEFAULT_BOOKINGS)
        self.save()

    def save(self):
        try:
            with open(DATA_FILE, "w", encoding="utf-8") as f:
                json.dump({
                    "users": self.users,
                    "trains": self.trains,
                    "bookings": self.bookings
                }, f, indent=2)
        except Exception as e:
            print(f"Error saving data: {e}")

    def validate_login(self, username, password, target_role):
        u = next((x for x in self.users if x["username"].lower() == username.lower() and x["password"] == password and x["role"].lower() == target_role.lower()), None)
        if not u:
            return None, f"Invalid username or password for {target_role} account."
        if u.get("suspended", False):
            return None, "This account has been suspended by Administration."
        return u, None

    def register_member(self, name, username, password):
        if any(x["username"].lower() == username.lower() for x in self.users):
            return False, "Username is already registered."
        new_u = {
            "id": f"m-{len(self.users)+1}",
            "username": username,
            "password": password,
            "role": "Member",
            "name": name,
            "suspended": False
        }
        self.users.append(new_u)
        self.save()
        return True, "Member account created successfully!"


# ════════════════════════════════════════════════════════════════════
# 2. MODERN DESKTOP GUI IMPLEMENTATION
# ════════════════════════════════════════════════════════════════════

# Brand Palette
CLR_RED_HEADER = "#B91C1C"  # Crimson Red
CLR_RED_ACCENT = "#DC2626"  # Vibrant Red
CLR_GOLD       = "#D97706"  # Amber Gold
CLR_GOLD_LIGHT = "#FEF3C7"  # Soft Gold
CLR_BG_MAIN    = "#F8FAFC"  # Clean Porcelain Slate
CLR_CARD_BG    = "#FFFFFF"  # Pure White
CLR_TEXT_DARK  = "#0F172A"  # Dark Slate
CLR_TEXT_MUTED = "#64748B"  # Muted Slate
CLR_BORDER     = "#E2E8F0"  # Crisp Border
CLR_GREEN      = "#16A34A"  # Emerald Green

class PamojaRailApp(tk.Tk):
    def __init__(self):
        super().__init__()
        self.title("Pamoja Rail Services - Control Center")
        self.geometry("1100x720")
        self.minsize(980, 640)
        self.configure(bg=CLR_BG_MAIN)

        self.db = DataStore()
        self.current_user = None

        self.setup_styles()
        self.show_login_screen()

    def setup_styles(self):
        style = ttk.Style(self)
        style.theme_use("clam")

        # Treeview styling
        style.configure("Treeview",
                        background=CLR_CARD_BG,
                        foreground=CLR_TEXT_DARK,
                        rowheight=28,
                        fieldbackground=CLR_CARD_BG,
                        font=("Segoe UI", 10))
        style.configure("Treeview.Heading",
                        background=CLR_RED_HEADER,
                        foreground="#FFFFFF",
                        font=("Segoe UI", 10, "bold"),
                        padding=6)
        style.map("Treeview.Heading", background=[("active", CLR_RED_ACCENT)])
        style.map("Treeview", background=[("selected", CLR_RED_ACCENT)], foreground=[("selected", "#FFFFFF")])

    def clear_window(self):
        for widget in self.winfo_children():
            widget.destroy()

    # ────────────────────────────────────────────────────────────
    # SCREEN 1: LOGIN PORTAL
    # ────────────────────────────────────────────────────────────
    def show_login_screen(self):
        self.clear_window()
        self.current_user = None

        # Outer Container
        outer = tk.Frame(self, bg=CLR_BG_MAIN)
        outer.place(relx=0.5, rely=0.5, anchor="center")

        # Card Box
        card = tk.Frame(outer, bg=CLR_CARD_BG, padx=40, pady=35, highlightbackground=CLR_RED_HEADER, highlightthickness=3)
        card.pack()

        # Logo / Badge
        lbl_badge = tk.Label(card, text="🚆 PAMOJA RAIL SERVICES", font=("Segoe UI", 12, "bold"), bg=CLR_RED_HEADER, fg="#FFFFFF", padx=16, pady=5)
        lbl_badge.pack(pady=(0, 15))

        lbl_title = tk.Label(card, text="Commercial Railway Portal", font=("Segoe UI", 18, "bold"), bg=CLR_CARD_BG, fg=CLR_TEXT_DARK)
        lbl_title.pack()

        lbl_sub = tk.Label(card, text="Please select your account type and sign in", font=("Segoe UI", 10), bg=CLR_CARD_BG, fg=CLR_TEXT_MUTED)
        lbl_sub.pack(pady=(2, 20))

        # Role Selector (Admin vs Member)
        self.login_role = tk.StringVar(value="Admin")
        frame_role = tk.Frame(card, bg=CLR_CARD_BG)
        frame_role.pack(fill="x", pady=(0, 20))

        btn_role_admin = tk.Radiobutton(frame_role, text="Administrator", variable=self.login_role, value="Admin",
                                        font=("Segoe UI", 10, "bold"), bg=CLR_CARD_BG, fg=CLR_RED_HEADER,
                                        selectcolor=CLR_GOLD_LIGHT, activebackground=CLR_CARD_BG,
                                        command=self._on_role_change)
        btn_role_admin.pack(side="left", expand=True)

        btn_role_member = tk.Radiobutton(frame_role, text="Member Portal", variable=self.login_role, value="Member",
                                         font=("Segoe UI", 10, "bold"), bg=CLR_CARD_BG, fg=CLR_RED_HEADER,
                                         selectcolor=CLR_GOLD_LIGHT, activebackground=CLR_CARD_BG,
                                         command=self._on_role_change)
        btn_role_member.pack(side="right", expand=True)

        # Username Field
        lbl_u = tk.Label(card, text="USERNAME", font=("Segoe UI", 9, "bold"), bg=CLR_CARD_BG, fg=CLR_TEXT_MUTED, anchor="w")
        lbl_u.pack(fill="x", pady=(5, 2))
        self.ent_user = tk.Entry(card, font=("Segoe UI", 11), width=32, relief="solid", bd=1)
        self.ent_user.pack(pady=(0, 12), ipady=4)
        self.ent_user.insert(0, "admin")

        # Password Field
        lbl_p = tk.Label(card, text="PASSWORD", font=("Segoe UI", 9, "bold"), bg=CLR_CARD_BG, fg=CLR_TEXT_MUTED, anchor="w")
        lbl_p.pack(fill="x", pady=(5, 2))
        self.ent_pass = tk.Entry(card, font=("Segoe UI", 11), width=32, show="•", relief="solid", bd=1)
        self.ent_pass.pack(pady=(0, 20), ipady=4)
        self.ent_pass.insert(0, "admin123")

        # Sign In Button
        self.btn_login = tk.Button(card, text="Sign In as Administrator", font=("Segoe UI", 11, "bold"),
                                   bg=CLR_RED_HEADER, fg="#FFFFFF", activebackground=CLR_RED_ACCENT,
                                   activeforeground="#FFFFFF", relief="flat", cursor="hand2",
                                   command=self.do_login)
        self.btn_login.pack(fill="x", ipady=8, pady=(0, 12))

        # Register Member Link
        self.btn_reg = tk.Button(card, text="New passenger? Register Member Account", font=("Segoe UI", 9, "underline"),
                                 bg=CLR_CARD_BG, fg=CLR_GOLD, activebackground=CLR_CARD_BG, activeforeground=CLR_RED_HEADER,
                                 relief="flat", cursor="hand2", command=self.show_register_dialog)
        self.btn_reg.pack()

        # Keyboard Enter binds to login
        self.bind("<Return>", lambda e: self.do_login())

    def _on_role_change(self):
        role = self.login_role.get()
        if role == "Admin":
            self.btn_login.configure(text="Sign In as Administrator")
            self.ent_user.delete(0, tk.END)
            self.ent_user.insert(0, "admin")
            self.ent_pass.delete(0, tk.END)
            self.ent_pass.insert(0, "admin123")
        else:
            self.btn_login.configure(text="Sign In as Member")
            self.ent_user.delete(0, tk.END)
            self.ent_user.insert(0, "john.kamau")
            self.ent_pass.delete(0, tk.END)
            self.ent_pass.insert(0, "member123")

    def do_login(self):
        u = self.ent_user.get().strip()
        p = self.ent_pass.get().strip()
        role = self.login_role.get()

        if not u or not p:
            messagebox.showwarning("Required", "Please enter both username and password.", parent=self)
            return

        user, err = self.db.validate_login(u, p, role)
        if err:
            messagebox.showerror("Sign In Failed", err, parent=self)
            return

        self.current_user = user
        self.show_dashboard_screen()

    def show_register_dialog(self):
        dlg = tk.Toplevel(self)
        dlg.title("Passenger Member Registration")
        dlg.geometry("400x320")
        dlg.resizable(False, False)
        dlg.configure(bg=CLR_BG_MAIN, padx=25, pady=20)
        dlg.transient(self)
        dlg.grab_set()

        tk.Label(dlg, text="Register New Member Account", font=("Segoe UI", 13, "bold"), bg=CLR_BG_MAIN, fg=CLR_RED_HEADER).pack(pady=(0, 15))

        tk.Label(dlg, text="Full Name:", font=("Segoe UI", 9, "bold"), bg=CLR_BG_MAIN, fg=CLR_TEXT_DARK, anchor="w").pack(fill="x")
        ent_n = tk.Entry(dlg, font=("Segoe UI", 10))
        ent_n.pack(fill="x", pady=(2, 10))

        tk.Label(dlg, text="Choose Username:", font=("Segoe UI", 9, "bold"), bg=CLR_BG_MAIN, fg=CLR_TEXT_DARK, anchor="w").pack(fill="x")
        ent_u = tk.Entry(dlg, font=("Segoe UI", 10))
        ent_u.pack(fill="x", pady=(2, 10))

        tk.Label(dlg, text="Choose Password:", font=("Segoe UI", 9, "bold"), bg=CLR_BG_MAIN, fg=CLR_TEXT_DARK, anchor="w").pack(fill="x")
        ent_p = tk.Entry(dlg, font=("Segoe UI", 10), show="•")
        ent_p.pack(fill="x", pady=(2, 15))

        def save():
            n = ent_n.get().strip()
            un = ent_u.get().strip()
            pw = ent_p.get().strip()
            if not n or not un or not pw:
                messagebox.showwarning("Warning", "All fields are required.", parent=dlg)
                return
            ok, msg = self.db.register_member(n, un, pw)
            if ok:
                messagebox.showinfo("Success", msg, parent=dlg)
                dlg.destroy()
                self.login_role.set("Member")
                self._on_role_change()
                self.ent_user.delete(0, tk.END)
                self.ent_user.insert(0, un)
                self.ent_pass.delete(0, tk.END)
                self.ent_pass.insert(0, pw)
            else:
                messagebox.showerror("Error", msg, parent=dlg)

        tk.Button(dlg, text="Register Account", font=("Segoe UI", 10, "bold"), bg=CLR_RED_HEADER, fg="#FFFFFF",
                  relief="flat", cursor="hand2", command=save).pack(fill="x", ipady=6)

    # ────────────────────────────────────────────────────────────
    # SCREEN 2: MAIN DASHBOARD & TABS
    # ────────────────────────────────────────────────────────────
    def show_dashboard_screen(self):
        self.clear_window()
        self.unbind("<Return>")

        is_admin = (self.current_user["role"] == "Admin")

        # TOP BAR
        top_bar = tk.Frame(self, bg=CLR_RED_HEADER, height=60, padx=20)
        top_bar.pack(fill="x", side="top")

        lbl_logo = tk.Label(top_bar, text="🚆 PAMOJA RAIL SERVICES", font=("Segoe UI", 14, "bold"), bg=CLR_RED_HEADER, fg="#FFFFFF")
        lbl_logo.pack(side="left", pady=14)

        mode_text = "ADMINISTRATOR CONTROL CENTER" if is_admin else "PASSENGER PORTAL"
        lbl_mode = tk.Label(top_bar, text=f"MODE: {mode_text}", font=("Segoe UI", 10, "bold"), bg=CLR_RED_HEADER, fg=CLR_GOLD_LIGHT)
        lbl_mode.pack(side="left", padx=25, pady=16)

        btn_logout = tk.Button(top_bar, text="Sign Out", font=("Segoe UI", 9, "bold"), bg="#991B1B", fg="#FFFFFF",
                               activebackground=CLR_RED_ACCENT, activeforeground="#FFFFFF", relief="flat", padx=12, pady=4,
                               cursor="hand2", command=self.show_login_screen)
        btn_logout.pack(side="right", pady=14)

        lbl_user = tk.Label(top_bar, text=f"👤 {self.current_user['name']}", font=("Segoe UI", 10), bg=CLR_RED_HEADER, fg="#FFFFFF")
        lbl_user.pack(side="right", padx=15, pady=16)

        # MAIN WORKSPACE SPLIT (Sidebar + Content)
        workspace = tk.Frame(self, bg=CLR_BG_MAIN)
        workspace.pack(fill="both", expand=True)

        # SIDEBAR
        sidebar = tk.Frame(workspace, bg="#1E293B", width=220, padx=10, pady=15)
        sidebar.pack(fill="y", side="left")
        sidebar.pack_propagate(False)

        # CONTENT CONTAINER
        self.content_area = tk.Frame(workspace, bg=CLR_BG_MAIN, padx=25, pady=20)
        self.content_area.pack(fill="both", expand=True, side="right")

        # Sidebar Buttons
        self.nav_buttons = {}

        def add_nav(text, cmd, key):
            btn = tk.Button(sidebar, text=f"  {text}", font=("Segoe UI", 10, "bold"), anchor="w",
                            bg="#1E293B", fg="#F8FAFC", activebackground=CLR_RED_HEADER, activeforeground="#FFFFFF",
                            relief="flat", padx=10, pady=10, cursor="hand2", command=cmd)
            btn.pack(fill="x", pady=4)
            self.nav_buttons[key] = btn

        if is_admin:
            add_nav("📊 Operations Dashboard", self.nav_dashboard, "dash")
            add_nav("📋 Manage Bookings", self.nav_bookings, "bookings")
            add_nav("👥 Passenger Manifest", self.nav_passengers, "passengers")
            add_nav("🚆 Train Fleet & Routes", self.nav_trains, "trains")
            add_nav("📈 System Reports", self.nav_reports, "reports")
            add_nav("🛡️ User Management", self.nav_users, "users")
        else:
            add_nav("📊 My Travel Overview", self.nav_dashboard, "dash")
            add_nav("🎟️ Book a Ticket", self.nav_book_ticket, "book")
            add_nav("📋 My Bookings", self.nav_bookings, "bookings")
            add_nav("🚆 Train Timetable", self.nav_trains, "trains")

        # Default View
        self.nav_dashboard()

    def _highlight_nav(self, active_key):
        for k, btn in self.nav_buttons.items():
            if k == active_key:
                btn.configure(bg=CLR_RED_HEADER, fg="#FFFFFF")
            else:
                btn.configure(bg="#1E293B", fg="#F8FAFC")

    def _clear_content(self):
        for widget in self.content_area.winfo_children():
            widget.destroy()

    # ────────────────────────────────────────────────────────────
    # VIEW: DASHBOARD
    # ────────────────────────────────────────────────────────────
    def nav_dashboard(self):
        self._highlight_nav("dash")
        self._clear_content()

        is_admin = (self.current_user["role"] == "Admin")

        # Header Title
        title_text = "Operations Overview Dashboard" if is_admin else f"Welcome back, {self.current_user['name']}!"
        sub_text = "Real-time railway network metrics, fleet and booking status" if is_admin else "Your personal Pamoja Rail travel overview"

        tk.Label(self.content_area, text=title_text, font=("Segoe UI", 16, "bold"), bg=CLR_BG_MAIN, fg=CLR_RED_HEADER).pack(anchor="w")
        tk.Label(self.content_area, text=sub_text, font=("Segoe UI", 10), bg=CLR_BG_MAIN, fg=CLR_TEXT_MUTED).pack(anchor="w", pady=(2, 20))

        # Metric Cards Container
        cards_frame = tk.Frame(self.content_area, bg=CLR_BG_MAIN)
        cards_frame.pack(fill="x", pady=(0, 25))

        def make_card(parent, title, val, color):
            c = tk.Frame(parent, bg=CLR_CARD_BG, padx=20, pady=15, relief="solid", bd=1, highlightbackground=CLR_BORDER)
            c.pack(side="left", fill="both", expand=True, padx=8)
            tk.Label(c, text=title, font=("Segoe UI", 9, "bold"), bg=CLR_CARD_BG, fg=CLR_TEXT_MUTED).pack(anchor="w")
            tk.Label(c, text=val, font=("Segoe UI", 20, "bold"), bg=CLR_CARD_BG, fg=color).pack(anchor="w", pady=(5, 0))

        if is_admin:
            total_b = len(self.db.bookings)
            members = len([u for u in self.db.users if u["role"] == "Member"])
            active_t = len([t for t in self.db.trains if "Active" in t["status"] or "On Time" in t["status"]])
            delayed = len([t for t in self.db.trains if "Delayed" in t["status"] or "Maintenance" in t["status"]])

            make_card(cards_frame, "TOTAL BOOKINGS", str(total_b), CLR_RED_HEADER)
            make_card(cards_frame, "REGISTERED MEMBERS", str(members), CLR_TEXT_DARK)
            make_card(cards_frame, "ON-TIME FLEET", str(active_t), CLR_GREEN)
            make_card(cards_frame, "ALERTS / DELAYED", str(delayed), CLR_GOLD)
        else:
            my_b = [b for b in self.db.bookings if b["member"] == self.current_user["username"]]
            spent = sum(b["fare"] for b in my_b)
            make_card(cards_frame, "MY BOOKINGS", str(len(my_b)), CLR_RED_HEADER)
            make_card(cards_frame, "TOTAL SPENT (KES)", f"{spent:,.0f}", CLR_GOLD)
            make_card(cards_frame, "ACCOUNT STATUS", "Active", CLR_GREEN)

        # Quick Actions Box
        quick_box = tk.LabelFrame(self.content_area, text=" Quick Services ", font=("Segoe UI", 11, "bold"),
                                  bg=CLR_CARD_BG, fg=CLR_RED_HEADER, padx=20, pady=15, relief="solid", bd=1)
        quick_box.pack(fill="both", expand=True)

        if is_admin:
            tk.Label(quick_box, text="System Administrator Actions:", font=("Segoe UI", 10, "bold"), bg=CLR_CARD_BG, fg=CLR_TEXT_DARK).pack(anchor="w", pady=(0, 10))
            btn_row = tk.Frame(quick_box, bg=CLR_CARD_BG)
            btn_row.pack(anchor="w")
            tk.Button(btn_row, text="➕ Add New Train Schedule", bg=CLR_RED_HEADER, fg="#FFFFFF", font=("Segoe UI", 10, "bold"),
                      relief="flat", padx=12, pady=6, cursor="hand2", command=self.show_add_train_dialog).pack(side="left", padx=(0, 10))
            tk.Button(btn_row, text="📋 View All Reservations", bg=CLR_TEXT_DARK, fg="#FFFFFF", font=("Segoe UI", 10, "bold"),
                      relief="flat", padx=12, pady=6, cursor="hand2", command=self.nav_bookings).pack(side="left", padx=10)
            tk.Button(btn_row, text="📊 Run Analytics Report", bg=CLR_GOLD, fg="#FFFFFF", font=("Segoe UI", 10, "bold"),
                      relief="flat", padx=12, pady=6, cursor="hand2", command=self.nav_reports).pack(side="left", padx=10)
        else:
            tk.Label(quick_box, text="Book your next rail trip seamlessly across Kenya:", font=("Segoe UI", 10), bg=CLR_CARD_BG, fg=CLR_TEXT_DARK).pack(anchor="w", pady=(0, 10))
            tk.Button(quick_box, text="🎟️ Reserve a Ticket Now", bg=CLR_RED_HEADER, fg="#FFFFFF", font=("Segoe UI", 11, "bold"),
                      relief="flat", padx=16, pady=8, cursor="hand2", command=self.nav_book_ticket).pack(anchor="w")

    # ────────────────────────────────────────────────────────────
    # VIEW: BOOKINGS MANAGEMENT
    # ────────────────────────────────────────────────────────────
    def nav_bookings(self):
        self._highlight_nav("bookings")
        self._clear_content()

        is_admin = (self.current_user["role"] == "Admin")
        title = "Comprehensive Reservation Ledger" if is_admin else "My Ticket Bookings"
        tk.Label(self.content_area, text=title, font=("Segoe UI", 16, "bold"), bg=CLR_BG_MAIN, fg=CLR_RED_HEADER).pack(anchor="w")

        # Top Buttons
        btn_bar = tk.Frame(self.content_area, bg=CLR_BG_MAIN)
        btn_bar.pack(fill="x", pady=(10, 12))

        tk.Button(btn_bar, text="➕ New Reservation", bg=CLR_RED_HEADER, fg="#FFFFFF", font=("Segoe UI", 9, "bold"),
                  relief="flat", padx=12, pady=5, cursor="hand2", command=self.show_add_booking_dialog).pack(side="left")

        if is_admin:
            tk.Button(btn_bar, text="🗑️ Cancel/Delete Selected", bg="#991B1B", fg="#FFFFFF", font=("Segoe UI", 9, "bold"),
                      relief="flat", padx=12, pady=5, cursor="hand2", command=self._delete_selected_booking).pack(side="left", padx=10)

        # Treeview Table
        cols = ("ref", "passenger", "route", "train", "class", "date", "fare", "booked_by", "status")
        tree = ttk.Treeview(self.content_area, columns=cols, show="headings", selectmode="browse")
        self.tree_bookings = tree

        tree.heading("ref", text="Ticket Ref")
        tree.heading("passenger", text="Passenger Name")
        tree.heading("route", text="Route")
        tree.heading("train", text="Train")
        tree.heading("class", text="Class")
        tree.heading("date", text="Travel Date")
        tree.heading("fare", text="Fare (KES)")
        tree.heading("booked_by", text="Booked By")
        tree.heading("status", text="Status")

        tree.column("ref", width=90, anchor="center")
        tree.column("passenger", width=140)
        tree.column("route", width=160)
        tree.column("train", width=120)
        tree.column("class", width=100)
        tree.column("date", width=100, anchor="center")
        tree.column("fare", width=90, anchor="e")
        tree.column("booked_by", width=100)
        tree.column("status", width=90, anchor="center")

        tree.pack(fill="both", expand=True)
        self._populate_bookings_table()

    def _populate_bookings_table(self):
        for item in self.tree_bookings.get_children():
            self.tree_bookings.delete(item)

        is_admin = (self.current_user["role"] == "Admin")
        items = self.db.bookings if is_admin else [b for b in self.db.bookings if b["member"] == self.current_user["username"]]

        for b in items:
            self.tree_bookings.insert("", "end", iid=b["id"], values=(
                b["ref"], b["passenger"], b["route"], b["train"], b["class"],
                b["date"], f"{b['fare']:,}", b["member"], b["status"]
            ))

    def _delete_selected_booking(self):
        sel = self.tree_bookings.selection()
        if not sel:
            messagebox.showinfo("Select", "Please select a booking to delete.", parent=self)
            return
        b_id = sel[0]
        if messagebox.askyesno("Confirm", "Are you sure you want to cancel and delete this reservation?", parent=self):
            self.db.bookings = [b for b in self.db.bookings if b["id"] != b_id]
            self.db.save()
            self._populate_bookings_table()
            messagebox.showinfo("Deleted", "Booking deleted successfully.", parent=self)

    # ────────────────────────────────────────────────────────────
    # VIEW: PASSENGER MANIFEST
    # ────────────────────────────────────────────────────────────
    def nav_passengers(self):
        self._highlight_nav("passengers")
        self._clear_content()

        tk.Label(self.content_area, text="Passenger Manifest & Travel History", font=("Segoe UI", 16, "bold"), bg=CLR_BG_MAIN, fg=CLR_RED_HEADER).pack(anchor="w")
        tk.Label(self.content_area, text="Consolidated passenger activity and loyalty statistics", font=("Segoe UI", 10), bg=CLR_BG_MAIN, fg=CLR_TEXT_MUTED).pack(anchor="w", pady=(2, 15))

        cols = ("name", "trips", "spend", "top_class", "status")
        tree = ttk.Treeview(self.content_area, columns=cols, show="headings", selectmode="browse")
        tree.heading("name", text="Passenger Name")
        tree.heading("trips", text="Total Trips")
        tree.heading("spend", text="Total Spent (KES)")
        tree.heading("top_class", text="Preferred Class")
        tree.heading("status", text="Account Status")

        tree.column("name", width=180)
        tree.column("trips", width=100, anchor="center")
        tree.column("spend", width=140, anchor="e")
        tree.column("top_class", width=140, anchor="center")
        tree.column("status", width=120, anchor="center")
        tree.pack(fill="both", expand=True)

        for u in [x for x in self.db.users if x["role"] == "Member"]:
            u_bookings = [b for b in self.db.bookings if b["member"].lower() == u["username"].lower()]
            total_spent = sum(b["fare"] for b in u_bookings)
            classes = [b["class"] for b in u_bookings]
            top_cls = max(set(classes), key=classes.count) if classes else "Economy"
            st = "Suspended" if u.get("suspended") else "Active"
            tree.insert("", "end", values=(u["name"], len(u_bookings), f"{total_spent:,}", top_cls, st))

    # ────────────────────────────────────────────────────────────
    # VIEW: TRAIN FLEET & TIMETABLE
    # ────────────────────────────────────────────────────────────
    def nav_trains(self):
        self._highlight_nav("trains")
        self._clear_content()

        is_admin = (self.current_user["role"] == "Admin")
        tk.Label(self.content_area, text="Train Fleet & Schedule Operations", font=("Segoe UI", 16, "bold"), bg=CLR_BG_MAIN, fg=CLR_RED_HEADER).pack(anchor="w")

        btn_bar = tk.Frame(self.content_area, bg=CLR_BG_MAIN)
        btn_bar.pack(fill="x", pady=(10, 12))

        if is_admin:
            tk.Button(btn_bar, text="➕ Add New Train Fleet", bg=CLR_RED_HEADER, fg="#FFFFFF", font=("Segoe UI", 9, "bold"),
                      relief="flat", padx=12, pady=5, cursor="hand2", command=self.show_add_train_dialog).pack(side="left")
            tk.Button(btn_bar, text="🔄 Toggle Status", bg=CLR_GOLD, fg="#FFFFFF", font=("Segoe UI", 9, "bold"),
                      relief="flat", padx=12, pady=5, cursor="hand2", command=self._toggle_train_status).pack(side="left", padx=10)

        cols = ("num", "route", "dep", "arr", "time", "cap", "eco", "first", "vip", "status")
        tree = ttk.Treeview(self.content_area, columns=cols, show="headings", selectmode="browse")
        self.tree_trains = tree

        tree.heading("num", text="Train No")
        tree.heading("route", text="Route")
        tree.heading("dep", text="Departure")
        tree.heading("arr", text="Arrival")
        tree.heading("time", text="Time")
        tree.heading("cap", text="Capacity")
        tree.heading("eco", text="Economy")
        tree.heading("first", text="First Class")
        tree.heading("vip", text="VIP")
        tree.heading("status", text="Operational Status")

        tree.column("num", width=120)
        tree.column("route", width=150)
        tree.column("dep", width=130)
        tree.column("arr", width=130)
        tree.column("time", width=70, anchor="center")
        tree.column("cap", width=70, anchor="center")
        tree.column("eco", width=80, anchor="e")
        tree.column("first", width=80, anchor="e")
        tree.column("vip", width=80, anchor="e")
        tree.column("status", width=120, anchor="center")

        tree.pack(fill="both", expand=True)
        self._populate_trains_table()

    def _populate_trains_table(self):
        for item in self.tree_trains.get_children():
            self.tree_trains.delete(item)
        for t in self.db.trains:
            self.tree_trains.insert("", "end", iid=t["id"], values=(
                t["num"], t["route"], t["dep"], t["arr"], t["time"], t["cap"],
                f"{t['eco']:,}", f"{t['first']:,}", f"{t['vip']:,}", t["status"]
            ))

    def _toggle_train_status(self):
        sel = self.tree_trains.selection()
        if not sel:
            messagebox.showinfo("Select", "Please select a train from the table.", parent=self)
            return
        t_id = sel[0]
        tr = next((t for t in self.db.trains if t["id"] == t_id), None)
        if tr:
            statuses = ["On Time / Active", "Delayed", "Maintenance", "Cancelled"]
            idx = statuses.index(tr["status"]) if tr["status"] in statuses else 0
            tr["status"] = statuses[(idx + 1) % len(statuses)]
            self.db.save()
            self._populate_trains_table()
            messagebox.showinfo("Updated", f"Status for '{tr['num']}' changed to: {tr['status']}", parent=self)

    # ────────────────────────────────────────────────────────────
    # VIEW: SYSTEM REPORTS & ANALYTICS
    # ────────────────────────────────────────────────────────────
    def nav_reports(self):
        self._highlight_nav("reports")
        self._clear_content()

        tk.Label(self.content_area, text="System Analytics & Operational Reports", font=("Segoe UI", 16, "bold"), bg=CLR_BG_MAIN, fg=CLR_RED_HEADER).pack(anchor="w")

        # Top Summary Box
        total_rev = sum(b["fare"] for b in self.db.bookings)
        sum_box = tk.Frame(self.content_area, bg=CLR_CARD_BG, padx=15, pady=12, relief="solid", bd=1)
        sum_box.pack(fill="x", pady=(10, 15))

        tk.Label(sum_box, text=f"Total Revenue: KES {total_rev:,.0f}   |   Confirmed Bookings: {len(self.db.bookings)}   |   Active Fleet: {len(self.db.trains)}",
                 font=("Segoe UI", 11, "bold"), bg=CLR_CARD_BG, fg=CLR_RED_HEADER).pack(anchor="w")

        cols = ("ref", "route", "passenger", "class", "fare", "date")
        tree = ttk.Treeview(self.content_area, columns=cols, show="headings")
        tree.heading("ref", text="Ticket Ref")
        tree.heading("route", text="Route")
        tree.heading("passenger", text="Passenger")
        tree.heading("class", text="Class")
        tree.heading("fare", text="Revenue (KES)")
        tree.heading("date", text="Date")

        tree.column("ref", width=100, anchor="center")
        tree.column("route", width=180)
        tree.column("passenger", width=150)
        tree.column("class", width=120)
        tree.column("fare", width=110, anchor="e")
        tree.column("date", width=110, anchor="center")

        tree.pack(fill="both", expand=True)

        for b in self.db.bookings:
            tree.insert("", "end", values=(b["ref"], b["route"], b["passenger"], b["class"], f"{b['fare']:,}", b["date"]))

    # ────────────────────────────────────────────────────────────
    # VIEW: USER MANAGEMENT (ADMIN ONLY)
    # ────────────────────────────────────────────────────────────
    def nav_users(self):
        self._highlight_nav("users")
        self._clear_content()

        tk.Label(self.content_area, text="Member Account Administration", font=("Segoe UI", 16, "bold"), bg=CLR_BG_MAIN, fg=CLR_RED_HEADER).pack(anchor="w")
        tk.Label(self.content_area, text="Manage passenger permissions, suspension, and account removals", font=("Segoe UI", 10), bg=CLR_BG_MAIN, fg=CLR_TEXT_MUTED).pack(anchor="w", pady=(2, 12))

        btn_bar = tk.Frame(self.content_area, bg=CLR_BG_MAIN)
        btn_bar.pack(fill="x", pady=(0, 10))

        tk.Button(btn_bar, text="⚠️ Suspend / Reinstate Account", bg=CLR_GOLD, fg="#FFFFFF", font=("Segoe UI", 9, "bold"),
                  relief="flat", padx=12, pady=5, cursor="hand2", command=self._toggle_user_suspend).pack(side="left")
        tk.Button(btn_bar, text="🗑️ Delete Account", bg="#991B1B", fg="#FFFFFF", font=("Segoe UI", 9, "bold"),
                  relief="flat", padx=12, pady=5, cursor="hand2", command=self._delete_user).pack(side="left", padx=10)

        cols = ("username", "name", "role", "bookings", "status")
        tree = ttk.Treeview(self.content_area, columns=cols, show="headings", selectmode="browse")
        self.tree_users = tree

        tree.heading("username", text="Username")
        tree.heading("name", text="Full Name")
        tree.heading("role", text="Role")
        tree.heading("bookings", text="Total Bookings")
        tree.heading("status", text="Account Status")

        tree.column("username", width=130)
        tree.column("name", width=180)
        tree.column("role", width=100, anchor="center")
        tree.column("bookings", width=100, anchor="center")
        tree.column("status", width=120, anchor="center")

        tree.pack(fill="both", expand=True)
        self._populate_users_table()

    def _populate_users_table(self):
        for item in self.tree_users.get_children():
            self.tree_users.delete(item)
        for u in self.db.users:
            b_count = len([b for b in self.db.bookings if b["member"].lower() == u["username"].lower()])
            st = "Suspended" if u.get("suspended") else "Active"
            self.tree_users.insert("", "end", iid=u["id"], values=(
                u["username"], u["name"], u["role"], b_count, st
            ))

    def _toggle_user_suspend(self):
        sel = self.tree_users.selection()
        if not sel:
            messagebox.showinfo("Select", "Please select a user from the table.", parent=self)
            return
        u_id = sel[0]
        user = next((u for u in self.db.users if u["id"] == u_id), None)
        if not user:
            return
        if user["username"].lower() == "admin":
            messagebox.showwarning("Security", "Cannot suspend the Primary System Administrator.", parent=self)
            return
        user["suspended"] = not user.get("suspended", False)
        self.db.save()
        self._populate_users_table()
        state = "suspended" if user["suspended"] else "reinstated"
        messagebox.showinfo("Account Updated", f"Account '{user['username']}' has been {state}.", parent=self)

    def _delete_user(self):
        sel = self.tree_users.selection()
        if not sel:
            messagebox.showinfo("Select", "Please select a user from the table.", parent=self)
            return
        u_id = sel[0]
        user = next((u for u in self.db.users if u["id"] == u_id), None)
        if not user:
            return
        if user["username"].lower() == "admin":
            messagebox.showwarning("Security", "Cannot delete the Primary System Administrator.", parent=self)
            return
        if messagebox.askyesno("Confirm", f"Permanently delete user '{user['username']}'?", parent=self):
            self.db.users = [u for u in self.db.users if u["id"] != u_id]
            self.db.save()
            self._populate_users_table()
            messagebox.showinfo("Deleted", "Member account removed.", parent=self)

    # ────────────────────────────────────────────────────────────
    # VIEW: MEMBER BOOK A TICKET
    # ────────────────────────────────────────────────────────────
    def nav_book_ticket(self):
        self._highlight_nav("book")
        self.show_add_booking_dialog()

    # ────────────────────────────────────────────────────────────
    # MODAL DIALOGS
    # ────────────────────────────────────────────────────────────
    def show_add_booking_dialog(self):
        dlg = tk.Toplevel(self)
        dlg.title("New Rail Ticket Reservation")
        dlg.geometry("450x420")
        dlg.resizable(False, False)
        dlg.configure(bg=CLR_BG_MAIN, padx=25, pady=20)
        dlg.transient(self)
        dlg.grab_set()

        tk.Label(dlg, text="Reserve Rail Journey", font=("Segoe UI", 13, "bold"), bg=CLR_BG_MAIN, fg=CLR_RED_HEADER).pack(pady=(0, 15))

        tk.Label(dlg, text="Passenger Full Name:", font=("Segoe UI", 9, "bold"), bg=CLR_BG_MAIN, fg=CLR_TEXT_DARK, anchor="w").pack(fill="x")
        ent_p = tk.Entry(dlg, font=("Segoe UI", 10))
        ent_p.pack(fill="x", pady=(2, 10))
        default_name = self.current_user["name"] if self.current_user["role"] == "Member" else ""
        ent_p.insert(0, default_name)

        tk.Label(dlg, text="Select Route:", font=("Segoe UI", 9, "bold"), bg=CLR_BG_MAIN, fg=CLR_TEXT_DARK, anchor="w").pack(fill="x")
        routes = [t["route"] for t in self.db.trains]
        cmb_r = ttk.Combobox(dlg, values=routes, state="readonly", font=("Segoe UI", 10))
        cmb_r.pack(fill="x", pady=(2, 10))
        cmb_r.current(0)

        tk.Label(dlg, text="Ticket Class:", font=("Segoe UI", 9, "bold"), bg=CLR_BG_MAIN, fg=CLR_TEXT_DARK, anchor="w").pack(fill="x")
        classes = ["Economy (KES 1,500)", "First Class (KES 3,000)", "VIP Sleeper (KES 6,000)"]
        cmb_c = ttk.Combobox(dlg, values=classes, state="readonly", font=("Segoe UI", 10))
        cmb_c.pack(fill="x", pady=(2, 10))
        cmb_c.current(0)

        tk.Label(dlg, text="Travel Date (YYYY-MM-DD):", font=("Segoe UI", 9, "bold"), bg=CLR_BG_MAIN, fg=CLR_TEXT_DARK, anchor="w").pack(fill="x")
        ent_d = tk.Entry(dlg, font=("Segoe UI", 10))
        ent_d.pack(fill="x", pady=(2, 20))
        ent_d.insert(0, "2026-11-15")

        def confirm():
            pname = ent_p.get().strip()
            route = cmb_r.get()
            t_class_raw = cmb_c.get()
            date = ent_d.get().strip()

            if not pname or not date:
                messagebox.showwarning("Warning", "Please fill out all fields.", parent=dlg)
                return

            if "Economy" in t_class_raw:
                cls_name = "Economy"
                fare = 1500
            elif "First" in t_class_raw:
                cls_name = "First Class"
                fare = 3000
            else:
                cls_name = "VIP Sleeper"
                fare = 6000

            # Match train
            matched_train = next((t["num"] for t in self.db.trains if t["route"] == route), "Express 101")
            new_ref = f"PRS-{9400 + len(self.db.bookings) + 1}"

            new_booking = {
                "id": f"bk-{len(self.db.bookings)+1}",
                "ref": new_ref,
                "train": matched_train,
                "route": route,
                "passenger": pname,
                "class": cls_name,
                "date": date,
                "fare": fare,
                "member": self.current_user["username"],
                "status": "Confirmed"
            }
            self.db.bookings.append(new_booking)
            self.db.save()

            messagebox.showinfo("Confirmed!", f"Ticket Reserved Successfully!\n\nReference: {new_ref}\nFare: KES {fare:,}\nTrain: {matched_train}", parent=dlg)
            dlg.destroy()
            self.nav_bookings()

        tk.Button(dlg, text="Confirm & Issue Ticket", font=("Segoe UI", 10, "bold"), bg=CLR_RED_HEADER, fg="#FFFFFF",
                  relief="flat", cursor="hand2", command=confirm).pack(fill="x", ipady=6)

    def show_add_train_dialog(self):
        dlg = tk.Toplevel(self)
        dlg.title("Add Train Schedule")
        dlg.geometry("420x400")
        dlg.resizable(False, False)
        dlg.configure(bg=CLR_BG_MAIN, padx=25, pady=20)
        dlg.transient(self)
        dlg.grab_set()

        tk.Label(dlg, text="Add Train Fleet Schedule", font=("Segoe UI", 13, "bold"), bg=CLR_BG_MAIN, fg=CLR_RED_HEADER).pack(pady=(0, 15))

        tk.Label(dlg, text="Train Number / Name:", font=("Segoe UI", 9, "bold"), bg=CLR_BG_MAIN, fg=CLR_TEXT_DARK, anchor="w").pack(fill="x")
        ent_num = tk.Entry(dlg, font=("Segoe UI", 10))
        ent_num.pack(fill="x", pady=(2, 8))
        ent_num.insert(0, "Express 103")

        tk.Label(dlg, text="Route Name:", font=("Segoe UI", 9, "bold"), bg=CLR_BG_MAIN, fg=CLR_TEXT_DARK, anchor="w").pack(fill="x")
        ent_r = tk.Entry(dlg, font=("Segoe UI", 10))
        ent_r.pack(fill="x", pady=(2, 8))
        ent_r.insert(0, "Nairobi - Mombasa")

        tk.Label(dlg, text="Departure Time (e.g. 09:30):", font=("Segoe UI", 9, "bold"), bg=CLR_BG_MAIN, fg=CLR_TEXT_DARK, anchor="w").pack(fill="x")
        ent_t = tk.Entry(dlg, font=("Segoe UI", 10))
        ent_t.pack(fill="x", pady=(2, 8))
        ent_t.insert(0, "09:30")

        tk.Label(dlg, text="Passenger Capacity:", font=("Segoe UI", 9, "bold"), bg=CLR_BG_MAIN, fg=CLR_TEXT_DARK, anchor="w").pack(fill="x")
        ent_c = tk.Entry(dlg, font=("Segoe UI", 10))
        ent_c.pack(fill="x", pady=(2, 18))
        ent_c.insert(0, "560")

        def save_train():
            num = ent_num.get().strip()
            route = ent_r.get().strip()
            t = ent_t.get().strip()
            c = ent_c.get().strip()
            if not num or not route:
                messagebox.showwarning("Warning", "Train Number and Route are required.", parent=dlg)
                return
            cap_val = int(c) if c.isdigit() else 500
            new_tr = {
                "id": f"tr-{len(self.db.trains)+101}",
                "num": num,
                "route": route,
                "dep": "Nairobi Terminus",
                "arr": "Destination Terminus",
                "time": t,
                "cap": cap_val,
                "eco": 1500,
                "first": 3000,
                "vip": 6000,
                "status": "On Time / Active"
            }
            self.db.trains.append(new_tr)
            self.db.save()
            messagebox.showinfo("Success", f"Train '{num}' added to active schedules!", parent=dlg)
            dlg.destroy()
            self.nav_trains()

        tk.Button(dlg, text="Save Train Record", font=("Segoe UI", 10, "bold"), bg=CLR_RED_HEADER, fg="#FFFFFF",
                  relief="flat", cursor="hand2", command=save_train).pack(fill="x", ipady=6)


# ════════════════════════════════════════════════════════════════════
# 3. INTERACTIVE TERMINAL CLI MODE (FALLBACK IF NO DISPLAY)
# ════════════════════════════════════════════════════════════════════

def run_cli_mode(db):
    print("=" * 60)
    print("🚆 PAMOJA RAIL SERVICES - TERMINAL CONTROL CENTER")
    print("=" * 60)
    u_input = input("Username [default: admin]: ").strip() or "admin"
    p_input = input("Password [default: admin123]: ").strip() or "admin123"

    user, err = db.validate_login(u_input, p_input, "Admin")
    if not user:
        user, err = db.validate_login(u_input, p_input, "Member")
    if not user:
        print(f"❌ Login Failed: {err}")
        return

    print(f"\n✅ Signed in as: {user['name']} ({user['role']} Mode)")
    while True:
        print("\n" + "-" * 50)
        print("1. View Active Train Fleet & Timetable")
        print("2. View All Bookings")
        print("3. Reserve a New Ticket")
        if user["role"] == "Admin":
            print("4. View Passenger Manifest")
            print("5. Member User Oversight")
        print("0. Exit")
        choice = input("\nSelect Option: ").strip()

        if choice == "1":
            print("\n🚆 ACTIVE TRAIN FLEET:")
            for t in db.trains:
                print(f" • {t['num']:<18} | {t['route']:<20} | Time: {t['time']} | Status: {t['status']}")
        elif choice == "2":
            print("\n📋 BOOKINGS LIST:")
            items = db.bookings if user["role"] == "Admin" else [b for b in db.bookings if b["member"] == user["username"]]
            for b in items:
                print(f" • [{b['ref']}] {b['passenger']:<16} | {b['route']:<20} | {b['class']} | KES {b['fare']:,}")
        elif choice == "3":
            pname = input("Passenger Name: ").strip() or user["name"]
            route = input("Route [default: Nairobi - Mombasa]: ").strip() or "Nairobi - Mombasa"
            ref = f"PRS-{9400 + len(db.bookings) + 1}"
            db.bookings.append({
                "id": f"bk-{len(db.bookings)+1}",
                "ref": ref,
                "train": "Express 101",
                "route": route,
                "passenger": pname,
                "class": "First Class",
                "date": "2026-11-20",
                "fare": 3000,
                "member": user["username"],
                "status": "Confirmed"
            })
            db.save()
            print(f"✅ Ticket Confirmed! Ref: {ref} (KES 3,000)")
        elif choice == "4" and user["role"] == "Admin":
            print("\n👥 PASSENGER MANIFEST:")
            for m in [x for x in db.users if x["role"] == "Member"]:
                print(f" • {m['name']:<18} ({m['username']}) - Status: {'Suspended' if m.get('suspended') else 'Active'}")
        elif choice == "5" and user["role"] == "Admin":
            print("\n🛡️ USERS:")
            for m in db.users:
                print(f" • {m['username']:<15} | Role: {m['role']} | Status: {'Suspended' if m.get('suspended') else 'Active'}")
        elif choice == "0":
            print("Exiting Pamoja Rail Control Center.")
            break


# ════════════════════════════════════════════════════════════════════
# 4. APPLICATION ENTRYPOINT
# ════════════════════════════════════════════════════════════════════

def main():
    db = DataStore()

    # If --cli argument is passed, run terminal interactive mode
    if "--cli" in sys.argv:
        run_cli_mode(db)
        return

    # Try launching native Desktop GUI
    try:
        app = PamojaRailApp()
        app.mainloop()
    except Exception as e:
        print(f"\n[Notice] Could not initialize graphical window ({e}).")
        print("Switching to interactive terminal mode:\n")
        run_cli_mode(db)

if __name__ == "__main__":
    main()

