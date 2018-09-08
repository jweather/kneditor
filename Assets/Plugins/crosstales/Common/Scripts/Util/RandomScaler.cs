using UnityEngine;

namespace Crosstales.Common.Util
{
    /// <summary>Random scale changer.</summary>
    //[HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_demo_1_1_util_1_1_random_scaler.html")] //TODO update URL
    public class RandomScaler : MonoBehaviour
    {
        #region Variables

        public Vector3 ScaleMin = new Vector3(0.1f, 0.1f, 0.1f);
        public Vector3 ScaleMax = new Vector3(3, 3, 3);
        public bool Uniform = true;
        public Vector2 ChangeInterval = new Vector2(5, 15);

        private Transform tf;
        private Vector3 endScale;
        private float elapsedTime = 0f;
        private float changeTime = 0f;
        private Vector3 startScale;
        private float lerpTime = 0f;

        #endregion


        #region MonoBehaviour methods

        public void Start()
        {
            tf = transform;

            elapsedTime = changeTime = Random.Range(ChangeInterval.x, ChangeInterval.y);

            startScale = tf.localScale;
        }

        public void Update()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > changeTime)
            {
                if (Uniform)
                {
                    endScale.x = endScale.y = endScale.z = Random.Range(ScaleMin.x, Mathf.Abs(ScaleMax.x));
                }
                else
                {
                    endScale.x = Random.Range(ScaleMin.x, Mathf.Abs(ScaleMax.x));
                    endScale.y = Random.Range(ScaleMin.y, Mathf.Abs(ScaleMax.y));
                    endScale.z = Random.Range(ScaleMin.z, Mathf.Abs(ScaleMax.z));
                }

                changeTime = Random.Range(ChangeInterval.x, ChangeInterval.y);

                lerpTime = elapsedTime = 0f;
            }

            tf.localScale = Vector3.Lerp(startScale, endScale, lerpTime);

            if (lerpTime < 1f)
            {
                lerpTime += Time.deltaTime / (changeTime - 0.1f);
            }
            else
            {
                startScale = tf.localScale;
            }
        }

        #endregion
    }
}
// © 2015-2018 crosstales LLC (https://www.crosstales.com)