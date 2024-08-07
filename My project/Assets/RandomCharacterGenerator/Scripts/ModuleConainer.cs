using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandomCharacter
{
	[System.Serializable]
#pragma warning disable CS0661 // El tipo define operator == or operator !=, pero no reemplaza a Object.GetHashCode()
#pragma warning disable CS0660 // El tipo define operator == or operator !=, pero no reemplaza a override Object.Equals(object o)
	public class ModuleConainer
#pragma warning restore CS0660 // El tipo define operator == or operator !=, pero no reemplaza a override Object.Equals(object o)
#pragma warning restore CS0661 // El tipo define operator == or operator !=, pero no reemplaza a Object.GetHashCode()
	{
		public bool fold = false;

		public bool overrideLayer, overrideTag, overrideName;
		public int layer;
		public string tag = "Untagged", name = "Unamed";

		[SerializeField] GameObject module;
		public GameObject setModule
		{
			get => module;
			set
			{
				if (value != module)
				{
					module = value;
					parent = null;
				}
			}
		}
		public Transform parent;

		public Enumerations.ModuleUse moduleUse = Enumerations.ModuleUse.CopyComponents;
		public bool useContainerAnimator = true;

		public ModuleConainer(GameObject module)
		{
			this.setModule = module;
		}

		public static bool operator ==(ModuleConainer moduleA, ModuleConainer moduleB)
		{
			return moduleA.setModule == moduleB.setModule;
		}

		public static bool operator !=(ModuleConainer moduleA, ModuleConainer moduleB)
		{
			return moduleA.setModule != moduleB.setModule;
		}
	}
}