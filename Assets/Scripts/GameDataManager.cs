using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
	#region MaxStep/MaxRound

	int maxRound = 6;
	public int MaxRound
	{
		get { return maxRound; }
	}


	int maxStep = 4;
	public int MaxStep
	{
		get { return maxStep; }
	}

	#endregion


	#region AreaCardData_ Sprite, MaximumNumOfCard

	[SerializeField] List<Sprite> AreaSpriteA;
	[SerializeField] List<Sprite> AreaSpriteB;
	[SerializeField] List<Sprite> AreaSpriteC;

	//TODO: Only Getter
	public List<List<Sprite>> AreaCardSprites = new List<List<Sprite>>();

	public Dictionary<string, List<int>> AreaCardData = new Dictionary<string, List<int>>
	{
		{ "A", new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 } },
		{ "B", new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 } },
		{ "C", new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 } }
	};

	#endregion


	#region BuildingCardData

	public List<Sprite> BuildingCardSprites;
	public Dictionary<int, string> BuildingCardData = new Dictionary<int, string>
	{
		{ 0,  "CoalPlant"}, {1, "Coal"}, {2, "GasPlant"}, {3, "Gas"}, {4, "AtomicPlant"}, {5, "UraniumProcessor"}, {6, "Uranium"}, {7, "BioEnergyPlant"}, {8, "GeoTemperaturePlant"},
		{9, "HydroPlant"}, {10, "SolarPlant"},{11, "WindPlant"}, {12, "AmusementPark"}, {13, "Hospital"},{14, "Museum"},{15, "Park"}, {16, "PublicHousing"}, {17, "PollutionDecreaser"}
	};
	public int BuildingCardNum;

	#endregion

	private void Awake()
	{
		//필요 시 GameDataMager 데이터 초기화
		AreaCardSprites.Add(AreaSpriteA);
		AreaCardSprites.Add(AreaSpriteB);
		AreaCardSprites.Add(AreaSpriteC);

		//건물카드 개수 초기화
		BuildingCardNum = BuildingCardData.Count;
	}
}
