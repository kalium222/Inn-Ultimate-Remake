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
        string currItemName = HeroInteraction.instance.Bag.GetCurrentItemName();
        if (currItemName == "emptyhanded") {
            holdingRenderer.enabled = false;
            return;
        }
        GameObject currItemObject = HeroInteraction.instance.Bag.GetCurrentItem();
        if (heroAttack.isAttacking) {
            holdingRenderer.enabled = false;
        } else if (currItemObject.GetComponent<Wearable>() != null 
        && currItemObject.GetComponent<Wearable>().IsWearing) {
            holdingRenderer.enabled = false;
        } else {
            HoldCurrentItem(currItemObject);
        }
    }

    private void HoldCurrentItem(GameObject currentItemObject) {
        holdingRenderer.enabled = true;
        holdingRenderer.sprite = currentItemObject.GetComponent<SpriteRenderer>().sprite;
        holdingRenderer.flipX = (HeroController.instance.lookDirection == 1);
        transform.position =  HeroController.instance.transform.position + new Vector3(
            offsetx*HeroController.instance.lookDirection, offsety, 0
        );
    }
}
