using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector]
    public DoorManager doorManager;
    [HideInInspector]
    public GameStageManager gameStageManager;
    [HideInInspector]
    public GameObjectStateManager gameObjectStateManager;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        doorManager = GetComponent<DoorManager>();
        if (doorManager == null) {
            throw new System.Exception("DoorManager not found!");
        }
        gameStageManager = GetComponent<GameStageManager>();
        if (gameStageManager == null) {
            throw new System.Exception("GameStageManager not found!");
        }
        gameObjectStateManager = GetComponent<GameObjectStateManager>();
        if (gameObjectStateManager == null) {
            throw new System.Exception("GameObjectStateManager not found!");
        }
    }

    // A public method to load a new scene asynchronously
    public void ChangeScene(string targetSceneName, string targetPortalName) {
        StartCoroutine(LoadSceneAsyncCoroutine(targetSceneName, targetPortalName));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string targetSceneName, string targetPortalName) {
        // First fade in the curtain
        yield return GameUIManager.instance.CurtainFadingIn();
        // Then load the new scene
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name != targetSceneName) {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Additive);
            while (!asyncLoad.isDone) {
                yield return null;
            }
            SceneManager.UnloadSceneAsync(currentScene);
        }
        // The new scene is now loaded, and the old one is unloaded
        // Get to the right place in the Scene
        Portal target = GameObject.Find(targetPortalName).GetComponent<Portal>();
        if (target.name == targetPortalName) {
            HeroController.instance.transform.position = target.transform.position;
        }
        // Then fade out the curtain
        yield return GameUIManager.instance.CurtainFadingOut();
    }

    // -------------Test functions
    void LogGameObject() {
        foreach (GameObject item in GameObject.FindObjectsOfType<GameObject>()) {
            Debug.Log(item.name);
        }
    }

    void TestRoomExchange() {
        doorManager.SetDoortoRoom("FirstFloorDoor", "Room2Door");

    }

    void TestGameStage() {
        gameStageManager.Next();
        Debug.Log("Current stage: " + gameStageManager.CurrentStage);
    }
    // ------------------------

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            TestGameStage();
        }
        // if (Input.GetKeyDown(KeyCode.Y)) {
        //     gameObjectStateManager.LogList();
        // }
        // if (Input.GetKeyDown(KeyCode.J)) {
        //     doorManager.ShowAllDoors();
        // }
    }

}