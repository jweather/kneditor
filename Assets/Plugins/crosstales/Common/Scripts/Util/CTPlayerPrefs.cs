using UnityEngine;

namespace Crosstales.Common.Util
{
    /// <summary>Wrapper for the PlayerPrefs.</summary>
    public static class CTPlayerPrefs
    {

        /// <summary>
        /// Exists the key?
        /// </summary>
        /// <param name="key">Key for the PlayerPrefs.</param>
        /// <returns>Value for the key.</returns>
        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        /// <summary>
        /// Deletes all keys.
        /// </summary>
        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }

        /// <summary>
        /// Delete the key.
        /// </summary>
        /// <param name="key">Key to delete in the PlayerPrefs.</param>
        public static void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        /// <summary>
        /// Saves all modifications.
        /// </summary>
        public static void Save()
        {
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Allows to get a string from a key.
        /// </summary>
        /// <param name="key">Key for the PlayerPrefs.</param>
        /// <returns>Value for the key.</returns>
        public static string GetString(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        /// <summary>
        /// Allows to get a float from a key.
        /// </summary>
        /// <param name="key">Key for the PlayerPrefs.</param>
        /// <returns>Value for the key.</returns>
        public static float GetFloat(string key)
        {
            //PlayerPrefs.SetFloat();
            //PlayerPrefs.SetInt();
            //PlayerPrefs.SetString();

            return PlayerPrefs.GetFloat(key);
        }

        /// <summary>
        /// Allows to get an int from a key.
        /// </summary>
        /// <param name="key">Key for the PlayerPrefs.</param>
        /// <returns>Value for the key.</returns>
        public static int GetInt(string key)
        {
            return PlayerPrefs.GetInt(key);
        }

        /// <summary>
        /// Allows to get a bool from a key.
        /// </summary>
        /// <param name="key">Key for the PlayerPrefs.</param>
        /// <returns>Value for the key.</returns>
        public static bool GetBool(string key)
        {

            if (string.IsNullOrEmpty(key))
                throw new System.ArgumentNullException("key");

            return "true".CTEquals(PlayerPrefs.GetString(key)) ? true : false;
        }

        /// <summary>
        /// Allows to set a string for a key.
        /// </summary>
        /// <param name="key">Key for the PlayerPrefs.</param>
        /// <param name="value">Value for the PlayerPrefs.</param>
        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        /// <summary>
        /// Allows to set a float for a key.
        /// </summary>
        /// <param name="key">Key for the PlayerPrefs.</param>
        /// <param name="value">Value for the PlayerPrefs.</param>
        public static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        /// <summary>
        /// Allows to set an int for a key.
        /// </summary>
        /// <param name="key">Key for the PlayerPrefs.</param>
        /// <param name="value">Value for the PlayerPrefs.</param>
        public static void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        /// <summary>
        /// Allows to set a bool for a key.
        /// </summary>
        /// <param name="key">Key for the PlayerPrefs.</param>
        /// <param name="value">Value for the PlayerPrefs.</param>
        public static void SetBool(string key, bool value)
        {
            if (string.IsNullOrEmpty(key))
                throw new System.ArgumentNullException("key");

            PlayerPrefs.SetString(key, value ? "true" : "false");
        }
    }
}
// © 2015-2018 crosstales LLC (https://www.crosstales.com)