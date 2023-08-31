using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    // A class for storing the mapping of doors
    public class Portal {
        public string SceneName;
        public string DoorName;
        public Portal(string sceneName, string doorName) {
            SceneName = sceneName;
            DoorName = doorName;
        }
        public Portal(Portal that) {
            SceneName = that.SceneName;
            DoorName = that.DoorName;
        }
    }

    Dictionary<Portal, Portal> PortalPairTable;

    // The initial mapping of doors, used for reseting every new round
    readonly Dictionary<Portal, Portal> InitialPortalPairTable = new Dictionary<Portal, Portal>() {
        {new Portal("FirstFloor", "FirstFloorDoor"), new Portal("Room0", "Room0Door")},
        {new Portal("SecondFloor", "SecondFloorDoor1"), new Portal("Room1", "Room1Door")},
        {new Portal("SecondFloor", "SecondFloorDoor2"), new Portal("Room2", "Room2Door")},
        {new Portal("SecondFloor", "SecondFloorDoor3"), new Portal("Room3", "Room3Door")},
        {new Portal("SecondFloor", "SecondFloorDoor4"), new Portal("Room4", "Room4Door")},
        {new Portal("Basement", "BasementDoor"), new Portal("Room5", "Room5Door")}
    };
    
    public void ResetMapping() {
        PortalPairTable = new Dictionary<Portal, Portal>(InitialPortalPairTable);
    }

    private void Awake() {
        ResetMapping();
    }

    // Swap the target of two doors
    // To make it seemed to change the space of rooms
    public void SwapRoom(string door1, string door2) {
        Portal target1 = GetTargetPortal(door1);
        PortalPairTable[GetPortal(door1)] = GetTargetPortal(door2);
        PortalPairTable[GetPortal(door2)] = target1;
    }

    // set the target of a door to a room
    // ajust the remaining mapping
    public void SetDoortoRoom(string door, string room) {
        Portal originaltarget = GetTargetPortal(door);
        Portal originalinitial = GetTargetPortal(room);
        if (PortalPairTable.ContainsKey(GetPortal(door))) {
            PortalPairTable[GetPortal(door)] = GetPortal(room);
        } else {
            throw new KeyNotFoundException("Door " + door + " not found in doors");
        }
        if (PortalPairTable.ContainsKey(originalinitial)) {
            PortalPairTable[originalinitial] = originaltarget;
        } else {
            throw new KeyNotFoundException("Door " + originalinitial + " not found in doors");
        }
    }

    public Portal GetPortal(string doorName) {
        foreach (KeyValuePair<Portal, Portal> pair in PortalPairTable) {
            if (pair.Key.DoorName == doorName) return pair.Key;
            if (pair.Value.DoorName == doorName) return pair.Value;
        }
        throw new KeyNotFoundException("Door " + doorName + " not found in doorRoomBijection");
    }

    public Portal GetTargetPortal(string doorName) {
        foreach (KeyValuePair<Portal, Portal> pair in PortalPairTable) {
            if (pair.Key.DoorName == doorName) return pair.Value;
            if (pair.Value.DoorName == doorName) return pair.Key;
        }
        throw new KeyNotFoundException("Door " + doorName + " not found in doorRoomBijection");
    }

    public string GetTargetScene(string doorName) {
        return GetTargetPortal(doorName).SceneName;
    }

    public string GetTargetDoor(string doorName) {
        return GetTargetPortal(doorName).DoorName;
    }

    public void ShowAllDoors() {
        foreach (KeyValuePair<Portal, Portal> pair in PortalPairTable) {
            Debug.Log(pair.Key.DoorName + " -> " + pair.Value.DoorName);
        }
    }

}