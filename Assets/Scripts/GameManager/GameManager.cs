using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : SingletonMono<GameManager>
{
    public Control Control { get; private set; }
    [HideInInspector]
    public DoorManager doorManager;
    [HideInInspector]
    public GameStageManager gameStageManager;
    [HideInInspector]
    public GameObjectStateManager gameObjectStateManager;

    protected override void Awake() {
        base.Awake();
        Control = new();
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

    private void OnEnable()
    {
        Control.Enable();
    }

    // End the current round and start a new one
    public void EndingRound() {
        StartCoroutine(EndingRoundCoroutine());
    }
    public IEnumerator EndingRoundCoroutine() {
        GameUIManager.instance.ClearDialogBox();
        HeroController.Instance.CanMove = false;
        HeroInteraction.instance.CanInteract = false;
        HeroController.Instance.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(3f);
        yield return GameUIManager.instance.CurtainFadingIn(true);
        while (!Input.anyKeyDown) {
            yield return null;
        }
        ResetRound();
        yield return LoadSceneAsyncCoroutine("Room1", "Room1Door", false);
        HeroController.Instance.GetComponent<SpriteRenderer>().enabled = true;
        yield return GameUIManager.instance.CurtainFadingOut();
        HeroController.Instance.CanMove = true;
        HeroInteraction.instance.CanInteract = true;
        
    }

    public void ResetRound() {
        // Reset the mapping of doors in the Room0.
        doorManager.ResetMapping();
        // Reset all gameobject states
        gameObjectStateManager.Clear();
        // Clear the subobjects of the GameManager
        DestroyAllSubObjects();
        // TODO: Game stage
    }

    private void DestroyAllSubObjects() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    // A public method to load a new scene asynchronously
    public void ChangeScene(string targetSceneName, string targetPortalName) {
        StartCoroutine(LoadSceneAsyncCoroutine(targetSceneName, targetPortalName));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string targetSceneName, string targetPortalName, bool isFading = true) {
        // First fade in the curtain
        if (isFading) yield return GameUIManager.instance.CurtainFadingIn();
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
            HeroController.Instance.transform.position = target.transform.position;
        }
        // Then fade out the curtain
        if (isFading) yield return GameUIManager.instance.CurtainFadingOut();
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