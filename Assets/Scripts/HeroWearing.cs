using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroWearing : MonoBehaviour
{
    public static HeroWearing instance;
    private GameObject wearingObject = null;
    public GameObject WearingObject {
        get { return wearingObject; }
        set {
            wearingObject = value;
            // TODO: useful?
            // if (wearingObject == null) {
            //     wearingRenderer.sprite = null;
            // } else {
            //     wearingRenderer.sprite = wearingObject.GetComponent<SpriteRenderer>().sprite;
            // }
        }
    }
    private SpriteRenderer wearingRenderer;
    
    private void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        wearingRenderer = GetComponent<SpriteRenderer>();
        if (wearingRenderer == null) throw new System.Exception("wearingRenderer is null");
    }

    private void Update() {
        if (wearingObject == null) {    // wearing nothing
            gameObject.GetComponent<SpriteRenderer>().sprite = null;
        } else {    //wearing object exists
            // check the correctness of wearingObject
            if (wearingObject.GetComponent<Wearable>() == null)
                throw new System.Exception("wearingObject.GetComponent<Wearable>() is null");
            // TODO: check the wearing sprite
            wearingRenderer.sprite = wearingObject.GetComponent<Wearable>().wearingSprite;
            wearingRenderer.flipX = (HeroController.instance.lookDirection == 1);
            transform.position =  HeroController.instance.transform.position + new Vector3(
                wearingObject.GetComponent<Wearable>().offsetX*HeroController.instance.lookDirection,
                wearingObject.GetComponent<Wearable>().offsetY, 0
            );

        }
    }

}
