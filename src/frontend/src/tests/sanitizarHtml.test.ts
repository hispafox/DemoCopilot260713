import { describe, expect, it } from 'vitest'
import { normalizarHtmlEditor, sanitizarHtml } from '../utils/sanitizarHtml'

describe('sanitizarHtml', () => {
  it('normaliza html vacio del editor', () => {
    expect(normalizarHtmlEditor('<p></p>')).toBe('')
    expect(normalizarHtmlEditor('<p><br></p>')).toBe('')
  })

  it('elimina atributos no permitidos', () => {
    const html = sanitizarHtml('<p onclick="alert(1)">hola</p><img src="x" onerror="alert(1)" />')

    expect(html).toContain('<p>hola</p>')
    expect(html).toContain('<img src="x">')
    expect(html).not.toContain('onclick')
    expect(html).not.toContain('onerror')
  })
})