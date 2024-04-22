using UnityEngine;

public abstract class CommonBehaviourBase
{
    private readonly GameObject gameObject;
    public CommonBehaviourBase(GameObject gameObject) {
        this.gameObject = gameObject;
    }
}

