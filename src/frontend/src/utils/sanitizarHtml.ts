import DOMPurify from 'dompurify'

export function sanitizarHtml(html: string) {
  return DOMPurify.sanitize(html, {
    USE_PROFILES: { html: true },
    ALLOWED_ATTR: ['href', 'target', 'rel', 'src', 'alt', 'title', 'class'],
  })
}

export function normalizarHtmlEditor(html: string) {
  const htmlRecortado = html.trim()

  if (!htmlRecortado || htmlRecortado === '<p></p>' || htmlRecortado === '<p><br></p>') {
    return ''
  }

  return html
}