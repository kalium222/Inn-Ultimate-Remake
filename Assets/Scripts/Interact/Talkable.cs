using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // DialogManager. Contain the all the dialog and the logic of dialog and special event
    // all the talkable object should a manager derived from DialogManager
    protected DialogManager dialogManager;

    protected virtual void Start() {
        GameUIManager.instance.OnDecisionMade.AddListener(OnDecisionMade);
        leftKey = HeroInteraction.instance.previousItemKey;
        rightKey = HeroInteraction.instance.nextItemKey;
        confirmKey = HeroInteraction.instance.interactKey;
        DialogManagerInit();
    }

    private void OnDestroy() {
        GameUIManager.instance.OnDecisionMade.RemoveListener(OnDecisionMade);
    }

    private void OnDecisionMade(bool isYes) {
        this.isYes = isYes;
    }

    // set the dialogManager
    protected virtual void DialogManagerInit() {
        dialogManager = new DialogManager();
    }

    protected virtual void DoSpecialEvent() {
        Debug.Log("Special Event");
    }
    
    override public void Interact() {
        StartCoroutine(RunDialogCoroutine());
    }

    private IEnumerator RunDialogCoroutine() {
        // Start at next frame
        yield return null;
        // First disable all movement and interaction
        HeroController.instance.CanMove = false;
        HeroInteraction.instance.CanInteract = false;
        
        // run the dialogs
        List<Dialogue> currentConversation = dialogManager.GetCurrentConversation();
        foreach (Dialogue dialogue in currentConversation) {
            yield return GameUIManager.instance.DialogCoroutine(dialogue.text, dialogue.isContinuing);
        }
        // Then do the special event
        if (dialogManager.IsSpecialConversation()&&isYes) DoSpecialEvent();

        // Then re-enable all movement and interaction
        HeroController.instance.CanMove = true;
        HeroInteraction.instance.CanInteract = true;
    }



    // ------------------------------Dialogue related classes-----------------------------

    // One dialogue that displayed in the dialog box at one time
    public class Dialogue {
        // Whether the dialog need player to make decision
        // otherwise, the the option UI will be only one "continue"
        public bool isContinuing;
        // the content text of this dialogue
        public string text;
        public Dialogue(bool isContinuing = true, string text = "Empty") {
            this.isContinuing = isContinuing;
            this.text = text;
        }
    }


    // Manage the dialogue
    public class DialogManager {
        // container for all the dialogues of this talkable object
        protected List<List<Dialogue>> normalConversations = new List<List<Dialogue>>();
        private List<Dialogue> specialConversation = new List<Dialogue>();

        public DialogManager() {
            string[] strings_1 = new string[]{"Test for normal conversation", "Test 1", "Test 2"};
            string[] strings_2 = new string[]{"Same as above", "Test 3", "Test 4"};
            string[] strings_3 = new string[]{"test for special conversation", "Test 5", "Test 6"};
            normalConversations.Add(new List<Dialogue>());
            normalConversations.Add(new List<Dialogue>());
            specialConversation = new List<Dialogue>();
            foreach (string s in strings_1) {
                normalConversations[0].Add(new Dialogue(true, s));
            }
            foreach (string s in strings_2) {
                normalConversations[1].Add(new Dialogue(true, s));
            }
            foreach (string s in strings_3) {
                specialConversation.Add(new Dialogue(true, s));
            }
        }

        // whether play special conversation according to the state of the manager
        // virtual for derived class
        virtual public bool IsSpecialConversation() {
            // according to the game stage
            return GameManager.instance.gameStageManager.CurrentStage == GameStageManager.Stage.Exploring;
        }

        // Manially initial functions
        public void SetSpecialDialogue(Dialogue dialogue) {
            specialConversation = new List<Dialogue>(){dialogue};
        }
        public void SetSpecialDialogue(List<Dialogue> conversation) {
            specialConversation = conversation;
        }
        public void SetNormalConversations(Dialogue dialogue) {
            normalConversations = new List<List<Dialogue>>(){new List<Dialogue>(){dialogue}};
        }
        public void SetNormalConversations(List<Dialogue> conversation) {
            normalConversations = new List<List<Dialogue>>(){conversation};
        }

        // Return the current conversation according
        // to the current state of the talkable object
        virtual public List<Dialogue> GetCurrentConversation() {
            if (IsSpecialConversation()) {
                return specialConversation;
            } else {
                if ((int)GameManager.instance.gameStageManager.CurrentStage<normalConversations.Count)
                    return normalConversations[(int)GameManager.instance.gameStageManager.CurrentStage];
                else
                    return normalConversations[0];
            }
        }

    }
}