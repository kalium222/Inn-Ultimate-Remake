using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable :  Interactable
{
    // get the Keycode from HeroInteraction to make sure the key is the same
    static private KeyCode leftKey;
    static private KeyCode rightKey;
    static private KeyCode confirmKey;

    public GameObject specialEventObject;

    // status parameters of one conversation
    private bool isYes = true;
    // DialogManager. Check in tool class
    protected DialogManager dialogManager;

    protected virtual void Start() {
        leftKey = HeroInteraction.instance.previousItemKey;
        rightKey = HeroInteraction.instance.nextItemKey;
        confirmKey = HeroInteraction.instance.interactKey;
        dialogManager = new DialogManager();
        DialogManagerInit();
    }

    protected virtual void DialogManagerInit() {
        dialogManager.setNormalConversations(new List<Conversation>(){
            new Conversation(true, dialogStage.any, "This is base class for talkable"),
        });
        dialogManager.setSpecialConversation(new Conversation(false, dialogStage.any, "Test Special."));
    }

    protected virtual void SpecialEvent() {
        Debug.Log("Special Event");
    }
    
    override public void Interact() {
        StartCoroutine(runDialogCoroutine());
    }

    private IEnumerator runDialogCoroutine() {
        // Start at next frame
        yield return null;
        // First disable all movement and interaction
        HeroController.instance.CanMove = false;
        HeroInteraction.instance.CanInteract = false;
        // reset the dialog
        dialogManager.ResetConversation();

        // Then run the dialog
        while (!dialogManager.isEndConversation()) {

            // set the dialog box
            Conversation currentConversation = dialogManager.getCurrentConversation();
            GameUIManager.instance.setDialogBox(currentConversation.text, currentConversation.isContinuing);

            // take the input and set the options
            bool confirm = Input.GetKeyDown(confirmKey);
            if (!currentConversation.isContinuing) {
                if (Input.GetKeyDown(leftKey) || (Input.GetAxis("Horizontal") < -0.1f)) {
                    isYes = true;
                } else if (Input.GetKeyDown(rightKey) || (Input.GetAxis("Horizontal") > 0.1f)) {
                    isYes = false;
                }
                GameUIManager.instance.setDialogOption(currentConversation.isContinuing, isYes);
            }

            // update
            dialogManager.UpdateConversation(confirm, isYes);
            yield return null;
        }

        // Then do the special event
        if (dialogManager.isSpecialEvent()) {
            SpecialEvent();
        }

        // Then clear the dialog box
        GameUIManager.instance.clearDialogBox();
        // Then re-enable all movement and interaction
        HeroController.instance.CanMove = true;
        HeroInteraction.instance.CanInteract = true;
    }



    // ------------------------------Tool class-----------------------------

    // Lable the dialog to select whether to show in current stage
    public enum dialogStage{
        stage1, stage2, first, any
    }

    // One conversation
    [System.Serializable]
    public class Conversation {
        public bool isContinuing;
        public dialogStage stage;
        public string text;
        public Conversation() {
            isContinuing = true;
            stage = dialogStage.any;
            text = "Empty";
        }
        public Conversation(bool isContinuing = true, dialogStage stage = dialogStage.any, string text = "Empty") {
            this.isContinuing = isContinuing;
            this.stage = stage;
            this.text = text;
        }
    }

    // Manage the dialog
    public class DialogManager {
        private int currentConversationIndex = 0;
        private bool endConversation = false;
        private bool happenSpecialEvent = false;
        protected List<Conversation> normalConversations;
        private Conversation specialConversation;

        protected virtual bool isSpecialConversation() {
            return false;
        }

        public bool isSpecialEvent() {
            return happenSpecialEvent;
        }

        public void setSpecialConversation(Conversation conversation = null) {
            specialConversation = conversation;
        }

        public void setNormalConversations(List<Conversation> conversations) {
            normalConversations = conversations;
        }

        public bool isEndConversation() {
            return endConversation;
        }

        // Return the current conversation no matter it is special or normal
        public Conversation getCurrentConversation() {
            if (isSpecialConversation()) {
                if (specialConversation == null) {
                    Debug.LogError("Special conversation is not set");
                    return null;
                }
                return specialConversation;
            } else {
                if (currentConversationIndex >= normalConversations.Count) {
                    Debug.LogError("Normal conversation is out of range");
                    return null;
                }
                return normalConversations[currentConversationIndex];
            }
        }

        // Update the current conversation index no matter it is special or normal
        // Update the endConversation flag
        public void UpdateConversation(bool confirm, bool isYes) {
            if (isSpecialConversation()) {
                // Case for special conversation
                if (confirm) {
                    if (!getCurrentConversation().isContinuing&&isYes) {
                        // flag for special event
                        happenSpecialEvent = true;
                    } 
                    endConversation = true;
                }
            } else {
                // case of normal
                if (confirm) {
                    while (currentConversationIndex < normalConversations.Count) {
                        currentConversationIndex++;
                        if (currentConversationIndex >= normalConversations.Count) break;
                        if ( ((int)GameManager.instance.stageManager.currentStage == (int)getCurrentConversation().stage) 
                            || (getCurrentConversation().stage == dialogStage.any) ) {
                            break;
                        }
                    }
                }
                // Mechanism: end the conversation when the index is out of range
                if (currentConversationIndex >= normalConversations.Count) {
                    endConversation = true;
                }
            }
        }

        public void ResetConversation() {
            endConversation = false;
            currentConversationIndex = 0;
            happenSpecialEvent = false;
        }

    }
}
