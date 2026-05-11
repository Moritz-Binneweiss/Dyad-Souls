# **Dyad Souls**

## Inhalt / Contents

1. [Deutsch](#dyad-souls)
2. [English](#dyad-souls-english-version)

## **Mitwirkende**

- **Moritz Binneweiß** - Models, Designs, Development
- **Sebastian Schuster** - Animations, Effects, Development

Unity Version: 6000.2.6f2

### Figma Board

https://www.figma.com/board/uUtF92ZtxAHNdhbk2m4GBN/Dyad-Souls?node-id=0-1&t=A3mF4doLudXmPWks-1

### GitHub Repo

https://github.com/Moritz-Binneweiss/Dyad-Souls

### Link zum Video

https://youtu.be/CG7OlZzKkzw

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

- **Behavior Tree Komplexität**: Die Entwicklung eines umfangreichen Behaviour Trees erforderte ausführliche auseinandersetzung und stellt oft Probleme dar, da wir das Tool zum ersten mal benutzt hatten.

- **Animation-Code Synchronisation**: Die Synchronisation von Animations-Events mit Code-Logic (Damage-Dealing, Attack-Ranges) war kritisch für den Bossfight. Die Zentralisierung in `EnemyDamage.cs` löste Inkonsistenzen. Aber dennoch war es ein häufiges Fehlerkriterium.

- **Input System Device Binding**: Das Binden der Input-Devices (Keyboard vs. Gamepad) vorallem beim Charater Selector hatte anfangs erstmal ein bisschen Verständnis und beharrlichkeit erfordert um damit es richtig funktioniert.

- **Animationen**: Um erfolgreiche Animationen umzusetzen, selbst mit einem Tool wie UMotion Pro ist immernoch sehr sehr Zeitaufwendig und selbst dann gibt es immer wieder etwas zu verbessern. Das Thema ist und erfordert sehr intensive Auseinandersetzung und Übung.

- **Generell**: Man muss sagen durch viele verschiedenen Komponenten im Spiel war oft die Herausforderung den Scope Creep unter Kontrolle zu halten, da wenn man viele verschiedene Elemente umsetzt, hat man ja nicht für alles genügend Zeit und dadurch könnte man bei allem immer wieder verbesserungen einbringen.

## Besondere Leistungen

- **Vollständig implementierter Behavior Tree**: Komplexes Boss-AI-Systems mit 10+ Custom Actions und Entscheidungsfindung basierend auf Spieler-Proximity, Prozentzahlen und Boss-Health.

- **Elden Ring-inspiriertes Damage Preview System**: Implementation einer visuellen Ghost Health Bar für den Boss, die Schaden visuell anzeigt bevor er abgezogen wird.

- **Polished Combat Feel**: Integration von Camera Shake, Gamepad-Vibration, Stamina-Management, Health Regeneration, Dodge-Rolls, Poisitionstausch, Attack-Buffering und responsives Movement.

- **Dynamische Kamera**: Smooth Transitions zwischen Splitscreen und Fullscreen mit Coroutine-basierter Animation, die sich an Player-Tod anpasst.

- **Umfangreiches Animation System**: Insgesamt über 25+ Animationen erstellt und oder bearbeitet mit UMotion Pro.

- **Selbst erstellte Assets**: Background, Models und Animationen wurden eigenständig erstellt, bearbeitet oder erweitert von Images, Golem Asset von Kevin Iglesias, Mixamo und weiteren Inspirationen.

- **Zweite Phase des Boss**: Phase Transition Cutscene, anderes Movement und Animationen, anderer Behaviour Tree, anderes Model, Effekte, etc.

## Verwendete Assets

- **Behavior Designer** von Opsive (https://assetstore.unity.com/packages/tools/visual-scripting/behavior-designer-behavior-trees-for-everyone-15277) - Behavior Tree System für Boss-AI (bereitgestellt von den Betreuern)
- **Behavior Designer - Movement Pack** von Opsive - Erweiterte Movement Actions für Behavior Trees (bereitgestellt von den Betreuern)

- **UMotion Pro** von Soxware Interactive (https://assetstore.unity.com/packages/tools/animation/umotion-pro-animation-editor-95991) - Professional Animation Editor (bereitgestellt von den Betreuern)

- **Scene Switcher Pro** von Ajay Uthaman (https://assetstore.unity.com/packages/tools/gui/scene-switcher-pro-313355) für schnelleres Scene Switching

- **FREE - 32 RPG Animations** von Blink, ein paar Animationen erweitert oder direkt verwendet (https://assetstore.unity.com/packages/3d/animations/free-32-rpg-animations-215058)

- **Free Quick Effects Vol. 1** von Gabriel Aguiar Prod, ein paar Effekte erweitert oder direkt verwendet (https://assetstore.unity.com/packages/vfx/particles/free-quick-effects-vol-1-304424)

- **Giant Animations FREE** von Kevin Iglesias, ein paar Animationen erweitert oder direkt verwendet (https://assetstore.unity.com/packages/p/giant-animations-free-215962)

- **Giant Monster Model - Golem** von Kevin Iglesias, Model erweitert zu unserer eigenen Varianten (https://assetstore.unity.com/packages/p/giant-monster-model-golem-278960)

- **Human Basic Motions FREE** von Kevin Iglesias, ein paar Animationen erweitert oder direkt verwendet (https://assetstore.unity.com/packages/3d/animations/human-basic-motions-free-154271)

- **Particle Pack** von Unity Technologies, ein paar Effekte erweitert oder direkt verwendet (https://assetstore.unity.com/packages/p/particle-pack-127325)

- **Blender Material und Texture Palette** von Imphenzia, für Coloring der 3D Assets (https://imphenzia.com/assets)

- **Cinematic Cutscene** erstellt aus Referenzbildern und Prompt von DeeVid AI (https://deevid.ai/de/image-to-video?utm_source=google&utm_medium=cpc&utm_campaign=de-competitor&utm_term=kaiber&utm_content=pc&gad_source=1&gad_campaignid=22753869722&gbraid=0AAAAAq898mOdAFCz08IgmKlupgCQCRGXy&gclid=Cj0KCQiAgbnKBhDgARIsAGCDdlcOOl_qlBdYVUjZ4OVjlCZ-byazqxuQ_hwl8JQ2pTDeb5BV-MS3pVgaAtK2EALw_wcB) erweitert mit Real-Time Intermediate Flow Estimation for Video Frame Interpolation(https://github.com/hzwer/ECCV2022-RIFE und der https://nmkd.itch.io/flowframes) und zusätzlich hochskaliert von Wink (https://wink.ai/ai-upscaler/upload) und danach selbst noch farblich korrigiert

- **Background Images** erstellt von Copilot ChatGPT und leicht verändert

- **Gamepad Icon** von Hafizh Hapis (https://www.flaticon.com/authors/hafizh-hapis)

- **Mouse Icon** von Vector Squad (https://www.flaticon.com/authors/vector-squad)

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

#### **15.01.2026**

Abschluss:

- Ghost Health Bar System implementiert (Elden Ring-style Damage Preview)
- Smooth Camera Transitions zwischen Splitscreen und Fullscreen
- Camera Shake und Gamepad Vibration bei Damage
- Code Cleanup und Refactoring
- Animation Events optimiert
- Behavior Tree erweitert mit Subtrees
- Finales Polishing und Bug Fixes
- Finale Präsentation gehalten

---

# **Dyad Souls (English Version)**

## **Contributors**

- **Moritz Binneweiß** - Models, Designs, Development
- **Sebastian Schuster** - Animations, Effects, Development

Unity Version: 6000.2.6f2

### Figma Board

https://www.figma.com/board/uUtF92ZtxAHNdhbk2m4GBN/Dyad-Souls?node-id=0-1&t=A3mF4doLudXmPWks-1

### GitHub Repo

https://github.com/Moritz-Binneweiss/Dyad-Souls

### Link to Video

https://youtu.be/CG7OlZzKkzw

## Start-Up Guide

1. Download the project from GitHub (e.g., as a zip)
2. Extract the zip file
3. Start/open the project in Unity (Version: 6000.2.6f2)
4. Open the MainMenu scene
5. Select characters for Player 1 and Player 2
6. The Arena scene loads and the game begins

## Project Description

Dyad Souls is a cooperative 3D souls-like boss fight game for two players in split-screen. Inspired by Dark Souls and Elden Ring, two players must fight simultaneously against an AI-controlled boss driven by a complex behavior tree. Players have to manage their stamina, dodge attacks, and find the perfect moment to strike. The project relies on Unity's new Input System for device-based multiplayer gameplay and uses UMotion Pro for high-quality combat animations.

## Technologies Used

- **Unity 6000.2.6f2** as the game engine with Universal Render Pipeline (URP)
- **Unity's new Input System** for flexible controller and keyboard support
- **Behavior Designer** (third-party asset) with Movement Pack for complex boss AI behavior patterns
- **UMotion Pro** for animation creation and editing
- **NavMesh** for boss pathfinding and movement
- **Unity Animator** with state machines for player and boss animations
- **Animation Events** for precise timing of damage triggers during attacks
- **CharacterController** for physics-based player movement
- **Cinemachine** for a dynamic split-screen camera with automatic adjustment
- **Coroutines** for time-based effects (camera shake, stamina regeneration, etc.)
- **Particle System** for creating special effects
- **Blender** for creating and editing 3D models and assets

## Special Challenges / Lessons Learned

- **Behavior Tree Complexity**: Developing an extensive behavior tree required a lot of in-depth work and often caused problems, as we were using the tool for the first time.

- **Animation-Code Synchronization**: Synchronizing animation events with code logic (damage dealing, attack ranges) was critical for the boss fight. Centralizing this in `EnemyDamage.cs` solved inconsistencies, but it still remained a frequent source of errors.

- **Input System Device Binding**: Binding input devices (keyboard vs. gamepad), especially in the character selector, initially required significant understanding and persistence to make it work correctly.

- **Animations**: Creating successful animations, even with a tool like UMotion Pro, is still very time-consuming, and even then there is always room for improvement. This topic requires intensive engagement and practice.

- **General**: Because of many different components in the game, a recurring challenge was keeping scope creep under control. When implementing many different elements, there is never enough time for everything, and every part could always be improved further.

## Special Achievements

- **Fully Implemented Behavior Tree**: Complex boss AI system with 10+ custom actions and decision-making based on player proximity, percentages, and boss health.

- **Elden Ring-Inspired Damage Preview System**: Implementation of a visual ghost health bar for the boss that shows damage visually before it is deducted.

- **Polished Combat Feel**: Integration of camera shake, gamepad vibration, stamina management, health regeneration, dodge rolls, position swap, attack buffering, and responsive movement.

- **Dynamic Camera**: Smooth transitions between split-screen and fullscreen with coroutine-based animation that adapts to player death.

- **Extensive Animation System**: A total of over 25 animations created and/or edited with UMotion Pro.

- **Self-Created Assets**: Backgrounds, models, and animations were independently created, edited, or extended from images, the golem asset by Kevin Iglesias, Mixamo, and other inspirations.

- **Second Boss Phase**: Phase transition cutscene, different movement and animations, different behavior tree, different model, effects, etc.

## Assets Used

- **Behavior Designer** by Opsive (https://assetstore.unity.com/packages/tools/visual-scripting/behavior-designer-behavior-trees-for-everyone-15277) - Behavior tree system for boss AI (provided by the supervisors)
- **Behavior Designer - Movement Pack** by Opsive - Advanced movement actions for behavior trees (provided by the supervisors)

- **UMotion Pro** by Soxware Interactive (https://assetstore.unity.com/packages/tools/animation/umotion-pro-animation-editor-95991) - Professional animation editor (provided by the supervisors)

- **Scene Switcher Pro** by Ajay Uthaman (https://assetstore.unity.com/packages/tools/gui/scene-switcher-pro-313355) for faster scene switching

- **FREE - 32 RPG Animations** by Blink, some animations were extended or used directly (https://assetstore.unity.com/packages/3d/animations/free-32-rpg-animations-215058)

- **Free Quick Effects Vol. 1** by Gabriel Aguiar Prod, some effects were extended or used directly (https://assetstore.unity.com/packages/vfx/particles/free-quick-effects-vol-1-304424)

- **Giant Animations FREE** by Kevin Iglesias, some animations were extended or used directly (https://assetstore.unity.com/packages/p/giant-animations-free-215962)

- **Giant Monster Model - Golem** by Kevin Iglesias, model was extended into our own variants (https://assetstore.unity.com/packages/p/giant-monster-model-golem-278960)

- **Human Basic Motions FREE** by Kevin Iglesias, some animations were extended or used directly (https://assetstore.unity.com/packages/3d/animations/human-basic-motions-free-154271)

- **Particle Pack** by Unity Technologies, some effects were extended or used directly (https://assetstore.unity.com/packages/p/particle-pack-127325)

- **Blender Material and Texture Palette** by Imphenzia, for coloring 3D assets (https://imphenzia.com/assets)

- **Cinematic Cutscene** created from reference images and prompts from DeeVid AI (https://deevid.ai/de/image-to-video?utm_source=google&utm_medium=cpc&utm_campaign=de-competitor&utm_term=kaiber&utm_content=pc&gad_source=1&gad_campaignid=22753869722&gbraid=0AAAAAq898mOdAFCz08IgmKlupgCQCRGXy&gclid=Cj0KCQiAgbnKBhDgARIsAGCDdlcOOl_qlBdYVUjZ4OVjlCZ-byazqxuQ_hwl8JQ2pTDeb5BV-MS3pVgaAtK2EALw_wcB), extended with Real-Time Intermediate Flow Estimation for Video Frame Interpolation (https://github.com/hzwer/ECCV2022-RIFE and https://nmkd.itch.io/flowframes), additionally upscaled with Wink (https://wink.ai/ai-upscaler/upload), and then color-corrected manually

- **Background Images** created by Copilot ChatGPT and slightly modified

- **Gamepad Icon** by Hafizh Hapis (https://www.flaticon.com/authors/hafizh-hapis)

- **Mouse Icon** by Vector Squad (https://www.flaticon.com/authors/vector-squad)

## Controls

| Key (Keyboard & Mouse) / Button (Gamepad) |                          Function                           |
| :---------------------------------------: | :---------------------------------------------------------: |
|         **W,A,S,D / Left Stick**          |                          Movement                           |
|          **Mouse / Right Stick**          |                         Move Camera                         |
|  **Left Mouse Button / Right Shoulder**   |                        Light Attack                         |
|  **Right Mouse Button / Right Trigger**   |                        Heavy Attack                         |
|  **Middle Mouse Button / Left Trigger**   |                       Special Attack                        |
|         **Space / Button South**          |                            Jump                             |
|     **Left Shift / Left Stick Press**     |                           Sprint                            |
|      **Left Control / Button East**       |                        Dodge / Roll                         |
|            **C / Button West**            |                           Crouch                            |
|         **Escape / Start Button**         |                            Pause                            |
|         **F / Right Stick Press**         |                    Focus (when in range)                    |
|           **E / Button North**            | Interact / Position swap (both players hold simultaneously) |

## Logs

#### **02.10.2024**

Meeting:

- Decided on a co-op boss fight project
- Repository was set up
- UMotionPro and BehaviourDesigner packages were integrated into the project

Goal:

- Start prototyping (movement, simple attack/hit system, etc.)

For the presentation:

- 5-10 minutes
- Project introduction
- Inspiration
- Design ideas
- Gameplay functionality
- Challenges
- Solution approaches
- Unity techniques
- Approximate timeline
- Project goal
- MVP (Minimum Viable Product)
- Nice-to-haves

#### **16.10.2024**

Meeting:

- Naming issue was discussed
- Current status was shown
- Plans for the next week were discussed

Goal:

- Get familiar with the tools
- Combat animations, boss can attack
- Get into behavior trees
- Minimal boss fight possible
- Prototype across all functions

#### **23.10.2024**

Meeting:

- Behavior Designer start was shown
- Split-screen and 2-player movement were shown

Goal:

- More animations
- Taking damage (boss and player)
- Attack buffer
- Elden Ring wiki (boss AI behavior inspiration)

#### **30.10.2024**

Meeting:

- Prefer one complex boss instead of several simpler ones
- Consider what special features the boss can have
- Focus on combat

Goal:

- Continue working on MVP
- Focus on the single boss fight

#### **06.11.2024**

Meeting:

- LockOnTarget was shown
- Wave-based gameplay was shown
- Extended movement, attacks, and animations were shown
- Enemy behavior tree functionality and animations were shown

Goal:

- Fix and improve animation bugs
- Fix and improve behavior tree bugs
- Create own basic assets

#### **13.11.2024**

Meeting:

- Crouch was shown
- Behavior tree expansion and improvements were shown
- Animations were partially improved and shown
- Initial integration of own Blender assets was shown

Goal:

- Reach MVP goal

- (Lobby/preparation room)
- Work on issues
- Improve and expand things
- Prepare "presentation"

For the presentation:

- Show and explain technological background
- Show prototype and what has been achieved so far
- Goals until the end
- 10-15 min

#### **20.11.2024**

Meeting:

- Presentation was held

Goal:

- Start nice-to-have features
- Improve and expand

#### **04.12.2024**

Meeting:

- Refactoring and bug fixing were shown
- New animations were shown

Goal:

- Add more animations
- Improve and expand

#### **11.12.2024**

Meeting:

- New animations were shown
- Cascadeur was reviewed

Goal:

- Add more animations
- Improve and expand
- Start special effects
- Try out the Cascadeur tool

#### **18.12.2024**

Meeting:

- Effects shader was shown and discussed ->
- Cascadeur was briefly tested
- Goals were presented

Goal:

- Add more animations
- Improve and expand
- Expand special effects
- Start sound effects

#### **08.01.2026**

Meeting:

- Polishing effects (slashes, environmental, etc.)
- Phase 2 (design, boss, effects, etc.)
- Updated models
- Subtrees in behavior trees

Goal:

- Cinematics
- Small improvements (camera shake, colliders for walls, etc.)
- Improvements and small extensions

Presentation:

- 15 min
- Explain technical challenges
- Show technology (behavior trees, animation events, split-screen system)
- Demonstrate features
- Present gameplay

#### **15.01.2026**

Finalization:

- Ghost health bar system implemented (Elden Ring-style damage preview)
- Smooth camera transitions between split-screen and fullscreen
- Camera shake and gamepad vibration on damage
- Code cleanup and refactoring
- Animation events optimized
- Behavior tree expanded with subtrees
- Final polishing and bug fixes
- Final presentation held
