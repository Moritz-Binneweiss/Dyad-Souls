# Boss Behavior Tree - Smooth Attack System Implementation Guide

## Übersicht
Dieses Guide zeigt, wie Sie Ihren Boss Behavior Tree optimieren können für smoothere und dynamischere Attacken.

## Problem-Analyse des aktuellen Systems

### Aktuelle Schwächen:
1. **Repetitive Attacks**: Jede Attacke hat gleiche Wahrscheinlichkeit
2. **Starre Cooldowns**: Feste Cooldown-Zeiten unabhängig von Situation
3. **Keine Attack Chains**: Attacken sind isoliert, keine fließenden Combos
4. **Abrupte Transitions**: Harte Cuts zwischen Animationen
5. **Fehlende Dynamik**: Keine Anpassung an Boss Health oder Kampfintensität

## Neue Komponenten

### 1. AttackSelector.cs
**Zweck**: Intelligente Attacken-Auswahl basierend auf Distanz, History und Gewichtung

**Features**:
- Distanz-basierte Attack-Auswahl (Close/Mid/Far Range)
- Gewichtungssystem für verschiedene Attacken
- Anti-Repetition System (reduziert Wahrscheinlichkeit kürzlich verwendeter Attacks)
- Combo-Chain Tracking

**Verwendung im Behavior Tree**:
```
Selector
├── Sequence [High Priority Attacks]
│   ├── IsCooldownReady (name: "SpecialAttack")
│   ├── AttackSelector -> selectedAttackType
│   ├── Conditional: Check if "SpecialAttack" selected
│   └── ExecuteSpecialAttack
│
└── Sequence [Normal Attacks]
    ├── AttackSelector -> selectedAttackType
    └── SmoothAttackExecutor (uses selectedAttackType)
```

**Parameter Setup**:
- `closeRangeMax`: 2.5 (Melee Range)
- `midRangeMax`: 5.0 (Medium Range)
- `farRangeMax`: 12.0 (Long Range)
- `lightAttackWeight`: 40 (häufig)
- `heavyAttackWeight`: 30 (mittel)
- `comboAttackWeight`: 20 (seltener)
- `rangeAttackWeight`: 50 (in Range bevorzugt)
- `specialAttackWeight`: 15 (selten aber impactful)

### 2. SmoothAttackExecutor.cs
**Zweck**: Führt Attacken mit smoothen Transitions und optionaler Ziel-Rotation aus

**Features**:
- Animation Blending (CrossFade statt Play)
- Anticipation Phase (Wind-up vor Attacke)
- Recovery Phase (Cool-down nach Attacke)
- Smooth Rotation zum Ziel während Attacke
- Dynamische Attack-Duration basierend auf Type

**Verwendung**:
Ersetzt einzelne Attack-Tasks (AttackRightHand, AttackHeavy, etc.)

**Parameter Setup**:
- `blendTime`: 0.2s (smooth transition)
- `smoothRotateToTarget`: true
- `rotationSpeed`: 3.0
- `anticipationTime`: 0.2s (gibt Player Reaktionszeit)
- `recoveryTime`: 0.3s (verhindert instant Follow-up)

### 3. DynamicCooldownManager.cs
**Zweck**: Intelligente Cooldowns die sich an Kampfsituation anpassen

**Features**:
- Health-basierte Anpassung (niedrige Health = kürzere Cooldowns)
- Repetition Penalty (gleiche Attacke wiederholt = längere Cooldown)
- Combat Intensity Tracking (schnelle Combos = kürzere Cooldowns)
- Min/Max Clamping für Balance

**Verwendung**:
Ersetzt die statische `SetCooldown` Task

**Parameter Setup**:
- `baseCooldownDuration`: 3.0s (Standard)
- `adjustByHealth`: true
- `lowHealthThreshold`: 30% (unter 30% Health wird aggressiver)
- `penalizeRepetition`: true
- `repetitionPenalty`: 1.5s (Extra Cooldown bei Wiederholung)
- `minCooldown`: 0.5s (nie zu schnell)
- `maxCooldown`: 10s (nie zu langsam)

### 4. AttackChainManager.cs
**Zweck**: Ermöglicht fließende Attack-Chains und Combos

