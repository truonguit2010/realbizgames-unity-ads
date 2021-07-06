
namespace RealbizGames.Ads
{
    public class InterstitialAdConfig
    {
        public float reloadIntervalSeconds = 30;

        public float restrictIntervalSeconds = 5.0f;

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