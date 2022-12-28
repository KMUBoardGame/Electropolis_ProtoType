using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCard : MonoBehaviour
{
	string cardType;
	public string CardType
	{
		get { return cardType;  }
	}

	int cardNum;
	public int CardNum
	{
		get { return cardNum; }
	}

	Transform FrontSide;
	Transform BackSide;

	private void Start()
	{
		// ?: Front, Back 저장 후 불러오기가 안 됨
		FrontSide = transform.GetChild(0);
		BackSide = transform.GetChild(1);
	}


	public void InitializeAreaCardInfo(string _CardType, int _CardNum)
	{
		cardType = _CardType;
		cardNum = _CardNum;

		transform.name = cardType + cardNum.ToString();
	}

	public void SetFrontSprite(Sprite cardSprite)
	{
		transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = cardSprite;
	}
}
