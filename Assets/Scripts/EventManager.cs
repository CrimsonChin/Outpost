////using UnityEngine;
////using System.Collections;
////using System;
////using System.Collections.Generic;

////public class EventManager : MonoBehaviour
////{
////    private IDictionary<string, IList<Action>> _subscribers;

////    public EventManager()
////    {
////        _subscribers = new Dictionary<string, IList<Action>>();
////    }

////    public void Subscribe(string key, Action action)
////    {
////        if (_subscribers.ContainsKey(key))
////        {
////            _subscribers[key].Add(action);
////        }
////        else
////        {
////            _subscribers.Add(key, new List<Action> { action });
////        }
////    }

////    public void UnSubscribe(string key, Action action)
////    {
////        if (_subscribers.ContainsKey(key))
////        {
////            _subscribers[key].Remove(action);
////        }
////    }

////    public void Publish(string key)
////    {
////        IList<Action> subscribers;
////        if (_subscribers.TryGetValue(key, out subscribers))
////        {
////            foreach (Action subscriber in subscribers)
////            {
////                subscriber.Invoke();
////            }
////        }
////    }

////    public void ClearAllSubscribers()
////    {
////        foreach (string key in _subscribers.Keys)
////        {
////            _subscribers.Remove(key);
////        }
////    }
////}
