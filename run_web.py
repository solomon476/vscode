#!/usr/bin/env python3
"""
Pamoja Rail Services - Web Application Runner
Launches a local HTTP server and opens index.html in your default web browser.
"""

import http.server
import socketserver
import webbrowser
import os
import sys

PORT = 8000

def run():
    # Ensure current directory is the project directory
    script_dir = os.path.dirname(os.path.abspath(__file__))
    os.chdir(script_dir)

    handler = http.server.SimpleHTTPRequestHandler

    # Allow port reuse so restarting doesn't fail with Address In Use
    socketserver.TCPServer.allow_reuse_address = True

    try:
        with socketserver.TCPServer(("", PORT), handler) as httpd:
            url = f"http://localhost:{PORT}/index.html"
            print("=" * 60)
            print("🚆 PAMOJA RAIL SERVICES - WEB APPLICATION")
            print("=" * 60)
            print(f"Server URL:   {url}")
            print(f"Directory:    {script_dir}")
            print("To stop:      Press Ctrl+C in this terminal")
            print("=" * 60)

            # Attempt to launch browser automatically
            try:
                webbrowser.open(url)
            except Exception:
                pass

            httpd.serve_forever()
    except KeyboardInterrupt:
        print("\n[OK] Web server stopped successfully.")
    except Exception as e:
        print(f"\n[Error] Could not start server on port {PORT}: {e}")
        print("Tip: If the port is busy, you can run: python3 -m http.server 8080")

if __name__ == "__main__":
    run()

