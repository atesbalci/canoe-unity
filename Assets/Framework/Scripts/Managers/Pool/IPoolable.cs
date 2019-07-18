using UnityEngine;

namespace Framework.Scripts.Managers.Pool
{
    public interface IPoolable
    {
        void OnRelease();
        GameObject GetGameObject();
    }
}