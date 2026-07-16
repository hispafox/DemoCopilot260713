import os
import shutil
from pathlib import Path

root = Path('c:/w/repos/DemoCopilot260713/docs')
root.mkdir(exist_ok=True)

subdirs = [
    root / 'requisitos',
    root / 'planificacion',
    root / 'auditorias',
    root / 'catalogos',
    root / 'github',
    root / 'recursos' / 'word',
    root / 'recursos' / 'scripts',
]
for subdir in subdirs:
    subdir.mkdir(parents=True, exist_ok=True)

moves = {
    'documento-requisitos-aplicacion.md': 'requisitos/documento-requisitos-aplicacion.md',
    'backlog-alta-usuarios-asignacion-tareas.md': 'planificacion/backlog-alta-usuarios-asignacion-tareas.md',
    'borradores-issues-categorizacion-tareas-2026-07-15.md': 'planificacion/borradores-issues-categorizacion-tareas-2026-07-15.md',
    'planificacion-categorias-tareas-2026-07-15.md': 'planificacion/planificacion-categorias-tareas-2026-07-15.md',
    'planificacion-categorizar-tareas-2026-07-15.md': 'planificacion/planificacion-categorizar-tareas-2026-07-15.md',
    'tickets-github-alta-usuarios-asignacion.md': 'planificacion/tickets-github-alta-usuarios-asignacion.md',
    'ejecucion-creacion-issues-categorias-2026-07-15.md': 'planificacion/ejecucion-creacion-issues-categorias-2026-07-15.md',
    'informe-auditoria-requisitos-360-2026-07-15.md': 'auditorias/informe-auditoria-requisitos-360-2026-07-15.md',
    'informe-calidad-aplicacion-2026-07-15.md': 'auditorias/informe-calidad-aplicacion-2026-07-15.md',
    'guia-agente-verificador-requisitos-360.md': 'auditorias/guia-agente-verificador-requisitos-360.md',
    'catalogo-agentes-proyecto.md': 'catalogos/catalogo-agentes-proyecto.md',
    'catalogo-skills-agnostico-tecnologia.md': 'catalogos/catalogo-skills-agnostico-tecnologia.md',
    'catalogo-skills-aplicacion-tareas.md': 'catalogos/catalogo-skills-aplicacion-tareas.md',
    'guia-uso-github-y-docs.md': 'github/guia-uso-github-y-docs.md',
    'GHCOPMP-temario.docx': 'recursos/word/GHCOPMP-temario.docx',
    'informe-auditoria-requisitos-360-2026-07-15-desde-cero-v7.docx': 'recursos/word/informe-auditoria-requisitos-360-2026-07-15-desde-cero-v7.docx',
    'informe-auditoria-requisitos-360-2026-07-15-diseno-mejorado-v2.docx': 'recursos/word/informe-auditoria-requisitos-360-2026-07-15-diseno-mejorado-v2.docx',
    'informe-auditoria-requisitos-360-2026-07-15-diseno-mejorado-v3.docx': 'recursos/word/informe-auditoria-requisitos-360-2026-07-15-diseno-mejorado-v3.docx',
    'informe-auditoria-requisitos-360-2026-07-15-diseno-mejorado-v4.docx': 'recursos/word/informe-auditoria-requisitos-360-2026-07-15-diseno-mejorado-v4.docx',
    'informe-auditoria-requisitos-360-2026-07-15-diseno-mejorado-v5.docx': 'recursos/word/informe-auditoria-requisitos-360-2026-07-15-diseno-mejorado-v5.docx',
    'informe-auditoria-requisitos-360-2026-07-15-diseno-mejorado-v6.docx': 'recursos/word/informe-auditoria-requisitos-360-2026-07-15-diseno-mejorado-v6.docx',
    'informe-auditoria-requisitos-360-2026-07-15-diseno-mejorado.docx': 'recursos/word/informe-auditoria-requisitos-360-2026-07-15-diseno-mejorado.docx',
    'informe-auditoria-requisitos-360-2026-07-15.docx': 'recursos/word/informe-auditoria-requisitos-360-2026-07-15.docx',
    'plantilla-estilo-word-ghcopmp.md': 'recursos/word/plantilla-estilo-word-ghcopmp.md',
    'manual-usuario-flujo-mejora-word.md': 'recursos/word/manual-usuario-flujo-mejora-word.md',
    'mejorar_diseno_word.py': 'recursos/scripts/mejorar_diseno_word.py',
    'generar_informe_auditoria_360_v7_desde_cero.py': 'recursos/scripts/generar_informe_auditoria_360_v7_desde_cero.py',
}

for src_name, dst_rel in moves.items():
    src = root / src_name
    dst = root / dst_rel
    if src.exists() and not dst.exists():
        dst.parent.mkdir(parents=True, exist_ok=True)
        shutil.move(str(src), str(dst))

(root / 'README.md').write_text(
    '# Documentación del proyecto\n\n'
    'Esta carpeta se organiza por propósito para facilitar la navegación y mantenimiento.\n\n'
    '## Estructura\n\n'
    '- [requisitos](requisitos/) - requisitos funcionales y alcance del producto.\n'
    '- [planificacion](planificacion/) - backlog, issues y planificación del trabajo.\n'
    '- [auditorias](auditorias/) - informes y guías de revisión.\n'
    '- [catalogos](catalogos/) - catálogos de agentes, skills y recursos.\n'
    '- [github](github/) - guías y referencias de uso de GitHub y documentación.\n'
    '- [recursos](recursos/) - plantillas, scripts y documentos auxiliares.\n',
    encoding='utf-8',
)

print('Docs reorganized successfully')
