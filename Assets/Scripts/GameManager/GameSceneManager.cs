using UnityEngine;

/// <summary>
/// Handle all the mechanics within the GameScene
/// For instance, room changing, portal mapping
/// game round, game time...
/// </summary>
public class GameSceneManager : StaticInstanceMono<GameSceneManager> {

    [SerializeField]
    private GameObject m_rooms;
    [SerializeField]
    private GameObject m_initFocusedRoom;

    // All the functional modules
    public RoomsManager roomsManager { get; private set; }

    protected override void Awake() {
        base.Awake();
        // TODO: chekc the necessary GameObjects
        roomsManager = new();
    }

    private void Start() {
        roomsManager.FocusOnRoom(m_initFocusedRoom);
    }

}

