

# Explorador en la Selva - Videojuego 3D en Unity

Proyecto académico desarrollado en Unity 3D enfocado en el diseño de niveles, mecánicas de exploración, inteligencia artificial básica de fauna silvestre y condiciones de victoria por eventos.

---
## Historia y Contexto Narrativo

En lo profundo de una selva olvidada por la civilización, yace una antigua reliquia custodiada en el legendario Cofre del sol. Pocos expedicionarios han logrado adentrarse en esta espesura, y ninguno ha regresado para contarlo.

Alex, un audaz explorador decidido a desentrañar los misterios de la región, emprende la travesía más peligrosa de su carrera. Sin embargo, no viaja solo: a su lado camina Jack, su fiel perro compañero, cuyo instinto resulta vital para orientarse en el terreno desconocido.

Para alcanzar el cofre y consagrarse como un auténtico pionero, Alex debe cruzar un ecosistema indómito donde el peligro acecha a cada paso:
Fauna Territorial: Un imponente oso que defiende ferozmente su territorio y un tigre que patrulla incansablemente los pasos angostos del bosque.

El objetivo de Alex es sobrevivir al asedio de la fauna salvaje, esquivar las trampas del camino y llegar al claro donde descansa el cofre para reclamar el tesoro y completar la expedición con éxito.

##  Descripción del Proyecto
El jugador toma el control de Alex, un explorador que debe sortear diversos peligros en un entorno selvático (animales salvajes, trampas explosivas y obstáculos naturales) con el apoyo de su fiel compañero canino, hasta alcanzar el cofre del tesoro que marca la victoria de la expedición.

---
##  Recursos de Terceros

Para el desarrollo del entorno visual, animaciones y ambientación sonora se integraron recursos de uso libre provenientes de las siguientes plataformas:

* **Unity Asset Store:** Modelos 3D de vegetación, terreno selvático, fauna ambiental y el prefab del cofre del tesoro.
* **Adobe Mixamo:** Modelo del explorador Alex y conjunto de animaciones para su sistema de movimiento (Idle, Walk, Run).
* **Pixabay:** Efectos sonoros (SFX) para la mecha de la trampa, la explosión y los sonidos ambientales.
---
##  Controles del Juego

| Acción | Tecla / Entrada |
| :--- | :--- |
| **Movimiento** | `W`, `A`, `S`, `D` o Flechas de dirección |
| **Cámara** | Movimiento del Mouse |
| **Salto / Interacción** | Barra espaciadora (`Space`) |
| **Pausa / Salir** | `Esc` |

---

## Mecánicas y Elementos Implementados

* **Controlador de Personaje (`ControladorJugador.cs`):** Movimiento tridimensional fluido con sincronización automática de animaciones (Idle, Walk, Run).
* **Fauna Silvestre e Inteligencia Artificial:**
  * **Oso (`OsoEnemigo.cs`):** Sistema de alerta, persecución y ataque al entrar en su radio de visión; cuenta con transición a estado pacífico al completarse el objetivo.
  * **Tigre patrullero (`EnemigoPatrulla.cs`):** Movimiento continuo en trayectoria predefinida con ciclos de caminata en bucle.
  * **Fauna pasiva:** Ciervos y animales ambientales integrados en el ecosistema.
* **Compañero Canino (`PerroCompanero.cs`):** Lógica de seguimiento y apoyo dinámico al personaje principal.
* **Condición de Victoria (`MetaCofre.cs`):** Detección de llegada al cofre, freno de controles del jugador, desactivación de hostilidad de enemigos, animación de apertura del cofre y despliegue del panel de nivel completado.
* **Controlador General (`GameManager.cs`):** Gestión de estados de juego, vida y condiciones de fin de partida.

---

## Estructura Principal de Scripts (`Assets/_Scripts/`)

* **`Coleccionable.cs`:** Detección de colisión, recolección de ítems y actualización del contador en el juego.
* **`ControladorJugador.cs`:** Lógica de desplazamiento, físicas y parámetros del Animator de Alex.
* **`EnemigoPatrulla.cs`:** Rutina de patrulla en bucle continuo y traslación del tigre.
* **`GameManager.cs`:** Gestión del ciclo de vida de la partida, control de vidas y estados de juego.
* **`MetaCofre.cs`:** Detección de llegada a la meta, animación de apertura del cofre y pacificación de enemigos.
* **`MovimientoAgua.cs`:** Desplazamiento de texturas/coordenadas UV para simular la corriente del agua en ríos y lagunas.
* **`OsoEnemigo.cs`:** Detección por proximidad, máquina de estados de persecución y ataque cuerpo a cuerpo.
* **`PerroCompanero.cs`:** Lógica de seguimiento y navegación del perro Jack hacia el jugador.
* **`TrampaMortal.cs`:** Detección por proximidad (Trigger), activación de la trampa y aplicación de daño al explorador.

---
> **Nota para la evaluación:** Se utilizó IA como herramienta de asistencia técnica para la depuración de errores en consola (C#), calibración de estados en el *Animator* y resolución de incidencias en Git.
