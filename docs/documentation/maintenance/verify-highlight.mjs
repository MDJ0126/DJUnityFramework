import assert from 'node:assert/strict';
import { highlightCSharp } from './csharp-highlight.mjs';

const source = 'public class Pawn {\n/* multi\nline */\nstring text = @"<tag>\n""value""";\nif (true) Move(12.5f); // 설명\n}';
const lines = highlightCSharp(source);
assert.equal(lines.length, source.split('\n').length);
assert(lines[0].includes('cs-keyword'));
assert(lines[0].includes('cs-type'));
assert(lines[2].includes('cs-comment'));
assert(lines[3].includes('&lt;tag&gt;'));
assert(lines[4].includes('cs-string'));
assert(lines[5].includes('cs-control'));
assert(lines[5].includes('cs-method'));
assert(lines[5].includes('cs-number'));
const decoded = lines.join('\n').replace(/<[^>]+>/g, '').replaceAll('&quot;', '"').replaceAll('&gt;', '>').replaceAll('&lt;', '<').replaceAll('&amp;', '&');
assert.equal(decoded, source, 'Highlighting must preserve source text');
assert(!highlightCSharp('"</script><img>"').join('').includes('<img>'));
console.log('PASS: C# tokens, multiline comments/strings, line counts, source preservation and HTML escaping.');
