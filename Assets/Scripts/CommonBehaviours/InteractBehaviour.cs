using System;
using System.Collections.Generic;
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
public class InteractBehaviour : CommonBehaviourBase {

    private readonly List<Collider2D> m_reachableList = new();
    public int ReachableCount => m_reachableList.Count;
    private int m_selectedIndex = -1;
#nullable enable
    public Collider2D? Selected {
        get 
        {
            if (ReachableCount==0) return null;
            else return m_reachableList[m_selectedIndex%ReachableCount];
        }
    }
#nullable disable

    /// <summary>
    /// Invoked when reach or exit an item, or change the selected item
    /// mainly do the things related to highlighting
    /// </summary>
    public event Action<GameObject> RefreshHighlight;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="interactLayer">the layer where all the 
    /// interactable items stay in</param>
    /// <param name="interactCollider2D">the range that the entity
    /// can reach</param>
    public InteractBehaviour(GameObject gameObject)
    : base(gameObject) {}

    public void Interact(InputAction.CallbackContext _) {
        if ( !Selected ) {
#if SCRIPT_TEST_INTERACT
            Debug.Log("Selected nothing!");
#endif
            return;
        }
        bool res = Selected.TryGetComponent<ItemController>
            (out ItemController itemController);
        if (!res) {
            Utils.LackingPropertyException.NoComponent(
                itemController.ToString(), 
                Selected.name
            );
        }

        itemController.Interact();
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
            if (other.gameObject.TryGetComponent(out ItemController item)) 
                RefreshHighlight += item.OnRefreshHighlight;
        }
        // TODO: highlight things
        RefreshHighlight?.Invoke(Selected?.gameObject);
#if SCRIPT_TEST_INTERACT
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
        RefreshHighlight?.Invoke(Selected?.gameObject);
        if (other.gameObject.TryGetComponent(out ItemController item)) 
            RefreshHighlight -= item.OnRefreshHighlight;
#if SCRIPT_TEST_INTERACT
        Debug.Log("leave " + other.name);
#endif
    }

    public void OnNextSelected(InputAction.CallbackContext _) {
        m_selectedIndex++;
        RefreshHighlight?.Invoke(Selected?.gameObject);
#if SCRIPT_TEST_INTERACT
        string name;
        if (Selected) name = Selected.name;
        else name = "nothing";
        Debug.Log("select " + name);
        LogAllReachable();
#endif
    }

#if SCRIPT_TEST_INTERACT
    public void LogAllReachable() {
        foreach (var collider in m_reachableList)
            Debug.Log(collider.name);
    }
#endif

}

public interface IInteractable {
    public void Interact();
}

