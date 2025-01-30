using System;
using System.Collections.Generic;
using UnityEngine;

namespace _StickBlast.Script.Game.Management
{
    public class EventManager : MonoBehaviour
    {
        private static EventManager _instance;

        public static EventManager Instance
        {
            get
            {
                if (!_instance)
                {
                    GameObject singleton = new GameObject(nameof(EventManager));
                    _instance = singleton.AddComponent<EventManager>();
                }

                return _instance;
            }
        }
        
        private readonly Dictionary<string, Action> _eventDictionary = new Dictionary<string, Action>();
        private readonly List<IEventListener> _eventListeners = new List<IEventListener>();

        private void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
        }

        public void StartListening(string eventName, Action listener)
        {
            if (!_eventDictionary.ContainsKey(eventName))
                _eventDictionary[eventName] = null;
            
            _eventDictionary[eventName] += listener;
        }

        public void StopListening(string eventName, Action listener)
        {
            if (_eventDictionary.ContainsKey(eventName))
                _eventDictionary[eventName] -= listener;
        }

        public void TriggerEvent(string eventName)
        {
            if (_eventDictionary.TryGetValue(eventName, out Action listener))
                listener?.Invoke();
        }

        public void RegisterListener(IEventListener listener)
        {
            if(!_eventListeners.Contains(listener))
                _eventListeners.Add(listener);
        }

        public void UnregisterListener(IEventListener listener)
        {
            if (_eventListeners.Contains(listener))
                _eventListeners.Remove(listener);
        }

        public T GetListener<T>() where T : IEventListener
        {
            foreach (var listener in _eventListeners)
            {
                if (listener is T)
                {
                    return (T)listener;
                }
            }

            return default;
        }
    }
    
    public interface IEventListener { }   
}