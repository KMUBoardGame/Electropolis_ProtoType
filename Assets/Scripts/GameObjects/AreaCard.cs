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


	public void InitializeAreaCardInfo(string _CardType, int _CardNum)
	{
		cardType = _CardType;
		cardNum = _CardNum;

		transform.name = CardType + CardNum.ToString();
	}
}
