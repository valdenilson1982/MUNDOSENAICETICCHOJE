using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RandomCharacter
{
    [CustomEditor(typeof(RandomAreaInstancer))]
    public class RandomInstancerEditor : Editor
    {
        [MenuItem("GameObject/3D Object/Massive/Area Instancer")]
        [MenuItem("Massive/Area Instancer")]
        public static void CreateInstancer()
        {
            RandomAreaInstancer instance = new GameObject("Area instancer").AddComponent<RandomAreaInstancer>();
            instance.transform.parent = Selection.activeTransform;
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
        }

        SerializedObject character_SO;

        public override void OnInspectorGUI()
        {
            RandomAreaInstancer instancer = (RandomAreaInstancer)target;

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
                    EditorGUILayout.HelpBox("The instancer can't be destroyed if't is continously generating", MessageType.Warning);
                }
                if (!instancer.generateOnInterval)
                {
                    instancer.destroyOnGenerate = EditorGUILayout.Toggle("Destroy on generate", instancer.destroyOnGenerate);
                }

                GUILayout.Space(10);

                instancer.colliderType = (RandomAreaInstancer.ColliderType)EditorGUILayout.EnumPopup("Collider", instancer.colliderType);

                switch (instancer.colliderType)
                {
                    case RandomAreaInstancer.ColliderType.BoxCollider:
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

                    case RandomAreaInstancer.ColliderType.CapsuleCollider:
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

                GUILayout.Space(20);

                instancer.spawnMethod = (RandomAreaInstancer.SpawnMethod)EditorGUILayout.EnumPopup("Spawn Method", instancer.spawnMethod);

                GUILayout.Space(5);

                switch (instancer.spawnMethod)
                {
                    case RandomAreaInstancer.SpawnMethod.FixedPosition:
                        instancer.useRandomRotation = EditorGUILayout.ToggleLeft("Use Random Rotation", instancer.useRandomRotation);
                        instancer.placeUnparent = EditorGUILayout.ToggleLeft("Place unparent", instancer.placeUnparent);

                        GUILayout.Space(10);

                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("Add new spawn position"))
                        {
                            instancer.tempSpawnPositions = instancer.spawnPositions;
                            instancer.spawnPositions = new Transform[instancer.spawnPositions.Length + 1];

                            for (int i = 0; i < instancer.tempSpawnPositions.Length; i++)
                            {
                                instancer.spawnPositions[i] = instancer.tempSpawnPositions[i];
                            }
                        }

                        if (GUILayout.Button("Clear empty positions"))
                        {
                            List<Transform> positions = new List<Transform>();
                            for (int i = 0; i < instancer.spawnPositions.Length; i++)
                            {
                                if (instancer.spawnPositions[i])
                                {
                                    positions.Add(instancer.spawnPositions[i]);
                                }
                            }
                            instancer.spawnPositions = positions.ToArray();
                        }
                        EditorGUILayout.EndHorizontal();

                        for (int i = 0; i < instancer.spawnPositions.Length; i++)
                        {
                            instancer.spawnPositions[i] = EditorGUILayout.ObjectField("Spawn position " + (i + 1).ToString(), instancer.spawnPositions[i], typeof(Transform), true) as Transform;
                        }
                        break;

                    case RandomAreaInstancer.SpawnMethod.RandomPosition:
                        EditorGUILayout.HelpBox("In the Random Position context all the characters are unparent", MessageType.Info);

                        instancer.amount = EditorGUILayout.IntField("Number of Characters", instancer.amount);

                        instancer.useRandomRotation = EditorGUILayout.ToggleLeft("Use Random Rotation", instancer.useRandomRotation);

                        EditorGUILayout.PropertyField(character_SO.FindProperty("positioningMask"));
                        EditorGUILayout.PropertyField(character_SO.FindProperty("obstacleMask"));

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Spawn zone", GUILayout.Width(150));
                        EditorGUILayout.LabelField("Width", GUILayout.Width(40));
                        instancer.zone.x = EditorGUILayout.FloatField(instancer.zone.x);
                        GUILayout.Space(30);
                        EditorGUILayout.LabelField("Height", GUILayout.Width(40));
                        instancer.zone.y = EditorGUILayout.FloatField(instancer.zone.y);
                        //zone = EditorGUILayout.Vector2Field("Spawn zone", zone);
                        EditorGUILayout.EndHorizontal();

                        instancer.useDistanciation = EditorGUILayout.ToggleLeft("Use distanciation", instancer.useDistanciation);
                        if (instancer.useDistanciation)
                        {
                            EditorGUILayout.PropertyField(character_SO.FindProperty("checkMask"));

                            instancer.radius = EditorGUILayout.FloatField("Distance radius", instancer.radius);
                            instancer.attemts = EditorGUILayout.IntField("Try to instantiate times", instancer.attemts);
                            EditorGUILayout.HelpBox("Highs instantiation tries may increase the processing time and cause Unity freeze", MessageType.Warning);

                            if (instancer.colliderType == RandomAreaInstancer.ColliderType.None)
                            {
                                EditorGUILayout.HelpBox("If ther is no Collider in the new Character then they could be overlaped between the existing ones", MessageType.Warning);
                            }
                        }

                        break;
                }

                EditorGUILayout.EndVertical();

                GUILayout.Space(10);

                /*GUILayout.BeginVertical("Name Settings", "box");
                GUILayout.Space(20);
                useRandomName = EditorGUILayout.ToggleLeft("Random Name", useRandomName);
                if (!useRandomName)
                {
                    characterName = EditorGUILayout.TextField("Character Name", characterName);
                }
                EditorGUILayout.EndVertical();

                GUILayout.Space(10);*/

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
