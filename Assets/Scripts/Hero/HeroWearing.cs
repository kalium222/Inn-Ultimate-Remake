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
            wearingRenderer.flipX = (HeroController.Instance.LookDirection == 1);
            transform.position =  HeroController.Instance.transform.position + new Vector3(
                wearingObject.GetComponent<Wearable>().offsetX*HeroController.Instance.LookDirection,
                wearingObject.GetComponent<Wearable>().offsetY, 0
            );

        }
    }

}
