using System;
using System.Collections.Generic;
using UnityEngine;

// TODO: Item entity management

/// <summary>
/// meta data of an item
/// when changing the scene, we need two stages to correctly to
/// laod the meta data of all the relative items.
/// First all the existing derived items laod the data from ItemManager,
/// then ItemManager instantiate the rest items.
/// </summary>
public abstract class GameItemData {
    // HACK: : better uid?
    /// <summary>
    /// use name as uid
    /// </summary>
    public string name;
}

public abstract class GameItemDataHandler {
    protected abstract void SaveToManager();
    protected abstract void LoadFromManager();
}

/// <summary>
/// Position and Scene
/// </summary>
public class CollectableItemData : GameItemData {
    public Transform transform;
    public string sceneName;
    public GameObject prefab;
}

public class ContainerItemData : GameItemData {
    public bool isOpen = false;
    public int capacity = 1;
    public int remain;
}


public class GameItemsManager {

    // TODO: choose a proper container to hold the data
    // data? gameobject itself?
    private readonly Dictionary<string, GameItemData> 
        ItemDataTable = new();

    // events when items should do something to 
    // save and load data
    public static event Action OnSaveGameItemData;
    public static event Action OnLoadGameItemData;

    public void SaveGameItemData() {
        OnSaveGameItemData?.Invoke();
    }

    public void LoadGameItemData() {
        OnLoadGameItemData?.Invoke();
    }
}

