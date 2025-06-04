using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static object _lock = new object();
    private static bool _shuttingDown = false;

    public static T Instance
    {
        get
        {
            if (_shuttingDown)
            {
                Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed. Returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    // Cherche une instance existante
                    _instance = (T)FindFirstObjectByType(typeof(T));

                    // Si aucune, on en crée une nouvelle
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).Name + " (Singleton)");
                        _instance = singletonObject.AddComponent<T>();
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void OnApplicationQuit()
    {
        _shuttingDown = true;
    }

    protected virtual void OnDestroy()
    {
        _shuttingDown = true;
    }
}