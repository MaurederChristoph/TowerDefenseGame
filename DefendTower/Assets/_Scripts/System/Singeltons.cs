using UnityEngine;

public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour {
    public static T Instance { get; set; }
    protected virtual void Awake() => Instance = this as T;
    protected virtual void OnApplicationQuit() {
        Instance = null;
        Destroy(gameObject);
    }
}

public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour {
    protected override void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        base.Awake();
    }


}

public class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour {
    protected override void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        base.Awake();
    }
}