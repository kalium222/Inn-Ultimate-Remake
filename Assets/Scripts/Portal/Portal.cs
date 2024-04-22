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
    public void Teleport() {
        GameManager.Instance.ChangeScene(TargetScene, TargetPortal);
    }
}
