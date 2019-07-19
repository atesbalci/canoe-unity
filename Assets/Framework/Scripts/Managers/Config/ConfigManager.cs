using System.Collections;
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
            StartCoroutine(ChangeFramerate());
        }
        
        private IEnumerator ChangeFramerate() {
            yield return new WaitForSeconds(1);
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
    }
}