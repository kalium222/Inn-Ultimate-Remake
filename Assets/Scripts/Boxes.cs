using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxes : MonoBehaviour, IAttackableHandler
{
    // handle in inspector
    public Sprite boxSprite;
    public Sprite brokenBoxSprite;
    public GameObject content;

    private bool isBroken = false;
    
    public void OnAttack(in MeleeAttack meleeAttack) {
        // TODO: 
        if (meleeAttack.kind == MeleeAttack.MeleeAttackKind.blow) {
            Destroy(gameObject);
        }
    }
}
