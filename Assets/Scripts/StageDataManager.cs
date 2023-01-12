using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDataManager : MonoBehaviour
{

	#region Round Data_ No Initialization

		int currentRound;
		public int CurrentRound
		{
			get { return currentRound; }
			set { currentRound = value; }
		}

	#endregion

	#region Round Data_ Initialization Needed



	#endregion


	#region Step Data_ No Initialization

		int currentStep;
		public int CurrentStep
		{
			get { return currentStep; }
			set { currentStep = value; }
		}

		public Dictionary<string, int?> cityBoard;   //CityTile: BuildingCard
		public List<string> pickedAreaCards;

	#endregion

	#region Step Data_ Initialization Needed

		//These should be initialized After Every Round
		//매 라운드가 끝날 때마다 초기화되는 변수
		public int diceNum;
		//public List<GameObject> areaCards;
		public List<string> buildingCards;

	#endregion


	private void Awake()
	{
		currentRound = 1;
		currentStep = 1;

		//areaCards = new List<GameObject>();
		cityBoard = new Dictionary<string, int?>();
	}
}
