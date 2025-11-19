# Boss Behavior Tree - Elden Ring Style (Hoarah Loux)

## Kompletter Behavior Tree mit Angriffssystem

### Benötigte Komponenten am Boss GameObject:
- **NavMeshAgent** (Unity Standard)
- **Animator** (mit Attack-Animationen)
- **Behavior Tree** (Behavior Designer Component)

---

## Behavior Tree Struktur (Elden Ring Boss):

```
Entry
└── Repeater (Count: -1, läuft endlos)
    └── Selector (wählt erste erfolgreiche Option)
        ├── Sequence (Angriff wenn in Reichweite)
        │   ├── IsTargetInRange
        │   │   └── target: currentTarget
        │   │   └── range: 3.5
        │   │
        │   └── Selector (wähle Angriffstyp - probiert von oben nach unten)
        │       ├── PerformRangeAttack (30% Chance, 5s Cooldown, 5-10m Distanz)
        │       ├── PerformHeavyAttack (40% Chance, 4s Cooldown)
        │       ├── PerformComboAttack (50% Chance, 3s Cooldown)
        │       ├── PerformLeftAttack (100% Chance, 1.5s Cooldown)
        │       └── PerformRightAttack (100% Chance, 1.5s Cooldown)
        │
        └── MoveToClosestPlayer (Fallback: laufe zum Spieler)
            └── player1: Player1
            └── player2: Player2
            └── stoppingDistance: 3.0
```

---

## Setup-Anleitung:

### 1. Shared Variables erstellen:
- **player1** (GameObject, Public) → Weise Player 1 zu
- **player2** (GameObject, Public) → Weise Player 2 zu
- **currentTarget** (GameObject, Private) → Wird automatisch gesetzt

### 2. Tree aufbauen im Behavior Designer:

#### Root:
- **Repeater** (Count: -1)

#### Darunter:
- **Selector** (probiert Kinder von oben nach unten)

#### Im Selector (2 Kinder):

**Kind 1: Sequence (Angriffs-Branch)**
  - **IsTargetInRange** (Conditional)
    - target: `currentTarget`
    - range: `3.5` (Angriffsreichweite)
  
  - **Selector** (Attack-Auswahl)
    - **PerformRangeAttack** (Action)
      - target: `currentTarget`
      - animationTrigger: "RangeAttack"
      - attackDuration: 1.8
      - cooldownTime: 5.0
      - attackChance: 30.0
      - minRange: 5.0 (nicht zu nah)
      - maxRange: 10.0 (nicht zu weit)
      - faceTarget: true
    
    - **PerformHeavyAttack** (Action)
      - target: `currentTarget`
      - animationTrigger: "HeavyAttack"
      - attackDuration: 2.5
      - cooldownTime: 4.0
      - attackChance: 40.0
      - faceTarget: true
    
    - **PerformComboAttack** (Action)
      - target: `currentTarget`
      - animationTrigger: "AttackLeftAndRight"
      - attackDuration: 2.0
      - cooldownTime: 3.0
      - attackChance: 50.0
      - faceTarget: true
    
    - **PerformLeftAttack** (Action)
      - target: `currentTarget`
      - animationTrigger: "AttackLeft"
      - attackDuration: 1.0
      - cooldownTime: 1.5
      - attackChance: 100.0
      - faceTarget: true
    
    - **PerformRightAttack** (Action)
      - target: `currentTarget`
      - animationTrigger: "AttackRight"
      - attackDuration: 1.0
      - cooldownTime: 1.5
      - attackChance: 100.0 (Fallback)
      - faceTarget: true

**Kind 2: MoveToClosestPlayer (Fallback)**
  - player1: `player1`
  - player2: `player2`
  - stoppingDistance: 3.0
  - useAnimator: true
  - animatorBoolParameter: "isRunning"

---

## Wie es funktioniert:

### Logik-Flow:
1. **Selector** versucht erste Option: **Angriff**
   - **IsTargetInRange**: Ist Spieler in 3.5m Reichweite?
     - ✅ **JA**: Gehe zu Attack-Selector
     - ❌ **NEIN**: Sequence Failed → nächste Selector-Option

2. **Attack-Selector** probiert Angriffe (von oben nach unten):
   - **PerformRangeAttack**: 30% Chance, 5s Cooldown (nur wenn 5-10m entfernt)
     - Wenn zu nah/weit oder Cooldown/Random fehlschlägt → nächster
   - **PerformHeavyAttack**: 40% Chance, 4s Cooldown
     - Wenn Cooldown läuft oder Random-Roll fehlschlägt → nächster
   - **PerformComboAttack**: 50% Chance, 3s Cooldown
     - Left+Right Combo-Angriff
   - **PerformLeftAttack**: 100% Chance, 1.5s Cooldown
     - Schneller linker Schlag
   - **PerformRightAttack**: 100% Chance (Fallback)
     - Schneller rechter Schlag (letzter Fallback)

3. **Wenn kein Angriff möglich**: MoveToClosestPlayer
   - Boss läuft zum näheren Spieler
   - Stoppt bei 3m Distanz

### Elden Ring Eigenschaften:
- ✅ **5 verschiedene Angriffe**: Range, Heavy, Combo, Left, Right
- ✅ **Cooldowns**: Boss spammt nicht denselben Angriff
- ✅ **Varianz**: Zufälliger Mix aus Angriffen mit unterschiedlichen Chancen
- ✅ **Range Attack**: Boss kann aus Distanz angreifen (5-10m)
- ✅ **Combo-System**: Left+Right Kombination für Varianz
- ✅ **Intelligentes Movement**: Folgt immer dem näheren Spieler
- ✅ **Face Target**: Boss dreht sich zum Spieler vor Angriff

---

## Animator Setup:

### Benötigte Parameter:
- **isRunning** (Bool) - für Walk-Animation
- **AttackLeft** (Trigger) - Left Hand Attack
- **AttackRight** (Trigger) - Right Hand Attack
- **AttackLeftAndRight** (Trigger) - Combo Attack
- **HeavyAttack** (Trigger) - Heavy Attack
- **RangeAttack** (Trigger) - Range Attack

### States & Transitions:
- **Idle** (Default State)
- **Walk** ← Transition: `isRunning == true`
- **AttackWithLeftHand** ← Trigger: `AttackLeft`
- **AttackWithRightHand** ← Trigger: `AttackRight`
- **AttackWithLeftAndRightHand** ← Trigger: `AttackLeftAndRight`
- **HeavyAttack** ← Trigger: `HeavyAttack`
- **RangeAttack** ← Trigger: `RangeAttack`

Alle Attack-States haben Exit Time und gehen zurück zu Idle.

---

## Nächste Erweiterungen:

### Phase 2 System (wie echte Elden Ring Bosse):
```
Selector
├── Sequence (Phase 2 - bei < 50% HP)
│   ├── IsHealthBelow (50%)
│   └── Phase2Behavior
│       └── Aggresivere Angriffe, kürzere Cooldowns
│
└── Sequence (Phase 1 - Normal)
    └── [Aktueller Tree]
```

### Dodge/Roll System:
- Conditional: `IsPlayerAttacking`
- Action: `PerformDodgeRoll` (wie Margit/Hoarah Loux)

### Combo Chains:
- Sequence: Light → Light → Heavy
- Wie echte Elden Ring Boss-Combos

### Area-of-Effect Attacks:
- Stomp-Angriffe mit Radius-Damage
- Hoarah Loux's Boden-Schockwellen
