using UnityEngine;
using TMPro;

public class CityBuildingCardStack : MonoBehaviour
{
	[SerializeField] StageDataManager StageData;
	[SerializeField] GameDataManager GameData;
	[SerializeField] GameObject CityBuildingCardPrefab;

	float CityBuldingCardSpace = 2.6f;
	float TopStartPoint = 6.3f;

	public void HandOutCityBuildingCards()
	{
		int BuildingCount = StageData.buildingCards.Count;
		int index = 0;


		for (float y = TopStartPoint; y > TopStartPoint - CityBuldingCardSpace * BuildingCount; y -= CityBuldingCardSpace)
		{
			//Debug.Log(index);

			GameObject CityBuildingCard = Instantiate(CityBuildingCardPrefab, new Vector3(-12.0f, y, 0.0f), Quaternion.identity, transform) as GameObject;

			int BuildingCardNum = int.Parse(StageData.buildingCards[index]);

			CityBuildingCard.GetComponent<SpriteRenderer>().sprite = GameData.BuildingCardSprites[BuildingCardNum];
			CityBuildingCard.name = GameData.BuildingCardData[BuildingCardNum];

			index++;
		}
	}

	public void RetakeCityBuildingCards()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Destroy(transform.GetChild(i).gameObject);
		}
	}
}

