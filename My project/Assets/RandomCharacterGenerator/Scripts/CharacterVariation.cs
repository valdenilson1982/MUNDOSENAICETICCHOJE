using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomCharacter
{
	[System.Serializable]
	public struct CharacterVariations
	{
		public string name;

		public GameObject[] characterVariations;
		public Material[] materialVariations;

		public List<GameObject> TaggedCharacterVariations(Enumerations.CharacterTag tag)
		{
			List<GameObject> tagged = new List<GameObject>();
			foreach (GameObject character in characterVariations)
			{
				if (character.GetComponent<RandomCharacterInfo>().ContainsTag(tag))
				{
					tagged.Add(character);
				}
			}
			return tagged;
		}
	}
}