using UnityEngine;

namespace RealbizGames.Ads
{
    [DisallowMultipleComponent]
    public class Back2GameInterstitialTriggerBehaviour : MonoBehaviour
    {
        private void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus)
            {
                RealAdMonetizationImpl.DefaultInstance.OnApplicationResume();
            }
        }
    }
}