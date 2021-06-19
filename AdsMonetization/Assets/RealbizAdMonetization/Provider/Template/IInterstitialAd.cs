using System;

namespace RealbizGames.Ads
{
    public interface IInterstitialAd
    {
        void Init();
        void ShowInterstitial();
        bool isAvailableAd();
        void Update();
        void Destroy();

        DateTime lastInterstitialAdClosedTime { get; }
    }
}