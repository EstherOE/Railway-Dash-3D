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
	[Header("City ")]
	public GameObject[] smallBuildings;
	public GameObject[]biigBuildings;
	[Header("Forest")]
	public GameObject []trees;
	[Header("Mountains")]
	public GameObject[] rocks;
	
	public GameObject[] mountains;
	[Header("Wagons")]
	public GameObject[]wagons;
	[Header("Horses")]
	public GameObject [] horses;
	
	[Header("Desert")]
	public GameObject [] catcus;
	public Transform decoeLeft;
	public Transform decoright;
	public int minDecorCount=4;
	public int maxDecorCount=10;
	public float decoOffset=0.5f;
	public Vector2 ScaleRange=new Vector2(0.8f,1.4f);
	public biomeType biome= biomeType.Forest;
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
		int decorCount=  Random.Range(minDecorCount,maxDecorCount);
		for (int i = 0; i < decorCount; i++) {
			GameObject prefab= ChooseRandomDeccor();
			if(prefab==null) continue;
			bool spawnLef= (Random.value>0.5f);
			float randomz= Random.Range(3,chunkLenght-6);
			Transform baseSpawn= spawnLef ? decoeLeft:decoright;
			float offset=Random.Range(2, 6);
			Vector3 pos= new Vector3(parent.position.x+offset,0, parent.position.z+randomz);
			
			GameObject obj= Instantiate(prefab, pos,Quaternion.identity,parent);
			float scale= Random.Range(ScaleRange.x,ScaleRange.y);
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
	void ClearChildren(Transform parent)
	{
		for (int i = 0; i < parent.childCount; i++) {
			
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
		
		switch(biome)
		{
		case biomeType.Forest:
			return ChooseFrom(trees);
		case biomeType.Desert:
			return ChooseFrom(catcus);
		case biomeType.City:
			return ChooseFrom(smallBuildings,biigBuildings);
		case biomeType.Wagons:
			return ChooseFrom(wagons);
		case biomeType.Horses:
			return ChooseFrom(horses);
		case biomeType.Mountains:
			return ChooseFrom(mountains,rocks);
			break;
		default:
			return null;
	
		}
		return null;
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
