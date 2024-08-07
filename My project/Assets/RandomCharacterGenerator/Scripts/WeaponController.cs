using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WeaponController : MonoBehaviour {
	public WeaponHandle handling;

	public Transform L_HandPosition, R_HandPosition;
	public Transform shootPoint;

	public enum WeaponHandle
	{
		Right,
		Left,
		TwoHanded
	}

	private void Start()
	{
		if (!L_HandPosition)
		{
			L_HandPosition = new GameObject("Left_Hand").transform;
			L_HandPosition.parent = transform;
			L_HandPosition.localPosition = Vector3.zero;
			L_HandPosition.localRotation = Quaternion.identity;
		}
		if (!R_HandPosition)
		{
			R_HandPosition = new GameObject("Right_Hand").transform;
			R_HandPosition.parent = transform;
			R_HandPosition.localPosition = Vector3.zero;
			R_HandPosition.localRotation = Quaternion.identity;
		}
		if (!shootPoint)
		{
			shootPoint = new GameObject("Shoot_Point").transform;
			shootPoint.parent = transform;
			shootPoint.localPosition = Vector3.zero;
			shootPoint.localRotation = Quaternion.identity;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(L_HandPosition.position, "L_Hand_Icon.tiff", true);
		Gizmos.DrawIcon(R_HandPosition.position, "R_Hand_Icon.tiff", true);
		Gizmos.DrawIcon(shootPoint.position, "ShootPoint_Icon.tiff", true);
	}
}
