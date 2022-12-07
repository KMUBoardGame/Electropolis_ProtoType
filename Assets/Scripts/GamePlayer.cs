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
					Step2();

					break;
				case 4:
					HandOutCityBuildingCards();
					PutCityBuildingCards();
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
	[SerializeField] GameObject BuildingCardPrefab;
	[SerializeField] BuildingCardStack BuildingCardStack;


	void Step2()
	{
		ChooseBuildingCards();
	}


	void ChooseBuildingCards()
	{
		if (GetClickObjWithTag(BuildingCardPrefab.transform.tag) != null)
		{
			Transform ChosenBuildingCard = GetClickObjWithTag(BuildingCardPrefab.transform.tag);

			StageData.buildingCards.Add(ChosenBuildingCard.name);
			Destroy(ChosenBuildingCard.gameObject);

			if (StageData.buildingCards.Count >= StageData.diceNum)
			{
				ProcessFinishStep(1);

				BuildingCardStack.RetakeBuildingCards();
				BuildingCardStack.MixBuildingCards();
			}
		}
	}

	#endregion

	#region Step 3


	[Header("Step3 Materials")]

	[SerializeField] AreaCardStack AreaCardStack;

	/*

	bool IsAreaCardHanded = false;
	void HandOutAreaCards()
	{
		switch (StageData.CurrentRound)
		{
			case 1:
			case 2:
				AreaCardTypeIndex = 0;
				break;
			case 3:
			case 4:
				AreaCardTypeIndex = 1;
				break;
			case 5:
			case 6:
				AreaCardTypeIndex = 2;
				break;
		}
	}*/


	[SerializeField] Transform Stamp;

	[Tooltip("Position that you want to put Stamp back")]
	[SerializeField] Transform InitialStampPos;

	bool isHoldingStamp = false;
	/*
	void ChooseAreaCards()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		origin = ray.origin;
		dir = ray.direction;

		RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir);

		if (Input.GetMouseButtonDown(0))
		{
			for (int i = 0; i < hits.Length; i++)
			{
				//Debug.Log(i + " " + hits[i].collider);

				if (hits[i].collider != null)
				{
					if (hits[i].collider.tag == "AreaCard")
					{
						if (!isHoldingStamp)
						{
							// Flip Cards By Clicking
							if (hits[i].collider.transform.name == "Front")
							{
								hits[i].transform.gameObject.SetActive(false);
								hits[i].transform.parent.Find("Back").gameObject.SetActive(true);
							}
							else if (hits[i].collider.transform.name == "Back")
							{
								hits[i].transform.gameObject.SetActive(false);
								hits[i].transform.parent.Find("Front").gameObject.SetActive(true);
							}
						}
					}
					if (hits[i].collider.name == "Stamp")
					{
						// Check If Player is Holding Stamp / If we have to get Stamp back
						if (!isHoldingStamp)
							isHoldingStamp = true;
						else
						{
							if (hits.Length < 2)
							{
								Stamp.position = InitialStampPos.position;
								isHoldingStamp = false;
							}
							else
							{
								//When the Card is Selected By Stamp -> Under the stamp => hits[1] 
								Debug.Log(hits[1].transform.parent.name + " Picked");
								StageData.pickedAreaCards.Add(hits[1].transform.parent.name);

								ProcessFinishStep(2);

								for (int k = 0; k < AreaCardHouse.childCount; k++)
								{
									Destroy(AreaCardHouse.GetChild(k).gameObject);
								}

								Stamp.position = InitialStampPos.position;
								isHoldingStamp = false;
							}
						}
					}
				}
			}
		}

		if (isHoldingStamp)
		{
			//Make Player Hold the Stamp
			Stamp.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
		}
	}
		*/
	#endregion

	#region Step 4

		[Header("Step4 Materials")]
		[SerializeField]
		GameObject CityBuildingCardPrefab;
		[SerializeField]
		Transform CityBuildingCardHouse;

		bool IsCityboardReady = false;

		float CityBuldingCardSpace = 2.6f;
		float TopStartPoint = 6.3f;

		void HandOutCityBuildingCards()
		{
			if(!IsCityboardReady)
			{
				int BuildingCount = StageData.buildingCards.Count;
				int index = 0;


				for(float y = TopStartPoint; y > TopStartPoint - CityBuldingCardSpace * BuildingCount; y -= CityBuldingCardSpace)
				{
					//Debug.Log(index);

					GameObject CityBuildingCard = Instantiate(CityBuildingCardPrefab, new Vector3(-12.0f, y, 0.0f), Quaternion.identity, CityBuildingCardHouse) as GameObject;

					string BuildingCardNum = StageData.buildingCards[index];
					CityBuildingCard.name = "CityBuildingCard" + BuildingCardNum;
					CityBuildingCard.GetComponentInChildren<TextMeshPro>().text = BuildingCardNum;

					index++;
				}

				IsCityboardReady = true;
			}
		}


		bool IsHoldingCityBuildingCard;

		Transform HoldingCityBuildingCard;
		Vector3 HoldingCityBuildingCardInitialPos;

		void PutCityBuildingCards()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			origin = ray.origin;
			dir = ray.direction;

			RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir);

			if (Input.GetMouseButtonDown(0))
			{
				for (int i = 0; i < hits.Length; i++)
				{

					if (hits[i].collider != null)
					{
						//Debug.Log(hits[i].collider.name + " " + IsHoldingCityBuildingCard);

						if (!IsHoldingCityBuildingCard)
						{
							if (hits[i].collider.tag == "CityBuildingCard")
							{
								HoldingCityBuildingCard = hits[i].collider.transform;
								HoldingCityBuildingCardInitialPos = HoldingCityBuildingCard.position;

								IsHoldingCityBuildingCard = true;
							}
						}
						else
						{
							if (hits[i].collider.tag == "CityTile")
							{
								Debug.Log("Attach " + hits[i].collider.transform.parent.name);

								//빌딩 타일에 붙이기
								IsHoldingCityBuildingCard = false;

								HoldingCityBuildingCard.transform.parent = hits[i].collider.transform.parent.transform;
								HoldingCityBuildingCard.position = hits[i].collider.transform.position;

								//TODO: Put Data in StageData:cityBoard
								//StageData.cityBoard[hits[i].collider.transform.parent.name] = int.Parse(HoldingCityBuildingCard.name);
								HoldingCityBuildingCard = null;

								if (CityBuildingCardHouse.transform.childCount <= 0)
								{
									ProcessFinishStep(3);
									ProcessFinishRound();
								}

							}
							else if(hits.Length == 1)
							{
								IsHoldingCityBuildingCard = false;

								HoldingCityBuildingCard.position = HoldingCityBuildingCardInitialPos;
								HoldingCityBuildingCard = null;
							}
						}
					}
				}
			}

			if (IsHoldingCityBuildingCard)
			{
				if(HoldingCityBuildingCard.parent == CityBuildingCardHouse)
					HoldingCityBuildingCard.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
			}
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
					ProcessFinishStep(i);
					ProcessFinishRound();
				}
			}
		}
	}
}
