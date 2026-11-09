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
    script_dir = os.path.dirname(os.path.abspath(__file__))
    os.chdir(script_dir)

    handler = http.server.SimpleHTTPRequestHandler
    socketserver.TCPServer.allow_reuse_address = True

    try:
        with socketserver.TCPServer(("", PORT), handler) as httpd:
            url = f"http://localhost:{PORT}/index.html"
            print("=" * 65, flush=True)
            print("🚆 PAMOJA RAIL SERVICES - WEB SERVER IS ACTIVE & READY!", flush=True)
            print("=" * 65, flush=True)
            print(f"👉 Application URL:   {url}", flush=True)
            print("💡 In VS Code:       Press Ctrl+Shift+P -> 'Simple Browser: Show'", flush=True)
            print("                     then enter the URL to preview inside VS Code!", flush=True)
            print("🌐 In Web Browser:   Open Firefox or Chrome and visit the URL above.", flush=True)
            print("⛔ To Stop Server:   Press Ctrl+C in this terminal", flush=True)
            print("=" * 65, flush=True)

            try:
                webbrowser.open(url)
            except Exception:
                pass

            httpd.serve_forever()
    except KeyboardInterrupt:
        print("\n[OK] Web server stopped.", flush=True)
    except Exception as e:
        print(f"\n[Error] Could not start server on port {PORT}: {e}", flush=True)

if __name__ == "__main__":
    run()
