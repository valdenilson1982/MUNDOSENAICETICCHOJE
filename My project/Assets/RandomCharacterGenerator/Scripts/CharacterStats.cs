using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomCharacter
{
    [System.Serializable]
    /// <summary>
    /// The static values class for the characters
    /// </summary>
    public class CharacterStats
    {
        public string name
        {
            get;
            set;
        }

        public Sprite icon
        {
            get;
            set;
        }

        public int value
        {
            get;
            set;
        }
        public int minValue
        {
            get;
            set;
        }
        public int maxValue
        {
            get;
            set;
        }
        
        public bool rangedValue
        {
            get;
            private set;
        }
        public bool useProbability
        {
            get;
            private set;
        }
        public int probability
        {
            get;
            set;
        }

        public bool allowEditing;

        public int maxLimit
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the random stats and convert it to a static character stats
        /// </summary>
        /// <param name="randomStats"></param>
        public CharacterStats(RandomCharacterStat randomStats)
        {
            this.name = randomStats.name;
            this.icon = randomStats.icon;

            this.rangedValue = randomStats.rangedValue;
            this.minValue = randomStats.minValue;
            this.maxValue = randomStats.maxValue;
            this.value = Random.Range(minValue, maxValue);
            this.maxLimit = randomStats.maxLimit;

            this.useProbability = randomStats.useProbabilityTrigger;
            this.probability = Random.Range(randomStats.minProb, randomStats.maxProb);
        }

        public void Update()
        {
            if (maxValue >= (float)maxLimit * 0.9f)
            {
                maxLimit *= 2;
            }

            if (maxValue <= (float)maxLimit * 0.35f)
            {
                maxLimit /= 2;
            }
        }

        public int GetValue()
        {
            if (useProbability)
            {
                if (Random.Range(0, 101) <= probability)
                {
                    if (rangedValue)
                    {
                        return Random.Range(minValue, maxValue);
                    }
                    return value;
                }
                else
                {
                    return 0;
                }
            }

            if (rangedValue)
            {
                return Random.Range(minValue, maxValue);
            }

            return value;
        }
    }
}
