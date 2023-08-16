using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manage the state of gameobjects that should be saved and loaded
// when scene is changed or game is saved and loaded
public class GameObjectStateManager : MonoBehaviour
{
    // base class for all gameobject states
    public class GameObjectState {
        public Transform transform;
    }

    // a dictionary of gameobject states by object name
    private Dictionary<string, GameObjectState> gameobjectStates = new Dictionary<string, GameObjectState>();

    public void Add(string name, GameObjectState gameobjectState) {
        if (gameobjectStates.ContainsKey(name)) {
            gameobjectStates.Remove(name);
        }
        gameobjectStates.Add(name, gameobjectState);
    }

    public void Remove(string name) {
        gameobjectStates.Remove(name);
    }

    public bool Contains(string name) {
        return gameobjectStates.ContainsKey(name);
    }

    public GameObjectState Get(string name) {
        if (gameobjectStates.ContainsKey(name)) {
            return gameobjectStates[name];
        } else {
            Debug.LogWarning("GameobjectStateManager: "+name+" is not in the dictionary");
            return null;
        }
    }

}

// interface for gameobject state
public interface IGameObjectStateHandler {
    public void SavetoManager();
    public void LoadfromManager();
}
