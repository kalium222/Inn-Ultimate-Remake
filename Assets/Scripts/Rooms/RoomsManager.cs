using System.Collections.Generic;
using UnityEngine;

public enum Room {
    FirstFloor,
    SecondFloor,
}

public class RoomsManager {

    public readonly Dictionary<Room, GameObject> RoomMap = new();

    public void ChangerRoomTo(Room room) {
        // TODO:
    }
}

