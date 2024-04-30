using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO: Item entity management

/// <summary>
/// meta data of an item
/// when changing the scene, we need two stages to correctly to
/// laod the meta data of all the relative items.
/// First all the existing derived items laod the data from ItemManager,
/// then ItemManager instantiate the rest items.
/// </summary>
public class ItemData {
    // HACK: : better uid?
    /// <summary>
    /// use name as uid
    ///</summary>
    public string name;
    public GameObject prefeb;
}

/// <summary>
/// Position and Scene
/// </summary>
public class CollectableItemData : ItemData {
    public Transform transform;
    public Scene scene;
}

public class ContainerItemData : ItemData {
    public bool isOpen = false;
    public int capacity = 1;
    public int remain;
}

public class ItemsManager {
    private readonly List<ItemData> ItemDataTable = new();
}

public interface IItemDataLoad {
    public ItemData GetItemDate();
    public void SetItemData(ItemData itemdata);
}

