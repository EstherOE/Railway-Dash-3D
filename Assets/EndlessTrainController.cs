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
	public int minDecorCount=4;
	public int maxDecorCount=10;
	
	public Vector2 ScaleRange=new Vector2(0.8f,1.4f);
	
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
			float randomz= Random.Range(0,chunkLenght);
			
			float offset=side+Random.Range(0.5f,3f);
			Vector3 pos= new Vector3(parent.position.x+offset,0, parent.position.z+randomz);
			
			GameObject obj= Instantiate(prefab, pos,Quaternion.identity,parent);
			obj.transform.eulerAngles= new Vector3(0,Random.Range(0,360),0);
			float scale= Random.Range(ScaleRange.x,ScaleRange.y);
			obj.transform.localScale= new Vector3(scale,scale,scale);
		}
	}
	void ClearChildren(Transform parent)
	{
		for (int i = 0; i < parent.childCount; i++) {
			
			Destroy(parent.GetChild(i).gameObject);
		}
	}
	
	GameObject ChooseRandomDeccor(){
		
		int type= Random.Range(0,7);
		switch(type)
		{
		case 0:
			return smallBuildings.Length >0? smallBuildings[Random.Range(0,smallBuildings.Length)]:null;
		case 1:
			return biigBuildings.Length >0? biigBuildings[Random.Range(0,biigBuildings.Length)]:null;
		case 2:
			return trees.Length >0? trees[Random.Range(0,trees.Length)]:null;
		case 3:
			return rocks.Length >0? rocks[Random.Range(0,rocks.Length)]:null;
		case 4:
			return mountains.Length >0? mountains[Random.Range(0,mountains.Length)]:null;
		case 5:
			return wagons.Length >0? wagons[Random.Range(0,wagons.Length)]:null;
		case 6:
			return horses.Length >0?horses[Random.Range(0,horses.Length)]:null;
		
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
