using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaySetter : MonoBehaviour
{
	void Start()
    {
		//1. Game Objcet Scene Setting
		SetInitialGameObject();

		//2. Hand Final Card Out
		SetFinalCardNum();
	}


	[SerializeField]
	TextMeshProUGUI FinalCard;
	void SetFinalCardNum()
	{
		int RandomNum = Random.Range(1, 6);

		FinalCard.text = RandomNum.ToString() + "¹ø";
	}


	[SerializeField]
	GameObject GamePlayer;
	void SetInitialGameObject()
	{
		if (!GamePlayer.activeSelf)
			GamePlayer.SetActive(true);
	}
}
