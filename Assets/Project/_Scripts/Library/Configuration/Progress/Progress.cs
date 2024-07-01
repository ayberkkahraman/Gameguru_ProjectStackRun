using UnityEngine;

namespace Project._Scripts.Library.Configuration.Progress
{
  public static class Progress
  {
    #region Save / Load

    /// <summary>
    /// Save data with given key value
    /// </summary>
    public static void Save<T>(string key, T saveData)
    {
      string gameDataJson = Serializer.Serialize(saveData);
      PlayerPrefs.SetString(key, gameDataJson);
    }

    /// <summary>
    /// Load data with save key saved earlier
    /// </summary>
    public static T Load<T>(string key, T defaultData)
    {
      if (!PlayerPrefs.HasKey(key))
      {
        Debug.Log($">>{key}<< has not found in datas...");
        Save(key, defaultData);
        return defaultData;
      }
      
      string gameDataJson = PlayerPrefs.GetString(key);
      return Serializer.DeSerialize<T>(gameDataJson);
    }

    #endregion
  }
}
