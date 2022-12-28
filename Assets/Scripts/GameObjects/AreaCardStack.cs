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
		//���� ������ ������ ����1: AŸ�� �й��ؾ� ��
		HandOutCardStack("A");
	}

	public void HandOutCardStack(string CardType)
	{
		// TODO: ȿ���� ����ó�� ����
		if (CardType != "A" && CardType != "B" && CardType != "C") { Debug.Log("AreaCardType Out Of Range"); return; }

		List<int> AreaCardStack = GameData.AreaCardData[CardType];

		//A�� Ascii = 41 -> 'A' - A_ascii = 0, 'B' - A_ascii = 1, 'C' - A_ascii = 2 ...
		List<Sprite> CurrentAreaCardTypeSpriteList = GameData.AreaCardSprites[CardType[0] - A_ascii];


		for (int i = 0; i < 4; i++)
		{ 
			int StackCardIndex = Random.Range(0, AreaCardStack.Count);
			int AreaCardNumber = AreaCardStack[StackCardIndex];

			//Debug.Log(AreaCardNumber);

			GameObject AreaCard = Instantiate(AreaCardPrefab, AreaCardPoses[i], Quaternion.identity, transform) as GameObject;

			AreaCard.GetComponent<AreaCard>().InitializeAreaCardInfo(CardType, AreaCardNumber);

			Sprite CardSprite = CurrentAreaCardTypeSpriteList[AreaCardNumber % CurrentAreaCardTypeSpriteList.Count];

			//���� ����: �����ϱ�
			AreaCard.GetComponent<AreaCard>().SetFrontSprite(CardSprite);

			AreaCardStack.RemoveAt(StackCardIndex);  //���� ī��� �����Ϳ��� ����: �ߺ� ����
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
