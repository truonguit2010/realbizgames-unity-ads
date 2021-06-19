using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RealbizGames.Ads
{
    public interface IRealAdMonetization
    {
        void ShowRewardedAd(RewardedAdDTO dto);

        void ShowInterstitialAd();

        void ShowBanner();
    }
}