using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
	#region Initialization

	[SerializeField]
	GameDataManager GameData;

	[SerializeField]
	StageDataManager StageData;

	[SerializeField]
	GameObject GameFinisher;
	bool[] IsStepFinished;

	[Tooltip("Steps that are using in your game. After finishing current step, the next step will be appeared in the screen.")]
	[SerializeField]
	List<GameObject> StepMaterials;



	private void Awake()
	{
		IsStepFinished = new bool[GameData.MaxStep];
	}
	private void Start()
	{
		#region DONOTTOUCH
		for (int i = 0; i < GameData.MaxStep; i++)
		{
			IsStepFinished[i] = false;
			StepMaterials[i].SetActive(false);
		}

		StepMaterials[0].SetActive(true);
		GameFinisher.SetActive(false);
		#endregion

		IsCityboardReady = false;
	}

	#endregion


	private void Update()
	{
		if(StageData.CurrentRound <= GameData.MaxRound)
		{
			switch (StageData.CurrentStep)
			{
				case 1:
					Step1();

					break;
				case 2:
					Step2();

					break;
				case 3:
					Step3();

					break;
				case 4:
					Step4();
					PassStepForTest(3);

					break;
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


	#region Step 1

	[Header("Step1 Materials")]
	[SerializeField]
	Transform Dice;

	void Step1()
	{
		if(CheckClickWithTag(Dice.tag))
		{
			StageData.diceNum = Dice.GetComponent<Dice>().RollTheDice();

			ProcessFinishStep(0);
		}
	}

	#endregion

	#region Step 2


	[Header("Step2 Materials")]
	[SerializeField] BuildingCardStack BuildingCardStack;


	void Step2()
	{
		ChooseBuildingCards();
	}


	void ChooseBuildingCards()
	{
		Transform ChosenBuildingCard = GetClickObjWithTag("BuildingCard");

		if (ChosenBuildingCard != null)
		{
			StageData.buildingCards.Add(ChosenBuildingCard.name);
			Destroy(ChosenBuildingCard.gameObject);

			if (StageData.buildingCards.Count >= StageData.diceNum)
			{
				FinishAndResetStep2();
			}
		}
	}

	void FinishAndResetStep2()
	{
		//현재 스텝 끝내기
		ProcessFinishStep(1);

		//스텝 이후 업데이트 해야 할 내용: 뽑히지 않은 건물카드 삭제 후 새로운 건물카드들 재분배
		BuildingCardStack.RetakeBuildingCards();
		BuildingCardStack.MixBuildingCards();
	}

	#endregion

	#region Step 3

	[Header("Step3 Materials")]
	[SerializeField] AreaCardStack AreaCardStack;
	List<string> CardTypePerRound = new List<string> { "A", "A", "B", "B", "C", "C" };
	[SerializeField] Stamp Stamp;

	void Step3()
	{
		if(Stamp.ClickedObj != null)
		{
			Transform ClickedAreaCard = Stamp.ClickedObj;

			StageData.pickedAreaCards.Add(ClickedAreaCard.name);

			FinishAndResetStep3();
		}
	}

	void FinishAndResetStep3()
	{
		ProcessFinishStep(2);

		AreaCardStack.RetakeAreaCards();
		if (StageData.CurrentRound < GameData.MaxRound)
			AreaCardStack.HandOutCardStack(CardTypePerRound[StageData.CurrentRound]);
	}

	#endregion

	#region Step 4

		[Header("Step4 Materials")]
		[SerializeField]
		CityBuildingCardStack CityBuildingCardStack;

		bool IsCityboardReady;
		void Step4()
		{
			if (!IsCityboardReady)
			{
				CityBuildingCardStack.HandOutCityBuildingCards();

				IsCityboardReady = true;
			}

			if(CityBuildingCardStack.transform.childCount <= 0)
			{
				FinishAndResetStep4();
			}
		}

		void FinishAndResetStep4()
		{
			CityBuildingCardStack.RetakeCityBuildingCards();
			IsCityboardReady = false;

			ProcessFinishStep(3);

			ProcessFinishRound();
		}

	#endregion


	/// <summary>
	/// 한 스텝이 끝날 때마다 호출하는 함수. 해당 스텝이 끝난지 체크하는 bool을 true로 변경, 다음 스텝으로 창을 변경
	/// </summary>
	/// <param name="FinishStep"></param>
	void ProcessFinishStep(int FinishStep)
	{
		IsStepFinished[FinishStep] = true;
		StepMaterials[FinishStep].SetActive(false);

		if (FinishStep < GameData.MaxStep - 1)
		{
			StepMaterials[FinishStep + 1].SetActive(true);
			StageData.CurrentStep++;
		}
		else
		{
			//마지막 스텝일 경우 스텝 1의 창을 켜고 스텝 데이터를 1로 변경
			StepMaterials[0].SetActive(true);
			StageData.CurrentStep = 1;
		}
	}

	/// <summary>
	/// 한 라운드가 끝날 때마다 호출하는 함수. 현재 라운드의 어떤 스텝이 끝났는지 체크하는 리스트 초기화, 현재 라운드+=1
	/// </summary>
	void ProcessFinishRound()
	{
		for (int i = 0; i < GameData.MaxStep; i++)
		{
			IsStepFinished[i] = false;
		}

		StageData.buildingCards.Clear();

		StageData.areaCards.Clear();

		IsCityboardReady = false;

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
				if(i != GameData.MaxStep-1)
					ProcessFinishStep(i);
				else
				{
					FinishAndResetStep4();
				}
			}
		}
	}
}
