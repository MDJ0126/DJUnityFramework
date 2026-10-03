// Offline lexical highlighting. It does not perform compiler symbol analysis.
const escape = text => text.replaceAll('&', '&amp;').replaceAll('<', '&lt;').replaceAll('>', '&gt;').replaceAll('"', '&quot;');
const keywords = new Set('abstract as async await base bool break byte case catch char checked class const continue decimal default delegate do double else enum event explicit extern false finally fixed float for foreach goto if implicit in int interface internal is lock long namespace new null object operator out override params private protected public readonly record ref required return sbyte sealed short sizeof stackalloc static string struct switch this throw true try typeof uint ulong unchecked unsafe ushort using var virtual void volatile when where while with yield'.split(' '));
const control = new Set('await break case catch continue default do else finally for foreach goto if return switch throw try while yield'.split(' '));

export function highlightCSharp(source) {
  source = source.replaceAll('\r', '');
  const lines = [''];
  const tokens = /\/\/[^\n]*|\/\*[\s\S]*?(?:\*\/|(?![\s\S]))|(?:\$@|@\$|@)"(?:""|[^"])*(?:"|(?![\s\S]))|\$?"""[\s\S]*?(?:"""|(?![\s\S]))|\$?"(?:\\[\s\S]|[^"\\\n])*(?:"|(?![\s\S]))|'(?:\\.|[^'\\\n])*(?:'|(?![\s\S]))|^[ \t]*#[^\n]*|\b(?:0[xX][\da-fA-F]+|\d+(?:\.\d+)?(?:[eE][+-]?\d+)?[fFdDmMuUlL]*)\b|@?[A-Za-z_][\w]*|[\s\S]/gm;
  for (const match of source.matchAll(tokens)) {
    const token = match[0];
    let kind = '';
    if (token.startsWith('//') || token.startsWith('/*')) kind = 'comment';
    else if (/^(?:\$?"|@"|\$@"|@\$"|')/.test(token)) kind = 'string';
    else if (/^\s*#/.test(token)) kind = 'directive';
    else if (/^\d/.test(token)) kind = 'number';
    else if (keywords.has(token)) kind = control.has(token) ? 'control' : 'keyword';
    else if (/^@?[A-Za-z_]\w*$/.test(token)) {
      const after = source.slice(match.index + token.length);
      kind = /^\s*\(/.test(after) ? 'method' : /^[A-Z]/.test(token) ? 'type' : 'identifier';
    }
    token.split('\n').forEach((part, index) => {
      if (index) lines.push('');
      if (part) lines[lines.length - 1] += kind ? `<span class="cs-${kind}">${escape(part)}</span>` : escape(part);
    });
  }
  return lines;
}
