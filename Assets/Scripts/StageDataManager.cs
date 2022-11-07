using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDataManager : MonoBehaviour
{
	#region Round Data

		int currentRound;
		public int CurrentRound
		{
			get { return currentRound; }
			set { currentRound = value; }
		}
		int maxRound;
		public int MaxRound
		{
			get { return maxRound; }
		}

		//These should be initialized After Ending Game
		Dictionary<int, int> cityBoard;
		public List<string> pickedAreaCards;

	#endregion


	#region Step Data

		int currentStep;
		public int CurrentStep
		{
			get { return currentStep; }
			set { currentStep = value; }
		}
		int maxStep;
		public int MaxStep
		{
			get { return maxStep; }
		}

		//These should be initialized After Every Round
		public int diceNum;
		public List<string> areaCards;
		List<int> buildingCards;

	#endregion


	private void Awake()
	{
		maxRound = 6;
		maxStep = 4;

		currentRound = 1;
		currentStep = 1;

		areaCards = new List<string>();
	}
}
