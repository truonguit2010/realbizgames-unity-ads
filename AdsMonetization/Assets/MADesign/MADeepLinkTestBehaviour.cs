using UnityEngine;

namespace MADesign
{
    public class MADeepLinkTestBehaviour : MonoBehaviour
    {
        [SerializeField]
        private string testURL = "tetridoku://?action=rating&title=Rating+TetrisDoku&message=Do+you+love+my+game%3F";
        // Use this for initialization
        public void testClick()
        {
            MADeepLinkBehaviour.Instance.onDeepLinkActivated(testURL);
        }
    }
}