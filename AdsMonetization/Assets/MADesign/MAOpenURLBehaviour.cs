using UnityEngine;
using UnityEngine.UI;

namespace MADesign {

    public class MAOpenURLBehaviour : MonoBehaviour
    {
        [SerializeField]
        private string url = "https://privacy.realbizgames.com";

        // Use this for initialization
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                OpenUrlWithBrowser();
            });
        }

        public void OpenUrlWithBrowser()
        {
            Application.OpenURL(url);
        }
    }

}
