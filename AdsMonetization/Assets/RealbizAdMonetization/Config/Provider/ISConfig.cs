using UnityEngine;

namespace RealbizGames.Ads
{
    public class ISConfig
    {
        public string iosId;

        public string androidId;

        public string platfromId {
            get {
                if (Application.platform == RuntimePlatform.IPhonePlayer) {
                    return iosId;
                } else {
                    return androidId;
                }
            }
        }
    }
}