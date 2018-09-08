using UnityEngine;

namespace Crosstales.Common.Util
{
    /// <summary>Random rotation changer.</summary>
    //[HelpURL("https://www.crosstales.com/media/data/assets/radio/api/class_crosstales_1_1_radio_1_1_demo_1_1_util_1_1_random_rotator.html")] //TODO update URL
    public class RandomRotator : MonoBehaviour
    {
        #region Variables

        public Vector3 Speed = new Vector3(15, 15, 15);
        public Vector2 ChangeInterval = new Vector2(10, 20);

        private Transform tf;
        private Vector3 speed;
        private float elapsedTime = 0f;
        private float changeTime = 0f;

        #endregion


        #region MonoBehaviour methods

        public void Start()
        {
            tf = transform;

            elapsedTime = changeTime = Random.Range(ChangeInterval.x, ChangeInterval.y);
        }

        public void Update()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > changeTime)
            {
                speed.x = Random.Range(-Mathf.Abs(Speed.x), Mathf.Abs(Speed.x));
                speed.y = Random.Range(-Mathf.Abs(Speed.y), Mathf.Abs(Speed.y));
                speed.z = Random.Range(-Mathf.Abs(Speed.z), Mathf.Abs(Speed.z));
                changeTime = Random.Range(ChangeInterval.x, ChangeInterval.y);

                elapsedTime = 0f;
            }

            tf.Rotate(speed.x * Time.deltaTime, speed.y * Time.deltaTime, speed.z * Time.deltaTime);
        }

        #endregion
    }
}
// © 2015-2018 crosstales LLC (https://www.crosstales.com)