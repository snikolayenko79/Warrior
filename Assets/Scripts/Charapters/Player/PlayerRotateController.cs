using UnityEngine;

public class PlayerRotateController : InputListener
{
    private IRotable rotateTarget;

    public void SetTarget(IRotable _rotable)
    {
        rotateTarget = _rotable;
    }
    
    public override void HandleInput(string logicalActionName, InputContext context)
    {
        //Debug.Log("PlayerRotateController HandleInput: " + logicalActionName);
        if (logicalActionName == "Rotate")
        {
            rotateTarget.RotationDirection = context.FloatValue;
        }
    }

    protected virtual void Update()
    {
        //if (rotateTarget != null && rotateTarget.IsRotating)
        if (rotateTarget is { IsRotating : true})
        {
            Vector3 r = rotateTarget.Rotation;
            r.z += rotateTarget.RotationSpeed * rotateTarget.RotationDirection * Time.deltaTime;
            rotateTarget.Rotation = r;
        }
    }
}
