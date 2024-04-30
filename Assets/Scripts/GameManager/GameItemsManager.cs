using System;
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
public class GameItemData {
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
public class CollectableItemData : GameItemData {
    public Transform transform;
    public Scene scene;
}

public class ContainerItemData : GameItemData {
    public bool isOpen = false;
    public int capacity = 1;
    public int remain;
}

public class GameItemsManager {

    // TODO: choose a proper container to hold the data
    // data? gameobject itself?
    private readonly List<GameItemData> ItemDataTable = new();

    // events when items should do something to 
    // save and load data
    public static event Action OnSaveGameItemData;
    public static event Action OnLoadGameItemData;
}

