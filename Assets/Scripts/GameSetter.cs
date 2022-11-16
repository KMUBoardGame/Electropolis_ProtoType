using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSetter : MonoBehaviour
{

	[SerializeField]
	StageDataManager StageData;

	void Start()
    {
		//1. Hand Final Card Out
		SetFinalCardNum();

		//2. Set CityBoard
		SetCityBoard();



		MoveToGamePlay();
	}


	[Header("Game Player")]
	[SerializeField]
	GameObject GamePlayer;
	void MoveToGamePlay()
	{
		if (!GamePlayer.activeSelf)
			GamePlayer.SetActive(true);
	}


	#region Set Methods

		[Header("Final Card")]
		[SerializeField]
		TextMeshProUGUI FinalCard;
		void SetFinalCardNum()
		{
			int RandomNum = Random.Range(1, 6);

			FinalCard.text = RandomNum.ToString() + "¹ø";
		}


		[Header("City Board")]
		[SerializeField]
		GameObject CityTilePrefab;
		[SerializeField]
		Transform CityTileHouse;

		#region Modulizable

		float TileSpace = 3.25f;

		int XRowNum = 5;
		int YRowNum = 5;

		float XRowStart = -6.5f;
		float YRowStart = 6.0f;

		#endregion

		void SetCityBoard()
		{
			int? emptyNum = null;
			int? index = 0;

			float XRowEnd = XRowStart + TileSpace * XRowNum;
			float YRowEnd = YRowStart - TileSpace * YRowNum;

			for (float y = YRowStart; y > YRowEnd; y -= TileSpace)
			{
				for (float x = XRowStart; x < XRowEnd; x += TileSpace)
				{
					GameObject CityTile = Instantiate(CityTilePrefab, new Vector2(x, y), Quaternion.identity, CityTileHouse) as GameObject;
				
					string CityTileName = "CityTile" + index;

					CityTile.name = CityTileName;
					StageData.cityBoard.Add(CityTileName, emptyNum);

					index++;
				}
			}
		}

	#endregion
}
