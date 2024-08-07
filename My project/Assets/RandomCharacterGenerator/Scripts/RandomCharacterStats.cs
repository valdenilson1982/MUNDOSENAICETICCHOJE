using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomCharacter
{
	[System.Serializable]
	public class RandomCharacterStat
	{
		public string name;
		public int minValue = 2, maxValue = 6;
		public int maxLimit = 10;

		public Sprite icon;

		public bool rangedValue = false;
		public bool useProbabilityTrigger = false;

		public int minProb = 2, maxProb = 4;
		public int maxProbLimit = 100;

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
	}
}