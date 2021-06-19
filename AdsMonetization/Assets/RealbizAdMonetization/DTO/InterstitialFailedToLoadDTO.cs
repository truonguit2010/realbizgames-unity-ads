
namespace RealbizGames.Ads
{
    public class InterstitialFailedToLoadDTO
    {
        private string _code;

        private string _message;

        public InterstitialFailedToLoadDTO(string code, string message)
        {
            _code = code;
            _message = message;
        }

        public string Code { get => _code; }
        public string Message { get => _message; }
    }
}
