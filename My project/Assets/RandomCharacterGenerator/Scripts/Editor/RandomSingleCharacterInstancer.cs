using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RandomCharacter
{
    [CustomEditor(typeof(RandomCharacterInstancer))]
    public class RandomSingleCharacterInstancer : Editor
    {
        [MenuItem ("GameObject/3D Object/Massive/Single Instancer")]
        [MenuItem("Massive/Single Instancer")]
        public static void CreateInstancer()
        {
            RandomCharacterInstancer instance = new GameObject("Single instancer").AddComponent<RandomCharacterInstancer>();
            instance.transform.parent = Selection.activeTransform;
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
        }

        SerializedObject character_SO;

        public override void OnInspectorGUI()
        {
            RandomCharacterInstancer instancer = (RandomCharacterInstancer)target;

            GUILayout.BeginVertical("General Settings", "box");

            GUILayout.Space(20);

            instancer.characterSet = EditorGUILayout.ObjectField("Character Set", instancer.characterSet, typeof(RandomCharacterClass), false) as RandomCharacterClass;

            if (!instancer.characterSet)
            {
                EditorGUILayout.HelpBox("Character Set missing. Make sure your Character Set has ben loaded", MessageType.Warning);
                GUILayout.EndVertical();
            }
            else
            {
                character_SO = new SerializedObject(instancer.characterSet);

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Layer", GUILayout.Width(40));
                instancer.characterLayer = EditorGUILayout.LayerField(instancer.characterLayer, GUILayout.Width(125));
                GUILayout.Space(50);
                EditorGUILayout.LabelField("Tag", GUILayout.Width(40));
                instancer.characterTag = EditorGUILayout.TagField(instancer.characterTag, GUILayout.Width(125));
                GUILayout.EndHorizontal();

                switch (instancer.genre)
                {
                    case Enumerations.characterGenre.Female:
                        EditorGUILayout.HelpBox("Max possible combinations " + instancer.characterSet.MaxPossibleFemaleCombinations.ToString(), MessageType.Info);
                        break;

                    case Enumerations.characterGenre.Male:
                        EditorGUILayout.HelpBox("Max possible combinations " + instancer.characterSet.MaxPossibleMaleCombinations.ToString(), MessageType.Info);
                        break;

                    case Enumerations.characterGenre.Random:
                        EditorGUILayout.HelpBox("Max possible combinations " + instancer.characterSet.MaxPossibleCombinations.ToString(), MessageType.Info);
                        break;
                }

                GUILayout.Space(10);

                instancer.generateOnAwake = EditorGUILayout.Toggle("Generate on Awake", instancer.generateOnAwake);
                instancer.generateOnInterval = EditorGUILayout.Toggle("Interval generation", instancer.generateOnInterval);
                if (instancer.generateOnInterval)
                {
                    instancer.startDelay = EditorGUILayout.FloatField("Start delay", instancer.startDelay);
                    instancer.generationInterval = EditorGUILayout.FloatField("Generation interval", instancer.generationInterval);
                    instancer.amount = EditorGUILayout.IntField("Number of Characters", instancer.amount);
                    EditorGUILayout.HelpBox("The instancer can't be destroyed if't is continously generating", MessageType.Warning);
                }
                if (!instancer.generateOnInterval)
                {
                    instancer.destroyOnGenerate = EditorGUILayout.Toggle("Destroy on generate", instancer.destroyOnGenerate);
                }

                GUILayout.Space(10);

                instancer.colliderType = (RandomCharacterInstancer.ColliderType)EditorGUILayout.EnumPopup("Collider", instancer.colliderType);

                switch (instancer.colliderType)
                {
                    case RandomCharacterInstancer.ColliderType.BoxCollider:
                        instancer.isTrigger = EditorGUILayout.Toggle("Is Trigger", instancer.isTrigger);
                        instancer.material = EditorGUILayout.ObjectField("Material", instancer.material, typeof(PhysicMaterial), false) as PhysicMaterial;
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Center", GUILayout.Width(150));
                        instancer.center = EditorGUILayout.Vector3Field("", instancer.center);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Size", GUILayout.Width(150));
                        instancer.size = EditorGUILayout.Vector3Field("", instancer.size);
                        GUILayout.EndHorizontal();
                        break;

                    case RandomCharacterInstancer.ColliderType.CapsuleCollider:
                        instancer.isTrigger = EditorGUILayout.Toggle("Is Trigger", instancer.isTrigger);
                        instancer.material = EditorGUILayout.ObjectField("Material", instancer.material, typeof(PhysicMaterial), false) as PhysicMaterial;
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Center", GUILayout.Width(150));
                        instancer.center = EditorGUILayout.Vector3Field("", instancer.center);
                        GUILayout.EndHorizontal();
                        instancer.radius = EditorGUILayout.FloatField("Radius", instancer.radius);
                        instancer.height = EditorGUILayout.FloatField("Height", instancer.height);
                        break;
                }

                GUILayout.Space(10);

                GUILayout.Space(10);

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("Size modifier", GUILayout.Width(150));
                instancer.characterSet.minSize = EditorGUILayout.FloatField(instancer.characterSet.minSize, GUILayout.Width(50));
                EditorGUILayout.MinMaxSlider(
                    ref instancer.characterSet.minSize,
                    ref instancer.characterSet.maxSize, 0.2f, instancer.characterSet.maxSize * 2);
                instancer.characterSet.maxSize = EditorGUILayout.FloatField(instancer.characterSet.maxSize, GUILayout.Width(50));
                EditorGUILayout.EndHorizontal();                

                EditorGUILayout.EndVertical();

                GUILayout.Space(10);

                instancer.placeUnparent = EditorGUILayout.ToggleLeft("Place unparent", instancer.placeUnparent);

                GUILayout.Space(10);

                instancer.genre = (Enumerations.characterGenre)EditorGUILayout.EnumPopup("Character Genre", instancer.genre);
                if (instancer.genre == Enumerations.characterGenre.Male || instancer.genre == Enumerations.characterGenre.Random)
                {
                    instancer.haveBear = (Enumerations.desition)EditorGUILayout.EnumPopup("Have Bear", instancer.haveBear);
                }
                instancer.useMask = (Enumerations.desition)EditorGUILayout.EnumPopup("Use Mask", instancer.useMask);

                GUILayout.Space(10);

                GUILayout.BeginVertical("Inventory Settings", "box");
                GUILayout.Space(20);
                instancer.useBackPack = (Enumerations.desition)EditorGUILayout.EnumPopup("Use Backpack", instancer.useBackPack);
                EditorGUILayout.EndVertical();

                GUILayout.Space(10);

                GUILayout.BeginVertical("Cloth Settings", "box");
                GUILayout.Space(20);
                instancer.useGloves = (Enumerations.desition)EditorGUILayout.EnumPopup("Use Gloves", instancer.useGloves);
                instancer.useShirt = (Enumerations.desition)EditorGUILayout.EnumPopup("Use Shirt", instancer.useShirt);
                instancer.usePants = (Enumerations.desition)EditorGUILayout.EnumPopup("Use Pants", instancer.usePants);
                instancer.useBoots = (Enumerations.desition)EditorGUILayout.EnumPopup("Use Boots", instancer.useBoots);
                EditorGUILayout.EndVertical();

                GUILayout.Space(10);

                GUILayout.BeginVertical("Armor Settings", "box");
                GUILayout.Space(20);
                instancer.useHelmet = (Enumerations.desition)EditorGUILayout.EnumPopup("Use Helmet", instancer.useHelmet);
                instancer.useArmourChest = (Enumerations.desition)EditorGUILayout.EnumPopup("Armour Chest", instancer.useArmourChest);
                instancer.useShouldersProtection = (Enumerations.desition)EditorGUILayout.EnumPopup("Shoulders Protection", instancer.useShouldersProtection);
                instancer.useWristsProtection = (Enumerations.desition)EditorGUILayout.EnumPopup("Wrist Protection", instancer.useWristsProtection);
                instancer.useThighsProtection = (Enumerations.desition)EditorGUILayout.EnumPopup("Thighs Protection", instancer.useThighsProtection);
                instancer.useKneeProtection = (Enumerations.desition)EditorGUILayout.EnumPopup("Knee Protection", instancer.useKneeProtection);
                EditorGUILayout.EndVertical();

                GUILayout.Space(10);

                instancer.hasWeapon = (Enumerations.desition)EditorGUILayout.EnumPopup("Use a Weapon", instancer.hasWeapon);

                character_SO.ApplyModifiedProperties();

                EditorGUILayout.Space(10);
                if (instancer.characterSet)
                {
                    if (GUILayout.Button("Create Characters", GUILayout.Height(30)))
                    {
                        instancer.Generate();
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Character Set missing. Make sure your Character Set has ben loaded", MessageType.Error);
                }
            }

            GUILayout.Space(30);
        }
    }
}
