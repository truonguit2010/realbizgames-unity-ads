#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("sU7sWCdfaNBu4XnFQMOukhZtKYcbexikm5A77hPGxpb0jb7ZhUUZACq18pUlcnvxHOi8XJAn+3fRuMleqZlI2qERH+VyIVG9cJ55kx5jihOQthjYW0ZqQ/vgj1zBdpvltyeUzpbGQYnwo1omBeUe/4DD4zQp2b/lZhyYo4QivkB7fTSzBmBIn/uV/VDRY+DD0ezn6MtnqWcW7ODg4OTh4irFBAN0UGAjxG57HKdRSaMPw/AsS53o3Stpt7a/T9MAPHbD2sRZu04GY7JUxDSy5fHIUTGmvtK7aH6QamfUa04jAlswli3IQhGr1GB+XnDlKJcMq7T3NaELk1lkExe2eUlHyX1j4O7h0WPg6+Nj4ODhfPEFhNA/z0cpze8hsx6q4OPi4OHg");
        private static int[] order = new int[] { 6,4,8,5,5,13,9,9,8,11,11,12,12,13,14 };
        private static int key = 225;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
