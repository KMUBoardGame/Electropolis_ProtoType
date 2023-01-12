using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBoard : MonoBehaviour
{
	[SerializeField]
	StageDataManager StageData;

	private void Awake()
	{
		CityTiles = new List<CityTile>();
	}

	private void Start()
	{
		SetCityBoard();
	}

	[Header("City Board")]
	[SerializeField]
	GameObject CityTilePrefab;
	[SerializeField]
	Transform CityTileHouse;

	public List<CityTile> CityTiles;

	#region Modulizable

	float TileSpace = 3.25f;

	int XRowNum = 5;
	int YRowNum = 5;

	float XRowStart = -6.5f;
	float YRowStart = 6.0f;

	#endregion


	void SetCityBoard()
	{
		//int? emptyNum = null;
		//int? index = 0;

		int row = 0;
		int column = 0;

		float XRowEnd = XRowStart + TileSpace * XRowNum;
		float YRowEnd = YRowStart - TileSpace * YRowNum;

		for (float y = YRowStart; y > YRowEnd; y -= TileSpace)
		{
			for (float x = XRowStart; x < XRowEnd; x += TileSpace)
			{
				GameObject CityTile = Instantiate(CityTilePrefab, new Vector2(x, y), Quaternion.identity, CityTileHouse) as GameObject;


				string CityTileName = "CityTile " + row + column;
				CityTile.name = CityTileName;
				//StageData.cityBoard.Add(CityTileName, emptyNum);

				//TODO: 코드 수정 여지 있음
				CityTile.GetComponent<CityTile>().SetRowColumn(row, column);
				CityTiles.Add(CityTile.GetComponent<CityTile>());

				column++;
				//index++;
			}

			column = 0;
			row++;
		}
	}
}
