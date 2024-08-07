using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RandomCharacter.Enumerations;

namespace RandomCharacter
{
	[CreateAssetMenu(fileName = "New Character Class", menuName = "MASSIVE/Character Class")]
	public class RandomCharacterClass : ScriptableObject
	{
		public void Init()
		{
			if (MaleNamesList)
			{
				randomMaleNameGenerator = new RandomCharacterName();
				randomMaleNameGenerator.Init(MaleNamesList);
			}
			if (FemaleNamesList)
			{
				randomFemaleNameGenerator = new RandomCharacterName();
				randomFemaleNameGenerator.Init(FemaleNamesList);
			}
		}

#if UNITY_EDITOR
		[ContextMenu("Create area instancer")]
		void CreateAreaInstancer ()
		{
			Transform activeTransform = UnityEditor.Selection.activeTransform;
			RandomAreaInstancer instancer = new GameObject("Area instancer").AddComponent<RandomAreaInstancer>();
			instancer.transform.parent = activeTransform;
			instancer.transform.localPosition = Vector3.zero;
			instancer.transform.localRotation = Quaternion.identity;

			instancer.characterSet = this;
		}
#endif

		public LayerMask positioningMask, obstacleMask, checkMask;

		public int MaxPossibleMaleCombinations
		{
			get
			{
				int value = 0;
				foreach (CharacterVariations chars in maleVariations)
				{
					value += chars.characterVariations.Length * chars.materialVariations.Length;
				}
				value += materialVariations.Length * hairMaleVariations.Length;
				value += materialVariations.Length * bearVariations.Length;
				value += materialVariations.Length * maskVariations.Length;
				value += materialVariations.Length * backpackVariations.Length;
				value += materialVariations.Length * helmetVariation.Length;
				value += materialVariations.Length * maleArmourChest.Length;
				value += materialVariations.Length * shoulders.Length;
				value += materialVariations.Length * wrists.Length;
				value += materialVariations.Length * thighs.Length;
				value += materialVariations.Length * knees.Length;

				foreach (WeaponCollection weapon in weapons)
				{
					value += weapon.objectVariations.Length * weapon.materialVariations.Length;
				}

				return value;
			}
		}

		public int MaxPossibleFemaleCombinations
		{
			get
			{
				int value = 0;
				foreach (CharacterVariations chars in femaleVariations)
				{
					value += chars.characterVariations.Length * chars.materialVariations.Length;
				}
				value += materialVariations.Length * hairMaleVariations.Length;
				value += materialVariations.Length * bearVariations.Length;
				value += materialVariations.Length * maskVariations.Length;
				value += materialVariations.Length * backpackVariations.Length;
				value += materialVariations.Length * helmetVariation.Length;
				value += materialVariations.Length * femaleVariations.Length;
				value += materialVariations.Length * shoulders.Length;
				value += materialVariations.Length * wrists.Length;
				value += materialVariations.Length * thighs.Length;
				value += materialVariations.Length * knees.Length;

				foreach (WeaponCollection weapon in weapons)
				{
					value += weapon.objectVariations.Length * weapon.materialVariations.Length;
				}

				return value;
			}
		}

		public int MaxPossibleCombinations
		{
			get
			{
				return MaxPossibleMaleCombinations + MaxPossibleFemaleCombinations;
			}
		}

		public Transform parent;
		public string objectName = "New Character";
		public bool useRandomName = false;
		public string characterName = "Character name";
		public RandomCharacterName randomMaleNameGenerator;
		public RandomCharacterName randomFemaleNameGenerator;

		public TextAsset MaleNamesList;
		public TextAsset FemaleNamesList;

		public Sprite maleImage;
		public Sprite femaleImage;

		public bool useRandomImage;
		public List<Sprite> maleImages = new List<Sprite>();
		public List<Sprite> femaleImages = new List<Sprite>();

		public List<RandomCharacterStat> stats = new List<RandomCharacterStat>();

        #region Items
		

        [SerializeField] CharacterTemplate characterTemplate;
		
		public CharacterTemplate template
		{
			get=> characterTemplate;
			set
			{
				if (characterTemplate != value)
				{
					characterTemplate = value;
				}
			}
		}
        #endregion

        public CharacterVariations[] maleVariations;
		public CharacterVariations[] femaleVariations;
		[Space]
		public characterGenre genre;
		public RuntimeAnimatorController defaultController;

		public float minSize, maxSize;

		public Material[] materialVariations;
		public GameObject[] hairMaleVariations;
		public GameObject[] hairFemaleVariations;
		public desition haveBear = desition.Random;
		public GameObject[] bearVariations;

		[Header("Cloth Variations")]
		public desition useMask = desition.No;
		public GameObject[] maskVariations;

		public desition useBackPack = desition.No;
		public HumanBodyBones backpackParentBone = HumanBodyBones.UpperChest;
		public GameObject[] backpackVariations;
		bool attachInventory;
		public bool attachInventoryScript
		{
			get => attachInventory;
			set
			{
				attachInventory = value;
			}
		}

		/// <summary>
		/// The Inventory to be attached to the generated character
		/// </summary>
		public Component iScriptPrefab;

#if UNITY_EDITOR
		UnityEditor.MonoScript inventoryScript;
		public UnityEditor.MonoScript invScript
		{
			get => inventoryScript;
			set
			{
				if (inventoryScript != value)
				{
					inventoryScript = value;
					if (!value)
					{
						iScriptPrefab = null;
					}
				}
			}
		}
#endif

		public desition useHelmet = desition.No;
		public SimpleObjectCollection[] helmetVariation;

		public bool useNameTargeting = false;
		public desition useArmourChest = desition.Random;
		public SkinnedMeshRenderer[] maleArmourChest;
		public SkinnedMeshRenderer[] femaleArmourChest;

		#region Cloth variations
		public bool shirtNameTarget, pantsNameTarget, bootsNameTarget, glovesNameTarget;
		public desition useShirt, usePants, useBoots, useGloves;
		public SkinnedMeshRenderer[] maleShirts, malePants, maleBoots, maleGloves;
		public SkinnedMeshRenderer[] femaleShirts, femalePants, femaleBoots, femaleGloves;
		#endregion

		public desition useShouldersProtection = desition.Random;
		public DoubleObjectCollection[] shoulders;
		public desition useWristsProtection = desition.Random;
		public DoubleObjectCollection[] wrists;
		public desition useThighsProtection = desition.Random;
		public DoubleObjectCollection[] thighs;
		public desition useKneeProtection = desition.Random;
		public DoubleObjectCollection[] knees;

		public RandomCharacterCustomEvent events = new RandomCharacterCustomEvent();
		int selModule = 0;
		[SerializeField]public List<ModuleConainer> customBehavioursModules = new List<ModuleConainer> ();
		public ModuleConainer selectedModule
		{
			get => customBehavioursModules[selModule];
		}

