using System.Collections.Generic;
using UnityEngine;

public class InteractController : CommonBehaviourBase {
    protected readonly LayerMask m_interactLayer;
    protected readonly Collider2D m_interactCollider2D;
    protected List<GameObject> m_reachableList = new();

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="interactLayer">the layer where all the 
    /// interactable items stay in</param>
    /// <param name="interactCollider2D">the range that the entity
    /// can reach</param>
    public InteractController(GameObject gameObject, 
            LayerMask interactLayer, Collider2D interactCollider2D)
    : base(gameObject) {
        m_interactLayer = interactLayer;
        m_interactCollider2D = interactCollider2D;
    }

    
}
