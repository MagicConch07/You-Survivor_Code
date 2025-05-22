using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    private static bool _isDestroyed = false;

    public static T Instance
    {
        get
        {
            if (_isDestroyed)
            {
                _instance = null;
            }

            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();
                if (_instance == null)
                {
                    Debug.LogError($"{typeof(T).Name} singleton is not exist");
                }
                else
                {
                    _isDestroyed = false;
                }
            }

            return _instance;
        }
    }

    public void OnDestroy()
    {
        _isDestroyed = true;
    }
}