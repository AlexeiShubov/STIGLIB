using System.Collections.Generic;
using UnityEngine;

public enum SceneDataKeys
{
    SelectedImageNumber
}

public static class SceneDataHolder
{
    private static Dictionary<SceneDataKeys, object> _sceneData;

    public static void SetData(SceneDataKeys key, object value)
    {
        _sceneData ??= new Dictionary<SceneDataKeys, object>();
        
        if (_sceneData.ContainsKey(key))
        {
            _sceneData[key] = value;
        }
        else
        {
            _sceneData.Add(key, value);
        }
    }

    public static T GetData<T>(SceneDataKeys key)
    {
        if (_sceneData == null)
        {
            Debug.LogError("Data already exist!");
            
            return default;
        }

        if (_sceneData.ContainsKey(key))
        {
            return (T)_sceneData[key];
        }
        
        Debug.LogError($"Key {key} already exist!");

        return default;
    }
}