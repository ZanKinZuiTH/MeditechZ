'use strict';

async function fetchJSON(url){
  const res = await fetch(url,{cache:'no-store'});
  if(!res.ok) throw new Error('Fetch failed: '+url);
  return res.json();
}

function el(tag, cls, text){
  const e = document.createElement(tag);
  if(cls) e.className = cls;
  if(text) e.textContent = text;
  return e;
}

function renderOverview(root, data){
  root.innerHTML='';
  const items = [
    {label:'โปรเจกต์', value:data.project?.name||'-'},
    {label:'สถานะรวม', value:data.project?.status||'-'},
    {label:'ช่วงเวลา', value:(data.project?.start||'?')+' → '+(data.project?.end||'?')},
    {label:'ความคืบหน้า', value:(data.project?.progress??0)+'%'}
  ];
  items.forEach(i=>{
    const kpi = el('div','kpi');
    kpi.appendChild(el('div','label',i.label));
    kpi.appendChild(el('div','value',i.value));
    root.appendChild(kpi);
  });
}

function renderToday(root, today){
  root.innerHTML='';
  const items = [
    {title:'สิ่งที่จะทำวันนี้', value:(today?.plan||'-')},
    {title:'สิ่งที่ทำเสร็จ', value:(today?.done||'-')},
    {title:'อุปสรรค/ความเสี่ยง', value:(today?.risks||'-')},
    {title:'แผนถัดไป', value:(today?.next||'-')}
  ];
  items.forEach(i=>{
    const card = el('div','today-card');
    card.appendChild(el('div','title', i.title));
    card.appendChild(el('div','value', i.value));
    root.appendChild(card);
  });
}

function renderMilestones(root, list){
  root.innerHTML='';
  (list||[]).forEach(m=>{
    const item = el('div','m-item');
    item.appendChild(el('div','title', m.title||'-'));
    const meta = el('div','meta', `${m.start||'?'} → ${m.end||'?'} • ${m.status||'pending'}`);
    item.appendChild(meta);
    root.appendChild(item);
  });
}

function renderTasks(root, tasks){
  root.innerHTML='';
  (tasks||[]).forEach(t=>{
    const item = el('div','task');
    item.appendChild(el('div','title', t.title||'-'));
    if(t.owner){ item.appendChild(el('span','badge', t.owner)); }
    const st = el('span','state '+(t.status||'pending'), t.status||'pending');
    item.appendChild(st);
    root.appendChild(item);
  });
}

function renderChangelog(root, logs){
  root.innerHTML='';
  (logs||[]).slice(0,10).forEach(l=>{
    const item = el('div','c-item');
    item.appendChild(el('div','title', l.title||'-'));
    item.appendChild(el('div','meta', l.date||''));
    root.appendChild(item);
  });
}

function setProgress(percent){
  const bar = document.getElementById('progress-bar');
  const txt = document.getElementById('progress-text');
  if(!bar||!txt) return;
  const p = Math.max(0, Math.min(100, Number(percent)||0));
  bar.style.width = p+'%';
  txt.textContent = p+'%';
}

function setHeaderMeta(data){
  const period = document.getElementById('period');
  const status = document.getElementById('project-status');
  const today = document.getElementById('today-date');
  if(period) period.textContent = (data.project?.start||'?')+' → '+(data.project?.end||'?');
  if(status) status.textContent = data.project?.status||'-';
  if(today) today.textContent = new Date().toLocaleDateString('th-TH');
}

function renderDaily(root, daily){
  root.innerHTML='';
  (daily||[]).forEach(d=>{
    const item = el('div','daily-item');
    item.appendChild(el('div','title', d.title||'-'));
    item.appendChild(el('div','meta', `${d.date||''} • ${d.owner||''}`));
    root.appendChild(item);
  });
}

(async function init(){
  try{
    const data = await fetchJSON('data/status.json');
    setHeaderMeta(data);
    setProgress(data.project?.progress??0);
    renderOverview(document.getElementById('overview'), data);
    renderToday(document.getElementById('today-summary'), data.today);
    renderMilestones(document.getElementById('milestones'), data.milestones);
    renderTasks(document.getElementById('task-list'), data.tasks);
    renderDaily(document.getElementById('daily-list'), data.daily);
    renderChangelog(document.getElementById('changelog-list'), data.changelog);
    document.getElementById('last-updated').textContent = data.updatedAt || new Date().toISOString();
  }catch(err){
    console.error(err);
    document.getElementById('overview').textContent = 'ไม่สามารถโหลดข้อมูลได้';
  }
})();
