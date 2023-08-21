using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCard : MonoBehaviour
{
    #region Initialization

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


	string cardSide;

	private void Start()
	{
		Initialize();
	}

    #endregion

	void Initialize()
    {
		FrontSide = transform.GetChild(0);
		BackSide = transform.GetChild(1);

		BackSide.gameObject.SetActive(false);
		cardSide = "Front";
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


    private void OnMouseDown()
    {
		FlipAreaCard();
	}

    public void FlipAreaCard()
    {
		if(cardSide == "Front")
        {
			BackSide.gameObject.SetActive(true);
			FrontSide.gameObject.SetActive(false);

			cardSide = "Back";
		}
		else if(cardSide == "Back")
        {
			FrontSide.gameObject.SetActive(true);
			BackSide.gameObject.SetActive(false);

			cardSide = "Front";
		}
    }
}
