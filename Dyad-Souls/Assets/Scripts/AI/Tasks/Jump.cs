using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class Jump : Action
{
    public float jumpForce = 7f;
    private Rigidbody rb;

    public override void OnStart()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override TaskStatus OnUpdate()
    {
        if (rb == null)
            return TaskStatus.Failure;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        return TaskStatus.Success;
    }
}
