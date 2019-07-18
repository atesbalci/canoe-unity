using DG.Tweening;
using UnityEngine;

namespace Framework.Scripts.Managers.Config
{
    public class ConfigManager : BaseManager
    {
        protected override void Awake()
        {
            base.Awake();

            Application.targetFrameRate = 60;
            DOTween.Init(useSafeMode: false);
        }
    }
}