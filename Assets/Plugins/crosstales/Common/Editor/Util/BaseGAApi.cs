using UnityEngine;

namespace Crosstales.Common.EditorUtil
{
    /// <summary>Base GA-wrapper API.</summary>
    public abstract class BaseGAApi
    {
        #region Variables

        private static string clientId = SystemInfo.deviceUniqueIdentifier;
        private static string screenResolution = Screen.currentResolution.ToString();
        private static string userLanguage = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
        private static string unityVersion = Application.unityVersion;
        private static string os = SystemInfo.operatingSystem;
        private static string cpu = SystemInfo.processorType;
        private static int cpuCores = SystemInfo.processorCount;
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
        private static int cpuFrequency = SystemInfo.processorFrequency;
        private static string productName = Application.productName;
        private static string companyName = Application.companyName;
#else
        private static int cpuFrequency = 0;
        private static string productName = "UNKNOWN PRODUCT";
        private static string companyName = "UNKNOWN COMPANY";
#endif
        private static int memory = SystemInfo.systemMemorySize;
        private static string gpu = SystemInfo.graphicsDeviceName;
        private static int gpuMemory = SystemInfo.graphicsMemorySize;
        private static int gpuShaderLevel = SystemInfo.graphicsShaderLevel;

        #endregion


        #region Public methods

        /// <summary>Tracks an event from the asset.</summary>
        /// <param name="category">Specifies the event category.</param>
        /// <param name="action">Specifies the event action.</param>
        /// <param name="label">Specifies the event label.</param>
        /// <param name="value">Specifies the event value.</param>
        public static void Event(string name, string version, string category, string action, string label = "", int value = 0)
        {
            new System.Threading.Thread(() => trackEvent(name, version, category, action, label, value)).Start();
        }

        #endregion


        #region Private methods

        private static void trackEvent(string appName, string appVersion, string category, string action, string label, int value)
        {
            post(generalInfo(appName, appVersion) +
            "&t=event" +
            "&ec=" + category +
            "&ea=" + action +
            (string.IsNullOrEmpty(label) ? string.Empty : "&el=" + label) +
            (value > 0 ? "&ev=" + value : string.Empty) +
            customDimensions()
            );
        }

        private static void post(string postData)
        {
            byte[] data = new System.Text.ASCIIEncoding().GetBytes(postData);

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = Util.BaseHelper.RemoteCertificateValidationCallback;

                using (System.Net.WebClient client = new Common.Util.CTWebClient())
                {
                    client.Headers[System.Net.HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    client.Headers[System.Net.HttpRequestHeader.UserAgent] = "Mozilla/5.0 (" + userAgent() + ")";

                    client.UploadData("tcelloc/moc.scitylana-elgoog.www//:sptth".Reverse(), data);

                    if (Util.BaseConstants.DEV_DEBUG)
                        Debug.Log("Data uploaded: " + postData);
                }
            }
            catch (System.Exception ex)
            {
                if (Util.BaseConstants.DEV_DEBUG)
                    Debug.LogError("Could not upload GA-data: " + System.Environment.NewLine + ex);
            }
        }

        private static string userAgent()
        {
            if (Util.BaseHelper.isWindowsPlatform)
            {
                return "compatible; Windows NT 10.0; WOW64";
            }
            else if (Util.BaseHelper.isMacOSPlatform)
            {
                return "compatible; Macintosh; Intel Mac OS X";
            }
            else
            {
                return "compatible; X11; Linux i686";
            }
        }

        private static string generalInfo(string appName, string appVersion)
        {
            return "v=1&tid=" + "1-52901854-AU".Reverse() +
                "&ds=app" +
                "&cid=" + clientId +
                "&ul=" + userLanguage +
                "&an=" + appName +
                "&av=" + appVersion +
                "&sr=" + screenResolution;
        }

        private static string customDimensions()
        {
            return "&cd1=" + os +
                "&cd2=" + memory +
                "&cd3=" + cpu +
                "&cd4=" + gpu +
                "&cd5=" + productName +
                "&cd6=" + companyName +
                //"&cd7=" + usage +
                "&cd8=" + unityVersion +
                "&cd9=" + cpuCores +
                "&cd10=" + cpuFrequency +
                "&cd11=" + gpuMemory +
                "&cd12=" + gpuShaderLevel;
        }

        #endregion

    }
}
// © 2017-2018 crosstales LLC (https://www.crosstales.com)