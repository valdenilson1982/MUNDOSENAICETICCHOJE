using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RandomCharacter
{
	public class RandomObjectInfo : MonoBehaviour
	{
		public List<Enumerations.ObjectTag> tags;
		public bool ContainsTag(Enumerations.ObjectTag tag)
		{
			return tags.Contains(tag);
		}
	}
}

