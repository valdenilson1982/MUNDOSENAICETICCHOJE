using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RandomCharacter
{
    [CustomEditor(typeof(RandomCharacterClass))]
    public class RandomCharacterEditor : Editor
    {
        SerializedObject character_SO;
        int tabID = 0;

        MonoScript newScript;
        bool addingScript = false;

        public void OnEnable()
        {
            character_SO = new SerializedObject((RandomCharacterClass)target);
        }

        RandomCharacterStat[] oldStats;

        public override void OnInspectorGUI()
        {
            character_SO.Update();

            EditorGUI.BeginChangeCheck();

            tabID = GUILayout.Toolbar(tabID, new string[] { "General", "Cloth", "Male", "Female", "Weapons", "AddOns" });

            if (EditorGUI.EndChangeCheck())
            {
                character_SO.ApplyModifiedProperties();
                GUI.FocusControl(null);
            }

            switch (tabID)
            {
                case 0:
                    OnGeneralInpectorGUI(character_SO, (RandomCharacterClass)target);
                    break;

                case 1:
                    OnClothInpectorGUI(character_SO, (RandomCharacterClass)target);
                    break;

                case 2:
                    OnMaleInspectorGUI(character_SO, (RandomCharacterClass)target);
                    break;

                case 3:
                    OnFemaleInspectorGUI(character_SO, (RandomCharacterClass)target);
                    break;

                case 4:
                    OnWeaponInspectorGUI(character_SO);
                    break;

                case 5:
                    OnAddOnsInspectorGUI(character_SO, (RandomCharacterClass)target);
                    break;
            }

            character_SO.ApplyModifiedProperties();
        }

        public void OnGeneralInpectorGUI(SerializedObject SO, RandomCharacterClass target)
        {
            GUILayout.BeginVertical("Character name settings", "box");
            EditorGUILayout.Space(20);

            target.objectName = EditorGUILayout.TextField("Object name", target.objectName);

            GUILayout.BeginHorizontal();

            target.useRandomName = EditorGUILayout.ToggleLeft("Random name", target.useRandomName, GUILayout.Width(150));
            if (!target.useRandomName)
            {
                target.characterName = EditorGUILayout.TextField(target.characterName);

                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Male names list", GUILayout.Width(125));
                target.MaleNamesList = EditorGUILayout.ObjectField(target.MaleNamesList, typeof(TextAsset), false) as TextAsset;
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Female names list", GUILayout.Width(125));
                target.FemaleNamesList = EditorGUILayout.ObjectField(target.FemaleNamesList, typeof(TextAsset), false) as TextAsset;
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();

            EditorGUILayout.Space(20);

            GUILayout.BeginVertical("Character images", "box");

            EditorGUILayout.Space(20);

            target.useRandomImage = EditorGUILayout.ToggleLeft("Use random character image", target.useRandomImage);

            GUILayout.BeginHorizontal();
            GUILayout.Space(15);

            GUILayout.BeginVertical();

            if (target.useRandomImage)
            {
                EditorGUILayout.PropertyField(SO.FindProperty("maleImages"));
                EditorGUILayout.PropertyField(SO.FindProperty("femaleImages"));
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                EditorGUILayout.LabelField("Male image", GUILayout.Width(100));
                target.maleImage = EditorGUILayout.ObjectField(target.maleImage, typeof(Sprite), false) as Sprite;
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                EditorGUILayout.LabelField("Female image", GUILayout.Width(100));
                target.femaleImage = EditorGUILayout.ObjectField(target.femaleImage, typeof(Sprite), false) as Sprite;
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            EditorGUILayout.Space(20);

            EditorGUILayout.PropertyField(SO.FindProperty("defaultController"));
            GUILayout.Space(10);

            GUILayout.BeginVertical("Character template settings", "box");
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Space(50);
            EditorGUILayout.LabelField("Character template", GUILayout.Width(120));
            target.template = EditorGUILayout.ObjectField(target.template, typeof(CharacterTemplate), false) as CharacterTemplate;
            GUILayout.Space(50);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            if (target.template) {
                for (int m = 0; m < target.template.itemsMarkers.Count; m++)
                {
                    if (target.template.itemsMarkers[m].marker)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(50);
                        EditorGUILayout.LabelField("Marker name", GUILayout.Width(80));
                        target.template.itemsMarkers[m].marker.name = EditorGUILayout.TextField(target.template.itemsMarkers[m].marker.name);
                        if (GUILayout.Button("Remove marker", GUILayout.Width(100)))
                        {
                            DestroyImmediate(target.template.itemsMarkers[m].marker, true);
                            target.template.itemsMarkers.Remove(target.template.itemsMarkers[m]);
                            break;
                        }
                        GUILayout.Space(50);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Space(100);
                        if (GUILayout.Button("Add Item", GUILayout.Height(25)))
                        {
                            target.template.itemsMarkers[m].items.Add(null);
                        }
                        GUILayout.Space(100);
                        GUILayout.EndHorizontal();

                        for (int i = 0; i < target.template.itemsMarkers[m].items.Count; i++)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(50);
                            target.template.itemsMarkers[m].items[i] = EditorGUILayout.ObjectField(target.template.itemsMarkers[m].items[i], typeof(GameObject), false) as GameObject;
                            if (GUILayout.Button("Remove", GUILayout.Width(75)))
                            {
                                target.template.itemsMarkers[m].items.Remove(target.template.itemsMarkers[m].items[i]);
                                break;
                            }
                            GUILayout.Space(50);
                            GUILayout.EndHorizontal();
                        }
                    }
                    GUILayout.Space(10);
                }
            }
            GUILayout.EndVertical();

            EditorGUILayout.Space(20);

            GUILayout.BeginVertical("Character stats", "box");
            EditorGUILayout.Space(20);
            GUILayout.EndVertical();

            for (int i = 0; i < target.stats.Count; i++)
            {
                GUILayout.BeginVertical(target.stats[i].name, "box");

                GUILayout.BeginHorizontal();

                if (target.stats[i].icon)
                {
                    GUILayout.Box(target.stats[i].icon.texture, GUILayout.Height(50), GUILayout.Width(50));
                }
                else
                {
                    GUILayout.Space(50);
                }

                EditorGUILayout.Space(20);

                GUILayout.BeginVertical();

                EditorGUILayout.Space(20);

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Stat name", GUILayout.Width(120));
                target.stats[i].name = EditorGUILayout.TextField(target.stats[i].name);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Stat Icon", GUILayout.Width(120));
                target.stats[i].icon = EditorGUILayout.ObjectField(target.stats[i].icon, typeof(Sprite), false) as Sprite;
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                float minValue = target.stats[i].minValue;
                float maxValue = target.stats[i].maxValue;

                EditorGUILayout.LabelField("Stat range", GUILayout.Width(120));
                minValue = EditorGUILayout.IntField(Mathf.RoundToInt(minValue), GUILayout.Width(50));
                EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, 0, target.stats[i].maxLimit);
                maxValue = EditorGUILayout.IntField(Mathf.RoundToInt(maxValue), GUILayout.Width(50));

                target.stats[i].minValue = Mathf.RoundToInt(minValue);
                target.stats[i].maxValue = Mathf.RoundToInt(maxValue);

                GUILayout.EndHorizontal();

                target.stats[i].rangedValue = EditorGUILayout.ToggleLeft("Use ranged value", target.stats[i].rangedValue);
                if (target.stats[i].rangedValue)
                {
                    EditorGUILayout.HelpBox("With Ranged Value enabled the value will iterate between the min and max values and return randomly", MessageType.Info);
                }

                EditorGUILayout.Space(5);

                GUILayout.BeginHorizontal();
                target.stats[i].useProbabilityTrigger = EditorGUILayout.ToggleLeft("Probability stat", target.stats[i].useProbabilityTrigger, GUILayout.Width(108));

                EditorGUILayout.Space(10);

                if (target.stats[i].useProbabilityTrigger)
                {
                    float minProb = target.stats[i].minProb;
                    float maxProb = target.stats[i].maxProb;
                    //EditorGUILayout.LabelField("Probability range", GUILayout.Width(130));
                    minProb = EditorGUILayout.IntField(Mathf.RoundToInt(minProb), GUILayout.Width(50));
                    EditorGUILayout.MinMaxSlider(ref minProb, ref maxProb, 0, target.stats[i].maxProbLimit);
                    maxProb = EditorGUILayout.IntField(Mathf.RoundToInt(maxProb), GUILayout.Width(50));

                    target.stats[i].minProb = Mathf.RoundToInt(minProb);
                    target.stats[i].maxProb = Mathf.RoundToInt(maxProb);
                }

                GUILayout.EndHorizontal();

                target.stats[i].Update();

                EditorGUILayout.Space(10);

                if (GUILayout.Button("Delete Stat", GUILayout.Width(100)))
                {
                    target.stats.RemoveAt(i);
                    break;
                }

                GUILayout.EndVertical();

                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }

            EditorGUILayout.Space(15);

            if (GUILayout.Button("Add Stat", GUILayout.Height(30)))
            {
                target.stats.Add(new RandomCharacterStat());
            }

            EditorGUILayout.Space(20);
        }

        public void OnClothInpectorGUI(SerializedObject SO, RandomCharacterClass target)
        {
            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("materialVariations"));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("maskVariations"));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical("", "Box");            
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);            
            EditorGUILayout.PropertyField(SO.FindProperty("backpackVariations"));
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            target.backpackParentBone = (HumanBodyBones)EditorGUILayout.EnumPopup("Parent Bone", target.backpackParentBone);
            GUILayout.BeginHorizontal();
            target.attachInventoryScript = EditorGUILayout.ToggleLeft("Attach Inventory", target.attachInventoryScript, GUILayout.Width(150));
            if (target.attachInventoryScript)
            {
                target.invScript = EditorGUILayout.ObjectField(target.invScript, typeof(UnityEditor.MonoScript), false) as UnityEditor.MonoScript;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(50);
            if (!target.iScriptPrefab && target.invScript && target.attachInventoryScript)
            {

                if (GUILayout.Button("Create Inventory Object", GUILayout.Height(25)))
                {
                    string path = UnityEditor.EditorUtility.SaveFilePanelInProject("Choose a path for the new Inventory",
                       "New Inventory", "prefab", "Create Inventory");
                    if (path != null)
                    {
                        GameObject temp = new GameObject("Inventory");
                        System.Type type = target.invScript.GetClass();
                        temp.AddComponent(type);

                        GameObject tempPrefab = UnityEditor.PrefabUtility.SaveAsPrefabAsset(temp, path);
                        target.iScriptPrefab = tempPrefab.GetComponent(type);
                        DestroyImmediate(temp);
                    }
                }
            }
            if (target.iScriptPrefab && target.invScript)
            {
                if (target.invScript.GetClass() != target.iScriptPrefab.GetType())
                {
                    if (GUILayout.Button("Replace", GUILayout.Height(25)))
                    {
                        Component oldInventory = target.iScriptPrefab;
                        target.iScriptPrefab = target.iScriptPrefab.gameObject.AddComponent(target.invScript.GetClass());
                        DestroyImmediate(oldInventory, true);
                    }
                }
            }
            GUILayout.Space(50);
            GUILayout.EndHorizontal();

            if (target.invScript && target.iScriptPrefab && target.attachInventoryScript)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.BeginVertical();
                EditorGUILayout.Space(20);
                Editor tmpEditor = Editor.CreateEditor(target.iScriptPrefab);
                tmpEditor.OnInspectorGUI();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            EditorGUILayout.Space(20);

            GUILayout.BeginVertical("Armor variations", "box");
            EditorGUILayout.Space(20);
            GUILayout.EndVertical();

            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("helmetVariation"));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("shoulders"));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("wrists"));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("thighs"));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("knees"));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        public void OnMaleInspectorGUI(SerializedObject SO, RandomCharacterClass target)
        {
            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("maleVariations"));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("hairMaleVariations"));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("bearVariations"));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);

            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("maleArmourChest"));
            GUILayout.EndHorizontal();
            target.useNameTargeting = EditorGUILayout.ToggleLeft("Use name based retargeting", target.useNameTargeting);
            if (target.useNameTargeting)
            {
                EditorGUILayout.HelpBox("This means that you going to use the bone's name instead the Avatar Bones to retarget the mesh", MessageType.Warning);
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("maleShirts"));
            GUILayout.EndHorizontal();
            target.shirtNameTarget = EditorGUILayout.ToggleLeft("Use name based retargeting", target.shirtNameTarget);
            if (target.shirtNameTarget)
            {
                EditorGUILayout.HelpBox("This means that you going to use the bone's name instead the Avatar Bones to retarget the mesh", MessageType.Warning);
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("malePants"));
            GUILayout.EndHorizontal();
            target.pantsNameTarget = EditorGUILayout.ToggleLeft("Use name based retargeting", target.pantsNameTarget);
            if (target.pantsNameTarget)
            {
                EditorGUILayout.HelpBox("This means that you going to use the bone's name instead the Avatar Bones to retarget the mesh", MessageType.Warning);
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("maleBoots"));
            GUILayout.EndHorizontal();
            target.bootsNameTarget = EditorGUILayout.ToggleLeft("Use name based retargeting", target.bootsNameTarget);
            if (target.bootsNameTarget)
            {
                EditorGUILayout.HelpBox("This means that you going to use the bone's name instead the Avatar Bones to retarget the mesh", MessageType.Warning);
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("maleGloves"));
            GUILayout.EndHorizontal();
            target.glovesNameTarget = EditorGUILayout.ToggleLeft("Use name based retargeting", target.glovesNameTarget);
            if (target.glovesNameTarget)
            {
                EditorGUILayout.HelpBox("This means that you going to use the bone's name instead the Avatar Bones to retarget the mesh", MessageType.Warning);
            }
            GUILayout.EndVertical();
        }

        public void OnFemaleInspectorGUI(SerializedObject SO, RandomCharacterClass target)
        {
            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("femaleVariations"));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("hairFemaleVariations"));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.Space(10);

            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("femaleArmourChest"));
            GUILayout.EndHorizontal();
            target.useNameTargeting = EditorGUILayout.ToggleLeft("Use name based retargeting", target.useNameTargeting);
            if (target.useNameTargeting)
            {
                EditorGUILayout.HelpBox("This means that you going to use the bone's name instead the Avatar Bones to retarget the mesh", MessageType.Warning);
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("femaleShirts"));
            GUILayout.EndHorizontal();
            target.shirtNameTarget = EditorGUILayout.ToggleLeft("Use name based retargeting", target.shirtNameTarget);
            if (target.shirtNameTarget)
            {
                EditorGUILayout.HelpBox("This means that you going to use the bone's name instead the Avatar Bones to retarget the mesh", MessageType.Warning);
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("femalePants"));
            GUILayout.EndHorizontal();
            target.pantsNameTarget = EditorGUILayout.ToggleLeft("Use name based retargeting", target.pantsNameTarget);
            if (target.pantsNameTarget)
            {
                EditorGUILayout.HelpBox("This means that you going to use the bone's name instead the Avatar Bones to retarget the mesh", MessageType.Warning);
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("femaleBoots"));
            GUILayout.EndHorizontal();
            target.bootsNameTarget = EditorGUILayout.ToggleLeft("Use name based retargeting", target.bootsNameTarget);
            if (target.bootsNameTarget)
            {
                EditorGUILayout.HelpBox("This means that you going to use the bone's name instead the Avatar Bones to retarget the mesh", MessageType.Warning);
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("femaleGloves"));
            GUILayout.EndHorizontal();
            target.glovesNameTarget = EditorGUILayout.ToggleLeft("Use name based retargeting", target.glovesNameTarget);
            if (target.glovesNameTarget)
            {
                EditorGUILayout.HelpBox("This means that you going to use the bone's name instead the Avatar Bones to retarget the mesh", MessageType.Warning);
            }
            GUILayout.EndVertical();
        }

        public void OnWeaponInspectorGUI(SerializedObject SO)
        {
            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("weapons"));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        struct MyUserData
        {
            public readonly RandomCharacterClass target;
            public readonly Transform parent;

            public MyUserData(RandomCharacterClass target, Transform parent)
            {
                this.target = target;
                this.parent = parent;
            }
        }

        void OnTransformSelect(MyUserData data)
        {
            data.target.parent = data.parent;
        }

        public void OnAddOnsInspectorGUI(SerializedObject SO, RandomCharacterClass target)
        {


            GUILayout.BeginVertical("", "Box");
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(SO.FindProperty("events.events"));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            EditorGUILayout.Space(20);

            GUILayout.BeginVertical("Custom Behaviours", "box");

            EditorGUILayout.Space(20);

            GUILayout.EndVertical();

            //target.setModuleCount = EditorGUILayout.IntField("Size", target.setModuleCount);

            if (GUILayout.Button("Add module"))
            {
                target.customBehavioursModules.Add (new ModuleConainer(null));
                target.moduleCount++;
            }

            EditorGUILayout.Space(20);

            for (int i = 0; i < target.moduleCount; i++)
            {
                GUILayout.BeginHorizontal();
                string text = "";
                if (target.customBehavioursModules[i].fold)
                {
                    text = "-";
                }
                else
                {
                    text = ">";
                }
                if (GUILayout.Button(text, GUILayout.Width(20)))
                {
                    target.customBehavioursModules[i].fold = !target.customBehavioursModules[i].fold;
                }
                target.customBehavioursModules[i].setModule = EditorGUILayout.ObjectField(target.customBehavioursModules[i].setModule, typeof(GameObject), false) as GameObject;
                
                if (GUILayout.Button("Remove",GUILayout.Width(80)))
                {
                    target.customBehavioursModules.Remove(target.customBehavioursModules[i]);
                    target.moduleCount--;
                    break;
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (target.customBehavioursModules[i].fold)
                {
                    GUILayout.Space(20);
                }
                GUILayout.BeginVertical();
                if (target.customBehavioursModules[i].fold)
                {
                    GUILayout.BeginHorizontal();
                    target.customBehavioursModules[i].overrideLayer = EditorGUILayout.ToggleLeft("Override layer", target.customBehavioursModules[i].overrideLayer, GUILayout.Width(150));
                    if (target.customBehavioursModules[i].overrideLayer)
                    {
                        target.customBehavioursModules[i].layer = EditorGUILayout.LayerField(target.customBehavioursModules[i].layer);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    target.customBehavioursModules[i].overrideTag = EditorGUILayout.ToggleLeft("Override tag", target.customBehavioursModules[i].overrideTag, GUILayout.Width(150));
                    if (target.customBehavioursModules[i].overrideTag)
                    {
                        target.customBehavioursModules[i].tag = EditorGUILayout.TagField(target.customBehavioursModules[i].tag);
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Space(10);

                    Component[] components = new Component[0];
                    if (target.customBehavioursModules[i].setModule)
                    {
                        components = target.customBehavioursModules[i].setModule.GetComponents<Component>();

                        if (target.customBehavioursModules[i].setModule.GetComponent<Animator>() && target.customBehavioursModules[i].moduleUse == Enumerations.ModuleUse.ParentContainer)
                        {
                            EditorGUILayout.HelpBox("This Behaviour Module contains his own Animator Component. It's strongly recomended use this as the default Animator Controller.", MessageType.Warning);
                            target.customBehavioursModules[i].useContainerAnimator = EditorGUILayout.ToggleLeft("Use module Animator Controller as default", target.customBehavioursModules[i].useContainerAnimator);
                        }

                        EditorGUILayout.Space(20);

                        //target.ModuleContainer = EditorGUILayout.ToggleLeft("Use Module as default container", target.ModuleContainer);
                        target.customBehavioursModules[i].moduleUse = (Enumerations.ModuleUse)EditorGUILayout.EnumPopup("Module implementation", target.customBehavioursModules[i].moduleUse);

                        switch (target.customBehavioursModules[i].moduleUse)
                        {
                            case Enumerations.ModuleUse.ParentContainer:
                                Transform[] childs = target.customBehavioursModules[i].setModule.GetComponentsInChildren<Transform>();
                                string buttonName = "Choose a parent Transform";
                                if (!target.customBehavioursModules[i].parent)
                                {
                                    if (GUILayout.Button(buttonName, EditorStyles.miniButton))
                                    {
                                        GenericMenu menu = new GenericMenu();

                                        menu.AddItem(new GUIContent("None"), false, () => OnTransformSelect(new MyUserData(target, null)));

                                        foreach (Transform child in childs)
                                        {
                                            if (child.parent == target.customBehavioursModules[i].setModule.transform)
                                            {
                                                menu.AddItem(new GUIContent(child.name), false, () => OnTransformSelect(new MyUserData(target, child)));
                                            }
                                        }

                                        menu.ShowAsContext();
                                    }
                                }
                                else
                                {
                                    buttonName = target.parent.name;
                                    if (GUILayout.Button(buttonName, EditorStyles.miniButton))
                                    {
                                        GenericMenu menu = new GenericMenu();

                                        menu.AddItem(new GUIContent("None"), false, () => OnTransformSelect(new MyUserData(target, null)));

                                        foreach (Transform child in childs)
                                        {
                                            if (child.parent == target.customBehavioursModules[i].setModule.transform)
                                            {
                                                menu.AddItem(new GUIContent(child.name), target.parent.Equals(child), () => OnTransformSelect(new MyUserData(target, child)));
                                            }
                                        }

                                        menu.ShowAsContext();
                                    }
                                }
                                break;

                            case Enumerations.ModuleUse.CopyComponents:
                                EditorGUILayout.HelpBox("All the Components of the module will be copied into the generated character", MessageType.Info);
                                break;

                            case Enumerations.ModuleUse.CopyHierarchy:
                                EditorGUILayout.HelpBox("All the Components and child GameObjects of the module will be copied into the generated character", MessageType.Info);
                                break;
                        }

                        EditorGUILayout.Space(20);
                    }

                    foreach (Component component in components)
                    {
                        if (component.GetType() == typeof(Transform))
                            continue;

                        GUILayout.BeginVertical(component.GetType().ToString(), "box");
                        EditorGUILayout.Space(20);
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(10);
                        GUILayout.BeginVertical();
                        Editor tmpEditor = Editor.CreateEditor(component);
                        tmpEditor.OnInspectorGUI();
                        EditorGUILayout.Space(10);
                        if (GUILayout.Button("Remove component", GUILayout.Height(25)))
                        {
                            DestroyImmediate(target.customBehavioursModules[i].setModule.GetComponent(component.GetType()), true);
                        }
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                        EditorGUILayout.Space(10);
                    }



                    if (target.customBehavioursModules[i].setModule)
                    {

                        GUILayout.Space(15);
                        EditorGUILayout.HelpBox("You can add Sripts to the Behaviour Module. But if you want to add a Component you most go to the module prefab and add it manualy", MessageType.Info);
                        GUILayout.Space(15);

                        if (!addingScript)
                        {
                            EditorGUILayout.Space(20);

                            GUILayout.BeginHorizontal();
                            EditorGUILayout.Space(20);

                            if (GUILayout.Button("Add Behaviour", GUILayout.Height(30)))
                            {
                                addingScript = true;
                            }

                            EditorGUILayout.Space(20);
                            GUILayout.EndHorizontal();
                        }

                        if (addingScript)
                        {
                            EditorGUILayout.Space(20);

                            GUILayout.BeginVertical("", "box");
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(10);
                            GUILayout.BeginVertical();

                            GUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Behaviour to add", GUILayout.Width(100));
                            newScript = EditorGUILayout.ObjectField(newScript, typeof(MonoScript), false) as MonoScript;
                            GUILayout.EndHorizontal();

                            EditorGUILayout.Space(20);
                            if (newScript)
                            {
                                GUILayout.BeginHorizontal();
                                if (GUILayout.Button("Add Behaviour", GUILayout.Height(30)))
                                {
                                    target.customBehavioursModules[i].setModule.AddComponent(newScript.GetClass());
                                    addingScript = false;
                                    newScript = null;
                                }
                                if (GUILayout.Button("Cancel", GUILayout.Height(30)))
                                {
                                    addingScript = false;
                                    newScript = null;
                                }
                                GUILayout.EndHorizontal();
                            }
                            else
                            {
                                if (GUILayout.Button("Cancel", GUILayout.Height(30)))
                                {
                                    addingScript = false;
                                    newScript = null;
                                }
                            }

                            GUILayout.EndVertical();
                            GUILayout.EndHorizontal();

                            GUILayout.EndVertical();
                        }
                    }
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                EditorGUILayout.Space(20);
            }

            EditorGUILayout.Space(60);
        }

    }
}
