using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxes : MonoBehaviour, IAttackableHandler, IGameObjectStateHandler
{
    // handle in inspector
    public Sprite boxSprite;
    public Sprite brokenBoxSprite;
    public GameObject content;
    public float offsetX = 0.0f;
    public float offsetY = 0.0f;

    private bool isBroken = false;
    private SpriteRenderer spriteRenderer;

    // class for saved state
    class BoxesState : GameObjectStateManager.GameObjectState {
        public bool isBroken;
        public BoxesState(bool isBroken) {
            this.isBroken = isBroken;
        }
    }

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (content == null) throw new System.Exception("Content not found on " + gameObject.name);
        if (spriteRenderer == null) throw new System.Exception("SpriteRenderer not found on " + gameObject.name);
        if (brokenBoxSprite == null) {
            Debug.Log("Broken box sprite not found on " + gameObject.name);
            brokenBoxSprite = spriteRenderer.sprite;
        }
        if (boxSprite == null) {
            Debug.Log("Box sprite not found on " + gameObject.name);
            boxSprite = spriteRenderer.sprite;
        }
    }

    private void Start() {
        GameObjectStateManager.OnSave += SavetoManager;
        GameObjectStateManager.OnLoad += LoadfromManager;
        SetSprite();
    }

    private void OnDestroy() {
        GameObjectStateManager.OnSave -= SavetoManager;
        GameObjectStateManager.OnLoad -= LoadfromManager;
    }

    private void SetSprite() {
        spriteRenderer.sprite = isBroken ? brokenBoxSprite : boxSprite;
    }
    
    public void OnAttack(in MeleeAttack meleeAttack) {
        if (isBroken) return;
        if (meleeAttack.kind == MeleeAttack.MeleeAttackKind.blow) {
            isBroken = true;
            SetSprite();
            Instantiate(content, transform.position + new Vector3(offsetX, offsetY, 0), Quaternion.identity);
        }
    }

    public void SavetoManager() {
        GameManager.Instance.gameObjectStateManager.Add(gameObject.name, new BoxesState(isBroken));
    }

    public void LoadfromManager() {
        if (GameManager.Instance.gameObjectStateManager.Contains(gameObject.name)) {
            BoxesState state = (BoxesState)GameManager.Instance.gameObjectStateManager.Get(gameObject.name);
            isBroken = state.isBroken;
        }
        SetSprite();
    }
}
