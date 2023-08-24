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

    // state parameters
    // whether XuZhen has talked to the player for the first time
    private bool hasTalked = false;
    // whether XuZhen has got the cola
    private bool hasCola = false;
    // whether XuZhen has been killed by player
    private bool hasKilled = false;
    // whether XuZhen has died
    private bool hasDied = false;

    // class for saving and loading
    public class XuZhenState : GameObjectStateManager.GameObjectState {
        public bool hasTalked;
        public bool hasCola;
        public bool hasKilled;
        public bool hasDied;
        public float lastChangeTime;
        public XuZhenState(bool hasTalked, bool hasCola, bool hasKilled, bool hasDied, float lastChangeTime) {
            this.hasTalked = hasTalked;
            this.hasCola = hasCola;
            this.hasKilled = hasKilled;
            this.hasDied = hasDied;
            this.lastChangeTime = lastChangeTime;
        }
    }

    override protected void Awake() {
        base.Awake();
        animator = GetComponent<Animator>();
        if (animator == null) {
            Debug.LogError("XuZhen: No animator found!");
        }
        if (normalConversations.Count != 7) {
            Debug.LogError("XuZhen: normal conversation incorrect!");
        }
        
    }

    override protected void Start() {
        base.Start();
        GameObjectStateManager.OnSave += SavetoManager;
        GameObjectStateManager.OnLoad += LoadfromManager;
        SetAnimator();
    }

    private void OnDestroy() {
        GameObjectStateManager.OnSave -= SavetoManager;
        GameObjectStateManager.OnLoad -= LoadfromManager;
    }

    override public void Highlight(bool highlight = true) {}

    private void Update() {
        UpdateFeedLevel();
        SetAnimator();
    }
    private void UpdateFeedLevel() {
        if (hasKilled) return;
        if (hasCola) {
            currentFeedLevel = FeedLevel.overfull;
            return;
        }
        int feedLevel = (int)currentFeedLevel;
        feedLevel -= Mathf.FloorToInt((Time.time - lastChangeTime)/starvingDeltaTime);
        feedLevel = Mathf.Clamp(feedLevel, 0, 5);
        if (feedLevel != (int)currentFeedLevel) {
            currentFeedLevel = (FeedLevel)feedLevel;
            lastChangeTime = Time.time;
            SetAnimator();
        }
        if (currentFeedLevel == FeedLevel.die) {
            hasDied = true;
        }
    }

    private void SetAnimator() {
        animator.SetInteger("FeedLevel", (int)currentFeedLevel);
        animator.SetBool("Killed", hasKilled);
    }

    override protected void DoSpecialEvent() {
        string currentItemName = HeroInteraction.instance.bag.GetCurrentItemKind();
        GameObject currentItem = HeroInteraction.instance.bag.GetCurrentItem();
        switch (currentItemName)
        {
            case "Cola":
                hasCola = true;
                currentItem.GetComponent<Collectable>().Drop();
                currentItem.GetComponent<Collectable>().isCollected = true;
                currentItem.SetActive(false);
                StartCoroutine(base.RunConversationCoroutine(normalConversations[3]));
                break;
            case "BigFood":
                currentItem.GetComponent<Collectable>().Drop();
                currentItem.GetComponent<Collectable>().isCollected = true;
                currentItem.SetActive(false);
                currentFeedLevel = (FeedLevel)(Mathf.Clamp((int)currentFeedLevel + 2, 0, 5));
                if (currentFeedLevel == FeedLevel.explode) {
                    SetAnimator();
                    animator.SetTrigger("Kill");
                    break;
                }
                StartCoroutine(GameUIManager.instance.ConversationCoroutine(normalConversations[4]));
                break;
            case "RawBigFood":
                StartCoroutine(GameUIManager.instance.ConversationCoroutine(normalConversations[5]));
                break;
            default:
                break;
        }
    }

    override protected bool IsSpecialConversation() {
        return HeroInteraction.instance.bag.GetCurrentItemKind() switch
        {
            "RawBigFood" => true,
            "BigFood" => true,
            "Cola" => true,
            _ => false,
        };
    }

    override protected Conversation GetCurrentConversation() {
        if (hasDied) return null;
        else if (!hasTalked) {
            hasTalked = true;
            return normalConversations[0];
        } else if (hasCola) {
            return normalConversations[3];
        } else if (IsSpecialConversation()) {
            return specialConversation;
        } else {
            switch (currentFeedLevel) {
                case FeedLevel.die:
                case FeedLevel.explode:
                    return normalConversations[6];
                case FeedLevel.starving:
                case FeedLevel.hungry:
                    return normalConversations[2];
                case FeedLevel.full:
                case FeedLevel.overfull:
                    return normalConversations[1];
                default:
                    return null;
            }
        }
     }

    public void SavetoManager()
    {
        GameManager.instance.gameObjectStateManager.Add(gameObject.name, new XuZhenState(hasTalked, hasCola, hasKilled, hasDied, lastChangeTime));
    }

    public void LoadfromManager()
    {
        XuZhenState xuZhenState = (XuZhenState)GameManager.instance.gameObjectStateManager.Get(gameObject.name);
        hasTalked = xuZhenState.hasTalked;
        hasCola = xuZhenState.hasCola;
        hasKilled = xuZhenState.hasKilled;
        hasDied = xuZhenState.hasDied;
        lastChangeTime = xuZhenState.lastChangeTime;
    }

    public void OnAttack(in MeleeAttack meleeAttack)
    {
        if (hasKilled) return;
        animator.SetTrigger("Kill");
        hasKilled = true;
        hasDied = true;
        SetAnimator();
    }

}