		public int moduleCount;

		public desition hasWeapon = desition.No;
		public WeaponCollection[] weapons = new WeaponCollection[0];

		GameObject generatedCharacter;
		
		void RetargetMesh(SkinnedMeshRenderer sampleMesh, SkinnedMeshRenderer targetMesh, bool useNameRetargeting)
		{
			SkinnedMeshRenderer characterMesh = generatedCharacter.GetComponentInChildren<SkinnedMeshRenderer>();
			List<Transform> newBones = new List<Transform>();

			Animator sampleAnim = sampleMesh.GetComponent<Animator>();
			if (!sampleAnim)
			{
				if (sampleMesh.transform.parent != null)
				{
					sampleMesh.transform.parent.GetComponentInChildren<Animator>();
				}
			}
			Animator characterAnim = generatedCharacter.GetComponentInChildren<Animator>();

			int baseHierarchyCount = sampleMesh.transform.hierarchyCount;
			int targetHierarchyCount = characterMesh.transform.hierarchyCount;

			for (int i = 0; i < sampleMesh.bones.Length; i++)
			{
				if (!useNameRetargeting)
				{
					for (HumanBodyBones bone = HumanBodyBones.Hips; bone < HumanBodyBones.LastBone; bone++)
					{
						if (sampleMesh.bones[i] == sampleAnim.GetBoneTransform(bone))
						{
							newBones.Add(characterAnim.GetBoneTransform(bone));
							break;
						}
					}
				}
				else
				{
					string sampleName = sampleMesh.bones[i].name;

					foreach (Transform tm in characterMesh.bones)
					{
						string targetName = tm.name;
						if (sampleName == targetName)
						{
							newBones.Add(tm);
							break;
						}
					}
				}
			}

			targetMesh.bones = newBones.ToArray();
			targetMesh.rootBone = characterMesh.rootBone;
		}

		CharacterTag generationTag = CharacterTag.None;
		ObjectTag objectTag = ObjectTag.None;

		void PutArmor()
		{
			int shoulderIndex = Random.Range(0, shoulders.Length);
			int wristIndex = Random.Range(0, wrists.Length);
			int thighsIndex = Random.Range(0, thighs.Length);
			int kneesIndex = Random.Range(0, knees.Length);

			Animator anim = generatedCharacter.GetComponent<Animator>();

			desition selDesition = desition.No;

			if (shoulders.Length > 0)
			{
				if (useShouldersProtection == desition.Yes)
				{
					GameObject left = Instantiate(shoulders[shoulderIndex].LeftVariation, anim.GetBoneTransform(HumanBodyBones.LeftShoulder));
					left.transform.localPosition = template.L_ShoulderMarker.localPosition;
					left.transform.localRotation = template.L_ShoulderMarker.localRotation;

					GameObject right = Instantiate(shoulders[shoulderIndex].RightVariation, anim.GetBoneTransform(HumanBodyBones.RightShoulder));
					right.transform.localPosition = template.R_ShoulderMarker.localPosition;
					right.transform.localRotation = template.R_ShoulderMarker.localRotation;
				}

				if (useShouldersProtection == desition.Random)
				{
					selDesition = (desition)Mathf.RoundToInt(Random.value);
					if (selDesition == desition.Yes)
					{
						GameObject left = Instantiate(shoulders[shoulderIndex].LeftVariation, anim.GetBoneTransform(HumanBodyBones.LeftShoulder));
						left.transform.localPosition = template.L_ShoulderMarker.localPosition;
						left.transform.localRotation = template.L_ShoulderMarker.localRotation;

						GameObject right = Instantiate(shoulders[shoulderIndex].RightVariation, anim.GetBoneTransform(HumanBodyBones.RightShoulder));
						right.transform.localPosition = template.R_ShoulderMarker.localPosition;
						right.transform.localRotation = template.R_ShoulderMarker.localRotation;
					}
				}
			}

			//-----------------------

			if (thighs.Length > 0)
			{
				if (useThighsProtection == desition.Yes)
				{
					GameObject left = Instantiate(thighs[thighsIndex].LeftVariation, anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg));
					left.transform.localPosition = template.L_ThighMarker.localPosition;
					left.transform.localRotation = template.L_ThighMarker.localRotation;

					GameObject right = Instantiate(thighs[thighsIndex].RightVariation, anim.GetBoneTransform(HumanBodyBones.RightUpperLeg));
					right.transform.localPosition = template.R_ThighMarker.localPosition;
					right.transform.localRotation = template.R_ThighMarker.localRotation;
				}

				if (useThighsProtection == desition.Random)
				{
					selDesition = (desition)Mathf.RoundToInt(Random.value);
					if (selDesition == desition.Yes)
					{
						GameObject left = Instantiate(thighs[thighsIndex].LeftVariation, anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg));
						left.transform.localPosition = template.L_ThighMarker.localPosition;
						left.transform.localRotation = template.L_ThighMarker.localRotation;

						GameObject right = Instantiate(thighs[thighsIndex].RightVariation, anim.GetBoneTransform(HumanBodyBones.RightUpperLeg));
						right.transform.localPosition = template.R_ThighMarker.localPosition;
						right.transform.localRotation = template.R_ThighMarker.localRotation;
					}
				}
			}

			//----------------------------------

			if (wrists.Length > 0)
			{
				if (useWristsProtection == desition.Yes)
				{
					GameObject left = Instantiate(wrists[wristIndex].LeftVariation, anim.GetBoneTransform(HumanBodyBones.LeftLowerArm));
					left.transform.localPosition = template.L_WristMarker.localPosition;
					left.transform.localRotation = template.L_WristMarker.localRotation;

					GameObject right = Instantiate(wrists[wristIndex].RightVariation, anim.GetBoneTransform(HumanBodyBones.RightLowerArm));
					right.transform.localPosition = template.R_WristMarker.localPosition;
					right.transform.localRotation = template.R_WristMarker.localRotation;
				}

				if (useWristsProtection == desition.Random)
				{
					selDesition = (desition)Mathf.RoundToInt(Random.value);
					if (selDesition == desition.Yes)
					{
						GameObject left = Instantiate(wrists[wristIndex].LeftVariation, anim.GetBoneTransform(HumanBodyBones.LeftLowerArm));
						left.transform.localPosition = template.L_WristMarker.localPosition;
						left.transform.localRotation = template.L_WristMarker.localRotation;

						GameObject right = Instantiate(wrists[wristIndex].RightVariation, anim.GetBoneTransform(HumanBodyBones.RightLowerArm));
						right.transform.localPosition = template.R_WristMarker.localPosition;
						right.transform.localRotation = template.R_WristMarker.localRotation;
					}
				}
			}

			//----------------------------------

