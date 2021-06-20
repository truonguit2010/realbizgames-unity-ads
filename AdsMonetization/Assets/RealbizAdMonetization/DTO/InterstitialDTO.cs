
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
    }
}