**Features**:
- Definierte Chain-Patterns (z.B. Light → Heavy → Special)
- Dynamische Chain-Chance (nimmt mit Chain-Länge ab)
- Natural Flow zwischen Attacken
- Chain-Reset bei Pause

**Verwendung im Behavior Tree**:
```
Sequence [Attack with Chain Potential]
├── AttackSelector
├── SmoothAttackExecutor
└── Selector [Chain Check]
    ├── Sequence [Continue Chain]
    │   ├── AttackChainManager -> shouldContinueChain (Success)
    │   ├── Wait (timeBetweenChainAttacks)
    │   └── SmoothAttackExecutor (nextChainAttack)
    └── Success [End Chain]
```

**Parameter Setup**:
- `maxChainLength`: 3 (max 3 Attacks in einer Chain)
- `chainContinueChance`: 70% (hohe Chance für Combos)
- `decreasingChance`: true (Chance sinkt pro Hit)
- `timeBetweenChainAttacks`: 0.4s (kurze Pause zwischen Chain-Hits)

### 5. AnimationStateHelper.cs
**Zweck**: Verbesserte Animation-Kontrolle und State-Tracking

**Features**:
- Intelligente Blend-Time basierend auf Transition-Type
- Animation State Tracking
- Verhindert Animation-Replays zu schnell hintereinander
- Helper-Methoden für Behavior Tree Tasks

**Verwendung**:
Als Component auf Boss GameObject, wird von anderen Tasks referenziert

**Setup**:
Einfach als Component hinzufügen, Auto-Configuration der State-Durations

## Implementierungs-Schritte

### Schritt 1: Scripts hinzufügen
1. Kopiere alle 5 neuen Scripts in `Assets/Scripts/AI/BehaviorActions/`
2. Füge `AnimationStateHelper.cs` als Component zum Boss GameObject hinzu
3. Füge `GetHealthPercentage()` Methode zu `EnemyManager.cs` hinzu (bereits erledigt)

### Schritt 2: Behavior Tree Umstrukturierung

#### Altes System (aktuell):
```
Sequence
├── Conditional (IsInRange)
├── Conditional (IsCooldownReady)
├── AttackRightHand
└── SetCooldown
```

#### Neues System (empfohlen):
```
Selector [Attack Decision]
├── Sequence [Chained Attack]
│   ├── AttackChainManager -> shouldContinueChain
│   ├── [If Success: Continue Chain]
│   └── SmoothAttackExecutor (nextChainAttack)
│
└── Sequence [New Attack]
    ├── AttackSelector -> selectedAttackType
    ├── DynamicCooldownManager (cooldownName: selectedAttackType)
    └── SmoothAttackExecutor (attackType: selectedAttackType)
```

### Schritt 3: Parameter-Tuning

#### AttackSelector Weights (für balancierten Kampf):
```
Close Range (0-2.5m):
- Light: 40% (häufig, schnell)
- Heavy: 30% (mittel, mehr Damage)
- Combo: 20% (selten, gefährlich)

Mid Range (2.5-5m):
- Range: 50% (hauptsächlich)
- Combo: 15% (gap closer)
- Special: 15% (variiert)

Far Range (5m+):
- Range: 60% (hauptsächlich)
- Special: 30% (impactful)
```

#### Cooldowns (ausgeglichen):
```
Light Attack: 1.5s base
Heavy Attack: 2.5s base
Combo Attack: 4.0s base
Range Attack: 3.0s base
Special Attack: 6.0s base
```

### Schritt 4: Animation Setup

#### Animator Controller Anpassungen:
1. Füge Blend Trees hinzu für smoother Transitions
2. Setze Transition-Durations auf 0.1-0.3s
3. Aktiviere "Can Transition To Self" für Chain-Attacks
4. Füge Exit Time nur wo nötig hinzu (bevorzuge Interrupts)

#### Animation Event Setup:
- Behalte `DealAttackDamage()` Events in Animationen
- Füge optional `OnAttackAnticipation()` Event am Anfang hinzu
- Füge optional `OnAttackRecovery()` Event am Ende hinzu

## Best Practices

