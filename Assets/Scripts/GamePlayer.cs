using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
	#region Initialization

	[SerializeField]	GameDataManager GameData;
	[SerializeField]	StageDataManager StageData;
	[SerializeField]	GameObject GameFinisher;

	[Tooltip("Steps that are using in your game. After finishing current step, the next step will be appeared in the screen.")]
	List<GameObject> UsingStates;

	enum State
	{
		Dice, BuildingCard, AreaCard, CityBoard
	};


	//State currentState;
	List<State> currentStates;
	GameObject ActiveState;


	private void Awake()
	{
		UsingStates = new List<GameObject>();
		currentStates = new List<State>();
		GameFinisher.SetActive(false);
	}

	private void Start()
	{
		Initialize();

	}

	void Initialize()
	{
		InitializeStates();

		/*
		 * 
		 */
		IsCityBuildingCardsReady = false;
		/*
		 * 
		 */

		SetState(State.Dice);
	}

	void InitializeStates()
	{
		//사용할 States들을 추가
		UsingStates.Add(DiceState);
		UsingStates.Add(BuildingCardState);
		UsingStates.Add(AreaCardState);
		UsingStates.Add(CityboardState);

		for (int i = 0; i < UsingStates.Count; i++)
		{
			UsingStates[i].SetActive(false);
		}

	}

	#endregion


	/// <summary>
	/// 게임 종료 조건을 만족했는가
	/// </summary>
	bool isFinished()
	{
		//TODO: 시티보드가 다 찼을 때 -> 추가
		return (StageData.CurrentRound <= GameData.MaxRound);
	}

	private void Update()
	{
		if(isFinished())
		{
			for(int index = 0; index < currentStates.Count; index++)
			{
				switch (currentStates[index])
				{
					case State.Dice:
						RunDiceState();

						break;
					case State.BuildingCard:
						RunBuildingCardState();

						break;
					case State.AreaCard:
						RunAreaCardState();

						break;
					case State.CityBoard:
						RunCityboardState();
						PassStepForTest(3);

						break;
				}
			}
		}
		else
		{
			gameObject.SetActive(false);
			GameFinisher.SetActive(true);
		}
	}


	Vector2 origin, dir;
	bool CheckClickWithTag(string clickObjTag)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		origin = ray.origin;
		dir = ray.direction;

		RaycastHit2D hit = Physics2D.Raycast(origin, dir);


		if (Input.GetMouseButtonDown(0))
		{
			if (hit.collider != null && hit.transform.tag == clickObjTag)
			{
				return true;
			}
		}

		return false;
	}
	Transform GetClickObjWithTag(string clickObjTag)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		origin = ray.origin;
		dir = ray.direction;

		RaycastHit2D hit = Physics2D.Raycast(origin, dir);


		if (Input.GetMouseButtonDown(0))
		{
			if (hit.collider != null && hit.transform.tag == clickObjTag)
			{
				return hit.transform;
			}
		}

		return null;
	}


	#region DiceState

	[Header("DiceState Materials")]
	[SerializeField] GameObject DiceState;

	[SerializeField] Transform Dice;

	void RunDiceState()
	{
		if(CheckClickWithTag("Dice"))
		{
			StageData.diceNum = Dice.GetComponent<Dice>().RollTheDice();
			Debug.Log(StageData.diceNum);

		    UpdateDiceState();
		}
	}

	void UpdateDiceState()
	{
		currentStates.Clear();
		SetState(State.BuildingCard);
	}

	#endregion

	#region BuildingCardState


	[Header("BuildingCardState Materials")]
	[SerializeField] GameObject BuildingCardState;

	[SerializeField] BuildingCardStack BuildingCardStack;


	void RunBuildingCardState()
	{
		Transform ChosenBuildingCard = GetClickObjWithTag("BuildingCard");

		if (ChosenBuildingCard != null)
		{
			StageData.buildingCards.Add(ChosenBuildingCard.name);
			Destroy(ChosenBuildingCard.gameObject);

			if (StageData.buildingCards.Count >= StageData.diceNum)
			{
				UpdateBuildingCardState();
			}
		}
	}

	void UpdateBuildingCardState()
	{
		//다음 스텝으로 이동하기 전, 정리 작업
		BuildingCardStack.RetakeBuildingCards();
		BuildingCardStack.MixBuildingCards();

		//다음 스텝으로 이동
		currentStates.Clear();
		SetState(State.AreaCard);
	}


	#endregion

	#region AreaCardState

	[Header("AreaCardState Materials")]
	[SerializeField] GameObject AreaCardState;

	[SerializeField] AreaCardStack AreaCardStack;
	[SerializeField] Stamp Stamp;

	List<string> CardTypePerRound = new List<string> { "A", "A", "B", "B", "C", "C" };

	void RunAreaCardState()
	{
		if(Stamp.ClickedObj != null)
		{
			Transform ClickedAreaCard = Stamp.ClickedObj;
			StageData.pickedAreaCards.Add(ClickedAreaCard.name);

			UpdateAreaCardState();
		}
	}

	void UpdateAreaCardState()
	{
		AreaCardStack.RetakeAreaCards();
		if (StageData.CurrentRound < GameData.MaxRound)
			AreaCardStack.HandOutCardStack(CardTypePerRound[StageData.CurrentRound]);

		currentStates.Clear();
		SetState(State.CityBoard);
	}

	#endregion

	#region CityBuildingCardState

	[Header("CityboardState Materials")]
	[SerializeField] GameObject CityboardState;
	[SerializeField] CityBuildingCardStack CityBuildingCardStack;

	bool IsCityBuildingCardsReady;
	void RunCityboardState()
	{
		if (!IsCityBuildingCardsReady)
		{
			CityBuildingCardStack.HandOutCityBuildingCards();

			IsCityBuildingCardsReady = true;
		}

		if(CityBuildingCardStack.transform.childCount <= 0)
		{
			UpdateCityboardState();
		}
	}

	void UpdateCityboardState()
	{
		CityBuildingCardStack.RetakeCityBuildingCards();
		IsCityBuildingCardsReady = false;

		FinishRound();

		currentStates.Clear();
		SetState(State.Dice);
	}

	#endregion


	//SetState -> 현재 State 해제 후 매개변수 State On
	void SetState(State nextState)
	{
		if(ActiveState)	ActiveState.SetActive(false);

		switch(nextState)
		{
			case State.Dice:
				ActiveState = DiceState;
				break;
			case State.BuildingCard:
				ActiveState = BuildingCardState;
				break;
			case State.AreaCard:
				ActiveState = AreaCardState;
				break;
			case State.CityBoard:
				ActiveState = CityboardState;
				break;
		}

		currentStates.Add(nextState);
		ActiveState.SetActive(true);
	}


	/// <summary>
	/// 한 라운드가 끝날 때마다 호출하는 함수. 현재 라운드의 어떤 스텝이 끝났는지 체크하는 리스트 초기화, 현재 라운드+=1
	/// </summary>
	void FinishRound()
	{
		StageData.buildingCards.Clear();

		//StageData.areaCards.Clear();
		//IsCityBuildingCardsReady = false;

		StageData.CurrentRound++;
	}


	/// <summary>
	/// Attach StepPass Prefab in the step you want to pass by click
	/// </summary>
	void PassStepForTest(int i)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		origin = ray.origin;
		dir = ray.direction;

		RaycastHit2D hit = Physics2D.Raycast(origin, dir);

		if (Input.GetMouseButtonDown(0))
		{
			if (hit.collider != null && hit.collider.name == "StepPass")
			{
				UpdateCityboardState();
			}
		}
	}
}
