using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MADesign
{
    public class MALikeFacebookFanPageBehaviour : MonoBehaviour
    {
        [SerializeField]
        private string facebookFanPageId = "115736560270854";

        bool leftApp = false;

        // Use this for initialization
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                StartCoroutine(OpenFacebookFanPage());
            });
        }

        IEnumerator OpenFacebookFanPage()
        {
            string scheme = string.Format("fb://page/{0}", facebookFanPageId);
            Application.OpenURL(scheme);

            yield return new WaitForSeconds(1);

            if (!leftApp)
            {
                string webUrl = string.Format("https://www.facebook.com/{0}", facebookFanPageId);
                Application.OpenURL(webUrl);
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            leftApp = false;
        }

        private void OnApplicationPause(bool pause)
        {
            leftApp = true;
        }
    }

}
