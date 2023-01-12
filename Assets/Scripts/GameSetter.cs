using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSetter : MonoBehaviour
{
	[SerializeField]
	StageDataManager StageData;

	void Start()
    {
		//1. Hand Final Card Out
		SetFinalCardNum();

		MoveToGamePlay();
	}


	[Header("Game Player")]
	[SerializeField]
	GameObject GamePlayer;
	void MoveToGamePlay()
	{
		if (!GamePlayer.activeSelf)
			GamePlayer.SetActive(true);
	}


	#region Set Methods

		[Header("Final Card")]
		[SerializeField]
		TextMeshProUGUI FinalCard;
		void SetFinalCardNum()
		{
			int RandomNum = Random.Range(1, 6);

			FinalCard.text = RandomNum.ToString() + "¹ø";
		}

	#endregion
}
