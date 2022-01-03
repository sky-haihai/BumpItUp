using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<string, object> _data = new Dictionary<string, object>();

    public void SetData(string dataName, object value)
    {
        if (_data.ContainsKey(dataName))
        {
            _data[dataName] = value;
        }
        else
        {
            _data.Add(dataName, value);
        }
    }

    public object GetData(string dataName)
    {
        if (_data.ContainsKey(dataName))
        {
            return _data[dataName];
        }

        return null;
    }
}