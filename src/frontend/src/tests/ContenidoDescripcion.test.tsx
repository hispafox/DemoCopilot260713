import { render, screen } from '@testing-library/react'
import { describe, expect, it } from 'vitest'
import { ContenidoDescripcion } from '../components/ContenidoDescripcion'

describe('ContenidoDescripcion', () => {
  it('renderiza html permitido y elimina script peligroso', () => {
    render(
      <ContenidoDescripcion
        html={'<p>Texto <strong>rico</strong></p><script>alert("x")</script><a href="https://ejemplo.com">abrir</a>'}
      />,
    )

    expect(screen.getByText('rico', { selector: 'strong' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'abrir' })).toHaveAttribute('href', 'https://ejemplo.com')
    expect(document.querySelector('script')).toBeNull()
  })
})