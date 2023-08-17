using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Start() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    // When loading scene, save all gameobject states
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        OnSave?.Invoke();
    }

    // When last scene is unloaded, load all gameobject states
    private void OnSceneUnloaded(Scene scene) {
        OnLoad?.Invoke();
    }

    // Events for saving and loading gameobject states
    // All gameobjects that should be saved and loaded should subscribe to these events
    // !!! SUBSCRIBE in Start() instead of Awake() !!!
    // because Awake() will be called before OnSceneLoaded(), and call OnSave() before all gameobjects are loaded
    public static event Action OnSave;
    public static event Action OnLoad;

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
            // TODO: useful?
            // Debug.LogWarning("GameobjectStateManager: "+name+" is not in the dictionary");
            return null;
        }
    }

    public void LogList() {
        Debug.Log("GameobjectStateManager: LogList");
        foreach (KeyValuePair<string, GameObjectState> item in gameobjectStates) {
            Debug.Log("     " + item.Key);
        }
    }

}

// interface for gameobject state
public interface IGameObjectStateHandler {
    // For OnSave event
    public void SavetoManager();
    // For OnLoad event
    public void LoadfromManager();
}
