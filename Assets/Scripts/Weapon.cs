using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collectable
{
    public enum weaponsKind
    {
        sword, axe
    }
    public weaponsKind kind;
    public Sprite Laying;
    public Sprite Holded;
    public Sprite HoldedSilver;
    private bool isSilver = false;

    public override void Interact() {
        base.Interact();
        if (!isSilver) {
            gameObject.GetComponent<SpriteRenderer>().sprite = Holded;
        }
    }
}