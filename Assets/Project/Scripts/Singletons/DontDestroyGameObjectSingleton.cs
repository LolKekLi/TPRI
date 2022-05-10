using UnityEngine;

namespace Project
{
    public abstract class DontDestroyGameObjectSingleton<T> : GameObjectSingleton<T> where T : Component
    {
        
    }
}