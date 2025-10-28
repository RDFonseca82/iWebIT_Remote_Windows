const http = require('http');
const express = require('express');
const WebSocket = require('ws');

const app = express();
app.use(express.json());
app.get('/health', (req,res)=>res.send('OK'));
app.post('/token', (req,res)=>{
  const p = req.body.password;
  if (p === 'adminpass') {
    return res.json({ token: 'local-demo-token' });
  }
  res.status(401).json({ error: 'unauthorized' });
});

const server = http.createServer(app);
const wss = new WebSocket.Server({ server });

const agents = new Map();

wss.on('connection', (ws, req) => {
  ws.on('message', (msg) => {
    try {
      const j = JSON.parse(msg.toString());
      if (j.type === 'register' && j.payload && j.payload.agentId) {
        agents.set(j.payload.agentId, ws);
        ws.agentId = j.payload.agentId;
        broadcastAgentsList();
      } else if (j.type === 'signal' && j.payload && j.payload.target) {
        const target = agents.get(j.payload.target);
        if (target && target.readyState === WebSocket.OPEN) target.send(JSON.stringify(j.payload.msg));
      }
    } catch(e) { console.error(e); }
  });
  ws.on('close', ()=>{ if (ws.agentId) { agents.delete(ws.agentId); broadcastAgentsList(); } });
});

function broadcastAgentsList() {
  const list = Array.from(agents.keys());
  wss.clients.forEach(c => {
    if (c.readyState === WebSocket.OPEN) c.send(JSON.stringify({ type: 'agents_list', payload: list }));
  });
}

server.listen(8443, ()=>console.log('signaling server running on :8443'));
