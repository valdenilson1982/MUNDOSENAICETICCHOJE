using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomCharacter
{
	[SelectionBase]
    public class RandomCharacterBehaviour : MonoBehaviour
    {
        new public string name;
		public Sprite image;

		public Enumerations.characterGenre characterGenre;

        public List<CharacterStats> characterStats;
        public List<RandomCharacterCustomEvent.CustomEvent> characterEvents;

		public void InvokeEvent (string eventName)
		{
			foreach (RandomCharacterCustomEvent.CustomEvent e in characterEvents)
			{
				if (e.name == eventName)
				{
					e.events.Invoke();
					break;
				}
			}
		}

		public CharacterStats GetStat (string statName)
		{
			foreach (CharacterStats stat in characterStats)
			{
				if (stat.name == statName)
				{
					return stat;
				}
			}

			return null;
		}

		private void Start()
		{

		}

		private void Update()
		{
			
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
