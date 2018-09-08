using UnityEditor;
using Crosstales.FB.EditorUtil;

namespace Crosstales.FB.EditorTask
{
    /// <summary>Gather some tracing data for the asset.</summary>
    [InitializeOnLoad]
    public static class Tracer
    {
        #region Constructor

        static Tracer()
        {
            string lastDate = string.Empty;
            if (Common.Util.CTPlayerPrefs.HasKey(EditorConstants.KEY_TRACER_DATE))
            {
                lastDate = Common.Util.CTPlayerPrefs.GetString(EditorConstants.KEY_TRACER_DATE);
            }
 
            string date = System.DateTime.Now.ToString("yyyyMMdd"); // every day
            //string date = System.DateTime.Now.ToString("yyyyMMddHHmm"); // every minute (for tests)

            if (!date.Equals(lastDate))
            {
                GAApi.Event(typeof(Tracer).Name, "Startup");

                Common.Util.CTPlayerPrefs.SetString(EditorConstants.KEY_TRACER_DATE, date);
            }
        }

        #endregion

    }
}
// © 2017-2018 crosstales LLC (https://www.crosstales.com)