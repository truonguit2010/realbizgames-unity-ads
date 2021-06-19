
namespace RealbizGames.Ads
{
    public interface IRewardedAd
    {
        void Init();
        void ShowRewardedAd(RewardedAdDTO dto);

        void Update();
        
        void Destroy();
    }
}