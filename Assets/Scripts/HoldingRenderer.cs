using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingRenderer : MonoBehaviour
{
    SpriteRenderer holdingRenderer;
    HeroAttack heroAttack;

    const float offsetx = 0.17f;
    const float offsety = 0.42f;
    
    void Start() {
        holdingRenderer = GetComponent<SpriteRenderer>();
        heroAttack = HeroController.instance.GetComponent<HeroAttack>();
    }

    // Update is called once per frame
    void Update() {
        string currItemName = HeroInteraction.instance.Bag.getCurrentItemName();
        if (currItemName == "emptyhanded") {
            holdingRenderer.enabled = false;
            return;
        }
        GameObject currItemObject = HeroInteraction.instance.Bag.getCurrentItemObject();
        if (heroAttack.isAttacking) {
            holdingRenderer.enabled = false;
        } else if (currItemObject.GetComponent<Wearable>() != null 
        && currItemObject.GetComponent<Wearable>().IsWearing) {
            holdingRenderer.enabled = false;
        } else {
            holdingRenderer.enabled = true;
            for (int i=0; i<GameManager.instance.collectableManager.changedCollectableInfos.Count; i++) {
                GameObject curr = GameManager.instance.collectableManager.changedCollectableInfos[i].collectable;
                if (curr.name == HeroInteraction.instance.Bag.getCurrentItemName()) {
                    holdingRenderer.sprite = curr.GetComponent<SpriteRenderer>().sprite;
                    holdingRenderer.flipX = (HeroController.instance.lookDirection == 1);
                    transform.position =  HeroController.instance.transform.position + new Vector3(
                        offsetx*HeroController.instance.lookDirection, offsety, 0
                    );
                    break;
                }
            }
        }
    }
}
