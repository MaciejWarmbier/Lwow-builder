using System.Collections.Generic;
using UnityEngine;

public static class ConfigController
{
    public static string PathInResourcesToConfigsDir = "Configs";

    public static List<ScriptableObject> configs = new List<ScriptableObject>();

    public static T GetConfig<T>() where T : ScriptableObject
    {
        var foundConfig = configs.Find(c => c is T) as T;
        if (foundConfig != null)
        {
            return foundConfig;
        }

        T config = null;

        config = Resources.Load<T>(PathInResourcesToConfigsDir + "/" + typeof(T).Name);

        if (config != null)
        {
            configs.Add(config);
        }
        else
        {
            Debug.LogError($"Could not find config {PathInResourcesToConfigsDir}/{typeof(T).Name}");
        }

        return config;
    }

}
