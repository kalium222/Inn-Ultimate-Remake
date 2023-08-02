using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PortalTargetMap portalTargetMap = new PortalTargetMap();
    public CollectableManager collectableManager = new CollectableManager();
    public OpenableManager openableManager = new OpenableManager();
    public StageManager stageManager = new StageManager();

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            Debug.Log("Duplicate Game Manager self-destructing!");
        }
    }

    // A public method to load a new scene asynchronously
    public void LoadSceneAsync(string targetSceneName) {
        StartCoroutine(LoadSceneAsyncCoroutine(targetSceneName));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string targetSceneName) {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name != targetSceneName) {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Additive);
            while (!asyncLoad.isDone) {
                yield return null;
            }
            SceneManager.UnloadSceneAsync(currentScene);
        }
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
            if (original != null) {
                original.gameObject.SetActive(false);
            }
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
        portalTargetMap.SwapRoom("FirstFloorDoor", "SecondFloorDoor1");
    }

    void TestChangedCollectable() {
        foreach (CollectableInfo item in collectableManager.changedCollectableInfos) {
            Debug.Log(item.collectable.name);
        }
    }

    void TestGameStage() {
        stageManager.currentStage++;
    }
    // ------------------------

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            TestGameStage();
            Debug.Log("Current stage: " + stageManager.currentStage);
        }
        if (Input.GetKeyDown(KeyCode.Y)) {
            TestRoomExchange();
        }
    }

    // ------------------------Sub classes in GameManager------------------------

    // Game Stage
    public class StageManager {
        public enum Stage {
            stage1, stage2
        }
        public Stage currentStage = Stage.stage1;
    }
}




// Sub classes in GameManager

// A Dictionary to store the maps of portals and the targets
public class PortalTargetMap {
    Dictionary<string, Target> PortalTargetTable = new Dictionary<string, Target>(){
        {"FirstFloorDoor", new Target("Room0", "Room0Door")},
        {"SecondFloorDoor1", new Target("Room1", "Room1Door")},
        {"SecondFloorDoor2", new Target("Room2", "Room2Door")},
        {"SecondFloorDoor3", new Target("Room3", "Room3Door")},
        {"SecondFloorDoor4", new Target("Room4", "Room4Door")},
        {"BasementDoor", new Target("Room5", "Room5Door")},

        {"Room0Door", new Target("FirstFloor", "FirstFloorDoor")},
        {"Room1Door", new Target("SecondFloor", "SecondFloorDoor1")},
        {"Room2Door", new Target("SecondFloor", "SecondFloorDoor2")},
        {"Room3Door", new Target("SecondFloor", "SecondFloorDoor3")},
        {"Room4Door", new Target("SecondFloor", "SecondFloorDoor4")},
        {"Room5Door", new Target("Basement", "BasementDoor")},
    };

    // Swap the target of two doors
    // Also swap the target of the doors in the target room
    // To make it seemed to change the space of rooms
    public void SwapRoom(string door1, string door2) {
        string scene1 = "", scene2 = "";
        foreach (KeyValuePair<string, Target> vertex in PortalTargetTable) {
            if (vertex.Value.TargetDoorName == door1) {
                scene1 = vertex.Value.TargetSceneName;
            } else if (vertex.Value.TargetDoorName == door2) {
                scene2 = vertex.Value.TargetSceneName;
            }
        }
        if (scene1 == "" || scene2 == "") {
            Debug.LogError("Door not found!");
            return;
        }
        Target originalTarget1 = PortalTargetTable[door1];
        Target originalTarget2 = PortalTargetTable[door2];
        PortalTargetTable[door1] = originalTarget2;
        PortalTargetTable[door2] = originalTarget1;
        
        PortalTargetTable[originalTarget1.TargetDoorName] = new Target(scene2, door2);
        PortalTargetTable[originalTarget2.TargetDoorName] = new Target(scene1, door1);
    }

    public string GetTargetScene(string doorName) {
        return PortalTargetTable[doorName].TargetSceneName;
    }

    public string GetTargetDoor(string doorName) {
        return PortalTargetTable[doorName].TargetDoorName;
    }
}

// Not good?
public class Target {
    public string TargetSceneName;
    public string TargetDoorName;
    public Target(string targetSceneName, string targetDoorName) {
        TargetSceneName = targetSceneName;
        TargetDoorName = targetDoorName;
    }
}

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
