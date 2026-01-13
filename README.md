# **Dyad Souls**

## **Mitwirkende**

- **Moritz Binneweiß** - Models, Designs, Development
- **Sebastian Schuster** - Animations, Effects, Development

Unity Version: 6000.2.6f2

### Figma Board

https://www.figma.com/board/uUtF92ZtxAHNdhbk2m4GBN/Dyad-Souls?node-id=0-1&t=A3mF4doLudXmPWks-1

### GitHub Repo

https://github.com/Moritz-Binneweiss/Dyad-Souls

### Link zum Video

-

## Start-Up Guide

1. Projekt auf GitHub (z.B. als Zip) herunterladen
2. Zip entpacken
3. Projekt in Unity (Version: 6000.2.6f2) starten/öffnen
4. MainMenu Scene öffnen
5. Charakterauswahl treffen für Player 1 und Player 2
6. Arena Scene wird geladen und das Spiel beginnt

## Beschreibung des Projektes

Dyad Souls ist ein kooperatives 3D Souls-like Bossfight-Spiel für zwei Spieler im Splitscreen. Inspiriert von Dark Souls und Elden Ring, müssen zwei Spieler gleichzeitig gegen einen KI-gesteuerten Boss kämpfen, der durch einen komplexen Behavior Tree gesteuert wird. Die Spieler müssen ihre Ausdauer (Stamina) managen, Angriffen ausweichen und den perfekten Moment zum Zuschlagen finden. Das Projekt setzt auf Unity's neues Input System für gerätebasiertes Multiplayer-Gameplay und nutzt UMotion Pro für hochwertige Kampfanimationen.

## Verwendete Technologien

- **Unity 6000.2.6f2** als Game Engine mit Universal Render Pipeline (URP)
- **Unity's neues Input System** für flexible Controller- und Keyboard-Unterstützung
- **Behavior Designer** (Third-Party Asset) mit Movement Pack für komplexe KI-Verhaltensmuster des Bosses
- **UMotion Pro** für Animations Erstellung und bearbeitung
- **NavMesh** für Boss-Pathfinding und Bewegung
- **Unity Animator** mit State Machines für Player- und Boss-Animationen
- **Animation Events** für präzises Timing von Damage-Triggern während Angriffen
- **CharacterController** für physikbasierte Spielerbewegung
- **Cinemachine** für dynamische Splitscreen-Kamera mit automatischer Anpassung
- **Coroutines** für zeitbasierte Effekte (Camera Shake, Stamina Regeneration, etc.)
- **Partikel System** für Erstellung von Specifal Effects
- **Blender** für Erstellung und bearbeitung der 3D Models und Assets

## Besondere Herausforderungen / Lessions Learned

- **Behavior Tree Komplexität**: Die Entwicklung eines umfangreichen Behaviour Trees erforderte ausführliche auseinandersetzung und stellt oft Probleme dar, aufgrund von wenig Erfahrung mit dem Tool.

- **Animation-Code Synchronisation**: Die präzise Synchronisation von Animations-Events mit Code-Logic (Damage-Dealing, Attack-Ranges) war kritisch für den Bossfight. Die Zentralisierung in `EnemyDamage.cs` löste Inkonsistenzen. Aber dennoch war es ein häufiges Fehlerkriterium.

- **Input System Device Binding**: Das Binden der Input-Devices (Keyboard vs. Gamepad) vorallem beim Charater Selector hatte anfangs erstmal ein bisschen Verständnis und beharrlichkeit erfordert.

- **Animationen**: Um erfolgreiche Animationen umzusetzen, selbst mit einem Tool wie UMotion Pro ist immernoch sehr sehr Zeitaufwendig und selbst dann gibt es immer etwas zu verbessern. Das Thema ist und erfordert sehr intensive Auseinandersetzung und Übung.

## Besondere Leistungen

- **Vollständig implementierter Behavior Tree**: Entwicklung eines komplexen, Boss-AI-Systems mit 10+ Custom Actions und intelligenter Entscheidungsfindung basierend auf Spieler-Proximity und Boss-Health.

- **Elden Ring-inspiriertes Damage Preview System**: Implementation einer visuellen Ghost Health Bar für den Boss, die Schaden visuell anzeigt bevor er abgezogen wird.

- **Poliertes Combat Feel**: Integration von Camera Shake, Gamepad-Vibration, Stamina-Management, Dodge-Rolls, Attack-Buffering und responsive Movement für ein authentisches Souls-like Gefühl.

- **Dynamische Kamera**: Smooth Transitions zwischen Splitscreen und Fullscreen mit Coroutine-basierter Animation, die sich an Player-Tod anpasst.

- **Umfangreiches Animation System**: Über 25+ Animationen für Player und Boss, erstellt und oder bearbeitet mit UMotion Pro.

- **Selbst erstellte Assets**: Background, Models und Animationen wurden eigenständig erstellt, bearbeitet oder erweitert von Images, Golem Asset von Kevin Iglesias, Mixamo und weiteren Inspirationen.

- **Zweite Phase des Boss**: Phase Transition, anderes Movement, Model, Effects, etc.

## Verwendete Assets

