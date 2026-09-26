using UnityEngine;
using System.Collections.Generic;

public class Player : Charapter, ISpellCaster
{
    public void Spell()
    {
        if (MyAnimator)
            MyAnimator.SetTrigger("Spell");
    }
    
    public void HandleInput(string actionName, InputContext inputContext)
    {
        if (actionName == "Rotate")
        {
            Rotate();
        }
        
        if (actionName == "Move")
        {
            Walk();
        }
        
        if (actionName == "Attack")
        {
            Attack();
        }
        
        if (actionName == "Spell")
        {
            Spell();
        }
    }

    // void Update()
    // {
    //     Vector3 p = transform.localPosition;
    //     p -= transform.up * Time.deltaTime * 0.1f;
    //     transform.localPosition = p;
    // }
}
