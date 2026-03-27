# FlipFlop

**FlipFlop** es un juego en el que puedes **invertir la gravedad** para hacer que el personaje suba y baje, afectando también a los objetos del entorno. La mecánica principal consiste en “darle la vuelta” al escenario cambiando la dirección de la gravedad en el momento adecuado para avanzar y esquivar peligros.

## Mecánica principal: invertir gravedad
Durante la partida, el jugador puede **invertir la gravedad** (pasar de caer hacia abajo a “caer” hacia arriba). Esto permite:
- Cambiar entre suelo y techo como superficies “caminables”.
- Alterar el comportamiento de elementos físicos del entorno (cuerpos rígidos / trampas), creando situaciones tipo puzle o de habilidad.

## Controles
- **Moverse**: `←` / `→` (acciones `ui_left` y `ui_right`)
- **Invertir gravedad**: `Enter` o `Espacio` (acción `ui_accept`)  
  - Solo se permite cuando el personaje está tocando **suelo** o **techo**.

> Nota: actualmente el movimiento horizontal se basa en las acciones estándar `ui_left`/`ui_right` de Godot, y el “flip” usa `ui_accept`.

## Cómo ejecutar el proyecto
Este repositorio es un proyecto de **Godot 4.6** con scripts en **C# (.NET)**.

### Requisitos
- **Godot 4.6** (idealmente la misma versión del proyecto)
- **.NET SDK** (el proyecto apunta a `net8.0`)

### Pasos
1. Clona el repositorio:
   - `git clone <repo-url>`
2. Abre Godot y selecciona la carpeta del proyecto.
3. Ejecuta la escena principal (Play):
   - La escena principal configurada es `scena/main.tscn`.

## Estructura del proyecto
- `project.godot`: configuración del proyecto (escena principal, features, etc.)
- `scena/`: escenas del juego (incluye `main.tscn` y el jugador)
- `scripts/`:
  - `scripts/Player/Player.cs`: lógica del personaje (movimiento + flip de gravedad)
  - `scripts/trap/Gravity.cs`: área que cambia la dirección de la gravedad
- `assets/`: recursos gráficos (icono, sprites, etc.)

## Idea / objetivo del juego
El objetivo es avanzar por el nivel usando la inversión de gravedad para **reposicionarte** y **manipular el entorno**, evitando obstáculos y aprovechando la física para superar el escenario.

## Créditos
- Proyecto: **FlipFlop**
- Engine: Godot + C#

