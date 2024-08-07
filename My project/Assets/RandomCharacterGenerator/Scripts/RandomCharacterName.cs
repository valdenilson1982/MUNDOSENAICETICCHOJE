using UnityEngine;
using System.IO;

namespace RandomCharacter
{
    public class RandomCharacterName
    {
        string[] nameStrings;
        public bool initialized
        {
            get;
            private set;
        }

        void Split (TextAsset names)
        {
            string raw = names.text;
            string[] separators = new string[] { " ", "\r" };
            nameStrings = raw.Split(separators,System.StringSplitOptions.None);
            for (int i = 0; i < nameStrings.Length; i++)
            {
                nameStrings[i] = nameStrings[i].Remove(0,1);
            }
        }

        public void Init (TextAsset names)
        {
            initialized = true;
            Split(names);
        }

        public string GetName (int length)
        {
            string newName = "";
            for (int i = 0; i < length; i++)
            {
                newName += nameStrings[Random.Range(0, nameStrings.Length)];
                newName += " ";
            }

            return newName;
        }

    }
}