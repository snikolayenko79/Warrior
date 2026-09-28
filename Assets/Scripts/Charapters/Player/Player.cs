using UnityEngine;
using System.Collections.Generic;

public class Player : Charapter, IRotable, ISpellCaster
{
    [SerializeField] private float MyRotateSpeed = 100;

    public float RotationSpeed
    {
        get => MyRotateSpeed;
    }

    private float rotateDirection = 0;

    public float RotationDirection
    {
        get => rotateDirection;
        set => rotateDirection = value;
    }

    public bool IsRotating
    {
        get => RotationDirection != 0;
    }

    public Vector3 Rotation
    {
        get => transform.eulerAngles;
        set  => transform.rotation = Quaternion.Euler(value);
    }

    
    
    public void Spell()
    {
        if (MyAnimator)
            MyAnimator.SetTrigger("Spell");
    }
}