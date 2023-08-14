using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : Wearable
{
    // whether the bucket is full of water
    private bool isFull = false;
    public bool IsFull {
        get { return isFull; }
        set { isFull = value; }
    }

    private SpriteRenderer bucketRenderer;

    // different sprites for different states
    // handle in inspector
    public Sprite emptyBucketSprite;
    public Sprite fullBucketSprite;

    protected override void Awake() {
        base.Awake();
        bucketRenderer = GetComponent<SpriteRenderer>();
        if (bucketRenderer == null) throw new System.Exception("bucketRenderer is null");
    }

    protected override void Start() {
        base.Start();
        // check if the sprites are set
        if (emptyBucketSprite == null) throw new System.Exception("emptyBucketSprite is null");
        if (fullBucketSprite == null) throw new System.Exception("fullBucketSprite is null");
        UnFill();
    }

    public void Fill() {
        isFull = true;
        bucketRenderer.sprite = fullBucketSprite;
    }

    public void UnFill() {
        isFull = false;
        bucketRenderer.sprite = emptyBucketSprite;
    }

    override public void Use() {
        // TODO: pour water?
        if (isFull) base.Drop();
        base.Use();
    }

}
