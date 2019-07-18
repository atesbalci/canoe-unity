using UnityEngine;

namespace Framework.Scripts
{
    public abstract class BaseManager : MonoBehaviour
    {
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}