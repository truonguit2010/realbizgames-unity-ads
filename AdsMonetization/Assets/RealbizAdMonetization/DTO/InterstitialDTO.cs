
namespace RealbizGames.Ads
{
    public class InterstitialDTO
    {
        private string _actionIdentifer;

        public InterstitialDTO(string actionIdentifer)
        {
            _actionIdentifer = actionIdentifer;
        }

        public string ActionIdentifer { get => _actionIdentifer; }

        public bool HasActionIdentifer {
            get {
                return !string.IsNullOrEmpty(_actionIdentifer) && _actionIdentifer.Length > 2;
            }
        }

        public override string ToString()
        {
            return string.Format("[InterstitialDTO ActionIdentifer:{0}]", ActionIdentifer);
        }
    }
}