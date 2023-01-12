using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBuildingCard : MonoBehaviour
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
		ClickedObj = null;
	}

	private void OnMouseDown()
	{
		stampState = StampState.Hold;
	}

	private void Update()
	{
		if (stampState == StampState.Hold)
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
			if (hits[i].collider != null && hits[i].transform.tag == "CityTile")
			{
				ClickedObj = hits[i].transform;
				Debug.Log(transform.name + " attached to " + ClickedObj.parent.name);
			}
		}

		if(!ClickedObj) transform.position = initialPos;

		else
		{
			transform.position = ClickedObj.Find("CityTileFront").position;
			transform.parent = ClickedObj;

			//시티타일에 카드를 놓았을 때에, 카드정보를 시티타일에 전달
			ClickedObj.GetComponent<CityTile>().SetCityBuildingCard(gameObject);
		}
	}
}