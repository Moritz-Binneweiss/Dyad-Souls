So könnte ein README File für die Projektabgabe aussehen
Gerne diese Vorlage als Inspiration verwenden und für den eigenen UseCase anpassen.
---

# Games Projekt Titel 
Teammitglieder: Max Mustermann(123456), John Doe (654321)

Unity Version: 6000.3.0f1

## Verwendete Assets

- Asset 1: Robot Kyle - Unity Technologies https://assetstore.unity.com/packages/3d/characters/robots/robot-kyle-urp-4696
- Asset 2: `Name + (Optional) Publisher + <Link>`
- Asset 3: `Name + (Optional) Publisher + <Link>`

## (Optional) Startup 

Um das Projekt zu Starten muss eine Internet Verbindung Existieren.
Außerdem wird eine lokale Installation von `Software XY` benötigt.

## (Optional) Steuerung

| Taste | Funktion |
| :---: | :---: |
| **W** | Vorwärts bewegen |
| **S** | Rückwärts bewegen |
| **A** | Nach links bewegen |
| **D** | Nach rechts bewegen |
| **Maus** | Umschauen / Blickrichtung ändern |
| **LMB (Linke Maustaste)** | Schießen  |
| **R** | Nachladen |

## Beschreibung des Projektes
Dieses Prototyping-Projekt dient als Grundlage zur Evaluierung der ECS-Architektur (Entity Component System) in Unity und implementiert ein rudimentäres Kampfsystem mit Raycasting für Treffererkennung. Das Hauptziel ist die Optimierung der Performance bei einer hohen Anzahl von interagierenden Spielobjekten durch die Nutzung des Unity DOTS (Data-Oriented Technology Stack) Ansatzes. Die Asset-Pipeline ist so konfiguriert, dass sie Asynchrone Ladevorgänge unterstützt, um Ladezeiten zu minimieren und ein flüssiges Spielerlebnis zu gewährleisten.

## Verwendete Technologien

- Für die hoch performante Simulation des Gameplays wird primär der Data-Oriented Technology Stack (DOTS) genutzt.
- Speziell kommen hier die Module Entitys, Jobs und der Burst Compiler zum Einsatz, um die Parallelisierung und Effizienz der Komponentenverarbeitung zu maximieren.
- [...]

## Besondere Herausforderungen / Lessions Learned

## (Optional) Besondere Leistung