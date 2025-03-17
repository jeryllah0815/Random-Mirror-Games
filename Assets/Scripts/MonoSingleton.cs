using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)this;

            transform.parent = null;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning($"Duplicate instance warning: Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        // Clear the instance when the object is destroyed
        if (Instance == this)
        {
            Instance = null;
        }
    }
}