using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : Door
{
    public override void Interact() {
        if (!HeroController.instance.Climbed) {
            GameUIManager.instance.ShowDialogue("Too high to reach.");
        } else if (isSealed) {
            GameUIManager.instance.ShowDialogue("It's sealed.");
        } else {
            portal.Teleport();
        }
    }

    // TODO: ugly
    override protected void LoadTarget() {
        
    }

    override public void OnAttack(in MeleeAttack meleeAttack) {
        if (!HeroController.instance.Climbed) {
            return;
        }
        base.OnAttack(in meleeAttack);
    }
}
