using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityTile : MonoBehaviour
{
	public bool IsSoldOut;

	int Row;
	int Column;

	public GameObject CityBuildingCardOnTile;

	public void SetRowColumn(int row, int column)
	{
		Row = row;
		Column = column;
	}

	public void SetCityBuildingCard(GameObject cityBuildingCard)
	{
		CityBuildingCardOnTile = cityBuildingCard;
	}
}
