using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.UI
{
    /// <summary>Change the state of all Window panels.</summary>
    public class UIWindowManager : MonoBehaviour
    {
        #region Variables

        /// <summary>All Windows of the scene.</summary>
        [Tooltip("All Windows of the scene.")]
        public GameObject[] Windows;

        private Image image;
        private GameObject DontTouch;

        #endregion


        #region MonoBehaviour methods

        public void Start()
        {
            foreach (GameObject window in Windows)
            {
#if UNITY_2017_1_OR_NEWER
                image = window.transform.Find("Panel/Header").GetComponent<Image>();
#else
                image = window.transform.FindChild("Panel/Header").GetComponent<Image>();
#endif

                Color c = image.color;
                c.a = 0.2f;
                image.color = c;
            }
        }

        #endregion


        #region Public methods

        public void ChangeState(GameObject x)
        {
            foreach (GameObject window in Windows)
            {
                if (window != x)
                {
#if UNITY_2017_1_OR_NEWER
                    image = window.transform.Find("Panel/Header").GetComponent<Image>();
#else
                    image = window.transform.FindChild("Panel/Header").GetComponent<Image>();
#endif

                    Color c = image.color;
                    c.a = 0.2f;
                    image.color = c;
                }

#if UNITY_2017_1_OR_NEWER
                DontTouch = window.transform.Find("Panel/DontTouch").gameObject;
#else
                DontTouch = window.transform.FindChild("Panel/DontTouch").gameObject;
#endif
                DontTouch.SetActive(window != x);
            }
        }

        #endregion
    }
}
// © 2017-2018 crosstales LLC (https://www.crosstales.com)