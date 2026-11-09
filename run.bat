@echo off
title Pamoja Rail Services - Control Center
cd /d "%~dp0"

echo ===================================================
echo 🚆 Launching Pamoja Rail Services Desktop Control Center...
echo ===================================================

where python >nul 2>&1
if %errorlevel% equ 0 (
    python main.py
    exit /b
)

where py >nul 2>&1
if %errorlevel% equ 0 (
    py main.py
    exit /b
)

where python3 >nul 2>&1
if %errorlevel% equ 0 (
    python3 main.py
    exit /b
)

echo.
echo [ERROR] Python was not found on this computer!
echo.
echo Please install Python from: https://www.python.org/downloads/
echo Make sure to check the box: [x] "Add python.exe to PATH" during installation.
echo.
pause

