using UnityEngine;

public abstract class StaticInstanceMono<T> : MonoBehaviour
                                        where T : MonoBehaviour {
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;
    protected virtual void OnApplicationQuit() {
        Instance = null;
        Destroy(gameObject);
    }
}

public abstract class TempSingletonMono<T> : StaticInstanceMono<T>
                                            where T : MonoBehaviour {
    protected override void Awake() {
        if ( Instance!=null ) {
            Destroy(gameObject);
            return;
        }
        base.Awake();
    }
}

public abstract class SingletonMono<T> : TempSingletonMono<T>
                                        where T : MonoBehaviour {
    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}

