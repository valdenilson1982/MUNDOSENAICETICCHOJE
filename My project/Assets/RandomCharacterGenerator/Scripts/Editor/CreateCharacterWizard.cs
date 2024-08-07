using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RandomCharacter
{
    public class CreateCharacterWizard : EditorWindow
    {
        public RandomCharacterClass characterSet;
        SerializedObject character_SO;
        [SerializeField] string path;

        enum ColliderType
        {
            None,
            CapsuleCollider,
            BoxCollider
        }

        [SerializeField] ColliderType colliderType = ColliderType.None;
        [SerializeField] Vector3 size = Vector3.one, center = Vector3.zero;
        [SerializeField] float radius = 1, height = 2;
        [SerializeField] bool isTrigger = false;
        [SerializeField] PhysicMaterial material;

        [SerializeField] int characterLayer;
        [SerializeField] string characterTag = "Untagged";

        Vector2 scrollPos;

        [Min(1)] public int amount = 1;

        [SerializeField] Enumerations.characterGenre genre = Enumerations.characterGenre.Random;
        [SerializeField] Enumerations.desition haveBear = Enumerations.desition.Random;
        [SerializeField] Enumerations.desition useMask = Enumerations.desition.Random;
        [SerializeField] Enumerations.desition useBackPack = Enumerations.desition.Random;

        [SerializeField] Enumerations.desition useHelmet = Enumerations.desition.Random;
        [SerializeField] Enumerations.desition useArmourChest = Enumerations.desition.Random;
        [SerializeField] Enumerations.desition useShouldersProtection = Enumerations.desition.Random;
        [SerializeField] Enumerations.desition useWristsProtection = Enumerations.desition.Random;
        [SerializeField] Enumerations.desition useThighsProtection = Enumerations.desition.Random;
        [SerializeField] Enumerations.desition useKneeProtection = Enumerations.desition.Random;
        [SerializeField] Enumerations.desition hasWeapon = Enumerations.desition.Random;

        [SerializeField] Enumerations.desition useGloves = Enumerations.desition.Random;
        [SerializeField] Enumerations.desition useBoots = Enumerations.desition.Random;
        [SerializeField] Enumerations.desition useShirt = Enumerations.desition.Random;
        [SerializeField] Enumerations.desition usePants = Enumerations.desition.Random;

        [SerializeField] bool placeUnparent = false;

        enum SpawnMethod
        {
            Row,
            FixedPosition,
            RandomPosition
        }

        static CreateCharacterWizard window;

        [MenuItem("Massive/Create Characters", priority = 10)]
        static void CreateWizard()
        {
            window = (CreateCharacterWizard)EditorWindow.GetWindow(typeof(CreateCharacterWizard), false, "Create Character set");
            window.minSize = new Vector2(400, 700);
            window.Show();
        }

        private void CreateCharacters()
        {
            characterSet.Init();

            characterSet.genre = this.genre;
            characterSet.haveBear = this.haveBear;
            characterSet.useMask = this.useMask;
            characterSet.useBackPack = this.useBackPack;
            characterSet.useHelmet = this.useHelmet;
            characterSet.useArmourChest = this.useArmourChest;
            characterSet.useShouldersProtection = this.useShouldersProtection;
            characterSet.useWristsProtection = this.useWristsProtection;
            characterSet.useThighsProtection = this.useThighsProtection;
            characterSet.useKneeProtection = this.useKneeProtection;
            characterSet.hasWeapon = this.hasWeapon;

            characterSet.useGloves = this.useGloves;
            characterSet.useShirt = this.useShirt;
            characterSet.usePants = this.usePants;
            characterSet.useBoots = this.useBoots;
            for (int i = 0; i < amount; i++)
            {
                GameObject character = characterSet.Generate(Vector3.zero);

                if (characterSet.selectedModule.overrideLayer)
                {
                    character.layer = characterSet.selectedModule.layer;
                }
                else
                {
                    character.layer = characterLayer;
                }

                if (characterSet.selectedModule.overrideTag)
                {
                    character.tag = characterSet.selectedModule.tag;
                }
                else
                {
                    character.tag = characterTag;
                }

                switch (colliderType)
                {
                    case ColliderType.BoxCollider:
                        BoxCollider box = character.AddComponent<BoxCollider>();
                        box.isTrigger = isTrigger;
                        box.material = material;
                        box.size = size;
                        box.center = center;
                        break;

                    case ColliderType.CapsuleCollider:
                        CapsuleCollider capsule = character.AddComponent<CapsuleCollider>();
                        capsule.isTrigger = isTrigger;
                        capsule.material = material;
                        capsule.center = center;
                        capsule.radius = radius;
                        capsule.height = height;
                        break;
                }

                if (path != "")
                {
                    string fileName = characterSet.objectName + (i + 1).ToString() + ".prefab";
                    string savePath = path + "/" + fileName;
                    PrefabUtility.SaveAsPrefabAsset(character, savePath);
                }

                DestroyImmediate(character);

                EditorUtility.DisplayProgressBar("Creating character prefabs", characterSet.objectName + (i + 1), (float)i / (float)amount);
            }
            EditorUtility.ClearProgressBar();
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);

            GUILayout.BeginVertical("General Settings", "box");

            GUILayout.Space(20);

            characterSet = EditorGUILayout.ObjectField("Character Set", characterSet, typeof(RandomCharacterClass), false) as RandomCharacterClass;

            if (!characterSet)
            {
                EditorGUILayout.HelpBox("Character Set missing. Make sure your Character Set has ben loaded", MessageType.Warning);
                GUILayout.EndVertical();
            }
            else
            {
                character_SO = new SerializedObject(characterSet);

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Layer", GUILayout.Width(40));
                characterLayer = EditorGUILayout.LayerField(characterLayer,GUILayout.Width(125));
                GUILayout.Space(50);
                EditorGUILayout.LabelField("Tag", GUILayout.Width(40));
                characterTag = EditorGUILayout.TagField(characterTag,GUILayout.Width(125));
                GUILayout.EndHorizontal();

                switch (genre)
                {
                    case Enumerations.characterGenre.Female:
                        EditorGUILayout.HelpBox("Max possible combinations " + characterSet.MaxPossibleFemaleCombinations.ToString(), MessageType.Info);
                        break;

                    case Enumerations.characterGenre.Male:
                        EditorGUILayout.HelpBox("Max possible combinations " + characterSet.MaxPossibleMaleCombinations.ToString(), MessageType.Info);
                        break;

                    case Enumerations.characterGenre.Random:
                        EditorGUILayout.HelpBox("Max possible combinations " + characterSet.MaxPossibleCombinations.ToString(), MessageType.Info);
                        break;
                }

                GUILayout.Space(10);

                colliderType = (ColliderType)EditorGUILayout.EnumPopup("Collider", colliderType);

                switch (colliderType)
                {
                    case ColliderType.BoxCollider:
                        isTrigger = EditorGUILayout.Toggle("Is Trigger", isTrigger);
                        material = EditorGUILayout.ObjectField("Material", material, typeof(PhysicMaterial), false) as PhysicMaterial;
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Center", GUILayout.Width(150));
                        center = EditorGUILayout.Vector3Field("", center);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Size", GUILayout.Width(150));
                        size = EditorGUILayout.Vector3Field("", size);
                        GUILayout.EndHorizontal();
                        break;

                    case ColliderType.CapsuleCollider:
                        isTrigger = EditorGUILayout.Toggle("Is Trigger", isTrigger);
                        material = EditorGUILayout.ObjectField("Material", material, typeof(PhysicMaterial), false) as PhysicMaterial;
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Center", GUILayout.Width(150));
                        center = EditorGUILayout.Vector3Field("", center);
                        GUILayout.EndHorizontal();
                        radius = EditorGUILayout.FloatField("Radius", radius);
                        height = EditorGUILayout.FloatField("Height", height);
                        break;
                }

                GUILayout.Space(10);

                amount = EditorGUILayout.IntField("Amount", amount);

                GUILayout.Space(10);

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("Size modifier", GUILayout.Width(150));
                characterSet.minSize = EditorGUILayout.FloatField(characterSet.minSize, GUILayout.Width(50));
                EditorGUILayout.MinMaxSlider(
                    ref characterSet.minSize,
                    ref characterSet.maxSize, 0.2f, characterSet.maxSize * 2);
                characterSet.maxSize = EditorGUILayout.FloatField(characterSet.maxSize, GUILayout.Width(50));
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(20);

                EditorGUILayout.EndVertical();

                GUILayout.Space(10);

                genre = (Enumerations.characterGenre)EditorGUILayout.EnumPopup("Character Genre", genre);
                if (genre == Enumerations.characterGenre.Male || genre == Enumerations.characterGenre.Random)
                {
                    haveBear = (Enumerations.desition)EditorGUILayout.EnumPopup("Have Bear", haveBear);
                }
                useMask = (Enumerations.desition)EditorGUILayout.EnumPopup("Use Mask", useMask);

                GUILayout.Space(10);

                GUILayout.BeginVertical("Inventory Settings", "box");
                GUILayout.Space(20);
                useBackPack = (Enumerations.desition)EditorGUILayout.EnumPopup("Use Backpack", useBackPack);                
                EditorGUILayout.EndVertical();

                GUILayout.Space(10);

                GUILayout.BeginVertical("Cloth Settings", "box");
                GUILayout.Space(20);
                useGloves = (Enumerations.desition)EditorGUILayout.EnumPopup("Use Gloves", useGloves);
                useShirt = (Enumerations.desition)EditorGUILayout.EnumPopup("Use Shirt", useShirt);
                usePants = (Enumerations.desition)EditorGUILayout.EnumPopup("Use Pants", usePants);
                useBoots = (Enumerations.desition)EditorGUILayout.EnumPopup("Use Boots", useBoots);
                EditorGUILayout.EndVertical();

                GUILayout.Space(10);

                GUILayout.BeginVertical("Armor Settings", "box");
                GUILayout.Space(20);
                useHelmet = (Enumerations.desition)EditorGUILayout.EnumPopup("Use Helmet", useHelmet);
                useArmourChest = (Enumerations.desition)EditorGUILayout.EnumPopup("Armour Chest", useArmourChest);
                useShouldersProtection = (Enumerations.desition)EditorGUILayout.EnumPopup("Shoulders Protection", useShouldersProtection);
                useWristsProtection = (Enumerations.desition)EditorGUILayout.EnumPopup("Wrist Protection", useWristsProtection);
                useThighsProtection = (Enumerations.desition)EditorGUILayout.EnumPopup("Thighs Protection", useThighsProtection);
                useKneeProtection = (Enumerations.desition)EditorGUILayout.EnumPopup("Knee Protection", useKneeProtection);
                EditorGUILayout.EndVertical();

                GUILayout.Space(10);

                hasWeapon = (Enumerations.desition)EditorGUILayout.EnumPopup("Use a Weapon", hasWeapon);

                character_SO.ApplyModifiedProperties();
                GUILayout.Space(15);

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Path", GUILayout.Width(30));
                EditorGUILayout.LabelField(path);
                if (GUILayout.Button("Set Path", GUILayout.Width(60)))
                {
                    path = EditorUtility.OpenFolderPanel("Output path", "", "");
                }
                GUILayout.EndHorizontal();

                EditorGUILayout.Space(10);
                if (characterSet)
                {
                    if (GUILayout.Button("Create Characters", GUILayout.Height(30)))
                    {
                        CreateCharacters();
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Character Set missing. Make sure your Character Set has ben loaded", MessageType.Error);
                }
            }

            GUILayout.Space(30);
            EditorGUILayout.EndScrollView();
        }
    }
}