using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBookshelfUI : MonoBehaviour
{
    private List<GameObject> DoorPositions;
    private List<GameObject> RoomBooks;
    public void AccessBookshelf() {
        gameObject.SetActive(true);
    }

}
