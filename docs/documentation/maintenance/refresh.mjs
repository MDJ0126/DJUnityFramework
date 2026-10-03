import { readFile, writeFile } from 'node:fs/promises';
import { createHash } from 'node:crypto';
import { spawnSync } from 'node:child_process';
import { dirname, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

const root = resolve(dirname(fileURLToPath(import.meta.url)), '../../..');
const statePath = resolve(root, 'docs/documentation/maintenance/update-state.json');
const historyPath = resolve(root, 'docs/documentation/11-update-history.md');
const scopeVersion = 1;
function git(...args) {
  const result = spawnSync('git', args, { cwd: root, encoding: 'utf8', maxBuffer: 32 * 1024 * 1024 });
  if (result.error || result.status !== 0) throw new Error(result.error?.message || result.stderr.trim() || `git ${args.join(' ')} failed`);
  return result.stdout;
}
function list(output) { return output.split('\0').filter(Boolean); }
function inScope(file) {
  if (['README.md', 'LICENSE', 'NamingConvention.md', 'AGENTS.md', '.gitignore'].includes(file)) return true;
  if (file.startsWith('docs/documentation/')) {
    return !file.includes('/vendor/') && !file.endsWith('/11-update-history.md') && /\.(md|mjs)$/.test(file);
  }
  if (file.startsWith('ProjectSettings/') || file.startsWith('Packages/')) return /\.(asset|json|txt|meta)$/.test(file);
  if (!file.startsWith('Assets/') || file.startsWith('Assets/ArtResources/') || file.startsWith('Assets/TextMesh Pro/')) return false;
  return /\.(cs|meta|unity|prefab|inputactions|controller|overrideController|asset|asmdef|asmref|shader|shadergraph|shadersubgraph|uxml|uss|json|txt|md)$/.test(file);
}
async function snapshot() {
  const paths = [...new Set(list(git('ls-files', '-z', '--cached', '--others', '--exclude-standard')))].filter(inScope).sort();
  const hashes = {};
  for (const file of paths) {
    try {
      const content = (await readFile(resolve(root, file), 'utf8')).replaceAll('\r\n', '\n');
      hashes[file] = createHash('sha256').update(content).digest('hex');
    } catch (error) { if (error.code !== 'ENOENT') throw error; }
  }
  return hashes;
}
async function readState() {
  try { return JSON.parse(await readFile(statePath, 'utf8')); }
  catch (error) { if (error.code === 'ENOENT') return null; throw error; }
}
function validity(state, head) {
  if (!state) return '마지막 갱신 기록 없음';
  if (state.scopeVersion !== scopeVersion || !state.files) return '비교 범위 또는 스냅샷 형식 변경';
  if (!/^[0-9a-f]{40,64}$/.test(state.revision || '')) return '기준 리비전 형식 오류';
  const found = spawnSync('git', ['cat-file', '-e', `${state.revision}^{commit}`], { cwd: root });
  if (found.status !== 0) return '기준 리비전이 현재 저장소에 없음';
  const ancestor = spawnSync('git', ['merge-base', '--is-ancestor', state.revision, head], { cwd: root });
  if (ancestor.status !== 0) return '현재 HEAD가 마지막 기준에서 이어지는 이력이 아님';
  return null;
}
const [command = 'status', ...args] = process.argv.slice(2);
if (!['status', 'record'].includes(command)) throw new Error('Usage: refresh.mjs status | record --mode full|incremental --reviewed --note TEXT --unverified TEXT');
const previous = await readState();
const head = git('rev-parse', 'HEAD').trim();
const reason = validity(previous, head);
const files = await snapshot();
const snapshotChanges = Object.keys({ ...previous?.files, ...files }).sort().filter(file => previous?.files?.[file] !== files[file]).map(file => ({
  path: file,
  change: !previous?.files?.[file] ? 'added' : !files[file] ? 'deleted' : 'modified',
}));
const committedChanges = !reason ? list(git('diff', '--name-only', '-z', previous.revision, head, '--')) : [];
const uncommittedFiles = [...new Set([...list(git('diff', '--name-only', '-z', 'HEAD', '--')), ...list(git('ls-files', '-z', '--others', '--exclude-standard'))])].sort().filter(inScope);
const outsideScopeChanges = [...new Set([...committedChanges, ...list(git('diff', '--name-only', '-z', 'HEAD', '--')), ...list(git('ls-files', '-z', '--others', '--exclude-standard'))])].filter(file => !inScope(file) && !file.startsWith('docs/documentation/') && file !== 'AGENTS.md').sort();
const report = { previousRevision: previous?.revision || null, currentRevision: head, fullRefreshRequired: Boolean(reason), reason, committedChanges, snapshotChanges, uncommittedFiles, outsideScopeChanges, scannedFileCount: Object.keys(files).length };
if (command === 'status') {
  console.log(JSON.stringify(report, null, 2));
} else {
  const option = name => { const index = args.indexOf(name); return index >= 0 ? args[index + 1] : undefined; };
  const mode = option('--mode');
  const note = option('--note');
  const unverified = option('--unverified');
  if (!args.includes('--reviewed') || !['full', 'incremental'].includes(mode) || !note || unverified === undefined) throw new Error('Recording requires --reviewed, --mode full|incremental, --note and --unverified');
  if (reason && mode !== 'full') throw new Error(`Full review required: ${reason}`);
  // Require validated output before changing the baseline. Final history output is rebuilt afterward.
  const checked = spawnSync(process.execPath, [resolve(root, 'docs/documentation/maintenance/validate.mjs')], { cwd: root, encoding: 'utf8' });
  if (checked.status !== 0) throw new Error(checked.stderr || checked.stdout || 'Validation failed');
  if (git('rev-parse', 'HEAD').trim() !== head || JSON.stringify(await snapshot()) !== JSON.stringify(files)) throw new Error('Project changed during recording; inspect changes again');
  const updatedAt = new Date().toISOString();
  const koreaTime = new Intl.DateTimeFormat('sv-SE', { timeZone: 'Asia/Seoul', dateStyle: 'short', timeStyle: 'medium' }).format(new Date(updatedAt));
  let history;
  try { history = await readFile(historyPath, 'utf8'); }
  catch (error) { if (error.code !== 'ENOENT') throw error; history = '# 갱신 리비전과 이력\n\n상세 상태는 maintenance/update-state.json에 보관합니다.\n'; }
  const sourceCount = Object.keys(files).filter(file => /^Assets\/Scripts\/.*\.cs$/.test(file)).length;
  const entry = `\n## ${koreaTime} KST · ${mode === 'full' ? '전체 갱신' : '증분 갱신'}\n\n- SHA: ${head}\n- 내용: ${note.replaceAll('\n', ' ')}\n- 검증: 빌드·페이지·링크 통과. 미확인: ${unverified.replaceAll('\n', ' ')}\n`;
  // Keep the visible log short; retain full audit details outside the reading pages.
  const detailsPath = resolve(root, 'docs/documentation/maintenance/history-details.jsonl');
  let details;
  try { details = await readFile(detailsPath, 'utf8'); }
  catch (error) { if (error.code !== 'ENOENT') throw error; details = ''; }
  const review = { updatedAt, timeZone: 'Asia/Seoul', mode, previousRevision: previous?.revision || null, revision: head, sourceCount, fileCount: Object.keys(files).length, note, unverified, uncommittedFiles, outsideScopeChanges, files };
  await writeFile(detailsPath, details + JSON.stringify(review) + '\n', 'utf8');
  await writeFile(historyPath, history + entry, 'utf8');
  await writeFile(statePath, JSON.stringify({ scopeVersion, revision: head, updatedAt, timeZone: 'Asia/Seoul', mode, note, unverified, uncommittedFiles, outsideScopeChanges, files }, null, 2) + '\n', 'utf8');
  console.log(`Recorded ${mode} review at ${head}. Rebuild and validate the history page before reporting completion.`);
}
