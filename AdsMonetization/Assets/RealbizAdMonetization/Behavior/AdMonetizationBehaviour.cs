using UnityEngine;

namespace RealbizGames.Ads
{
    [DisallowMultipleComponent]
    public class AdMonetizationBehaviour : MonoBehaviour
    {
        private static AdMonetizationBehaviour instance;
        private void Awake() {
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            } else {
                Destroy(this.gameObject);
            }
        }
        void Update()
        {
            RealAdMonetizationImpl.DefaultInstance.Update();
        }
    }
}