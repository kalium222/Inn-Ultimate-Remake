using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dialoues;

public class XuZhen : Talkable, IGameObjectStateHandler, IAttackableHandler
{
    private Animator animator;
    // time that XuZhen will be starving to next level
    public float starvingDeltaTime = 60f;
    private enum FeedLevel {
        die, starving, hungry, full, overfull, explode
    }
    private FeedLevel currentFeedLevel = FeedLevel.overfull;
    private float lastChangeTime = 0f;

    // whether XuZhen has talked to the player
    private bool hasTalked = false;
    // whether XuZhen has got the cola
    private bool hasCola = false;

    // class for saving and loading
    public class XuZhenState : GameObjectStateManager.GameObjectState {
        public bool hasTalked;
        public bool hasCola;
        public float lastChangeTime;
        public XuZhenState(bool hasTalked, bool hasCola, float lastChangeTime) {
            this.hasTalked = hasTalked;
            this.hasCola = hasCola;
            this.lastChangeTime = lastChangeTime;
        }
    }

    override protected void Awake() {
        base.Awake();
        animator = GetComponent<Animator>();
        if (animator == null) {
            Debug.LogError("XuZhen: No animator found!");
        }
        SetAnimator();
        if (normalConversations.Count != 6) {
            Debug.LogError("XuZhen: normal conversation incorrect!");
        }
    }

    private void Update() {
        UpdateFeedLevel();
    }

    override public void Highlight(bool highlight = true) {}

    private void UpdateFeedLevel() {
        int feedLevel = (int)currentFeedLevel;
        feedLevel -= Mathf.FloorToInt((Time.time - lastChangeTime)/starvingDeltaTime);
        feedLevel = Mathf.Clamp(feedLevel, 0, 5);
        if (feedLevel != (int)currentFeedLevel) {
            currentFeedLevel = (FeedLevel)feedLevel;
            lastChangeTime = Time.time;
            Debug.Log("XuZhen: Feed level changed to " + currentFeedLevel);
            SetAnimator();
        }
    }

    private void SetAnimator() {
        animator.SetInteger("FeedLevel", (int)currentFeedLevel);
    }

    override protected void DoSpecialEvent() {
        // TODO:
        Debug.Log("XuZhen: Special Event");
    }

    override protected bool IsSpecialConversation() {
        return HeroInteraction.instance.bag.GetCurrentItemName() switch
        {
            "RawBigFood" => true,
            "BigFood" => true,
            "Cola" => true,
            _ => false,
        };
    }

    override protected Conversation GetCurrentConversation() {
        // TODO:
        return null;
    }

    public void SavetoManager()
    {
        GameManager.instance.gameObjectStateManager.Add(gameObject.name, new XuZhenState(hasTalked, hasCola, lastChangeTime));
    }

    public void LoadfromManager()
    {
        XuZhenState xuZhenState = (XuZhenState)GameManager.instance.gameObjectStateManager.Get(gameObject.name);
        hasTalked = xuZhenState.hasTalked;
        hasCola = xuZhenState.hasCola;
        lastChangeTime = xuZhenState.lastChangeTime;
    }

    public void OnAttack(in MeleeAttack meleeAttack)
    {
        animator.SetTrigger("Killed");
    }
}
