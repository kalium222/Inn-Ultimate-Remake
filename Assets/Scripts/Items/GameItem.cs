using UnityEngine;

public abstract class GameItemBase {
    protected GameItemData data;
    // the callback function 
    // should subscribe the event of GameItemsManager
    protected abstract void SaveData();
    protected abstract void LoadData();
}

public class ContainerItem : GameItemBase {
    protected override void SaveData() {
        // TODO:
    }

    protected override void LoadData() {
        // TODO:
    }
}

public class CollectableItem : GameItemBase {
    protected override void SaveData() {
        // TODO:
    }

    protected override void LoadData() {
        // TODO:
    }
}

