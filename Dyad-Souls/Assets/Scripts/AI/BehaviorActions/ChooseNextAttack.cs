using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

/// <summary>
/// Wählt die nächste Attacke basierend auf der letzten Attacke.
/// Implementiert Tree Sentinel-ähnliche Combo-Ketten.
/// </summary>
public class ChooseNextAttack : Action
{
    public SharedString lastAttack;
    public SharedString chosenAttack;
    public SharedFloat playerDistance;
    public SharedBool isPlayerBehind;
    
    // Combo-Counter für erweiterte Ketten
    public SharedInt comboCount;

    public override TaskStatus OnUpdate()
    {
        string next = ChooseNextAttackByContext();
        
        // Update combo counter
        if (comboCount.Value >= 3)
        {
            // Reset combo after 3 attacks, chance for pause or repositioning
            comboCount.Value = 0;
            if (Random.value < 0.3f)
            {
                next = "Reposition"; // Special state to move around player
            }
        }
        else
        {
            comboCount.Value++;
        }

        // Setze Variablen
        chosenAttack.Value = next;
        lastAttack.Value = next;

        return TaskStatus.Success;
    }

    private string ChooseNextAttackByContext()
    {
        string last = lastAttack.Value;
        float distance = playerDistance != null ? playerDistance.Value : 5f;
        bool behind = isPlayerBehind != null ? isPlayerBehind.Value : false;

        // Tree Sentinel-ähnliche Logik: Kontext-basierte Attackenwahl
        switch (last)
        {
            case "Right":
                return ChooseAfterRight(distance, behind);
                
            case "Left":
                return ChooseAfterLeft(distance, behind);
                
            case "LeftRight":
                return ChooseAfterLeftRight(distance, behind);
                
            case "Heavy":
                return ChooseAfterHeavy(distance, behind);
                
            case "Range":
                return ChooseAfterRange(distance, behind);
                
            default:
                // Erste Attacke - basierend auf Spielerposition
                return ChooseInitialAttack(distance, behind);
        }
    }

    private string ChooseAfterRight(float distance, bool behind)
    {
        float rand = Random.value;
        
        if (distance > 8f)
        {
            // Spieler weit weg - Range oder Charge
            return rand < 0.6f ? "Range" : "Heavy";
        }
        else if (behind)
        {
            // Spieler hinter Golem - Drehattacke oder Rückwärts
            return rand < 0.7f ? "LeftRight" : "Left";
        }
        else
        {
            // Nah und vorne - Combo-Fortsetzung
            if (rand < 0.4f) 
                return "Left";
            else if (rand < 0.7f) 
                return "Heavy";
            else 
                return "Range";
        }
    }

    private string ChooseAfterLeft(float distance, bool behind)
    {
        float rand = Random.value;
        
        if (rand < 0.5f) 
            return "Right";
        else if (rand < 0.8f) 
            return "LeftRight";
        else 
            return distance > 6f ? "Range" : "Heavy";
    }

    private string ChooseAfterLeftRight(float distance, bool behind)
    {
        float rand = Random.value;
        
        // Nach großer Combo-Attacke - meist Pause oder Einzelangriff
        if (rand < 0.4f) 
            return "Heavy";
        else if (rand < 0.7f) 
            return "Range";
        else 
            return "Right";
    }

    private string ChooseAfterHeavy(float distance, bool behind)
    {
        float rand = Random.value;
        
        if (distance < 4f)
        {
            // Sehr nah - schnelle Follow-up Attacken
            return rand < 0.6f ? "Right" : "Left";
        }
        else
        {
            // Weiter weg - Range oder erneuter Approach
            return rand < 0.5f ? "Range" : "Right";
        }
    }

    private string ChooseAfterRange(float distance, bool behind)
    {
        float rand = Random.value;
        
        // Nach Range-Attack - meist Approach für Nahkampf
        if (rand < 0.5f) 
            return "Right";
        else if (rand < 0.8f) 
            return "Heavy";
        else 
            return "Left";
    }

    private string ChooseInitialAttack(float distance, bool behind)
    {
        float rand = Random.value;
        
        if (distance > 10f)
        {
            // Sehr weit - Range Attack
            return "Range";
        }
        else if (distance > 6f)
        {
            // Mittel - Heavy Charge oder Range
            return rand < 0.6f ? "Heavy" : "Range";
        }
        else
        {
            // Nah - Nahkampfattacken
            if (rand < 0.4f) 
                return "Right";
            else if (rand < 0.7f) 
                return "Left";
            else 
                return "LeftRight";
        }
    }
}
