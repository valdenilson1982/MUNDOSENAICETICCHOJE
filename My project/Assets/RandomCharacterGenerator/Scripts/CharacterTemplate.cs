using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterTemplate : MonoBehaviour
{
	public Transform L_ShoulderMarker, R_ShoulderMarker;
	public Transform L_WristMarker, R_WristMarker;
	public Transform L_HandMarker, R_HandMarker;
	public Transform L_ThighMarker, R_ThighMarker;
	public Transform L_KneeMarker, R_KneeMarker;

	public List<ItemCollection> itemsMarkers = new List<ItemCollection>();

	[System.Serializable]
	public class ItemMarker
	{
		public Transform marker;
		[SerializeField]protected Animator anim;
		[SerializeField] HumanBodyBones parent = HumanBodyBones.Hips;
		public HumanBodyBones boneParent
		{
			get => parent;
			set
			{
				Transform tParent = anim.GetBoneTransform(value);
				if (parent != value)
				{
					parent = value;
					if (marker)
					{
						marker.parent = anim.GetBoneTransform(value);
					}
				}
			}
		}

		public ItemMarker(Animator anim)
		{
			this.anim = anim;
		}

		public ItemMarker() { }
	}

	[System.Serializable]
	public class ItemCollection : ItemMarker
	{
		public List<GameObject> items;

		public ItemCollection(Animator anim)
		{
			this.anim = anim;
		}
	}
}

