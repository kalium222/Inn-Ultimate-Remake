using UnityEngine;

public class TeleportBehaviour : CommonBehaviourBase {

    public TeleportBehaviour(
        GameObject gameObject,
        Vector3 targetPosition,
        Room targetRoom
    )
    : base(gameObject) {
        this.targetPosition = targetPosition;
        this.targetRoom = targetRoom;
    }

    public Vector3 targetPosition;
    public Room targetRoom;

    // TODO: get the instance
    private RoomsManager m_roomManager;

    public void Teleport(Transform transform) {
        transform.position = targetPosition;
        m_roomManager.ChangerRoomTo(targetRoom);
    }
}

