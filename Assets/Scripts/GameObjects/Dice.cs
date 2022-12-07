using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
	[SerializeField] int minNum;
	[SerializeField] int maxNum;

	private void Awake()
	{
		minNum = 1;	
		maxNum = 6;
	}

	public int RollTheDice()
	{
		int randomValue = Random.Range(minNum, maxNum + 1);

		return randomValue;
	}
}
