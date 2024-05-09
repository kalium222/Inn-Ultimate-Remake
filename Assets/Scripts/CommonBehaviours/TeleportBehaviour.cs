using UnityEngine;

public class TeleportBehaviour : CommonBehaviourBase {

    public TeleportBehaviour(
        GameObject gameObject,
        Vector3 targetPosition,
        GameObject targetRoom
    )
    : base(gameObject) {
        this.targetPosition = targetPosition;
        this.targetRoom = targetRoom;
        m_roomManager = GameSceneManager.Instance.roomsManager;
    }

    public Vector3 targetPosition;
    public GameObject targetRoom;

    // TODO: get the instance
    private RoomsManager m_roomManager;

    public void Teleport(Transform transform) {
        transform.position = targetPosition;
        m_roomManager.FocusOnRoom(targetRoom);
    }
}

