import { EditorContent, useEditor } from '@tiptap/react'
import { BubbleMenu } from '@tiptap/react/menus'
import CharacterCount from '@tiptap/extension-character-count'
import Image from '@tiptap/extension-image'
import Placeholder from '@tiptap/extension-placeholder'
import StarterKit from '@tiptap/starter-kit'
import type { DragEvent } from 'react'
import { useEffect, useId, useMemo, useState } from 'react'
import { ContenidoDescripcion } from './ContenidoDescripcion'
import { normalizarHtmlEditor } from '../utils/sanitizarHtml'

interface EditorDescripcionProps {
  id: string
  valor: string
  maxLength: number
  placeholder?: string
  onChange: (valor: string) => void
}

interface AccionEditor {
  etiqueta: string
  titulo: string
  activa?: boolean
  onClick: () => void
}

const extensionesBase = [
  StarterKit.configure({
    heading: {
      levels: [2, 3],
    },
    link: {
      openOnClick: false,
      autolink: true,
      defaultProtocol: 'https',
      HTMLAttributes: {
        rel: 'noreferrer noopener',
        target: '_blank',
      },
    },
  }),
  Image.configure({
    inline: false,
    allowBase64: true,
  }),
]

export function EditorDescripcion({ id, valor, maxLength, placeholder, onChange }: EditorDescripcionProps) {
  const [estaPantallaCompleta, setEstaPantallaCompleta] = useState(false)
  const [mensajeImagen, setMensajeImagen] = useState('Arrastra, pega o inserta una imagen.')
  const identificadorAyuda = useId()

  const editor = useEditor({
    immediatelyRender: false,
    extensions: [
      ...extensionesBase,
      CharacterCount.configure({
        limit: maxLength,
      }),
      Placeholder.configure({
        placeholder: placeholder ?? 'Describe la tarea con contexto, checklist o enlaces.',
      }),
    ],
    content: valor || '',
    editorProps: {
      attributes: {
        class: 'editor-descripcion__superficie',
      },
      handlePaste(_view, evento) {
        const archivos = Array.from(evento.clipboardData?.files ?? [])
        const imagen = archivos.find((archivo) => archivo.type.startsWith('image/'))

        if (!imagen) {
          return false
        }

        evento.preventDefault()
        void insertarImagenDesdeArchivo(imagen)
        return true
      },
      handleDrop(_view, evento) {
        const archivos = Array.from(evento.dataTransfer?.files ?? [])
        const imagen = archivos.find((archivo) => archivo.type.startsWith('image/'))

        if (!imagen) {
          return false
        }

        evento.preventDefault()
        void insertarImagenDesdeArchivo(imagen)
        return true
      },
    },
    onUpdate({ editor: editorActualizado }) {
      onChange(normalizarHtmlEditor(editorActualizado.getHTML()))
    },
  })

  useEffect(() => {
    if (!editor) {
      return
    }

    const htmlActual = normalizarHtmlEditor(editor.getHTML())
    const htmlExterno = normalizarHtmlEditor(valor)

    if (htmlActual !== htmlExterno) {
      editor.commands.setContent(htmlExterno || '', { emitUpdate: false })
    }
  }, [editor, valor])

  const acciones = useMemo<AccionEditor[]>(() => {
    if (!editor) {
      return []
    }

    return [
      {
        etiqueta: 'B',
        titulo: 'Negrita',
        activa: editor.isActive('bold'),
        onClick: () => editor.chain().focus().toggleBold().run(),
      },
      {
        etiqueta: 'I',
        titulo: 'Cursiva',
        activa: editor.isActive('italic'),
        onClick: () => editor.chain().focus().toggleItalic().run(),
      },
      {
        etiqueta: 'H2',
        titulo: 'Titulo',
        activa: editor.isActive('heading', { level: 2 }),
        onClick: () => editor.chain().focus().toggleHeading({ level: 2 }).run(),
      },
      {
        etiqueta: 'Lista',
        titulo: 'Lista',
        activa: editor.isActive('bulletList'),
        onClick: () => editor.chain().focus().toggleBulletList().run(),
      },
      {
        etiqueta: '1.',
        titulo: 'Lista numerada',
        activa: editor.isActive('orderedList'),
        onClick: () => editor.chain().focus().toggleOrderedList().run(),
      },
      {
        etiqueta: 'Cita',
        titulo: 'Cita',
        activa: editor.isActive('blockquote'),
        onClick: () => editor.chain().focus().toggleBlockquote().run(),
      },
      {
        etiqueta: '</>',
        titulo: 'Codigo',
        activa: editor.isActive('codeBlock') || editor.isActive('code'),
        onClick: () => editor.chain().focus().toggleCodeBlock().run(),
      },
      {
        etiqueta: 'Enlace',
        titulo: 'Enlace',
        activa: editor.isActive('link'),
        onClick: () => {
          const url = globalThis.prompt('Introduce la URL del enlace', 'https://')

          if (!url) {
            return
          }

          editor.chain().focus().extendMarkRange('link').setLink({ href: url }).run()
        },
      },
      {
        etiqueta: 'Imagen',
        titulo: 'Imagen',
        onClick: () => {
          const url = globalThis.prompt('Introduce la URL de la imagen', 'https://')

          if (!url) {
            return
          }

          editor.chain().focus().setImage({ src: url, alt: 'Imagen insertada' }).run()
        },
      },
    ]
  }, [editor])

  async function insertarImagenDesdeArchivo(archivo: File) {
    if (!editor) {
      return
    }

    const lector = new FileReader()

    lector.onload = () => {
      const resultado = typeof lector.result === 'string' ? lector.result : ''

      if (!resultado) {
        return
      }

      editor.chain().focus().setImage({ src: resultado, alt: archivo.name }).run()
      setMensajeImagen(`Imagen insertada: ${archivo.name}`)
    }

    lector.readAsDataURL(archivo)
  }

  if (!editor) {
    return null
  }

  return (
    <div className={`editor-descripcion editor-descripcion--wysiwyg${estaPantallaCompleta ? ' editor-descripcion--pantalla-completa' : ''}`}>
      <div className="editor-descripcion__encabezado">
        <div>
          <p className="editor-descripcion__eyebrow">Editor visual</p>
          <h3>Descripcion completa</h3>
          <p className="editor-descripcion__ayuda" id={identificadorAyuda}>
            Formato directo, barra flotante al seleccionar y soporte para imagenes por arrastre o pegado.
          </p>
        </div>
        <div className="editor-descripcion__estado">
          <span>{editor.storage.characterCount.characters()}/{maxLength}</span>
          <span>{editor.storage.characterCount.words()} palabras</span>
        </div>
      </div>

      <div className="editor-descripcion__barra" aria-label="Barra de herramientas de descripcion">
        {acciones.map((accion) => (
          <button
            key={accion.titulo}
            type="button"
            className={`editor-descripcion__accion${accion.activa ? ' editor-descripcion__accion--activa' : ''}`}
            onClick={accion.onClick}
            aria-pressed={accion.activa}
          >
            {accion.etiqueta}
          </button>
        ))}

        <div className="editor-descripcion__separador" />

        <button
          type="button"
          className="editor-descripcion__accion"
          onClick={() => editor.chain().focus().clearNodes().unsetAllMarks().run()}
        >
          Limpiar
        </button>
        <button
          type="button"
          className="editor-descripcion__accion"
          onClick={() => setEstaPantallaCompleta((valorActual) => !valorActual)}
          aria-pressed={estaPantallaCompleta}
        >
          {estaPantallaCompleta ? 'Cerrar pantalla completa' : 'Pantalla completa'}
        </button>
      </div>

      <div className="editor-descripcion__paneles">
        <section className="editor-descripcion__columna editor-descripcion__columna--edicion" aria-labelledby={`${id}-editar`}>
          <div className="editor-descripcion__cabecera-columna">
            <span id={`${id}-editar`}>Redactar</span>
            <small>Selecciona texto para ver acciones contextuales</small>
          </div>

          <div
            className="editor-descripcion__lienzo"
            onDragOver={(evento: DragEvent<HTMLDivElement>) => evento.preventDefault()}
          >
            <EditorContent editor={editor} aria-describedby={identificadorAyuda} />
            <p className="editor-descripcion__mensaje-imagen">{mensajeImagen}</p>
          </div>

          <div className="editor-descripcion__plantillas" aria-label="Sugerencias de contenido">
            <span>Resumen ejecutivo</span>
            <span>Checklist</span>
            <span>Bloque de codigo</span>
            <span>Enlace externo</span>
            <span>Captura o mockup</span>
          </div>
        </section>

        <section className="editor-descripcion__columna editor-descripcion__columna--preview" aria-label="Vista previa de descripcion">
          <div className="editor-descripcion__cabecera-columna">
            <span>Vista previa</span>
            <small>HTML saneado</small>
          </div>
          <div className="editor-descripcion__vista-previa">
            {valor.trim()
              ? <ContenidoDescripcion html={valor} className="contenido-enriquecido contenido-enriquecido--preview" />
              : <p className="editor-descripcion__vacio">Empieza a escribir para ver el resultado final.</p>}
          </div>
        </section>
      </div>

      <BubbleMenu editor={editor}>
        <div className="editor-descripcion__menu-flotante">
          <button type="button" onClick={() => editor.chain().focus().toggleBold().run()} aria-label="Negrita contextual">
            B
          </button>
          <button type="button" onClick={() => editor.chain().focus().toggleItalic().run()} aria-label="Cursiva contextual">
            I
          </button>
          <button type="button" onClick={() => editor.chain().focus().toggleBulletList().run()} aria-label="Lista contextual">
            Lista
          </button>
          <button
            type="button"
            onClick={() => {
              const url = globalThis.prompt('Introduce la URL del enlace', 'https://')

              if (!url) {
                return
              }

              editor.chain().focus().extendMarkRange('link').setLink({ href: url }).run()
            }}
            aria-label="Enlace contextual"
          >
            Enlace
          </button>
        </div>
      </BubbleMenu>
    </div>
  )
}