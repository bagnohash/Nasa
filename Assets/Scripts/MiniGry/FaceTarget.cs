using UnityEngine;
using System.Collections;

public class FaceTarget : MonoBehaviour
{
	public Transform target;
	private Vector3 v_diff;
	private float atan2;

	void Update()
	{
		v_diff = (target.position - transform.position);	
		atan2 = Mathf.Atan2 ( v_diff.y, v_diff.x );
		transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg );
		transform.localEulerAngles += new Vector3(0, 0, -90f);
	}
}