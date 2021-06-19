#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("sB/Kq/4xawodWnXxHXTwVPG2yUf26uOmxeP08u/g7+Xn8u/p6KbH87Ww3Lbkt422j4CF04KAlYTT1beV6uOmz+jlqLegtqKAhdOCjZWbx/aZF12YwdZtg2vY/wKrbbAk0crTaqbn6OKm5eP08u/g7+Xn8u/p6Kb2gLaJgIXTm5WHh3mCg7aFh4d5tps3tt5q3IK0Cu41CZtY4/V54djjOqwAzgBxi4eHg4OGtuS3jbaPgIXTpungpvLu46by7uPopuf29urv5edG5bXxcbyBqtBtXImniFw89Z/JM46tgIeDg4GEh5CY7vLy9vW8qanxi4CPrADOAHGLh4eDg4aFBIeHhtr5xy4ef1dM4Bqi7ZdWJT1inaxFmfHxqOf29urjqOXp66nn9vbq4+Xnjti2BIeXgIXTm6aCBIeOtgSHgraqpuXj9PLv4O/l5/Ljpvbp6u/l/wn1B+ZAnd2PqRQ0fsLOdua+GJNzomRtVzH2WYnDZ6FMd+v+a2EzkZHPXvAZtZLjJ/EST6uEhYeGhyUEh/Lv4O/l5/LjpuT/pufo/6b25/Tyu6Dhpgy17HGLBElYbSWpf9Xs3eIGkq1W78ES8I94cu0LqMYgccHL+Ykbu3Wtz66cTnhIMz+IX9iaUE27tpeAhdOCjJWMx/b26uOmz+jlqLfhCY4ypnFNKqqm6fYwuYe2CjHFSf+m5/X18+vj9abn5eXj9vLn6OXj3yGDj/qRxtCXmPJVMQ2lvcElU+mptgdFgI6tgIeDg4GEhLYHMJwHNQSHhoCPrADOAHHl4oOHtgd0tqyA/LYEh/C2iICF05uJh4d5goKFhIfo4qbl6eji7/Lv6ej1pungpvP14w2fD1h/zepzgS2ktoRunrh+1o9VgIXTm4iCkIKSrVbvwRLwj3hy7QtPn/Rz24hT+dkddKOFPNMJy9uLdzGdOxXEopSsQYmbMMsa2OVOzQaR1OPq7+fo5eOm6eim8u7v9abl4/TD+JnK7dYQxw9C8uSNlgXHAbUMB4Fq+78FDdWmVb5CNzkcyYztea16LSX3FMHV00cpqcc1fn1l9ktgJcpfsPlHAdNfIR8/tMR9XlP3GPgn1IKAlYTT1beVtpeAhdOCjJWMx/b2g4aFBIeJhrYEh4yEBIeHhmIXL4/v4O/l5/Lv6eimx/Py7un07/L/t7YEgj22BIUlJoWEh4SEh4S2i4CPM7wrcomIhhSNN6eQqPJTuotd5JD05+Xy7+XjpvXy5/Lj6+Po8vWotqjGIHHBy/mO2LaZgIXTm6WCnraQ5OrjpvXy5+ji5/TipvLj9Ov1pueQtpKAhdOChZWLx/b26uOm1Onp8qbFx7YEh6S2i4CPrADOAHGLh4eHoLaigIXTgo2Vm8f29urjpsXj9PKztLeytrWw3JGLtbO2tLa/tLeytvLu6fTv8v+3kLaSgIXTgoWVi8f2Llr4pLNMo1NfiVDtUiSipZdxJyqZAwUDnR+7wbF0Lx3GCKpSNxaUXjhy9R1oVOKJTf/Jsl4kuH/+ee1O4rOlk82T35s1EnFwGhhJ1jxH3tYTGPyKIsEN3VKQsbVNQonLSJLvV/bq46bU6enypsXHtpiRi7awtrK01iwMU1xielaPgbE28/On");
        private static int[] order = new int[] { 48,4,8,3,15,45,29,26,15,41,57,35,47,17,36,33,44,55,53,27,39,56,32,32,41,56,40,52,37,34,37,43,44,37,52,59,38,56,51,41,58,56,51,59,48,48,46,59,51,59,55,57,58,57,54,59,59,59,59,59,60 };
        private static int key = 134;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
