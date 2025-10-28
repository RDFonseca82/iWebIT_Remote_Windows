const ws = new WebSocket('ws://127.0.0.1:8443');
const agentsSelect = document.getElementById('agents');
const refreshBtn = document.getElementById('refresh');
const connectBtn = document.getElementById('connect');
const screenImg = document.getElementById('screen');

ws.addEventListener('open', () => {
  console.log('connected to signaling server');
});
ws.addEventListener('message', (ev) => {
  try {
    const j = JSON.parse(ev.data);
    if (j.type === 'agents_list') {
      agentsSelect.innerHTML = '';
      for (const a of j.payload) {
        const opt = document.createElement('option'); opt.value = a; opt.textContent = a;
        agentsSelect.appendChild(opt);
      }
    } else if (j.type === 'frame' && j.payload) {
      // payload expected as base64 jpeg string (demo)
      screenImg.src = 'data:image/jpeg;base64,' + j.payload;
    }
  } catch(e) { console.error(e); }
});

refreshBtn.onclick = () => { /* server broadcasts agent list automatically */ };
connectBtn.onclick = () => {
  const target = agentsSelect.value;
  if (!target) return alert('select agent');
  const msg = { type: 'signal', payload: { target: target, msg: { type: 'start_session', payload: { sessionId: 'sess-' + Date.now() } } } };
  ws.send(JSON.stringify(msg));
  alert('start_session sent (demo). Agent must implement sending frames.');
};
