import { readdir, readFile } from 'node:fs/promises';
import { resolve } from 'node:path';
import { createHash } from 'node:crypto';

export async function collectScripts(documentRoot) {
  const projectRoot = resolve(documentRoot, '../..');
  const scripts = [];
  async function visit(folder) {
    for (const entry of await readdir(resolve(projectRoot, folder), { withFileTypes: true })) {
      const path = `${folder}/${entry.name}`;
      if (entry.isDirectory()) await visit(path);
      else if (entry.name.endsWith('.cs')) {
        const source = await readFile(resolve(projectRoot, path), 'utf8');
        scripts.push({ id: 'script/' + encodeURIComponent(path), path, name: entry.name, source, hash: createHash('sha256').update(source).digest('hex') });
      }
    }
  }
  await visit('Assets/Scripts');
  return scripts.sort((a, b) => a.path.localeCompare(b.path, 'en'));
}
