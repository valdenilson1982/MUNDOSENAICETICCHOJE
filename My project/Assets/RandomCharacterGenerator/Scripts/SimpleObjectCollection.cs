using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomCharacter
{
	[System.Serializable]
	public struct SimpleObjectCollection
	{
		public GameObject[] objectVariations;
		public Material[] materialVariations;

		public List<GameObject> FindWithTag(Enumerations.ObjectTag tag)
		{
			List<GameObject> tagged = new List<GameObject>();
			foreach (GameObject obj in objectVariations)
			{
				if (obj.GetComponent<RandomObjectInfo>())
				{
					if (obj.GetComponent<RandomObjectInfo>().ContainsTag(tag))
					{
						tagged.Add(obj);
					}
				}
			}
			return tagged;
		}
	}

	[System.Serializable]
	public struct WeaponCollection
	{
		public string name;
		public RuntimeAnimatorController controller;

		public WeaponController[] objectVariations;
		public Material[] materialVariations;
	}
}