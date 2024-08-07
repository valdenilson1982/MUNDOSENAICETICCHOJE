using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RandomCharacter
{
    [CustomEditor(typeof(RandomCharacterBehaviour))]
    public class CharacterStatEditor : Editor
    {
        bool addingNewStat = false;
        RandomCharacterStat newStat;

        SerializedObject behaviour_SO;

        public void OnEnable()
        {
            behaviour_SO = new SerializedObject((RandomCharacterBehaviour)target);
        }

        public override void OnInspectorGUI()
        {
            RandomCharacterBehaviour behaviour = (RandomCharacterBehaviour)target;

            GUILayout.BeginVertical("Character Info", "box");
            EditorGUILayout.Space(20);

            GUILayout.BeginHorizontal();
            behaviour.image = EditorGUILayout.ObjectField(behaviour.image, typeof(Sprite), false,
                GUILayout.Width(75), GUILayout.Height(75)) as Sprite;
            GUILayout.Space(20);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Character name", GUILayout.Width(100));
            behaviour.name = EditorGUILayout.TextField(behaviour.name);
            GUILayout.EndHorizontal();
            EditorGUILayout.LabelField("Character genre: " + behaviour.characterGenre.ToString());
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            EditorGUILayout.Space(10);

            foreach (CharacterStats s in behaviour.characterStats)
            {
                GUILayout.BeginVertical(s.name, "box");

                EditorGUILayout.Space(20);

                GUILayout.BeginHorizontal();
                if (s.icon)
                {
                    GUILayout.Box(s.icon.texture, GUILayout.Width(50), GUILayout.Height(50));
                }
                else
                {
                    GUILayout.Space(50);
                }

                GUILayout.BeginVertical();
                if (s.allowEditing)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Stat name", GUILayout.Width(75));
                    s.name = EditorGUILayout.TextField(s.name);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Stat icon", GUILayout.Width(75));
                    s.icon = EditorGUILayout.ObjectField(s.icon, typeof(Sprite), false) as Sprite;
                    GUILayout.EndHorizontal();
                }


                if (s.rangedValue)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Value", GUILayout.Width(75));
                    if (!s.allowEditing)
                    {
                        EditorGUILayout.LabelField(s.minValue.ToString() + " - " + s.maxValue.ToString());
                    }
                    else
                    {
                        float minValue = s.minValue;
                        float maxValue = s.maxValue;
                        //EditorGUILayout.LabelField("Probability range", GUILayout.Width(130));
                        minValue = EditorGUILayout.IntField(Mathf.RoundToInt(minValue), GUILayout.Width(50));
                        EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, 0, s.maxLimit);
                        maxValue = EditorGUILayout.IntField(Mathf.RoundToInt(maxValue), GUILayout.Width(50));

                        s.minValue = Mathf.RoundToInt(minValue);
                        s.maxValue = Mathf.RoundToInt(maxValue);
                    }
                    GUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Value", GUILayout.Width(75));
                    if (!s.allowEditing)
                    {
                        EditorGUILayout.LabelField(s.value.ToString());
                    }
                    else
                    {
                        s.value = EditorGUILayout.IntField(s.value);
                    }
                    GUILayout.EndHorizontal();
                }

                if (s.useProbability)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Probability", GUILayout.Width(75));
                    if (!s.allowEditing)
                    {
                        EditorGUILayout.LabelField(s.probability.ToString() + " %");
                    }
                    else
                    {
                        //EditorGUILayout.LabelField("Probability range", GUILayout.Width(130));
                        s.probability = EditorGUILayout.IntSlider(s.probability, 0, 100);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();

                if (!s.allowEditing)
                {
                    if (GUILayout.Button("Edit Stat", GUILayout.Width(100), GUILayout.Height(20)))
                    {
                        s.allowEditing = true;
                    }
                }
                else
                {
                    if (GUILayout.Button("End Edit", GUILayout.Width(100), GUILayout.Height(20)))
                    {
                        s.allowEditing = false;
                    }
                }

                if (GUILayout.Button("Remove Stat", GUILayout.Width(100), GUILayout.Height(20)))
                {
                    behaviour.characterStats.Remove(s);
                    break;
                }
                GUILayout.EndVertical();

                GUILayout.EndHorizontal();

                GUILayout.EndVertical();

                s.Update();
            }

            if (!addingNewStat)
            {
                EditorGUILayout.Space(30);
                if (GUILayout.Button("Add new Stat", GUILayout.Height(30)))
                {
                    newStat = new RandomCharacterStat();
                    addingNewStat = true;
                }
            }

            if (addingNewStat)
            {
                EditorGUILayout.Space(20);

                GUILayout.BeginVertical(newStat.name, "box");

                GUILayout.BeginHorizontal();

                if (newStat.icon)
                {
                    GUILayout.Box(newStat.icon.texture, GUILayout.Height(50), GUILayout.Width(50));
                }
                else
                {
                    GUILayout.Space(50);
                }

                EditorGUILayout.Space(20);

                GUILayout.BeginVertical();

                EditorGUILayout.Space(20);

                newStat.name = EditorGUILayout.TextField("Stat name", newStat.name);

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Stat Icon", GUILayout.Width(150));
                newStat.icon = EditorGUILayout.ObjectField(newStat.icon, typeof(Sprite), false) as Sprite;
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                float minValue = newStat.minValue;
                float maxValue = newStat.maxValue;

                EditorGUILayout.LabelField("Stat range", GUILayout.Width(150));
                minValue = EditorGUILayout.IntField(Mathf.RoundToInt(minValue), GUILayout.Width(50));
                EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, 0, newStat.maxLimit);
                maxValue = EditorGUILayout.IntField(Mathf.RoundToInt(maxValue), GUILayout.Width(50));

                newStat.minValue = Mathf.RoundToInt(minValue);
                newStat.maxValue = Mathf.RoundToInt(maxValue);

                GUILayout.EndHorizontal();

                newStat.rangedValue = EditorGUILayout.ToggleLeft("Use ranged value", newStat.rangedValue);
                if (newStat.rangedValue)
                {
                    EditorGUILayout.HelpBox("With Ranged Value enabled the value will iterate between the min and max values and return randomly", MessageType.Info);
                }

                EditorGUILayout.Space(5);

                GUILayout.BeginHorizontal();
                newStat.useProbabilityTrigger = EditorGUILayout.ToggleLeft("Probability stat", newStat.useProbabilityTrigger, GUILayout.Width(130));

                EditorGUILayout.Space(18);

                if (newStat.useProbabilityTrigger)
                {
                    float minProb = newStat.minProb;
                    float maxProb = newStat.maxProb;
                    //EditorGUILayout.LabelField("Probability range", GUILayout.Width(130));
                    minProb = EditorGUILayout.IntField(Mathf.RoundToInt(minProb), GUILayout.Width(50));
                    EditorGUILayout.MinMaxSlider(ref minProb, ref maxProb, 0, newStat.maxProbLimit);
                    maxProb = EditorGUILayout.IntField(Mathf.RoundToInt(maxProb), GUILayout.Width(50));

                    newStat.minProb = Mathf.RoundToInt(minProb);
                    newStat.maxProb = Mathf.RoundToInt(maxProb);
                }

                GUILayout.EndHorizontal();

                newStat.Update();

                GUILayout.EndVertical();

                GUILayout.EndHorizontal();

                GUILayout.EndVertical();

                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Add Stat"))
                {
                    behaviour.characterStats.Add(new CharacterStats(newStat));
                    addingNewStat = false;
                }
                if (GUILayout.Button("Cancel"))
                {
                    addingNewStat = false;
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(behaviour_SO.FindProperty("characterEvents"));
        }
    }
}
