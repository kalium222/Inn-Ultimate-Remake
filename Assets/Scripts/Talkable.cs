using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dialoues;

public class Talkable :  Interactable
{
    // get the Keycode from HeroInteraction to make sure the key is the same
    static private KeyCode leftKey;
    static private KeyCode rightKey;
    static private KeyCode confirmKey;
    // handle in inspector
    public GameObject specialEventObject;

    // status parameters of one conversation
    // store the player's decision
    private bool isYes = true;

    // handle in inspector
    public List<Conversation> normalConversations;
    public Conversation specialConversation;

    protected virtual void Start() {
        GameUIManager.instance.OnDecisionMade.AddListener(OnDecisionMade);
        leftKey = HeroInteraction.instance.previousItemKey;
        rightKey = HeroInteraction.instance.nextItemKey;
        confirmKey = HeroInteraction.instance.interactKey;
    }

    private void OnDestroy() {
        GameUIManager.instance.OnDecisionMade.RemoveListener(OnDecisionMade);
    }

    private void OnDecisionMade(bool isYes) {
        this.isYes = isYes;
    }

    protected virtual void DoSpecialEvent() {
        Debug.Log("Special Event");
    }
    
    override public void Interact() {
        StartCoroutine(RunConversationCoroutine(GetCurrentConversation()));
    }

    protected IEnumerator RunConversationCoroutine(Conversation con) {
        // Start at next frame
        yield return null;
        // First disable all movement and interaction
        HeroController.instance.CanMove = false;
        HeroInteraction.instance.CanInteract = false;
        
        // run the dialogs
        yield return GameUIManager.instance.ConversationCoroutine(con);
        // Then do the special event
        if (IsSpecialConversation()&&isYes) DoSpecialEvent();
        
        // Then re-enable all movement and interaction
        HeroController.instance.CanMove = true;
        HeroInteraction.instance.CanInteract = true;
    }

    virtual protected Conversation GetCurrentConversation() {
        if (IsSpecialConversation()) return specialConversation;
        else if (normalConversations == null || normalConversations.Count == 0) return null;
        else 
            return normalConversations[(int)GameManager.instance.gameStageManager.CurrentStage%normalConversations.Count];
        
    }

    virtual protected bool IsSpecialConversation() {
        return GameManager.instance.gameStageManager.CurrentStage == GameStageManager.Stage.Exploring;
    }

}