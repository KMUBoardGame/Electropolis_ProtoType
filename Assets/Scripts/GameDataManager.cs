using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
	#region MaxStep/MaxRound

	int maxRound = 6;
	public int MaxRound
	{
		get { return maxRound; }
	}


	int maxStep = 4;
	public int MaxStep
	{
		get { return maxStep; }
	}

	#endregion


	#region
	//���ӿ��� �Ｎ���� �����ؾ��ϴ� ��������Ʈ �̹����� ����
	[SerializeField] List<Sprite> AreaSpriteA;
	[SerializeField] List<Sprite> AreaSpriteB;
	[SerializeField] List<Sprite> AreaSpriteC;

	//TODO: Only Getter
	public List<List<Sprite>> AreaCardSprites = new List<List<Sprite>>();

	public Dictionary<string, List<int>> AreaCardData = new Dictionary<string, List<int>>
	{
		{ "A", new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 } },
		{ "B", new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 } },
		{ "C", new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 } }
	};
	#endregion


	private void Awake()
	{
		//�ʿ� �� GameDataMager ������ �ʱ�ȭ
		AreaCardSprites.Add(AreaSpriteA);
		AreaCardSprites.Add(AreaSpriteB);
		AreaCardSprites.Add(AreaSpriteC);
	}
}
