using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RandomCharacter {
    [CustomEditor(typeof(CharacterTemplate))]
    public class LayoutEditor : Editor
    {
        SerializedObject layout_SO;

        public override void OnInspectorGUI()
        {
            CharacterTemplate layout = (CharacterTemplate)target;
            Animator anim = layout.GetComponent<Animator>();

            #region Shoulders
            GUILayout.BeginVertical("Shoulders", "box");
            EditorGUILayout.Space(20);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Left",GUILayout.Width(50));
            layout.L_ShoulderMarker = EditorGUILayout.ObjectField(layout.L_ShoulderMarker, typeof(Transform), true) as Transform;
            if (!layout.L_ShoulderMarker)
            {
                if (GUILayout.Button("Add", GUILayout.Width(50)))
                {
                    Transform shoulder = new GameObject("Left_Shoulder_Marker").transform;
                    shoulder.parent = anim.GetBoneTransform(HumanBodyBones.LeftShoulder);
                    shoulder.localPosition = Vector3.zero;
                    shoulder.localRotation = Quaternion.identity;
                    layout.L_ShoulderMarker = shoulder;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Right", GUILayout.Width(50));
            layout.R_ShoulderMarker = EditorGUILayout.ObjectField(layout.R_ShoulderMarker, typeof(Transform), true) as Transform;
            if (!layout.R_ShoulderMarker)
            {
                if (GUILayout.Button("Add", GUILayout.Width(50)))
                {
                    Transform shoulder = new GameObject("Right_Shoulder_Marker").transform;
                    shoulder.parent = anim.GetBoneTransform(HumanBodyBones.RightShoulder);
                    shoulder.localPosition = Vector3.zero;
                    shoulder.localRotation = Quaternion.identity;
                    layout.R_ShoulderMarker = shoulder;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            #endregion

            EditorGUILayout.Space(5);

            #region Wrists
            GUILayout.BeginVertical("Wrists", "box");
            EditorGUILayout.Space(20);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Left", GUILayout.Width(50));
            layout.L_WristMarker = EditorGUILayout.ObjectField(layout.L_WristMarker, typeof(Transform), true) as Transform;
            if (!layout.L_WristMarker)
            {
                if (GUILayout.Button("Add", GUILayout.Width(50)))
                {
                    Transform wrist = new GameObject("Left_Wrist_Marker").transform;
                    wrist.parent = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
                    wrist.localPosition = Vector3.zero;
                    wrist.localRotation = Quaternion.identity;
                    layout.L_WristMarker = wrist;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Right", GUILayout.Width(50));
            layout.R_WristMarker = EditorGUILayout.ObjectField(layout.R_WristMarker, typeof(Transform), true) as Transform;
            if (!layout.R_WristMarker)
            {
                if (GUILayout.Button("Add", GUILayout.Width(50)))
                {
                    Transform wrist = new GameObject("Right_Wrist_Marker").transform;
                    wrist.parent = anim.GetBoneTransform(HumanBodyBones.RightLowerArm);
                    wrist.localPosition = Vector3.zero;
                    wrist.localRotation = Quaternion.identity;
                    layout.R_WristMarker = wrist;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            #endregion

            EditorGUILayout.Space(5);

            #region Hands
            GUILayout.BeginVertical("Hands", "box");
            EditorGUILayout.Space(20);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Left", GUILayout.Width(50));
            layout.L_HandMarker = EditorGUILayout.ObjectField(layout.L_HandMarker, typeof(Transform), true) as Transform;
            if (!layout.L_HandMarker)
            {
                if (GUILayout.Button("Add", GUILayout.Width(50)))
                {
                    Transform hand = new GameObject("Left_Hand_Marker").transform;
                    hand.parent = anim.GetBoneTransform(HumanBodyBones.LeftHand);
                    hand.localPosition = Vector3.zero;
                    hand.localRotation = Quaternion.identity;
                    layout.L_HandMarker = hand;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Right", GUILayout.Width(50));
            layout.R_HandMarker = EditorGUILayout.ObjectField(layout.R_HandMarker, typeof(Transform), true) as Transform;
            if (!layout.R_HandMarker)
            {
                if (GUILayout.Button("Add", GUILayout.Width(50)))
                {
                    Transform hand = new GameObject("Right_Hand_Marker").transform;
                    hand.parent = anim.GetBoneTransform(HumanBodyBones.RightHand);
                    hand.localPosition = Vector3.zero;
                    hand.localRotation = Quaternion.identity;
                    layout.R_HandMarker = hand;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            #endregion

            EditorGUILayout.Space(5);

            #region Thighs
            GUILayout.BeginVertical("Thighs", "box");
            EditorGUILayout.Space(20);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Left", GUILayout.Width(50));
            layout.L_ThighMarker = EditorGUILayout.ObjectField(layout.L_ThighMarker, typeof(Transform), true) as Transform;
            if (!layout.L_ThighMarker)
            {
                if (GUILayout.Button("Add", GUILayout.Width(50)))
                {
                    Transform thigh = new GameObject("Left_Thigh_Marker").transform;
                    thigh.parent = anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
                    thigh.localPosition = Vector3.zero;
                    thigh.localRotation = Quaternion.identity;
                    layout.L_ThighMarker = thigh;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Right", GUILayout.Width(50));
            layout.R_ThighMarker = EditorGUILayout.ObjectField(layout.R_ThighMarker, typeof(Transform), true) as Transform;
            if (!layout.R_ThighMarker)
            {
                if (GUILayout.Button("Add", GUILayout.Width(50)))
                {
                    Transform thigh = new GameObject("Right_Thigh_Marker").transform;
                    thigh.parent = anim.GetBoneTransform(HumanBodyBones.RightUpperLeg);
                    thigh.localPosition = Vector3.zero;
                    thigh.localRotation = Quaternion.identity;
                    layout.R_ThighMarker = thigh;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            #endregion

            EditorGUILayout.Space(5);

            #region Knees
            GUILayout.BeginVertical("Knees", "box");
            EditorGUILayout.Space(20);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Left", GUILayout.Width(50));
            layout.L_KneeMarker = EditorGUILayout.ObjectField(layout.L_KneeMarker, typeof(Transform), true) as Transform;
            if (!layout.L_KneeMarker)
            {
                if (GUILayout.Button("Add", GUILayout.Width(50)))
                {
                    Transform thigh = new GameObject("Left_Knee_Marker").transform;
                    thigh.parent = anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
                    thigh.localPosition = Vector3.zero;
                    thigh.localRotation = Quaternion.identity;
                    layout.L_KneeMarker = thigh;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Right", GUILayout.Width(50));
            layout.R_KneeMarker = EditorGUILayout.ObjectField(layout.R_KneeMarker, typeof(Transform), true) as Transform;
            if (!layout.R_KneeMarker)
            {
                if (GUILayout.Button("Add", GUILayout.Width(50)))
                {
                    Transform thigh = new GameObject("Right_Knee_Marker").transform;
                    thigh.parent = anim.GetBoneTransform(HumanBodyBones.RightLowerLeg);
                    thigh.localPosition = Vector3.zero;
                    thigh.localRotation = Quaternion.identity;
                    layout.R_KneeMarker = thigh;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            #endregion

            EditorGUILayout.Space(5);

            #region Items
            GUILayout.BeginVertical("Items", "box");
            if (!EditorUtility.IsPersistent(target))
            {
                GUILayout.Space(20);
                GUILayout.BeginHorizontal();
                GUILayout.Space(100);
                if (GUILayout.Button("Add item marker", GUILayout.Height(25)))
                {
                    layout.itemsMarkers.Add(new CharacterTemplate.ItemCollection(anim));
                }
                GUILayout.Space(100);
                GUILayout.EndHorizontal();
                GUILayout.Space(10);
                for (int i = 0; i < layout.itemsMarkers.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        DestroyImmediate(layout.itemsMarkers[i].marker, true);
                        layout.itemsMarkers.Remove(layout.itemsMarkers[i]);
                        break;
                    }
                    layout.itemsMarkers[i].marker = EditorGUILayout.ObjectField(layout.itemsMarkers[i].marker, typeof(Transform), true) as Transform;
                    layout.itemsMarkers[i].boneParent = (HumanBodyBones)EditorGUILayout.EnumPopup(layout.itemsMarkers[i].boneParent, GUILayout.Width(100));

                    if (!layout.itemsMarkers[i].marker && anim.GetBoneTransform(layout.itemsMarkers[i].boneParent))
                    {
                        if (GUILayout.Button("Add", GUILayout.Width(75)))
                        {
                            Transform item = new GameObject("Item_Marker").transform;
                            item.parent = anim.GetBoneTransform(layout.itemsMarkers[i].boneParent);
                            item.localPosition = Vector3.zero;
                            item.localRotation = Quaternion.identity;
                            layout.itemsMarkers[i].marker = item;
                        }
                    }
                    GUILayout.EndHorizontal();

                    if (!anim.GetBoneTransform(layout.itemsMarkers[i].boneParent))
                    {
                        EditorGUILayout.HelpBox("This Avatar configuration does not contains a Transform for this Bone", MessageType.Error);
                    }
                }
            }
            else
            {
                GUILayout.Space(20);
                EditorGUILayout.HelpBox("You most open the prefab to make chenges in this fields", MessageType.Warning);
            }

            GUILayout.EndVertical();
            #endregion
        }
    }
}
