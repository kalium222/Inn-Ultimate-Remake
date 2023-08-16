using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxes : MonoBehaviour
{
    // TODO:
    public void OnAttack(in MeleeAttack meleeAttack) {
        // TODO: 
        if (meleeAttack.kind == MeleeAttack.MeleeAttackKind.blow) {
            Destroy(gameObject);
        }
    }
}
