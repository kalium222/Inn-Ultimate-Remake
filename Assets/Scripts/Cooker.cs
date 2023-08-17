using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooker : Talkable
{
    private const string RAW_BIG_FOOD = "RawBigFood";
    private const string BUCKET = "Bucket";

    override protected void DialogManagerInit() {
        base.dialogManager = new CookerDialogManager();
        base.dialogManager.setNormalConversations(new List<Conversation>(){
            new Conversation(true, dialogStage.any, "This can be used for cooking or geting water."),
            new Conversation(true, dialogStage.first, "Test First.")
        });
        base.dialogManager.setSpecialConversation(new Conversation(false, dialogStage.any, "Use this?"));
    }

    protected override void SpecialEvent() {
        // TODO: ugly
        string currentName = HeroInteraction.instance.bag.GetCurrentItemName();
        GameObject currentObject = HeroInteraction.instance.bag.GetCurrentItem();
        if (currentName == RAW_BIG_FOOD) {
            currentObject.GetComponent<Collectable>().Drop();
            currentObject.GetComponent<Collectable>().isCollected = true;
            currentObject.SetActive(false);
            GameObject BigFood = Instantiate<GameObject>(specialEventObject, gameObject.transform);
            BigFood.transform.position = HeroController.instance.transform.position;
        } else if (currentName == BUCKET) {
            Bucket bucket = currentObject.GetComponent<Bucket>();
            if (bucket.IsWearing || bucket.IsFull) return;
            bucket.Fill();
        } else {
            throw new System.Exception("In " + name + " incorrect special event");
        }
    }

    // ------------------Subclass for dialogmanager of Cooker-------------------
    private class CookerDialogManager : DialogManager {
        protected override bool isSpecialConversation() {
            return HeroInteraction.instance.bag.GetCurrentItemName() switch
            {
                RAW_BIG_FOOD => true,
                BUCKET => true,
                _ => false,
            };
        }
    }
}
