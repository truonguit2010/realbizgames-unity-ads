
namespace RealbizGames.Ads
{
    public class BannerAdConfig
    {
        public float reloadIntervalSeconds = 30f;
        public bool isBottom = true;
        private bool _enable = true;
        public bool enable
        {
            get
            {
                if (isRemoveAd)
                {
                    return false;
                }
                else
                {
                    return _enable;
                }
            }
            set
            {
                _enable = value;
            }
        }
        public bool isRemoveAd = false;
    }
}