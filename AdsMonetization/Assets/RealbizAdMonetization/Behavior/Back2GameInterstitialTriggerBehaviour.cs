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
                InterstitialDTO dto = new InterstitialDTO("back2game");
                RealAdMonetizationImpl.DefaultInstance.ShowAppOpenAd(dto);
            }
        }
    }
}