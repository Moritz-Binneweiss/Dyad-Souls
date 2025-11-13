using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Custom")]
[TaskDescription("Prüft, ob chosenAttack gleich dem angegebenen Wert ist.")]
public class CheckAttackType : Conditional
{
    public SharedString chosenAttack;
    public string attackName;

    public override TaskStatus OnUpdate()
    {
        if (chosenAttack == null || string.IsNullOrEmpty(chosenAttack.Value))
            return TaskStatus.Failure;

        return chosenAttack.Value == attackName ? TaskStatus.Success : TaskStatus.Failure;
    }
}
