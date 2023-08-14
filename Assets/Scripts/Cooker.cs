using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooker : Talkable
{
    override protected void DialogManagerInit() {
        base.dialogManager = new CookerDialogManager();
        base.dialogManager.setNormalConversations(new List<Conversation>(){
            new Conversation(true, dialogStage.any, "This can be used for cooking"),
            new Conversation(true, dialogStage.any, "Fuck you!"),
            new Conversation(true, dialogStage.first, "Test First.")
        });
        base.dialogManager.setSpecialConversation(new Conversation(false, dialogStage.any, "Cook the chicken?"));
    }

    protected override void SpecialEvent() {
        GameObject RawBigFood = HeroInteraction.instance.Bag.getCurrentItemObject();
        RawBigFood.GetComponent<Collectable>().Drop();
        RawBigFood.GetComponent<Collectable>().isCollected = true;
        RawBigFood.SetActive(false);
        GameObject BigFood = Instantiate<GameObject>(specialEventObject, gameObject.transform);
        BigFood.transform.position = HeroController.instance.transform.position;
        HeroInteraction.instance.SetAnimation();
    }

    // ------------------Subclass for dialogmanager of Cooker-------------------
    private class CookerDialogManager : DialogManager {
        protected override bool isSpecialConversation() {
            return HeroInteraction.instance.Bag.getCurrentItemName() == "RawBigFood";
        }
    }
}
