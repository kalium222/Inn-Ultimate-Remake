using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBookshelfUI : MonoBehaviour
{
    private List<GameObject> DoorPositions = new List<GameObject>();
    private List<GameObject> RoomBooks = new List<GameObject>();
    private const string booksName = "Books";
    private const string positionsName = "Positions";
    private DoorManager doorManager;

    [SerializeField]
    public const float attaching_distance = 100f;

    // For bookshelf
    // Activate the UI of the whole portal controller
    public void AccessBookshelf() {
        gameObject.SetActive(true);
    }

    // TODO: 
    private void Update() {
        if (gameObject.activeSelf && Input.GetKeyDown(KeyCode.Space)) {
            gameObject.SetActive(false);
        }
    }

    private void Start() {
        doorManager = GameManager.instance.doorManager;
        LoadBooksPositions();
        LoadFromPortalManager();
        BookUI.OnBookEndDrag += OnBookEndDrag;
    }

    private void OnDestroy() {
        // Unregister the event because it is static
        BookUI.OnBookEndDrag -= OnBookEndDrag;
    }

    private void LoadBooksPositions() {
        Transform books = transform.Find(booksName);
        Transform positions = transform.Find(positionsName);
        foreach (Transform book in books) {
            if (book.GetComponent<RectTransform>() == null) {
                throw new System.Exception("Book lacks RectTransform");
            }
            RoomBooks.Add(book.gameObject);
        }
        foreach (Transform position in positions) {
            if (position.GetComponent<RectTransform>() == null) {
                throw new System.Exception("Position lacks RectTransform");
            }
            DoorPositions.Add(position.gameObject);
        }
        if ((RoomBooks.Count != 6) || (DoorPositions.Count!=6)) {
            throw new System.Exception("Bookshelf and Positions not correct");
        }
    }

    // Load the correct positions of books from PortalManager
    private void LoadFromPortalManager() {
        foreach (GameObject book in RoomBooks) {
            string targetRoom = doorManager.GetTargetDoor(book.name);
            foreach (GameObject position in DoorPositions) {
                if (position.name == targetRoom) {
                    book.GetComponent<RectTransform>().anchoredPosition = position.GetComponent<RectTransform>().anchoredPosition;
                    break;
                }
            }
        }
    }

    // When ending draging the UI, invoking the event
    // set the position of book to the nearest position of room
    // update the doormanager
    private void OnBookEndDrag(GameObject book) {
        foreach (GameObject position in DoorPositions) {
            if (Vector2.Distance(book.GetComponent<RectTransform>().anchoredPosition, position.GetComponent<RectTransform>().anchoredPosition) < attaching_distance) {
                doorManager.SetDoortoRoom(position.name, book.name);
                break;
            }
        }
        LoadFromPortalManager();
    }
}
