using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dialoues
{
    // Conversation is a list of dialogues. Once the player talked the the talkable
    // object, the dialogues will be displayed in the dialog box one by one.
    [CreateAssetMenu(menuName = "NPC/Conversation")]
    [Serializable]
    public class Conversation : ScriptableObject, IEnumerable<Dialogue> {
        public List<Dialogue> dialogues = new List<Dialogue>();

        public void Add(Dialogue dialogue) {
            dialogues.Add(dialogue);
        }

        public void Remove(Dialogue dialogue) {
            dialogues.Remove(dialogue);
        }

        public void RemoveAll() {
            dialogues.Clear();
        }

        public IEnumerator<Dialogue> GetEnumerator() {
            return dialogues.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator(){
            return GetEnumerator();
        }
    }

    // One dialogue that displayed in the dialog box at one time
    [Serializable]
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
}




