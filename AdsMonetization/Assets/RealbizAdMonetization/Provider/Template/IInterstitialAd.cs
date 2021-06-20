using System;

namespace RealbizGames.Ads
{
    public interface IInterstitialAd
    {
        void Init();
        void ShowInterstitial(InterstitialDTO dto);
        bool isAvailableAd();
        void Update();
        void Destroy();

        DateTime lastInterstitialAdClosedTime { get; }
    }
}