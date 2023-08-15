using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wearable : Collectable
{
    // the sprite when wearing
    // handle in inspector
    public Sprite wearingSprite;
    // offset of the wearing sprite
    // handle in inspector
    public float offsetX;
    public float offsetY;

    private HeroWearing heroWearing;

    // state parameter to describe whether this item is weared by hero
    private bool isWearing = false;
    public bool IsWearing {
        get { return isWearing; }
        set { isWearing = value; }
    }

    protected override void Awake() {
        base.Awake();
        if (wearingSprite == null) {
            Debug.Log(name + " wearingSprite is null, tring to use the default sprite");
            if (GetComponent<SpriteRenderer>() == null) throw new System.Exception("GetComponent<SpriteRenderer>() is null");
            else wearingSprite = GetComponent<SpriteRenderer>().sprite;
        }
    }

    protected virtual void Start() {
        heroWearing = HeroWearing.instance;
        if (heroWearing == null) throw new System.Exception("heroWearing is null");
    }

    // work with heroWearing
    public void Wear() {
        GameObject wearingObject = heroWearing.WearingObject;
        if (wearingObject == gameObject) return;
        wearingObject?.GetComponent<Wearable>().UnWear();
        HeroWearing.instance.WearingObject = gameObject;
        isWearing = true;
    }

    public void UnWear() {
        heroWearing.WearingObject = null;
        isWearing = false;
    }

    override public void Use() {
        if (isWearing) UnWear();
        else Wear();
    }
    
}
