using System.Collections.Generic;
using UnityEngine;

// Handle the cameras to the selected room
// Room is the gameobject under the Rooms GameObject
public class RoomsManager {
    public readonly List<GameObject> RoomsList = new();
    public Camera activatedCamera {
        get {
            if ( activatedCamera==null) {
                // TODO: 
                throw new System.Exception("FUCK");
            }
            return activatedCamera;
        }
        set {
            activatedCamera = value;
        }
    }

    public RoomsManager() {
        // TODO: init the RoomsList
    }

    public void FocusOnRoom(GameObject targetRoom) {
#if SCRIPT_TEST
        Debug.Log("Changing Room to " + targetRoom.name);
#endif
        foreach (var room in RoomsList) {
            if (room==targetRoom) {
                // TODO: 
            } else {
                // TODO: 
            }
        }
    }
}

// Handle the mappings of doors and rooms controlled by the 
// bookshelf mechanism
public class RoomsMappingManger {
    // TODO: 
}

