using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    private Dictionary<string, Action<object>> _currentEvents;

    private void Awake()
    {
        _currentEvents = new Dictionary<string, Action<object>>();
    }

    public void Register(string eventName, Action<object> action)
    {
        if (_currentEvents.ContainsKey(eventName))
        {
            _currentEvents[eventName] += action;
        }
        else
        {
            _currentEvents.Add(eventName, action);
        }
    }

    public void UnRegister(string eventName, Action<object> action)
    {
        if (_currentEvents.ContainsKey(eventName))
        {
            _currentEvents[eventName] -= action;
        }
    }

    public void Invoke(string eventName, object value)
    {
        if (_currentEvents.ContainsKey(eventName))
        {
            _currentEvents[eventName].Invoke(value);
        }
        else
        {
            Debug.LogWarningFormat("Event : {0} have no subscribers", eventName);
        }
    }
}