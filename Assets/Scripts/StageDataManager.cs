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
		int maxRound = 6;
		public int MaxRound
		{
			get { return maxRound; }
		}

		//These should be initialized After Ending Game
		/// <summary>
		/// CityTileName: Building CardNum (nullable Int)
		/// </summary>
		public Dictionary<string, int?> cityBoard;   //CityTile: BuildingCard
		public List<string> pickedAreaCards;

	#endregion


	#region Step Data

		int currentStep;
		public int CurrentStep
		{
			get { return currentStep; }
			set { currentStep = value; }
		}

		int maxStep = 4;
		public int MaxStep
		{
			get { return maxStep; }
		}

		//These should be initialized After Every Round
		public int diceNum;
		public List<string> areaCards;
		public List<string> buildingCards;

	#endregion


	private void Awake()
	{
		currentRound = 1;
		currentStep = 1;

		areaCards = new List<string>();
		cityBoard = new Dictionary<string, int?>();
	}
}
