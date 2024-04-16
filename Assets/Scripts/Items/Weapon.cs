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

    public delegate void WeaponAttackHandler(weaponsKind kind, bool isSilver);
    public static event WeaponAttackHandler OnWeaponAttack;

    public override void Interact() {
        base.Interact();
        if (!isSilver) {
            gameObject.GetComponent<SpriteRenderer>().sprite = Holded;
        }
    }

    override public void Use() {
       OnWeaponAttack?.Invoke(kind, isSilver);
    }
}