- **Behavior Designer** von Opsive (https://assetstore.unity.com/packages/tools/visual-scripting/behavior-designer-behavior-trees-for-everyone-15277) - Behavior Tree System für Boss-AI (bereitgestellt von den Betreuern)
- **Behavior Designer - Movement Pack** von Opsive - Erweiterte Movement Actions für Behavior Trees (bereitgestellt von den Betreuern)
- **UMotion Pro** von Soxware Interactive (https://assetstore.unity.com/packages/tools/animation/umotion-pro-animation-editor-95991) - Professional Animation Editor (bereitgestellt von den Betreuern)
-

## Steuerung

| Taste (Tastatur & Maus) / Button (Gamepad) |                          Funktion                          |
| :----------------------------------------: | :--------------------------------------------------------: |
|          **W,A,S,D / Left Stick**          |                          Bewegung                          |
|           **Maus / Right Stick**           |                       Kamera bewegen                       |
|    **Linke Maustaste / Right Shoulder**    |                      Leichter Angriff                      |
|    **Rechte Maustaste / Right Trigger**    |                      Schwerer Angriff                      |
|   **Mittlere Maustaste / Left Trigger**    |                      Spezial Angriff                       |
|          **Space / Button South**          |                          Springen                          |
|     **Left Shift / Left Stick Press**      |                          Sprinten                          |
|       **Left Control / Button East**       |                     Ausweichen / Rolle                     |
|            **C / Button West**             |                           Ducken                           |
|         **Escape / Start Button**          |                           Pause                            |
|         **F / Right Stick Press**          |              Fokussieren (wenn in Reichweite)              |
|            **E / Button North**            | Interagieren / Positionstausch (beide gleichzeitig halten) |

## Protokolle

#### **02.10.2024**

Besprechung:

- Für Coop Bossfight Projekt entschieden
- Repository wurde aufgesetzt
- UMotionPro und BehaviourDesigner Packages ins Projekt eingebunden

Ziel:

- Anfangen zu Prototypen (Movement, Simples Schlag/Hit System, etc.)

Für die Präsentation:

- 5-10 Minuten
- Projekt Vorstellung
- Inspiration
- Ideen vom Design
- Gameplay Funktionalität
- Herausforderungen
- Lösungsansätze
- Unity Techniken
- ungefährer Zeitplan
- Projektziel
- MVP (Minimum Viable Product)
- Nice-To-Haves

#### **16.10.2024**

Besprechung:

- Namens Problem wurde besprochen
- aktueller Stand gezeigt
- Pläne für die nächste Woche besprochen

Ziel:

- Tools vertraut machen
- Kampfanimationen, Boss kann angreifen
- Behaviour Trees reinfinden
- minimaler Bosskampf möglich
- Prototyp über die ganzen Funktionen

#### **23.10.2024**

Besprechung:

- Behaviour Designer start gezeigt
- Splitscreen und 2 Player Movement gezeigt

Ziel:

- weitere Animationen
- Schaden nehmen (Boss und Player)
- Attack Buffer
- Elden Ring Wiki (Boss AI Behaviour Inspiration)

#### **30.10.2024**

Besprechung:

- Lieber ein komplexer Boss anstatt mehrerer simpleren
- Überlegen was kann der Boss spezielles
- Fokus auf Kampf

Ziel:

- an MVP weiterarbeiten
- Fokus auf den einen Bossfight

#### **06.11.2024**

Besprechung:

- LockOnTarget gezeigt
- Wave based Gameplay gezeigt
- erweitertes Movement, Angriffe und Animationen gezeigt
- Enemy Behaviour Tree, Funktionalität und Animationen gezeigt

Ziel:

- Animation Bugs beheben und verbessern
- Behaviour Tree Bugs beheben und verbessern
- eigene Basic Assets anfertigen

#### **13.11.2024**

Besprechung:

- Crouch gezeigt
- Behaviour Tree erweitert und verbessert gezeigt
- Animationen teilweise verbessert gezeigt
- Eigene Blender Assets angefangen einzubinden gezeigt

Ziel:

-MVP Ziel erreichen

- (Lobby/Vorbereitungsraum)
- an Problemen arbeiten
- Dinge Verbessern und erweitern
- "Präsentation" vorbereiten

Für die Präsentation:

- Technologische Hintergründe zeigen und erklären
- Prototype zeigen, was bisher alles erreicht wurde
- Ziele bis zum Ende
- 10-15min

#### **20.11.2024**

Besprechung:

- Präsentation gehalten

Ziel:

- Nice-To-Have Features anfangen
- Verbessern und Erweitern

#### **04.12.2024**

Besprechung:

- Refactoring and Bug fixing gezeigt
- Neue Animations gezeigt

Ziel:

- Weitere Animations hinzufügen
- Verbessern und Erweitern

#### **11.12.2024**

Besprechung:

- Neue Animations gezeigt
- Cascadeur angeschaut

Ziel:

- Weitere Animations hinzufügen
- Verbessern und Erweitern
- Special Effects anfangen
- Cascadeur Tool ausprobieren

#### **18.12.2024**

Besprechung:

- Effects Shader gezeigt und gefragt ->
- Cascadeur wurde ganz kurz angetestet
- Ziele vorgestellt

Ziel:

- Weitere Animations hinzufügen
- Verbessern und Erweitern
- Special Effects erweitern
- Sound Effects anfangen

#### **08.01.2026**

Besprechung:

- Polishing Effekte (Slashes, Environmental, etc.)
- Phase 2 (Design, Boss, Effekte, etc.)
- Updated Models
- Subtrees in Behaviour Trees

Ziel:

- Cinematics
- Kleinigkeiten (Camera Shake, Collider für Wände, etc.)
- Verbesserungen und kleine Erweiterungen

Präsentation:

- 15 min
- Technische Herausforderungen erklären
- Technik zeigen (Behavior Trees, Animation Events, Splitscreen System)
- Features demonstrieren
- Gameplay präsentieren

#### **13.01.2026**

Abschluss:

- Ghost Health Bar System implementiert (Elden Ring-style Damage Preview)
- Smooth Camera Transitions zwischen Splitscreen und Fullscreen
- Camera Shake und Gamepad Vibration bei Damage
- Code Cleanup und Refactoring
- Animation Events optimiert
- Behavior Tree erweitert mit Subtrees
- Finales Polishing und Bug Fixes
