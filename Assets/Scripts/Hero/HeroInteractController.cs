using System;
using System.Collections.Generic;
using Codice.CM.Client.Differences.Graphic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player should be able to press a key
/// to let the hero interact with one of the 
/// interactable items in certain range
/// described by a trigger collider of
/// interactionlayer. Also, the player can
/// press a key to switch the selected object.
/// </summary>
public class HeroInteractController : CommonBehaviourBase {
    private readonly LayerMask m_interactLayer;
    private readonly Collider2D m_interactCollider2D;
    private readonly List<Collider2D> m_reachableList = new();
    private int m_selectedIndex = -1;
    public Collider2D Selected {
        get {
            if (m_reachableList.Count==0) {
                // TODO: throw a proper exception
                throw new Exception("can not reach anything");
            }
            return m_reachableList[m_selectedIndex%m_reachableList.Count];
        }
    }

    public event Action OnReachableChanged;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="interactLayer">the layer where all the 
    /// interactable items stay in</param>
    /// <param name="interactCollider2D">the range that the entity
    /// can reach</param>
    public HeroInteractController(GameObject gameObject, 
            LayerMask interactLayer, Collider2D interactCollider2D)
    : base(gameObject) {
        m_interactLayer = interactLayer;
        m_interactCollider2D = interactCollider2D;
    }

    public void CheckAllContacted() {
        ContactFilter2D filter2D = new()
        {
            layerMask = m_interactLayer
        };
        m_interactCollider2D.OverlapCollider(filter2D, m_reachableList);

        #if SCRIPT_TEST
        LogAllReachable();
        #endif
    }

    /// <summary>
    /// Should be called in OnColliderEnter()
    /// add the collider into the List.
    /// </summary>
    /// <param name="other"></param>
    public void OnReachItem(Collider2D other) {
        if (!m_reachableList.Contains(other)) {
            m_reachableList.Add(other);
            m_selectedIndex = (m_reachableList.Count - 1) % m_reachableList.Count;
        }
        // TODO: highlight things
        #if SCRIPT_TEST
        Debug.Log("reach " + other.name);
        #endif
    }

    /// <summary>
    /// Should be called in OnColliderExit()
    /// remove the collider from the List
    /// </summary>
    /// <param name="other"></param>
    public void OnLeaveItem(Collider2D other) {
        m_reachableList.Remove(other);
        // TODO: highlight things
        #if SCRIPT_TEST
        Debug.Log("leave " + other.name);
        #endif
    }

    public void OnNextSelected(InputAction.CallbackContext _) {
        m_selectedIndex = (m_selectedIndex + 1) % m_reachableList.Count;
        #if SCRIPT_TEST
        Debug.Log("select " + Selected.name);
        #endif
    }

    #if SCRIPT_TEST
    private void LogAllReachable() {
        foreach (var collider in m_reachableList)
            Debug.Log(collider.name);
    }
    #endif
}

public interface IHighlightable {
    public void EnableHighlight();
    public void DisableHighlight();
}

