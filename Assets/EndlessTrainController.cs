using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTrainController : MonoBehaviour
{
	#region Player Settings
	
	[Header("Player")]
	public Transform player;
	public float laneMoveSpeed=10f;
	[Header("Forward Movements")]
	public float forwardSpeed=15f;
	public float maxSpeed=50f;
	public float accelr=0.1f;
	
	#endregion
	
	#region Lane Settings
	[Header("Lane etting")]
	public float landleft=-2f;
	public float landMiddle=0;
	public float landRight=2f;
	
	private int currentlane=1;
	private float targetX=1;
	
	[Header("Chunk Systems")]
	public Transform chunk;
	public float chunkLenght=50f;
	public float triggerDistance=10f;
	
	[Header("Land Decorations")]
	public Transform landleftT;
	public Transform landRightT;
	public GameObject[] smallBuildings;
	public GameObject[]biigBuildings;
	public GameObject []trees;
	public GameObject[] rocks;
	
	public GameObject[] mountains;
	public GameObject[]wagons;	
	public GameObject [] horses;
	
	public GameObject [] catcus;
	[Header("DecorLocation")]
	public Transform startLeft;
	public Transform endLeft;
	public Transform startRight;
	public Transform endRight;
	public int minDecorCount=4;
	public int maxDecorCount=10;
	public float decoOffset=0.5f;
	public Vector2 ScaleRange=new Vector2(0.8f,1.4f);
	public float num=2f;
	public biomeType biome= biomeType.Forest;
	public float minSpacing=4f;
	
	public enum biomeType{
		Forest,
		Desert,
		Wagons,
		Horses,
		City,
		Mountains
	}
	
	public float forestScale=1.8f;
	public float desertScale=1.5f;
	public float wagon=2.5f;
	public float horsescale=3.0f;
	public float cityscale=1.7f;
	public float mountainScale=1f;
	private List<Vector3>placedPos= new List<Vector3>();
	#endregion
	
	#region Obstacles & PowerUps  Settings
	#endregion
	
	#region Camera Follow Settings
	#endregion
	
	#region Game States Settings
	#endregion
	
	#region Damage & Recovery Settings
	#endregion
    // Start is called before the first frame update
    void Start()
    {
	    targetX= landMiddle;
    }

    // Update is called once per frame
    void Update()
    {
	    SetupLanes();
	    MovePlayerToLane();
	    HandleMovement();
	    HandleChunkMovement();
	   
    }
    
    #region Player 
    
	void SetupPlayer()	{}
	void HandleMovement()	{
		if(forwardSpeed <maxSpeed)
		{
			
			forwardSpeed +=accelr*Time.deltaTime;
			
			// Move train Forward
			player.Translate(Vector3.forward*forwardSpeed*Time.deltaTime);
		}
	}
	void MovePlayerToLane(){
		
		Vector3 pos= player.position;
		pos.x= Mathf.Lerp(pos.x, targetX,Time.deltaTime*laneMoveSpeed);
		player.position=pos;
	}
	void SetTargetLaneX(){
		if(currentlane ==0) targetX=landleft;
		if(currentlane ==1) targetX=landMiddle;
		if(currentlane==2) targetX=landRight;
	}
	void SetupLanes(){
		if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			
			if(currentlane >0)
			{
				currentlane--;
				SetTargetLaneX();
			}
		}
		if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			
			if(currentlane <2)
			{
				currentlane++;
				SetTargetLaneX();
			}
		}
		
	}
	#endregion
	
	#region Track & Rail System
	
	void SetupTracks(){}
	void UpdateRailObjects(){}
    #endregion
	
	#region World System
	void GenerateIntilChunk(){}
	void GenerateNextChunk(){}
	void ClearChunk(){}
	void MoveChunkForward(){
		chunk.position +=new Vector3(0,0,chunkLenght);
		biome=(biomeType)(Random.Range(0,5));
		SpawnLeft();
		Spawnright();
	}
	#endregion
	
	#region land Deoration
	void SpawnLeft(){
		ClearChildren(landleftT);
		SpawnDecorations(landleftT,-1);
	}
	void Spawnright(){
		ClearChildren(landRightT);
		SpawnDecorations(landRightT,1);
	}
	void ClearLandDecor(){}
	void HandleChunkMovement(){
		if(player.position.z >chunk.position.z+chunkLenght-triggerDistance){
			MoveChunkForward();
		}
		
	}
	void SpawnDecorations(Transform parent ,int side){
		int decorCount=  Random.Range(minDecorCount,maxDecorCount+1);

		Transform startT= (side==-1)? startLeft: startRight;
		Transform endT=(side==-1)? endLeft: endRight;
		for (int i = 0; i < decorCount; i++) {
			GameObject prefab= ChooseRandomDeccor();
			if(prefab==null) continue;
			Vector3  hosePos= Vector3.zero;
			bool foundValid=false;
			for(int atte=0 ;atte<10; atte++)
			{
			float randomZ= Random.Range(startT.position.z, endT.position.z);
			float randomx= Random.Range(startT.position.x, endT.position.x);
				Vector3 testPos= new Vector3(randomx,0, randomZ);
				if(IsPostion(testPos))
				{
					hosePos= testPos;
					foundValid= true;
					break;
				}
				else{
					hosePos= testPos;
				}
		
			}
			if(!foundValid )
			
				continue;
			
			placedPos.Add(hosePos);
			float scale= Random.Range(ScaleRange.x,ScaleRange.y);
			GameObject obj= Instantiate(prefab,hosePos,Quaternion.identity,parent);
			switch(biome)
			{
			case biomeType.Forest:
				scale *= forestScale;
				break;
			case biomeType.City:
				scale *=cityscale;
				break;
			case biomeType.Desert:
				scale *=desertScale;
				break;
			case biomeType.Horses:
				scale *=horsescale;
				break;
			case biomeType.Wagons:
				scale *=wagon;
				break;
			case biomeType.Mountains:
				scale*= mountainScale;
				break;
			}
			
			obj.transform.localScale= new Vector3(scale,scale,scale);
			//	obj.transform.eulerAngles= new Vector3(0,Random.Range(0,360),0);
	
		}
	}
	bool IsPostion(Vector3 pos)
	{
		foreach (var p in placedPos)
	{
			if (Vector3.Distance(p, pos) < minSpacing)
			return false;
	}
		return true;
	
	
  }
	void ClearChildren(Transform parent)
	{
		for (int i = parent.childCount-1 ;i >=0; i--) {
			
			Destroy(parent.GetChild(i).gameObject);
		}
		
	}
		GameObject ChooseFrom(params GameObject[][] groups)
		{
			List<GameObject[]> validGroups = new List<GameObject[]>();
			foreach (var g in groups)
				if (g != null && g.Length > 0)
					validGroups.Add(g);

			if (validGroups.Count == 0)
				return null;

			GameObject[] group = validGroups[Random.Range(0, validGroups.Count)];

			return group[Random.Range(0, group.Length)];
		}
	GameObject ChooseRandomDeccor(){
		GameObject prefab=null;
		switch(biome)
		{
		case biomeType.Forest:
			prefab= ChooseFrom(trees, catcus,smallBuildings);
			break;
		case biomeType.Desert:
			prefab =ChooseFrom(catcus, smallBuildings);
			break;
		case biomeType.City:
			prefab=ChooseFrom(biigBuildings, trees, catcus);
			break;
		case biomeType.Wagons:
			prefab= ChooseFrom(wagons,trees);
			break;
		case biomeType.Horses:
			prefab= ChooseFrom(horses, wagons);
			break;
		case biomeType.Mountains:
			prefab= ChooseFrom(mountains,rocks);
			break;
			
		
		default:
			return null;
	
		}
		
		if(prefab==null)
		{
			prefab= ChooseFrom(
				trees,
				catcus,
				smallBuildings,
				biigBuildings,
				wagons,
				mountains,
				rocks
			);
		}
	return prefab;
		}
	#endregion
	#region Obstacles 
	void SpawnNextObstacles(){}
	void RemoveObstacles(){}
	#endregion
 
    #region Coin S
	void SpawnCoins(){}
	void CollectCoin(){}
	void UpdateCoinUI(){}
	void ClearCoins(){}
	#endregion
	
    #region PowerUp Systems
	void SpawnPowerups(){}
	void ApplyPowerup(string type){}
	void ClearPowerup(){}
	#endregion
		
	#region Camera Follow
	void HandleCamera(){}
	#endregion
	
	#region Game States
	void StartGame(){}
	void GameOver(){}
	void RestartGame(){}
	void PausedGame(){}
	#endregion
	#region Damage and Recovery
	void TakeDamage(){}
	void FallfomTrain(){}
	void RecoverClimb(){}
	void GameOverCheck(){}
	#endregion
	
	#region Zone System
	void DectectZoneChange(){}
	void ApplyZoneEffects(){}
	#endregion
	
	#region Environment Settings
	void UpdateWeather(){}
	void ApplyWeatherEffects(){}
	#endregion
	
	#region Abilites Settings
	void ActiveAbility(string abilityName){}
	void DeActiveAbility(string abilityName){}
	void UnlockAbility(string abilityName){}
   #endregion
   
    #region Boss Settings
	void  UpdateBossBeahaviour(){}
	void SpawnBossTrain(){} 
	#endregion
	
	#region Combo System
	void UpdateCombo(){}
	void ResetCombo(){}
	#endregion
	
	#region Secret Room System
	void OpenSecretDoor(){}
	void EnterSecretRoom(){}
	#endregion
	
	#region Enemy System
	void SpawnEnemy(){}
	void RemoveEnemies(){}
	#endregion
	
	#region Item Box System
	void SpawnItemBox(){}
	void ClearItemBoxes(){}
	void ApplyItemBoxReward(string rewardType){}
	#endregion
	
	#region Customization Settings
	void ApplyPlayerSkin(){}
	void ApplyTrailEffect(){}
	#endregion
	
	#region DailyMission Settings
	void GenerateDailyMissions(){}
	void TrackDailyMissions(){}
	void CompleteDailyMissions(){}
	#endregion
	
	#region SpeedZones Settings
	void UpdateSpeedZone(){}
	void ApplySpeedZone(){}
	#endregion
}
