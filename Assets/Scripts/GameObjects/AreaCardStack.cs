using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCardStack : MonoBehaviour
{
	[SerializeField] GameDataManager GameData;
	[SerializeField] StageDataManager StageData;

	[SerializeField] GameObject AreaCardPrefab;

	Vector2[] AreaCardPoses = new Vector2[4] { new Vector2(-4.0f, 3.0f), new Vector2(4.0f, 3.0f),
																			new Vector2(-4.0f, -5.0f), new Vector2(4.0f, -5.0f)};

	int A_ascii;

	private void Awake()
	{
		A_ascii = (int)'A';
	}

	private void Start()
	{
		HandOutCardStack("C");
	}

	public void HandOutCardStack(string CardType)
	{
		// TODO: 효율적 예외처리 예정
		if (CardType != "A" && CardType != "B" && CardType != "C") { Debug.Log("AreaCardType Out Of Range"); return; }	 

		for (int i = 0; i < 4; i++)
		{
			List<int> AreaCardStack = GameData.AreaCardData[CardType];
			int AreaCardNumber = AreaCardStack[Random.Range(0, AreaCardStack.Count)];

			GameObject AreaCard = Instantiate(AreaCardPrefab, AreaCardPoses[i], Quaternion.identity, transform) as GameObject;
			AreaCard.GetComponent<AreaCard>().InitializeAreaCardInfo(CardType, AreaCardNumber);


			//여기부터는 에리어카드 스크립트에서 변경? 해야함?
			Transform FrontSide = AreaCard.transform.Find("Front");
			Transform BackSide = AreaCard.transform.Find("Back");

			//A의 Ascii = 41 -> 'A' - A_ascii = 0, 'B' - A_ascii = 1, 'C' - A_ascii = 2,  
			List<Sprite> CurrentAreaCardTypeSpriteList = GameData.AreaCardSprites[CardType[0]- A_ascii];
			// TOTHINK: 에리어카드 스프라이트 리스트를 게임데이터에 넣어야 하는지? 에리어카드 객체에 옮겨야 하는지?
			FrontSide.GetComponent<SpriteRenderer>().sprite = CurrentAreaCardTypeSpriteList[AreaCardNumber % CurrentAreaCardTypeSpriteList.Count];

			StageData.areaCards.Add(AreaCard);
		}
	}
}
