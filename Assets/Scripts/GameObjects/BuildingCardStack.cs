using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCardStack : MonoBehaviour
{
	[SerializeField] GameDataManager GameData;

	[SerializeField] GameObject BuildingCardPrefab;

	[SerializeField] int InitialXPos;
	[SerializeField] int InitialYPos;
	[SerializeField] int cardSpace;

	public void MixBuildingCards()
	{
		for (int x = -InitialXPos; x <= InitialXPos; x += cardSpace)
		{
			for (int y = -InitialYPos; y <= InitialYPos; y += cardSpace)
			{
				//spawn building cards | 16types
				int CardNum = Random.Range(0, GameData.BuildingCardNum);
				GameObject BuildingCard = Instantiate(BuildingCardPrefab, new Vector3(x, y, 0), Quaternion.identity, gameObject.transform) as GameObject;
				BuildingCard.GetComponentInChildren<TMPro.TextMeshPro>().text = CardNum.ToString();
				BuildingCard.name = CardNum.ToString();
			}
		}
	}

	private void Start()
	{
		MixBuildingCards();
	}

	public void RetakeBuildingCards()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Destroy(transform.GetChild(i).gameObject);
		}
	}
}
