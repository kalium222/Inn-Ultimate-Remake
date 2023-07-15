using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string TargetScene = "SecondFloor";
    public string TargetPortal = "SecondFloorDownStair";

    // Change the scene and teleport the player to the right place
    // Do this in a coroutine
    public void Teleport(GameObject teleportObject) {
        StartCoroutine(LoadSceneAsync(teleportObject));
        // GameManager.instance.LoadSceneAsync(TargetScene);
        // Portal target = GameObject.Find(TargetPortal).GetComponent<Portal>();
        // if (target.name == TargetPortal) {
        //     teleportObject.transform.position = target.transform.position;
        // }
    }

    private IEnumerator LoadSceneAsync(GameObject teleportObject)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name != TargetScene) {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(TargetScene, LoadSceneMode.Additive);
            while (!asyncLoad.isDone) {
                yield return null;
            }
            SceneManager.UnloadSceneAsync(currentScene);
        }
        
        // The new scene is now loaded, and the old one is unloaded
        // Get to the right place in the Scene
        Portal target = GameObject.Find(TargetPortal).GetComponent<Portal>();
        if (target.name == TargetPortal) {
            teleportObject.transform.position = target.transform.position;
        }
        // Load the collectables to the right status
        GameManager.instance.PreloadCollectables(SceneManager.GetActiveScene());
    }

    // Sync load scene
    private void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    
}
