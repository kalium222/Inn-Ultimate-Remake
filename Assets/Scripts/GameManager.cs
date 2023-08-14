using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public DoorManager doorManager;
    public GameStageManager gameStageManager;
    public CollectableManager collectableManager = new CollectableManager();
    public OpenableManager openableManager = new OpenableManager();

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            Debug.Log("Duplicate Game Manager self-destructing!");
        }
        doorManager = GetComponent<DoorManager>();
        gameStageManager = GetComponent<GameStageManager>();
    }


    // A public method to load a new scene asynchronously
    public void ChangeScene(string targetSceneName, string targetPortalName) {
        StartCoroutine(LoadSceneAsyncCoroutine(targetSceneName, targetPortalName));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string targetSceneName, string targetPortalName) {
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
        // Load the collectables to the right status
        PreloadCollectables(SceneManager.GetActiveScene());
    }

    // A public method to load the right status to all the collectables
    public void PreloadCollectables(Scene scene) {
        GameObject room = GameObject.Find(SceneManager.GetActiveScene().name);
        foreach (CollectableInfo item in collectableManager.changedCollectableInfos) {
            if (item.scenename == scene.name && !item.collectable.GetComponent<Collectable>().isCollected) {
                item.collectable.gameObject.SetActive(true);
            } else {
                item.collectable.gameObject.SetActive(false);
            }
            Transform original = GetChildGameObject(room.transform, item.collectable.name);
            original?.gameObject.SetActive(false);
        }
    }

    // Tool function
    // Find a child game object by name recursively
    // return null if not found
    private Transform GetChildGameObject(Transform fromTransform, string childName) {
        for (int i=0; i<fromTransform.childCount; i++) {
            Transform child = fromTransform.GetChild(i);
            // Debug.Log(child.name);
            if (child.name == childName) {
                return child;
            } else {
                Transform result = GetChildGameObject(child, childName);
                if (result != null) {
                    return result;
                }
            }
        }
        return null;
    }

    // -------------Test functions
    void LogGameObject() {
        foreach (GameObject item in GameObject.FindObjectsOfType<GameObject>()) {
            Debug.Log(item.name);
        }
    }

    void TestRoomExchange() {
        // doorManager.SwapRoom("FirstFloorDoor", "SecondFloorDoor1");
        doorManager.SetDoortoRoom("FirstFloorDoor", "Room2Door");

    }

    void TestChangedCollectable() {
        foreach (CollectableInfo item in collectableManager.changedCollectableInfos) {
            Debug.Log(item.collectable.name);
        }
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
        if (Input.GetKeyDown(KeyCode.Y)) {
            TestRoomExchange();
        }
        // if (Input.GetKeyDown(KeyCode.J)) {
        //     doorManager.ShowAllDoors();
        // }
    }

    // ------------------------Sub classes in GameManager------------------------
}




// Sub classes in GameManager

public class CollectableInfo {
    public string scenename;
    public Transform transformInScene;
    public GameObject collectable;
    public CollectableInfo(string scenename, Transform transformInScene, GameObject collectable) {
        this.scenename = scenename;
        this.transformInScene = transformInScene;
        this.collectable = collectable;
    }
}

public class CollectableManager {

    public List<CollectableInfo> changedCollectableInfos = new List<CollectableInfo>(){};

    public void setChangedCollectableInfos(CollectableInfo collectableInfo) {
        foreach (CollectableInfo item in changedCollectableInfos)
        {
            if (collectableInfo.collectable == item.collectable) {
                item.scenename = collectableInfo.scenename;
            }
        }
    }
}

public class OpenableManager {
    public struct OpenableInfo {
        public bool isOpened;
        public bool isEmpty;
        public OpenableInfo(bool isOpened, bool isEmpty) {
            this.isOpened = isOpened;
            this.isEmpty = isEmpty;
        }
    }

    public Dictionary<string, OpenableInfo> openableInfos;

    public OpenableManager() {
        openableInfos = new Dictionary<string, OpenableInfo>(){
            {"Room1Bed", new OpenableInfo(true, true)},
            {"Room4Bed", new OpenableInfo(false, false)},
            {"Refrigerator", new OpenableInfo(false, false)}
        };
    }

    public void setValue(string name, bool isOpened, bool isEmpty) {
        if (!openableInfos.ContainsKey(name)) {
            Debug.LogError("Openable not found!");
            return;
        }            
        openableInfos[name] = new OpenableInfo(isOpened, isEmpty);
    }
}
