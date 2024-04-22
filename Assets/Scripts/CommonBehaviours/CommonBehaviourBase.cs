using UnityEngine;

public abstract class CommonBehaviourBase {
    protected readonly GameObject gameObject;
    public CommonBehaviourBase(GameObject gameObject) {
        this.gameObject = gameObject;
    }
}
