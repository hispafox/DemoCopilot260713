import { sanitizarHtml } from '../utils/sanitizarHtml'

interface ContenidoDescripcionProps {
  html: string
  className?: string
}

export function ContenidoDescripcion({ html, className }: ContenidoDescripcionProps) {
  return <div className={className} dangerouslySetInnerHTML={{ __html: sanitizarHtml(html) }} />
}