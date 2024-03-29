using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamp : MonoBehaviour
{
	Vector3 initialPos;
	public enum StampState
	{
		Hold, Release
	};

	StampState stampState;

	public Transform ClickedObj;


	private void Start()
	{
		initialPos = transform.position;
		stampState = StampState.Release;
	}

	private void OnMouseDown()
	{
		stampState = StampState.Hold;
	}

	private void Update()
	{
		if(stampState == StampState.Hold)
		{
			transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
		}
	}

	private void OnMouseUp()
	{
		stampState = StampState.Release;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 origin = ray.origin;
		Vector3 dir = ray.direction;

		RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir);

		for (int i = 0; i < hits.Length; i++)
		{
			if (hits[i].collider != null && hits[i].transform.tag == "AreaCard")
			{
				ClickedObj = hits[i].transform;
				Debug.Log(ClickedObj);
			}
		} 

		transform.position = initialPos;
	}
}
