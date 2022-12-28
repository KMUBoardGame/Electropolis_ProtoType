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
		//게임 시작할 때에는 라운드1: A타입 분배해야 함
		HandOutCardStack("A");
	}

	public void HandOutCardStack(string CardType)
	{
		// TODO: 효율적 예외처리 예정
		if (CardType != "A" && CardType != "B" && CardType != "C") { Debug.Log("AreaCardType Out Of Range"); return; }

		List<int> AreaCardStack = GameData.AreaCardData[CardType];

		//A의 Ascii = 41 -> 'A' - A_ascii = 0, 'B' - A_ascii = 1, 'C' - A_ascii = 2 ...
		List<Sprite> CurrentAreaCardTypeSpriteList = GameData.AreaCardSprites[CardType[0] - A_ascii];


		for (int i = 0; i < 4; i++)
		{ 
			int StackCardIndex = Random.Range(0, AreaCardStack.Count);
			int AreaCardNumber = AreaCardStack[StackCardIndex];

			//Debug.Log(AreaCardNumber);

			GameObject AreaCard = Instantiate(AreaCardPrefab, AreaCardPoses[i], Quaternion.identity, transform) as GameObject;

			AreaCard.GetComponent<AreaCard>().InitializeAreaCardInfo(CardType, AreaCardNumber);

			Sprite CardSprite = CurrentAreaCardTypeSpriteList[AreaCardNumber % CurrentAreaCardTypeSpriteList.Count];

			//문제 있음: 수정하기
			AreaCard.GetComponent<AreaCard>().SetFrontSprite(CardSprite);

			AreaCardStack.RemoveAt(StackCardIndex);  //뽑은 카드는 데이터에서 제외: 중복 방지
		}
	}

	public void RetakeAreaCards()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Destroy(transform.GetChild(i).gameObject);
		}
	}
}
