using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
	#region Initialization

	[SerializeField]
	StageDataManager StageData;

	bool[] IsStepFinished;

	[SerializeField]
	List<GameObject> StepMaterials;


	private void Awake()
	{
		IsStepFinished = new bool[StageData.MaxStep];
	}
	private void Start()
	{
		for (int i = 0; i < StageData.MaxStep; i++)
		{
			IsStepFinished[i] = false;
			StepMaterials[i].SetActive(false);
		}

		StepMaterials[0].SetActive(true);
	}

	#endregion


	//TODO: 테스트용으로 step 2에 넣어둠 -> 추후 step 3으로 이동
	private void Update()
	{
		if(StageData.CurrentRound <= StageData.MaxRound)
		{
			switch (StageData.CurrentStep)
			{
				case 1:
					RollTheDice();

					break;
				case 2:
					Step2();

					break;
				case 3:
					HandOutAreaCards();
					ChooseAreaCards();

					break;
				case 4:

					break;
			}
		}
	}



	Vector2 origin, dir;

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
				/*Game Logic*/
				StageData.diceNum = Random.Range(1, 7);

				/*Game Update*/
				Debug.Log("Dice Rolled: " + StageData.diceNum);

				//End the Step
				ProcessFinishStep(0);
			}
		}
	}


	[Header("Step2 Materials")]
	[SerializeField]
	GameObject PassStep;

	void Step2()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		origin = ray.origin;
		dir = ray.direction;

		RaycastHit2D hit = Physics2D.Raycast(origin, dir);

		if (Input.GetMouseButtonDown(0))
		{
			if (hit.collider != null && hit.collider.name == PassStep.name)
				ProcessFinishStep(1);
		}
	}


	[Header("Step3 Materials")]
	[SerializeField]
	GameObject AreaCardPrefab;

	Vector2[] AreaCardPoses = new Vector2[4] { new Vector2(-4.0f, 3.0f), new Vector2(4.0f, 3.0f),
																			new Vector2(-4.0f, -5.0f), new Vector2(4.0f, -5.0f)};
	string[] AreaCardColor = new string[6] { "A", "A", "B", "B", "C", "C"};

	bool IsCardHanded = false;

	Dictionary<string, int> AreaCardAndClick = new Dictionary<string, int>();
	void HandOutAreaCards()
	{
		if(!IsCardHanded)
		{
			//Debug.Log("Hand Out Area Cards");

			for (int i = 0; i < 4; i++)
			{
				GameObject AreaCard = Instantiate(AreaCardPrefab, AreaCardPoses[i], Quaternion.identity, StepMaterials[2].transform)
														as GameObject;

				// ※Prefab -> Front: Active, Back: Inactive
				TextMeshPro FrontCardTMP = AreaCard.transform.Find("Front").GetComponentInChildren<TextMeshPro>();
				TextMeshPro BackCardTMP = AreaCard.transform.Find("Back").GetComponentInChildren<TextMeshPro>();

				string AreaCardNumber;

				//TODO: 현재 최악 반복: 1+15+14+13 = 43 vs 리스트에서 하나씩 뽑기 -> 비교 후 결정
				do
				{
					AreaCardNumber = AreaCardColor[StageData.CurrentRound] + Random.Range(1, 16);
					//Debug.Log(i + AreaCardNumber);
				} while (StageData.areaCards.Contains(AreaCardNumber));

				FrontCardTMP.text = AreaCardNumber;
				AreaCard.name = AreaCardNumber;

				//TODO: BackCardText -> GameDataManager.AreaCard Description
				BackCardTMP.text = AreaCardNumber + "Description";

				//에리어카드 종류/클릭 횟수를 저장하는 딕셔너리에 추가
				AreaCardAndClick.Add(AreaCardNumber, 0);

				//게임 스테이지 데이터를 저장하는 리스트에 추가
				StageData.areaCards.Add(AreaCardNumber);
			}

			IsCardHanded = true;
		}
	}

	bool isHoldingStamp = false;

	[SerializeField]
	Transform Stamp;
	[SerializeField]
	Transform InitialStampPos;

	void ChooseAreaCards()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		origin = ray.origin;
		dir = ray.direction;

		RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir);

		if (Input.GetMouseButtonDown(0))
		{
			for(int i = 0; i < hits.Length; i++)
			{
				//Debug.Log(i + " " + hits[i].collider);

				if (hits[i].collider != null)
				{
					if (hits[i].collider.tag == "AreaCard")
					{
						if(!isHoldingStamp)
						{
							/* Flip Cards By Clicking */
							//Debug.Log(hits[i].collider.name);
							AreaCardAndClick[hits[i].transform.name]++;


							if (AreaCardAndClick[hits[i].transform.name] % 2 == 1)
							{
								hits[i].transform.Find("Front").gameObject.SetActive(false);
								hits[i].transform.Find("Back").gameObject.SetActive(true);
							}
							else
							{
								hits[i].transform.Find("Back").gameObject.SetActive(false);
								hits[i].transform.Find("Front").gameObject.SetActive(true);
							}
						}
						/*
						else
						{
							// When the Card is Selected By Stamp 
							Debug.Log(hits[i].collider.name + "Picked");

							ProcessFinishStep(2);
						}
						*/
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
							}
						}
					}
				}
			}
		}

		if(isHoldingStamp)
		{
			/* Make Player Hold the Stamp */
			Stamp.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
		}
	}


	[Header("Step4 Materials")]
	GameObject CityBoard;



	/// <summary>
	/// 한 스텝이 끝날 때마다 호출하는 함수. 해당 스텝이 끝난지 체크하는 bool을 true로 변경, 다음 스텝으로 창을 변경
	/// </summary>
	/// <param name="FinishStep"></param>
	void ProcessFinishStep(int FinishStep)
	{
		IsStepFinished[FinishStep] = true;
		StepMaterials[FinishStep].SetActive(false);

		if (FinishStep < StageData.MaxStep - 1)
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
		for (int i = 0; i < StageData.MaxStep; i++)
		{
			IsStepFinished[i] = false;
		}

		StageData.CurrentRound++;
	}
}