			if (knees.Length > 0)
			{
				if (useKneeProtection == desition.Yes)
				{
					GameObject left = Instantiate(knees[kneesIndex].LeftVariation, anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg));
					left.transform.localPosition = template.L_KneeMarker.localPosition;
					left.transform.localRotation = template.L_KneeMarker.localRotation;

					GameObject right = Instantiate(knees[kneesIndex].RightVariation, anim.GetBoneTransform(HumanBodyBones.RightLowerLeg));
					right.transform.localPosition = template.R_KneeMarker.localPosition;
					right.transform.localRotation = template.R_KneeMarker.localRotation;
				}

				if (useKneeProtection == desition.Random)
				{
					selDesition = (desition)Mathf.RoundToInt(Random.value);
					if (selDesition == desition.Yes)
					{
						GameObject left = Instantiate(knees[kneesIndex].LeftVariation, anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg));
						left.transform.localPosition = template.L_KneeMarker.localPosition;
						left.transform.localRotation = template.L_KneeMarker.localRotation;

						GameObject right = Instantiate(knees[kneesIndex].RightVariation, anim.GetBoneTransform(HumanBodyBones.RightLowerLeg));
						right.transform.localPosition = template.R_KneeMarker.localPosition;
						right.transform.localRotation = template.R_KneeMarker.localRotation;
					}
				}
			}
		}

		void PutWeapon()
		{
			if (weapons.Length > 0)
			{
				Animator anim = generatedCharacter.GetComponent<Animator>();

				int collection = Random.Range(0, weapons.Length);
				int weaponIndex = Random.Range(0, weapons[collection].objectVariations.Length);
				if (hasWeapon == desition.Yes)
				{
					WeaponController weapon = weapons[collection].objectVariations[weaponIndex];
					WeaponController weaponL = null, weaponR = null;
					switch (weapon.handling)
					{
						case WeaponController.WeaponHandle.Left:
							weaponL = Instantiate<WeaponController>(weapon, anim.GetBoneTransform(HumanBodyBones.LeftHand));

							weaponL.transform.localPosition = template.L_HandMarker.localPosition;
							weaponL.transform.localRotation = template.L_HandMarker.localRotation;
							break;

						case WeaponController.WeaponHandle.Right:
							weaponR = Instantiate<WeaponController>(weapon, anim.GetBoneTransform(HumanBodyBones.RightHand));

							weaponR.transform.localPosition = template.R_HandMarker.localPosition;
							weaponR.transform.localRotation = template.R_HandMarker.localRotation;
							break;

						case WeaponController.WeaponHandle.TwoHanded:
							weaponL = Instantiate<WeaponController>(weapon, anim.GetBoneTransform(HumanBodyBones.LeftHand));
							weaponL.transform.localPosition = template.L_HandMarker.localPosition;
							weaponL.transform.localRotation = template.L_HandMarker.localRotation;

							weaponR = Instantiate<WeaponController>(weapon, anim.GetBoneTransform(HumanBodyBones.RightHand));
							weaponR.transform.localPosition = template.R_HandMarker.localPosition;
							weaponR.transform.localRotation = template.R_HandMarker.localRotation;
							break;
					}
					Renderer[] renderers = new Renderer[0];
					if (weaponL)
					{
						renderers = weaponL.GetComponentsInChildren<Renderer>();
						int randomMaterial = Random.Range(0, weapons[collection].materialVariations.Length);
						foreach (Renderer r in renderers)
						{
							r.material = weapons[collection].materialVariations[randomMaterial];
						}
					}
					if (weaponR)
					{
						renderers = weaponR.GetComponentsInChildren<Renderer>();
						int randomMaterial = Random.Range(0, weapons[collection].materialVariations.Length);
						foreach (Renderer r in renderers)
						{
							r.material = weapons[collection].materialVariations[randomMaterial];
						}
					}
					generatedCharacter.GetComponentInChildren<Animator>().runtimeAnimatorController = weapons[collection].controller;
				}
				desition selDesition = desition.No;
				if (hasWeapon == desition.Random)
				{
					selDesition = (desition)Mathf.RoundToInt(Random.value);
					if (selDesition == desition.Yes)
					{
						WeaponController weapon = weapons[collection].objectVariations[weaponIndex];
						WeaponController weaponL = null, weaponR = null;
						switch (weapon.handling)
						{
							case WeaponController.WeaponHandle.Left:
								weaponL = Instantiate<WeaponController>(weapon, anim.GetBoneTransform(HumanBodyBones.LeftHand));

								weaponL.transform.localPosition = template.L_HandMarker.localPosition;
								weaponL.transform.localRotation = template.L_HandMarker.localRotation;
								break;

							case WeaponController.WeaponHandle.Right:
								weaponR = Instantiate<WeaponController>(weapon, anim.GetBoneTransform(HumanBodyBones.RightHand));

								weaponR.transform.localPosition = template.R_HandMarker.localPosition;
								weaponR.transform.localRotation = template.R_HandMarker.localRotation;
								break;

							case WeaponController.WeaponHandle.TwoHanded:
								weaponL = Instantiate<WeaponController>(weapon, anim.GetBoneTransform(HumanBodyBones.LeftHand));
								weaponL.transform.localPosition = template.L_HandMarker.localPosition;
								weaponL.transform.localRotation = template.L_HandMarker.localRotation;

								weaponR = Instantiate<WeaponController>(weapon, anim.GetBoneTransform(HumanBodyBones.RightHand));
								weaponR.transform.localPosition = template.R_HandMarker.localPosition;
								weaponR.transform.localRotation = template.R_HandMarker.localRotation;
								break;
						}
						Renderer[] renderers = new Renderer[0];
						if (weaponL)
						{
							renderers = weaponL.GetComponentsInChildren<Renderer>();
							int randomMaterial = Random.Range(0, weapons[collection].materialVariations.Length);
							foreach (Renderer r in renderers)
							{
								r.material = weapons[collection].materialVariations[randomMaterial];
							}
						}
						if (weaponR)
						{
							renderers = weaponR.GetComponentsInChildren<Renderer>();
							int randomMaterial = Random.Range(0, weapons[collection].materialVariations.Length);
							foreach (Renderer r in renderers)
							{
								r.material = weapons[collection].materialVariations[randomMaterial];
							}
						}
						generatedCharacter.GetComponentInChildren<Animator>().runtimeAnimatorController = weapons[collection].controller;
					}
				}
			}
		}

		void PutShirt(characterGenre genre)
		{
			desition selDesition = useShirt;
			Animator anim = generatedCharacter.GetComponent<Animator>();

			if (genre == characterGenre.Female)
			{
				if (femaleShirts.Length == 0)
					return;

				if (selDesition == desition.Yes)
				{
					int shirtIndex = Random.Range(0, femaleShirts.Length);
					SkinnedMeshRenderer shirt = Instantiate<SkinnedMeshRenderer>(femaleShirts[shirtIndex]);
					shirt.transform.parent = generatedCharacter.transform;
					RetargetMesh(femaleShirts[shirtIndex], shirt, shirtNameTarget);
					shirt.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
				}
				if (selDesition == desition.Random)
				{
					selDesition = (desition)Mathf.RoundToInt(Random.value);
					if (selDesition == desition.Yes)
					{
						int shirtIndex = Random.Range(0, femaleShirts.Length);
						SkinnedMeshRenderer shirt = Instantiate<SkinnedMeshRenderer>(femaleShirts[shirtIndex]);
						shirt.transform.parent = generatedCharacter.transform;
						RetargetMesh(femaleShirts[shirtIndex], shirt, shirtNameTarget);
						shirt.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
					}
				}
			}

			if (genre == characterGenre.Male)
			{
				if (maleShirts.Length == 0)
					return;

				if (selDesition == desition.Yes)
				{
					int shirtIndex = Random.Range(0, maleShirts.Length);
					SkinnedMeshRenderer shirt = Instantiate<SkinnedMeshRenderer>(maleShirts[shirtIndex]);
					shirt.transform.parent = generatedCharacter.transform;
					RetargetMesh(maleShirts[shirtIndex], shirt, shirtNameTarget);
					shirt.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
				}
				if (selDesition == desition.Random)
				{
					selDesition = (desition)Mathf.RoundToInt(Random.value);
					if (selDesition == desition.Yes)
					{
						int shirtIndex = Random.Range(0, maleShirts.Length);
						SkinnedMeshRenderer shirt = Instantiate<SkinnedMeshRenderer>(maleShirts[shirtIndex]);
						shirt.transform.parent = generatedCharacter.transform;
						RetargetMesh(maleShirts[shirtIndex], shirt, shirtNameTarget);
						shirt.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
					}
				}
			}
		}

		void PutPants(characterGenre genre)
		{
			desition selDesition = usePants;
			Animator anim = generatedCharacter.GetComponent<Animator>();

			if (genre == characterGenre.Female)
			{
				if (femalePants.Length == 0)
					return;

				if (selDesition == desition.Yes)
				{
					int pantsIndex = Random.Range(0, femalePants.Length);
					SkinnedMeshRenderer shirt = Instantiate<SkinnedMeshRenderer>(femalePants[pantsIndex]);
					shirt.transform.parent = generatedCharacter.transform;
					RetargetMesh(femalePants[pantsIndex], shirt, pantsNameTarget);
					shirt.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
				}
				if (selDesition == desition.Random)
				{
					selDesition = (desition)Mathf.RoundToInt(Random.value);
					if (selDesition == desition.Yes)
					{
						int pantsIndex = Random.Range(0, femalePants.Length);
						SkinnedMeshRenderer pants = Instantiate<SkinnedMeshRenderer>(femalePants[pantsIndex]);
						pants.transform.parent = generatedCharacter.transform;
						RetargetMesh(femalePants[pantsIndex], pants, pantsNameTarget);
						pants.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
					}
				}
			}

			if (genre == characterGenre.Male)
			{
				if (malePants.Length == 0)
					return;

				if (selDesition == desition.Yes)
				{
					int shirtIndex = Random.Range(0, malePants.Length);
					SkinnedMeshRenderer shirt = Instantiate<SkinnedMeshRenderer>(malePants[shirtIndex]);
					shirt.transform.parent = generatedCharacter.transform;
					RetargetMesh(malePants[shirtIndex], shirt, pantsNameTarget);
					shirt.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
				}
				if (selDesition == desition.Random)
				{
					selDesition = (desition)Mathf.RoundToInt(Random.value);
					if (selDesition == desition.Yes)
					{
						int pantsIndex = Random.Range(0, malePants.Length);
						SkinnedMeshRenderer pants = Instantiate<SkinnedMeshRenderer>(malePants[pantsIndex]);
						pants.transform.parent = generatedCharacter.transform;
						RetargetMesh(malePants[pantsIndex], pants, pantsNameTarget);
						pants.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
					}
				}
			}
		}
		void PutBoots(characterGenre genre)
		{
			desition selDesition = usePants;
			Animator anim = generatedCharacter.GetComponent<Animator>();

			if (genre == characterGenre.Female)
			{
				if (femaleBoots.Length == 0)
					return;

				if (selDesition == desition.Yes)
				{
					int bootIndex = Random.Range(0, femaleBoots.Length);
					SkinnedMeshRenderer pants = Instantiate<SkinnedMeshRenderer>(femaleBoots[bootIndex]);
					pants.transform.parent = generatedCharacter.transform;
					RetargetMesh(femaleBoots[bootIndex], pants, bootsNameTarget);
					pants.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
				}
				if (selDesition == desition.Random)
				{
					selDesition = (desition)Mathf.RoundToInt(Random.value);
					if (selDesition == desition.Yes)
					{
						int bootIndex = Random.Range(0, femaleBoots.Length);
						SkinnedMeshRenderer boots = Instantiate<SkinnedMeshRenderer>(femaleBoots[bootIndex]);
						boots.transform.parent = generatedCharacter.transform;
						RetargetMesh(femaleBoots[bootIndex], boots, bootsNameTarget);
						boots.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
					}
				}
			}

			if (genre == characterGenre.Male)
			{
				if (maleBoots.Length == 0)
					return;

				if (selDesition == desition.Yes)
				{
					int bootIndex = Random.Range(0, maleBoots.Length);
					SkinnedMeshRenderer boots = Instantiate<SkinnedMeshRenderer>(maleBoots[bootIndex]);
					boots.transform.parent = generatedCharacter.transform;
					RetargetMesh(maleBoots[bootIndex], boots, bootsNameTarget);
					boots.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
				}
				if (selDesition == desition.Random)
				{
					selDesition = (desition)Mathf.RoundToInt(Random.value);
					if (selDesition == desition.Yes)
					{
						int bootIndex = Random.Range(0, maleBoots.Length);
						SkinnedMeshRenderer boots = Instantiate<SkinnedMeshRenderer>(maleBoots[bootIndex]);
						boots.transform.parent = generatedCharacter.transform;
						RetargetMesh(maleBoots[bootIndex], boots, bootsNameTarget);
						boots.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
					}
				}
			}
		}
		void PutGloves(characterGenre genre)
		{
			desition selDesition = useGloves;
			Animator anim = generatedCharacter.GetComponent<Animator>();

			if (genre == characterGenre.Female)
			{
				if (femaleGloves.Length == 0)
					return;

				if (selDesition == desition.Yes)
				{
					int glovesIndex = Random.Range(0, femaleGloves.Length);
					SkinnedMeshRenderer gloves = Instantiate<SkinnedMeshRenderer>(femaleGloves[glovesIndex]);
					gloves.transform.parent = generatedCharacter.transform;
					RetargetMesh(femaleGloves[glovesIndex], gloves, glovesNameTarget);
					gloves.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
				}
				if (selDesition == desition.Random)
				{
					selDesition = (desition)Mathf.RoundToInt(Random.value);
					if (selDesition == desition.Yes)
					{
						int glovesIndex = Random.Range(0, femaleGloves.Length);
						SkinnedMeshRenderer gloves = Instantiate<SkinnedMeshRenderer>(femaleGloves[glovesIndex]);
						gloves.transform.parent = generatedCharacter.transform;
						RetargetMesh(femaleGloves[glovesIndex], gloves, glovesNameTarget);
						gloves.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
					}
				}
			}

			if (genre == characterGenre.Male)
			{
				if (maleGloves.Length == 0)
					return;

				if (selDesition == desition.Yes)
				{
					int bootIndex = Random.Range(0, maleBoots.Length);
					SkinnedMeshRenderer boots = Instantiate<SkinnedMeshRenderer>(maleBoots[bootIndex]);
					boots.transform.parent = generatedCharacter.transform;
					RetargetMesh(maleBoots[bootIndex], boots, glovesNameTarget);
					boots.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
				}
				if (selDesition == desition.Random)
				{
					selDesition = (desition)Mathf.RoundToInt(Random.value);
					if (selDesition == desition.Yes)
					{
						int bootIndex = Random.Range(0, maleBoots.Length);
						SkinnedMeshRenderer boots = Instantiate<SkinnedMeshRenderer>(maleBoots[bootIndex]);
						boots.transform.parent = generatedCharacter.transform;
						RetargetMesh(maleBoots[bootIndex], boots, glovesNameTarget);
						boots.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
					}
				}
			}
		}

		void PutItems ()
		{
			foreach (CharacterTemplate.ItemCollection item in template.itemsMarkers)
			{
				if (item.items.Count > 0)
				{
					int itemIndex = Random.Range(0, item.items.Count);
					Transform parent = generatedCharacter.GetComponent<Animator>().GetBoneTransform(item.boneParent);
					if (parent)
					{
						GameObject newItem = Instantiate(item.items[itemIndex], parent);
						newItem.transform.localPosition = Vector3.zero;
						newItem.transform.localRotation = Quaternion.identity;
					}
				}
			}
		}

		/// <summary>
		/// Generate an unparent character
		/// </summary>
		/// <param name="position"></param>
		/// <param name="useRandomRotation"></param>
		public GameObject Generate(Vector3 position, bool useRandomRotation = false)
		{
			#region Character body placement
			int characterCollection = 0;
			//Generating the base character body
			characterGenre selGenre = genre;
			if (selGenre == characterGenre.Random)//we check if it's a random sex
				selGenre = (characterGenre)Mathf.RoundToInt(Random.value);//if it't true we assign then a defined sex

			if (selGenre == characterGenre.Female)
			{
				if (femaleVariations.Length > 0)
				{
					characterCollection = Random.Range(0, femaleVariations.Length);
					int characterIndex = Random.Range(0, femaleVariations[characterCollection].characterVariations.Length);
					GameObject selCharacter = femaleVariations[characterCollection].characterVariations[characterIndex];
					Quaternion rotation = Quaternion.identity;
					if (useRandomRotation)
					{
						rotation = Quaternion.Euler(0, Random.Range(-360f, 360f), 0);
					}
					generatedCharacter = Instantiate(selCharacter, position, rotation);
				}
				else
				{
					selGenre = characterGenre.Male;
				}
			}

			if (selGenre == characterGenre.Male)
			{
				if (maleVariations.Length > 0)
				{
					characterCollection = Random.Range(0, maleVariations.Length);
					int characterIndex = Random.Range(0, maleVariations[characterCollection].characterVariations.Length);
					GameObject selCharacter = maleVariations[characterCollection].characterVariations[characterIndex];
					Quaternion rotation = Quaternion.identity;
					if (useRandomRotation)
					{
						rotation = Quaternion.Euler(0, Random.Range(-360f, 360f), 0);
					}
					generatedCharacter = Instantiate(selCharacter, position, rotation);
				}
			}
			#endregion

			if (!generatedCharacter)//we check if the character exist in the world
			{
				Debug.LogError("The character variations are empties");
				return null;//if not we return to avoid errors
			}

			//the animator of the generated character
			//every one most have it
			Animator anim = generatedCharacter.GetComponent<Animator>();
			anim.runtimeAnimatorController = defaultController;

			desition selDesition = useHelmet;

			#region Helmet and Hair
			if (selDesition == desition.Random)
				selDesition = (desition)Mathf.RoundToInt(Random.value);

			if (selDesition == desition.No)
			{
				if (selGenre == characterGenre.Female && hairFemaleVariations.Length > 0)
				{
					int hairIndex = Random.Range(0, hairFemaleVariations.Length);
					GameObject hair = Instantiate(hairFemaleVariations[hairIndex], anim.GetBoneTransform(HumanBodyBones.Head));
				}
				if (selGenre == characterGenre.Male && hairMaleVariations.Length > 0)
				{
					int hairIndex = Random.Range(0, hairMaleVariations.Length);
					Instantiate(hairMaleVariations[hairIndex], anim.GetBoneTransform(HumanBodyBones.Head));
				}
			}
			if (selDesition == desition.Yes && helmetVariation.Length > 0)
			{
				int usedCollection = Random.Range(0, helmetVariation.Length);
				int helmetIndex = Random.Range(0, helmetVariation[usedCollection].objectVariations.Length);
				Instantiate(helmetVariation[usedCollection].objectVariations[helmetIndex], anim.GetBoneTransform(HumanBodyBones.Head));
			}
			else if (selDesition == desition.Yes)
			{
				if (selGenre == characterGenre.Female && hairFemaleVariations.Length > 0)
				{
					int hairIndex = Random.Range(0, hairFemaleVariations.Length);
					GameObject hair = Instantiate(hairFemaleVariations[hairIndex], anim.GetBoneTransform(HumanBodyBones.Head));
				}
				if (selGenre == characterGenre.Male && hairMaleVariations.Length > 0)
				{
					int hairIndex = Random.Range(0, hairMaleVariations.Length);
					Instantiate(hairMaleVariations[hairIndex], anim.GetBoneTransform(HumanBodyBones.Head));
				}
			}
			#endregion

			#region Mask and Beard
			selDesition = useMask;
			if (selDesition == desition.Random)
				selDesition = (desition)Mathf.RoundToInt(Random.value);

			if (selDesition == desition.No && selGenre == characterGenre.Male)
			{
				desition beardDesition = haveBear;
				if (beardDesition == desition.Random)
					beardDesition = (desition)Mathf.RoundToInt(Random.value);

				if (beardDesition == desition.Yes && bearVariations.Length > 0)
				{
					int beardIndex = Random.Range(0, bearVariations.Length);
					Instantiate(bearVariations[beardIndex], anim.GetBoneTransform(HumanBodyBones.Head));
				}
			}
			if (selDesition == desition.Yes && maskVariations.Length > 0)
			{
				int maskIndex = Random.Range(0, maskVariations.Length);
				Instantiate(maskVariations[maskIndex], anim.GetBoneTransform(HumanBodyBones.Head));
			}
			else if (selDesition == desition.Yes && selGenre == characterGenre.Male)
			{
				desition beardDesition = haveBear;
				if (beardDesition == desition.Random)
					beardDesition = (desition)Mathf.RoundToInt(Random.value);

				if (beardDesition == desition.Yes && bearVariations.Length > 0)
				{
					int beardIndex = Random.Range(0, bearVariations.Length);
					Instantiate(bearVariations[beardIndex], anim.GetBoneTransform(HumanBodyBones.Head));
				}
			}
			#endregion

			#region Armor Placing
			selDesition = useArmourChest;
			if (selDesition == desition.Random)
				selDesition = (desition)Mathf.RoundToInt(Random.value);

			if (selDesition == desition.Yes)
			{
				if (selGenre == characterGenre.Female && femaleArmourChest.Length > 0)
				{
					int armorIndex = Random.Range(0, femaleArmourChest.Length);
					SkinnedMeshRenderer armorChest = Instantiate<SkinnedMeshRenderer>(femaleArmourChest[armorIndex], generatedCharacter.transform);
					RetargetMesh(femaleArmourChest[armorIndex], armorChest, useNameTargeting);
					armorChest.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
				}
				if (selGenre == characterGenre.Male && maleArmourChest.Length > 0)
				{
					int armorIndex = Random.Range(0, maleArmourChest.Length);
					SkinnedMeshRenderer armorChest = Instantiate<SkinnedMeshRenderer>(maleArmourChest[armorIndex], generatedCharacter.transform);
					RetargetMesh(maleArmourChest[armorIndex], armorChest, useNameTargeting);
					armorChest.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
				}
			}

			PutArmor();
            #endregion

            #region Assign Backpack
            selDesition = useBackPack;
			if (selDesition == desition.Random)
				selDesition = (desition)Mathf.RoundToInt(Random.value);

			if (selDesition == desition.Yes && backpackVariations.Length > 0)
			{
				Transform parent = anim.GetBoneTransform(backpackParentBone);
				int backpackIndex = Random.Range(0, backpackVariations.Length);
				GameObject backpack = Instantiate(backpackVariations[backpackIndex], parent);
			}
			if (attachInventory && iScriptPrefab)
			{
				generatedCharacter.AddComponent(iScriptPrefab);
			}
			#endregion

			#region Assign the items to the character
			PutItems();
            #endregion

            #region Assign the materials
            int materialIndex = Random.Range(0, materialVariations.Length);
			Renderer[] meshes = generatedCharacter.GetComponentsInChildren<Renderer>();
			foreach (Renderer mesh in meshes)
			{
				if (mesh.GetType() == typeof(SkinnedMeshRenderer))
				{
					if (selGenre == characterGenre.Female)
					{
						int characterMaterial = Random.Range(0, femaleVariations[characterCollection].materialVariations.Length);
						mesh.material = femaleVariations[characterCollection].materialVariations[characterMaterial];
						continue;
					}
					if (selGenre == characterGenre.Male)
					{
						int characterMaterial = Random.Range(0, maleVariations[characterCollection].materialVariations.Length);
						mesh.material = maleVariations[characterCollection].materialVariations[characterMaterial];
						continue;
					}
				}
				mesh.material = materialVariations[materialIndex];
			}
			#endregion

			PutWeapon();//put a weapon on his hands
			PutShirt(selGenre);
			PutBoots(selGenre);
			PutGloves(selGenre);
			PutPants(selGenre);

			#region Module Configuration
			selModule = Random.Range(0, moduleCount);

			if (selectedModule.moduleUse == ModuleUse.CopyComponents && selectedModule.setModule)
			{
				Component[] components = selectedModule.setModule.GetComponents<Component>();
				foreach (Component component in components)
				{
					if (component.GetType() == typeof(Transform))
						continue;

					if (component.GetType() == typeof(Animator))
						continue;

					Component existing = generatedCharacter.GetComponent(component.GetType());

					if (existing)
					{
						existing.CopyComponent(component);
					}
					else
					{
						generatedCharacter.AddComponent(component);
					}
				}

				ExportData(generatedCharacter, selGenre);

				return generatedCharacter;
			}

			if (selectedModule.moduleUse == ModuleUse.CopyHierarchy && selectedModule.setModule)
			{
				GameObject container = Instantiate(selectedModule.setModule, position, generatedCharacter.transform.rotation);
				while (container.transform.childCount > 0)
				{
					container.transform.GetChild(0).parent = generatedCharacter.transform;
				}

				Component[] components = selectedModule.setModule.GetComponents<Component>();
				foreach (Component component in components)
				{
					if (component.GetType() == typeof(Transform))
						continue;

					if (component.GetType() == typeof(Animator))
						continue;

					Component existing = generatedCharacter.GetComponent(component.GetType());

					if (existing)
					{
						existing.CopyComponent(component);
					}
					else
					{
						generatedCharacter.AddComponent(component);
					}
				}
				DestroyImmediate(container);

				ExportData(generatedCharacter, selGenre);

				return generatedCharacter;
			}

			if (selectedModule.moduleUse == ModuleUse.ParentContainer && selectedModule.setModule)
			{
				GameObject container = Instantiate(selectedModule.setModule, position, generatedCharacter.transform.rotation);
				if (selectedModule.parent)
				{
					generatedCharacter.transform.parent = container.transform.Find(selectedModule.parent.name);
				}
				else
				{
					generatedCharacter.transform.parent = container.transform;
				}

				if (container.GetComponent<Animator>())
				{
					if (selectedModule.useContainerAnimator)
					{
						container.GetComponent<Animator>().CopyComponent(generatedCharacter.GetComponent<Animator>());
						DestroyImmediate(generatedCharacter.GetComponent<Animator>());
					}
				}

				ExportData(generatedCharacter, selGenre);

				return container;
			}
			#endregion

			ExportData(generatedCharacter, selGenre);

			return generatedCharacter;
		}

		/// <summary>
		/// Generate a character child of a given transform
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="useRandomRotation"></param>
		public GameObject Generate(Transform transform)
		{
			#region Character body placement
			int characterCollection = 0;
			//Generating the base character body
			characterGenre selGenre = genre;
			if (selGenre == characterGenre.Random)//we check if it's a random sex
				selGenre = (characterGenre)Mathf.RoundToInt(Random.value);//if it't true we assign then a defined sex

			if (selGenre == characterGenre.Female)
			{
				if (femaleVariations.Length > 0)
				{
					characterCollection = Random.Range(0, femaleVariations.Length);
					int characterIndex = Random.Range(0, femaleVariations[characterCollection].characterVariations.Length);
					GameObject selCharacter = femaleVariations[characterCollection].characterVariations[characterIndex];
					generatedCharacter = Instantiate(selCharacter, transform.position, Quaternion.identity, transform);
				}
				else
				{
					selGenre = characterGenre.Male;
				}
			}

			if (selGenre == characterGenre.Male)
			{
				if (maleVariations.Length > 0)
				{
					characterCollection = Random.Range(0, maleVariations.Length);
					int characterIndex = Random.Range(0, maleVariations[characterCollection].characterVariations.Length);
					GameObject selCharacter = maleVariations[characterCollection].characterVariations[characterIndex];
					generatedCharacter = Instantiate(selCharacter, transform.position, Quaternion.identity, transform);
				}
			}
			#endregion

			if (!generatedCharacter)//we check if the character exist in the world
			{
				Debug.LogError("The character variations are empties");
				return null;//if not we return to avoid errors
			}

			//the animator of the generated character
			//every one most have it
			Animator anim = generatedCharacter.GetComponent<Animator>();

			desition selDesition = useHelmet;

			#region Helmet and Hair
			if (selDesition == desition.Random)
				selDesition = (desition)Mathf.RoundToInt(Random.value);

			if (selDesition == desition.No)
			{
				if (selGenre == characterGenre.Female && hairFemaleVariations.Length > 0)
				{
					int hairIndex = Random.Range(0, hairFemaleVariations.Length);
					GameObject hair = Instantiate(hairFemaleVariations[hairIndex], anim.GetBoneTransform(HumanBodyBones.Head));
				}
				if (selGenre == characterGenre.Male && hairMaleVariations.Length > 0)
				{
					int hairIndex = Random.Range(0, hairMaleVariations.Length);
					Instantiate(hairMaleVariations[hairIndex], anim.GetBoneTransform(HumanBodyBones.Head));
				}
			}
			if (selDesition == desition.Yes && helmetVariation.Length > 0)
			{
				int usedCollection = Random.Range(0, helmetVariation.Length);
				int helmetIndex = Random.Range(0, helmetVariation[usedCollection].objectVariations.Length);
				Instantiate(helmetVariation[usedCollection].objectVariations[helmetIndex], anim.GetBoneTransform(HumanBodyBones.Head));
			}
			else if (selDesition == desition.Yes)
			{
				if (selGenre == characterGenre.Female && hairFemaleVariations.Length > 0)
				{
					int hairIndex = Random.Range(0, hairFemaleVariations.Length);
					GameObject hair = Instantiate(hairFemaleVariations[hairIndex], anim.GetBoneTransform(HumanBodyBones.Head));
				}
				if (selGenre == characterGenre.Male && hairMaleVariations.Length > 0)
				{
					int hairIndex = Random.Range(0, hairMaleVariations.Length);
					Instantiate(hairMaleVariations[hairIndex], anim.GetBoneTransform(HumanBodyBones.Head));
				}
			}
			#endregion

			#region Mask and Beard
			selDesition = useMask;
			if (selDesition == desition.Random)
				selDesition = (desition)Mathf.RoundToInt(Random.value);

			if (selDesition == desition.No && selGenre == characterGenre.Male)
			{
				desition beardDesition = haveBear;
				if (beardDesition == desition.Random)
					beardDesition = (desition)Mathf.RoundToInt(Random.value);

				if (beardDesition == desition.Yes && bearVariations.Length > 0)
				{
					int beardIndex = Random.Range(0, bearVariations.Length);
					Instantiate(bearVariations[beardIndex], anim.GetBoneTransform(HumanBodyBones.Head));
				}
			}
			if (selDesition == desition.Yes && maskVariations.Length > 0)
			{
				int maskIndex = Random.Range(0, maskVariations.Length);
				Instantiate(maskVariations[maskIndex], anim.GetBoneTransform(HumanBodyBones.Head));
			}
			else if (selDesition == desition.Yes && selGenre == characterGenre.Male)
			{
				desition beardDesition = haveBear;
				if (beardDesition == desition.Random)
					beardDesition = (desition)Mathf.RoundToInt(Random.value);

				if (beardDesition == desition.Yes && bearVariations.Length > 0)
				{
					int beardIndex = Random.Range(0, bearVariations.Length);
					Instantiate(bearVariations[beardIndex], anim.GetBoneTransform(HumanBodyBones.Head));
				}
			}
			#endregion

			#region Armor Placing
			selDesition = useArmourChest;
			if (selDesition == desition.Random)
				selDesition = (desition)Mathf.RoundToInt(Random.value);

			if (selDesition == desition.Yes)
			{
				if (selGenre == characterGenre.Female && femaleArmourChest.Length > 0)
				{
					int armorIndex = Random.Range(0, femaleArmourChest.Length);
					SkinnedMeshRenderer armorChest = Instantiate<SkinnedMeshRenderer>(femaleArmourChest[armorIndex], generatedCharacter.transform);
					RetargetMesh(femaleArmourChest[armorIndex], armorChest, useNameTargeting);
					armorChest.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
				}
				if (selGenre == characterGenre.Male && maleArmourChest.Length > 0)
				{
					int armorIndex = Random.Range(0, maleArmourChest.Length);
					SkinnedMeshRenderer armorChest = Instantiate<SkinnedMeshRenderer>(maleArmourChest[armorIndex], generatedCharacter.transform);
					RetargetMesh(maleArmourChest[armorIndex], armorChest, useNameTargeting);
					armorChest.rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
				}
			}

			PutArmor();
			#endregion

			#region Assign Backpack
			selDesition = useBackPack;
			if (selDesition == desition.Random)
				selDesition = (desition)Mathf.RoundToInt(Random.value);

			if (selDesition == desition.Yes && backpackVariations.Length > 0)
			{
				Transform parent = anim.GetBoneTransform(backpackParentBone);
				int backpackIndex = Random.Range(0, backpackVariations.Length);
				GameObject backpack = Instantiate(backpackVariations[backpackIndex], parent);
				if (attachInventory && iScriptPrefab)
				{
					generatedCharacter.AddComponent(iScriptPrefab);
				}
			}
			#endregion

			#region Assign the materials
			int materialIndex = Random.Range(0, materialVariations.Length);
			Renderer[] meshes = generatedCharacter.GetComponentsInChildren<Renderer>();
			foreach (Renderer mesh in meshes)
			{
				if (mesh.GetType() == typeof(SkinnedMeshRenderer))
				{
					if (selGenre == characterGenre.Female)
					{
						int characterMaterial = Random.Range(0, femaleVariations[characterCollection].materialVariations.Length);
						mesh.material = femaleVariations[characterCollection].materialVariations[characterMaterial];
						continue;
					}
					if (selGenre == characterGenre.Male)
					{
						int characterMaterial = Random.Range(0, maleVariations[characterCollection].materialVariations.Length);
						mesh.material = maleVariations[characterCollection].materialVariations[characterMaterial];
						continue;
					}
				}
				mesh.material = materialVariations[materialIndex];
			}
			#endregion

			PutWeapon();//put a weapon on his hands
			PutShirt(selGenre);
			PutBoots(selGenre);
			PutGloves(selGenre);
			PutPants(selGenre);

			#region Module Configuration
			selModule = Random.Range(0, moduleCount);

			if (selectedModule.moduleUse == ModuleUse.CopyComponents && selectedModule.setModule)
			{
				Component[] components = selectedModule.setModule.GetComponents<Component>();
				foreach (Component component in components)
				{
					if (component.GetType() == typeof(Transform))
						continue;

					if (component.GetType() == typeof(Animator))
						continue;

					Component existing = generatedCharacter.GetComponent(component.GetType());

					if (existing)
					{
						existing.CopyComponent(component);
					}
					else
					{
						generatedCharacter.AddComponent(component);
					}
				}

				ExportData(generatedCharacter, selGenre);

				return generatedCharacter;
			}

			if (selectedModule.moduleUse == ModuleUse.CopyHierarchy && selectedModule.setModule)
			{
				GameObject container = Instantiate(selectedModule.setModule, transform.position, transform.rotation);
				while (container.transform.childCount > 0)
				{
					container.transform.GetChild(0).parent = generatedCharacter.transform;
				}

				Component[] components = selectedModule.setModule.GetComponents<Component>();
				foreach (Component component in components)
				{
					if (component.GetType() == typeof(Transform))
						continue;

					if (component.GetType() == typeof(Animator))
						continue;

					Component existing = generatedCharacter.GetComponent(component.GetType());

					if (existing)
					{
						existing.CopyComponent(component);
					}
					else
					{
						generatedCharacter.AddComponent(component);
					}
				}

				DestroyImmediate(container);

				ExportData(generatedCharacter, selGenre);

				return generatedCharacter;
			}

			if (selectedModule.moduleUse == ModuleUse.ParentContainer && selectedModule.setModule)
			{
				GameObject container = Instantiate(selectedModule.setModule, transform.position, transform.rotation, transform);
				if (selectedModule.parent)
				{
					generatedCharacter.transform.parent = container.transform.Find(selectedModule.parent.name);
				}
				else
				{
					generatedCharacter.transform.parent = container.transform;
				}

				if (container.GetComponent<Animator>())
				{
					if (selectedModule.useContainerAnimator)
					{
						container.GetComponent<Animator>().CopyComponent(generatedCharacter.GetComponent<Animator>());
						DestroyImmediate(generatedCharacter.GetComponent<Animator>());
					}
				}

				ExportData(container, selGenre);

				return container;
			}
			#endregion

			ExportData(generatedCharacter, selGenre);

			return generatedCharacter;
		}

		void ExportData(GameObject character, characterGenre genre)
		{
			if (!character.GetComponent<RandomCharacterBehaviour>())
			{
				RandomCharacterBehaviour behaviour = character.AddComponent<RandomCharacterBehaviour>();
				behaviour.name = characterName;

				behaviour.characterEvents = new List<RandomCharacterCustomEvent.CustomEvent>();
				behaviour.characterStats = new List<CharacterStats>();

				foreach (RandomCharacterCustomEvent.CustomEvent cEvent in events.events)
				{
					behaviour.characterEvents.Add(cEvent);

				}
				foreach (RandomCharacterStat stat in stats)
				{
					behaviour.characterStats.Add(new CharacterStats(stat));

				}

				if (useRandomImage)
				{
					if (genre == characterGenre.Male)
					{
						behaviour.image = maleImages[Random.Range(0, maleImages.Count)];
					}

					if (genre == characterGenre.Female)
					{
						behaviour.image = femaleImages[Random.Range(0, maleImages.Count)];
					}
				}
				else
				{
					if (genre == characterGenre.Male)
					{
						behaviour.image = maleImage;
					}

					if (genre == characterGenre.Female)
					{
						behaviour.image = femaleImage;
					}
				}

				if (attachInventoryScript)
				{
					Component inventory = character.AddComponent(iScriptPrefab.GetType());
					inventory = iScriptPrefab;
				}

				if (useRandomName)
				{
					if (genre == characterGenre.Male)
					{
						characterName = randomMaleNameGenerator.GetName(2);
					}
					if (genre == characterGenre.Female)
					{
						characterName = randomFemaleNameGenerator.GetName(2);
					}
					behaviour.name = characterName;

				}

				behaviour.characterGenre = genre;

				if (selectedModule.overrideName)
				{
					character.name = selectedModule.name;
				}
				else
				{
					character.name = objectName;
				}

				return;
			}

			RandomCharacterBehaviour existingBehaviour = character.GetComponent<RandomCharacterBehaviour>();
			existingBehaviour.name = characterName;

			existingBehaviour.characterEvents = new List<RandomCharacterCustomEvent.CustomEvent>();
			existingBehaviour.characterStats = new List<CharacterStats>();

			foreach (RandomCharacterCustomEvent.CustomEvent cEvent in events.events)
			{
				existingBehaviour.characterEvents.Add(cEvent);
			}
			foreach (RandomCharacterStat stat in stats)
			{
				existingBehaviour.characterStats.Add(new CharacterStats(stat));

			}

			if (useRandomImage)
			{
				if (genre == characterGenre.Male)
				{
					existingBehaviour.image = maleImages[Random.Range(0, maleImages.Count)];
				}

				if (genre == characterGenre.Female)
				{
					existingBehaviour.image = femaleImages[Random.Range(0, maleImages.Count)];
				}
			}
			else
			{
				if (genre == characterGenre.Male)
				{
					existingBehaviour.image = maleImage;
				}

				if (genre == characterGenre.Female)
				{
					existingBehaviour.image = femaleImage;
				}
			}

			if (useRandomName)
			{
				if (genre == characterGenre.Male)
				{
					characterName = randomMaleNameGenerator.GetName(2);
				}
				if (genre == characterGenre.Female)
				{
					characterName = randomFemaleNameGenerator.GetName(2);
				}				

				existingBehaviour.name = characterName;
			}

			if (attachInventoryScript)
			{
				Component inventory = character.AddComponent(iScriptPrefab.GetType());
				inventory = iScriptPrefab;
			}

			existingBehaviour.characterGenre = genre;

			if (selectedModule.overrideName)
			{
				character.name = selectedModule.name;
			}
			else
			{
				character.name = objectName;
			}
		}
	}
}
