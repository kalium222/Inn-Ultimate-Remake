using UnityEngine;

public abstract class GameItemBase {
    protected GameItemData data;
    protected abstract void SaveData();
    protected abstract void LoadData();
}

public class CollectableItem {}