### 1. Cooldown Naming Convention
```csharp
// Kategorien-basiert (empfohlen)
"LightAttack" // Alle Light Attacks teilen Cooldown
"HeavyAttack" // Alle Heavy Attacks teilen Cooldown
"SpecialAttack" // Alle Special Attacks teilen Cooldown

// Spezifisch (für unique Attacken)
"Stomp1"
"BestialRoar"
"Earthquake"
```

### 2. Distance Check Optimization
Verwende Shared Variables für häufig genutzte Werte:
```
SharedFloat closeRange (2.5)
SharedFloat midRange (5.0)
SharedFloat farRange (12.0)
```

### 3. Attack Priority System
Strukturiere BT mit Priority:
```
Selector [Attack Priority]
├── 1. Counter/Dodge (highest priority)
├── 2. Special Attacks (cooldown-gated)
├── 3. Chain Continuation (if in chain)
└── 4. Normal Attacks (default)
```

### 4. Debugging
Alle neuen Scripts haben Debug.Log() Statements. Aktiviere bei Problemen:
```csharp
// In AttackSelector.cs
Debug.Log($"Selected Attack: {selectedAttack} for distance {distance}");

// In AttackChainManager.cs
Debug.Log($"Chain continues! Count: {chainCount}");
```

## Vorher/Nachher Vergleich

### Vorher:
- ❌ Starre Attack-Reihenfolge
- ❌ Gleiche Cooldown immer
- ❌ Keine Combos
- ❌ Abrupte Animation-Cuts
- ❌ Repetitive Patterns

### Nachher:
- ✅ Intelligente Attack-Auswahl
- ✅ Dynamische Cooldowns
- ✅ Fließende Attack-Chains
- ✅ Smooth Animation-Blending
- ✅ Variabler Kampfstil

## Erweiterte Features (Optional)

### 1. Phase-System
Boss verhält sich anders bei verschiedenen Health-Levels:
```csharp
// In AttackSelector.cs
if (enemyManager.GetHealthPercentage() < 30f)
{
    // Phase 2: Aggressiver
    specialAttackWeight.Value *= 2f;
    comboFollowUpChance.Value = 80f;
}
```

### 2. Player Behavior Tracking
Boss lernt Player-Verhalten:
```csharp
// Tracke wie oft Player dodged
// Passe Attack-Timing an
// Fake-out Attacks bei predictable Patterns
```

### 3. Environmental Awareness
Boss nutzt Arena:
```csharp
// Range Attacks bei hoher Distanz
// Stomp bei vielen Players in der Nähe
// Roar wenn Player healing
```

## Performance Considerations

- ✅ Alle Dictionary-Lookups sind O(1)
- ✅ Static Dictionaries werden geteilt (kein Memory-Overhead pro Instance)
- ✅ Keine Physics-Updates in Tasks
- ✅ Minimale GC-Allocations

## Troubleshooting

### Problem: Attacken triggern nicht
**Lösung**: 
- Prüfe `selectedAttackType` Shared Variable
- Verifiziere Range-Checks in AttackSelector
- Check Animator Controller Layer-Weights

### Problem: Zu schnelle Attacken
**Lösung**:
- Erhöhe `baseCooldownDuration` in DynamicCooldownManager
- Erhöhe `recoveryTime` in SmoothAttackExecutor
- Reduziere `chainContinueChance` in AttackChainManager

### Problem: Boss rotiert nicht zum Player
**Lösung**:
- Setze `smoothRotateToTarget` auf true
- Erhöhe `rotationSpeed`
- Prüfe ob `target` SharedGameObject korrekt gesetzt ist

### Problem: Animationen sind abgehackt
**Lösung**:
- Reduziere `blendTime` nicht unter 0.1s
- Prüfe Animator Controller Transition-Settings
- Verifiziere Animation-Clips haben korrekte Frame-Rates

## Nächste Schritte

1. ✅ Scripts implementieren
2. ✅ Behavior Tree umstrukturieren
3. ⏳ Parameter tunen und testen
4. ⏳ Animations polieren
5. ⏳ Playtesting und Balancing

## Support & Weiterentwicklung

Bei Fragen oder Problemen:
- Check Debug-Logs
- Verwende Gizmos für Visual Debugging
- Teste einzelne Tasks isoliert
- Iteriere Parameter schrittweise

Viel Erfolg beim Verfeinern Ihres Boss-Kampfes! 🎮
