using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

namespace RandomCharacter
{
    [System.Serializable]
    public class RandomCharacterCustomEvent
    {
        public List<CustomEvent> events = new List<CustomEvent>()
    {
        new CustomEvent()
        {
            name = "New Custom Event"
        }
    };

        [System.Serializable]
        public struct CustomEvent
        {
            public string name;
            public UnityEvent events;

            public void Invoke()
            {
                if (events != null)
                {
                    events.Invoke();
                }
            }
        }
    }
}