using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RandomCharacter
{
	public class RandomCharacterInfo : MonoBehaviour
	{
		public List<Enumerations.CharacterTag> tags;
		public bool ContainsTag(Enumerations.CharacterTag tag)
		{
			return tags.Contains(tag);
		}

		void OnAnimatorIK()
		{
			WeaponController controller = GetComponentInChildren<WeaponController>();
			Animator anim = GetComponent<Animator>();
			if (controller != null)
			{
				if (controller.L_HandPosition != null)
				{
					anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
					anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
					anim.SetIKPosition(AvatarIKGoal.LeftHand, controller.L_HandPosition.position);
					anim.SetIKRotation(AvatarIKGoal.LeftHand, controller.L_HandPosition.rotation);
				}
			}
		}
	}
}

