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
		for (int i = 0; i < GameData.MaxStep; i++)
		{
			IsStepFinished[i] = false;
			StepMaterials[i].SetActive(false);
		}

		StepMaterials[0].SetActive(true);
		GameFinisher.SetActive(false);
	}

	#endregion


	private void Update()
	{
		if(StageData.CurrentRound <= GameData.MaxRound)
		{
			switch (StageData.CurrentStep)
			{
				case 1:
					RollTheDice();

					break;
				case 2:
					HandOutBuildingCards();
					ChooseBuildingCards();

					break;
				case 3:
					HandOutAreaCards();
					ChooseAreaCards();

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

	#region Step 1

	[Header("Step1 Materials")]
	[SerializeField]
	GameObject Dice;

	void RollTheDice()
	{
		//Debug.Log("Roll the Dice");
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		origin = ray.origin;
		dir = ray.direction;

		RaycastHit2D hit = Physics2D.Raycast(origin, dir);

		/*User Behavior (If User Click the Dice)*/
		if (Input.GetMouseButtonDown(0))
		{
			if (hit.collider != null && hit.transform.name == Dice.name)
			{
				Debug.Log(StageData.CurrentRound + "/" + GameData.MaxRound);

				/*Game Logic*/
				StageData.diceNum = Random.Range(1, 7);

				/*Game Update*/
				Debug.Log("Dice Rolled: " + StageData.diceNum);

				//End the Step
				ProcessFinishStep(0);
			}
		}
	}

	#endregion

	#region Step 2


	[Header("Step2 Materials")]
	[SerializeField]
	GameObject BuildingCardPrefab;
	[SerializeField]
	Transform BuildingCardHouse;

	bool IsBuildingCardHanded = false;

	void HandOutBuildingCards()
	{
		if (!IsBuildingCardHanded)
		{
			for (int x = -15; x <= 15; x += 3)
			{
				for (int y = -6; y <= 6; y += 3)
				{
					//spawn building cards | 16types
					int CardNum = Random.Range(1, 17);
					GameObject BuildingCard = Instantiate(BuildingCardPrefab, new Vector3(x, y, 0), Quaternion.identity, BuildingCardHouse.transform) as GameObject;
					BuildingCard.GetComponentInChildren<TextMeshPro>().text = CardNum.ToString();
					BuildingCard.name = CardNum.ToString();
				}
			}

			//타일 세부 설정, 랜덤 발급

			IsBuildingCardHanded = true;
		}
	}

	void ChooseBuildingCards()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		origin = ray.origin;
		dir = ray.direction;

		RaycastHit2D hit = Physics2D.Raycast(origin, dir);

		if (Input.GetMouseButtonDown(0))
		{
			if (hit.collider != null)
			{
				if (hit.collider.tag == "BuildingCard")
				{
					StageData.buildingCards.Add(hit.collider.name);
					Destroy(hit.collider.gameObject);
					//Debug.Log(hit.collider.name);

					if (StageData.buildingCards.Count >= StageData.diceNum)
					{
						ProcessFinishStep(1);

						//CleanUp
						for (int i = 0; i < BuildingCardHouse.childCount; i++)
						{
							Destroy(BuildingCardHouse.GetChild(i).gameObject);
						}
					}
				}
			}
		}
	}

	#endregion

	#region Step 3


	[Header("Step3 Materials")]

	[SerializeField] GameObject AreaCardPrefab;
	[SerializeField] Transform AreaCardHouse;

	Vector2[] AreaCardPoses = new Vector2[4] { new Vector2(-4.0f, 3.0f), new Vector2(4.0f, 3.0f),
																		    new Vector2(-4.0f, -5.0f), new Vector2(4.0f, -5.0f)};

	int AreaCardTypeIndex;  //2라운드마다 타입이 바뀜. ex) 1,2라운드 -> 0(A), 3,4라운드 -> 1(B), 5,6라운드 -> 2(C)
	List<string> AreaCardTypePer2Round = new List<string> { "A", "B", "C" };

	bool IsAreaCardHanded = false;
	void HandOutAreaCards()
	{
		if (!IsAreaCardHanded)
		{
			//Debug.Log("Hand Out Area Cards");

			//TODO: 좀 더 효율적인 방법이 있을 것
			switch (StageData.CurrentRound)
			{
				case 1:	case 2:
					AreaCardTypeIndex = 0;
					break;
				case 3: case 4:
					AreaCardTypeIndex = 1;
					break;
				case 5: case 6:
					AreaCardTypeIndex = 2;
					break;
			}
			
			for (int i = 0; i < 4; i++)
			{
				GameObject AreaCard = Instantiate(AreaCardPrefab, AreaCardPoses[i], Quaternion.identity, AreaCardHouse) as GameObject;
				
				string CurrentAreaCardType = AreaCardTypePer2Round[AreaCardTypeIndex];
				List<int> LeftAreaCards = GameData.AreaCardData[CurrentAreaCardType];   //참조 복사이기 때문에 원본에도 영향 줌


				int RandomNum = Random.Range(0, LeftAreaCards.Count);

				int AreaCardNumber = LeftAreaCards[RandomNum];
				string AreaCardName = CurrentAreaCardType + AreaCardNumber;
				AreaCard.name = AreaCardName;


				Transform FrontSide = AreaCard.transform.Find("Front");
				Transform BackSide = AreaCard.transform.Find("Back");

				List<Sprite> CurrentAreaCardTypeSpriteList = GameData.AreaCardSprites[AreaCardTypeIndex];
				FrontSide.GetComponent<SpriteRenderer>().sprite = CurrentAreaCardTypeSpriteList[AreaCardNumber % CurrentAreaCardTypeSpriteList.Count];
				//Debug.Log(AreaCardNumber + " % " + CurrentAreaCardTypeSpriteList.Count + " = " +  AreaCardNumber % CurrentAreaCardTypeSpriteList.Count);

				//FrontCard.text = 카드 보상/조건;
				//BackCard.text = 카드 추가 설명


				//게임 스테이지 데이터를 저장하는 리스트에 추가
				StageData.areaCards.Add(AreaCardName);

				//놓여진 에리어카드를 리스트에서 제거
				LeftAreaCards.Remove(RandomNum);
			}

			IsAreaCardHanded = true;
		}
	}


	[SerializeField] Transform Stamp;

	[Tooltip("Position that you want to put Stamp back")]
	[SerializeField] Transform InitialStampPos;

	bool isHoldingStamp = false;
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
							/* Flip Cards By Clicking */
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
						/* Check If Player is Holding Stamp / If we have to get Stamp back */
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
								/* When the Card is Selected By Stamp -> Under the stamp => hits[1] */
								Debug.Log(hits[1].collider.name + " Picked");
								StageData.pickedAreaCards.Add(hits[1].collider.name);

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
			/* Make Player Hold the Stamp */
			Stamp.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
		}
	}

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


		IsBuildingCardHanded = false;
		IsAreaCardHanded = false;
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
