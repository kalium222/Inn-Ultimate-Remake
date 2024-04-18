using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dialoues;

public class Cooker : Talkable
{
    private const string RAW_BIG_FOOD = "RawBigFood";
    private const string BUCKET = "Bucket";

    protected override void DoSpecialEvent() {
        // TODO: ugly
        string currentName = HeroInteraction.instance.bag.GetCurrentItemKind();
        GameObject currentObject = HeroInteraction.instance.bag.GetCurrentItem();
        if (currentName == RAW_BIG_FOOD) {
            currentObject.GetComponent<Collectable>().Drop();
            currentObject.GetComponent<Collectable>().isCollected = true;
            currentObject.SetActive(false);
            GameObject BigFood = Instantiate<GameObject>(specialEventObject, gameObject.transform);
            BigFood.transform.position = HeroController.Instance.transform.position;
        } else if (currentName == BUCKET) {
            Bucket bucket = currentObject.GetComponent<Bucket>();
            if (bucket.IsWearing || bucket.IsFull) return;
            bucket.Fill();
        } else {
            throw new System.Exception("In " + name + " incorrect special event");
        }
    }

    override protected bool IsSpecialConversation() {
        return HeroInteraction.instance.bag.GetCurrentItemKind() switch
        {
            RAW_BIG_FOOD => true,
            BUCKET => true,
            _ => false,
        };
    }

}
