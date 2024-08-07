using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomCharacter
{
    public abstract class RandomInstanceGenerator : MonoBehaviour
    {
        public RandomCharacterClass characterSet;

        public bool destroyOnGenerate, generateOnAwake, generateOnInterval;
        public float generationInterval = 2, startDelay;
        public int amount;
        int currentAmount;

        private void Start()
        {
            if (generateOnAwake)
            {
                Generate();
            }

            if (generateOnInterval)
            {
                StartCoroutine(GenerateOnIntervals());
            }

            if (destroyOnGenerate && !generateOnInterval)
            {
                Destroy(this);
            }
        }

        public virtual IEnumerator GenerateOnIntervals ()
        {
            yield return new WaitForSeconds(startDelay);
            while (true)
            {
                if (amount > 0)
                {
                    if (currentAmount < amount)
                    {
                        Generate();
                        currentAmount++;
                        yield return new WaitForSeconds(generationInterval);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    Generate();
                    currentAmount++;
                    yield return new WaitForSeconds(generationInterval);
                }
            }
        }

        public virtual void Generate()
        {

        }
    }
}
