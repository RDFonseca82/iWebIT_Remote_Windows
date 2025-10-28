Remote Agent Demo Project
=========================

This archive contains a minimal skeleton of:
 - agent_windows (C# .NET Worker service) - minimal skeleton
 - signaling_server (Node.js) - simple WebSocket relay server
 - client_electron (Electron) - simple client that lists agents and requests session

How to use (basic):
 - Signaling server:
    cd signaling_server
    npm install
    node server.js
 - Agent (Windows .NET 7):
    cd agent_windows
    dotnet restore
    dotnet publish -c Release -r win-x64 --self-contained false
    (install the produced exe as a service with sc.exe create ...)
 - Client (Electron):
    cd client_electron
    npm install
    npm start

Security notes:
 - This is a demo skeleton. Use only on private trusted networks.
 - No authentication beyond simple demo token. Replace with JWT/mTLS for production.
 - The agent currently does not implement frame sending or file transfer - it's a skeleton to extend.

