using UnityEngine;

namespace Framework.Scripts
{
    public abstract class BaseScreen<T> : MonoBehaviour where T : BaseScreenResources
    {
        public T Resources { get; private set; }

        protected virtual void Awake()
        {
            Resources = FindObjectOfType<T>();
        }
    }
}