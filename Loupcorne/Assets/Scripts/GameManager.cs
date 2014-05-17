using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Transform playerSpawn;
    //[SerializeField] private Transform guardSpawnsParent;
    private List<SpawnPoint> guardSpawns;
    [SerializeField] private int maxGuards; 
    [SerializeField] private int maxPeons;
    [SerializeField] UnityEngine.Object guardPrefab;
    [SerializeField] UnityEngine.Object peonPrefab;
    [SerializeField] Transform guardsParent;
    [SerializeField] Transform peonsParent;
    [SerializeField] private int arenaSize;

    private UnitsManager _um;

    private UIObjectivePanel uiTimer;

    private UnityEngine.Object guardSpawnsPrefab;

    public delegate void GameReadyEventHandler(GameManager sender);
    public event GameReadyEventHandler OnGameReady;

	void Start () 
    {
        _um = UnitsManager.Instance;

        maxGuards *= GameStats.difficulty;
        GameStats.nbPeon += maxPeons;
        arenaSize *= GameStats.difficulty;

        CreateGuardSpawns();

        CreatePeons();

        CreateGuards();

        Transform player = _um.player.transform;
        player.position = playerSpawn.position;
        
        UnitsManager.AllGuardRemoved += ArenaCompleted;
        Player.PlayerIsDead += GameOver;

        this.ApplyDifficulty(GameStats.difficulty);

        Transform ui = GameObject.FindGameObjectWithTag("GUI").transform;
		uiTimer = ui.FindChild("ObjectivePanel").GetComponent<UIObjectivePanel>();
        switch (GameStats.difficulty)
        {
            case 1:
                GameStats.timer = 180;
                break;
            case 2:
                GameStats.timer = 180;
                break;
            case 3:
                GameStats.timer = 180;
                break;
        }

        if (this.OnGameReady != null)
        {
            this.OnGameReady(this);
        }
	}

    private void CreatePeons()
    {
        for (int i = 0; i < maxPeons; i++)
        {
            GameObject goPeon = Instantiate(peonPrefab) as GameObject;
            _um.AddPeon(goPeon.GetComponent<Peon>());
            float eulerY = UnityEngine.Random.Range(0f, 359f);
            goPeon.transform.eulerAngles = new Vector3(goPeon.transform.eulerAngles.x, eulerY, goPeon.transform.eulerAngles.z);
            NavMeshHit hit = new NavMeshHit();
            float posX;
            float posZ;
            do
            {
                posX = UnityEngine.Random.Range(-arenaSize / 2, arenaSize / 2);
                posZ = UnityEngine.Random.Range(-arenaSize / 2, arenaSize / 2);
            } while (!NavMesh.SamplePosition(new Vector3(posX, 0, posZ), out hit, 1, -1));
            goPeon.transform.position = new Vector3(posX, 1, posZ);

            goPeon.transform.parent = peonsParent;
        }
    }

    private void CreateGuardSpawns()
    {
        string spawnsHolderPath = @"Game/GuardSpawns_" + GameStats.difficulty.ToString();
        Transform spawnsHolder = (Instantiate(Resources.Load(spawnsHolderPath)) as GameObject).transform;

        guardSpawns = new List<SpawnPoint>();
        for (int i = 0; i < spawnsHolder.childCount; i++)
        {
            SpawnPoint sp = spawnsHolder.GetChild(i).GetComponent<SpawnPoint>();
            guardSpawns.Add(sp);
        }

        //Shuffle spawn points
        System.Random rng = new System.Random();
        int n = guardSpawns.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            SpawnPoint value = guardSpawns[k];
            guardSpawns[k] = guardSpawns[n];
            guardSpawns[n] = value;
        }  
    }

    private void CreateGuards()
    {
        for (int i = 0; i < maxGuards; i++)
        {
            //Debug.Log("create guards");
            GameObject goGuard = Instantiate(guardPrefab) as GameObject;
            _um.AddGuard(goGuard.GetComponent<Guard>());
            float eulerY = UnityEngine.Random.Range(0f, 359f);
            goGuard.transform.eulerAngles = new Vector3(goGuard.transform.eulerAngles.x, eulerY, goGuard.transform.eulerAngles.z);
			goGuard.transform.parent = guardsParent;
			Debug.Log("guard spawns = " + guardSpawns.Count);
            foreach (SpawnPoint sp in guardSpawns)
            {
                if (sp.AddGuard(goGuard))
				{
					Debug.Log("Added to spawn point: " + sp.transform.position);
					break;
				}
                    
            }
            
        }
    }

    private void ApplyDifficulty(int difficulty)
    {
        LoupCorne.Framework.IDatatable<LoupCorne.Framework.SimDescriptor> descriptorDatatable
            = LoupCorne.Framework.Database.Instance.GetDatatable<LoupCorne.Framework.SimDescriptor>();

        LoupCorne.Framework.SimDescriptor difficultyDescriptor = descriptorDatatable.GetElement(string.Format("Difficulty{0}", difficulty));

        List<Entity> entities = new List<Entity>();
        entities.AddRange(this._um.peons.ToArray());
        entities.AddRange(this._um.guards.ToArray());
        
        entities.ForEach(e => e.DifficultyDescriptor = difficultyDescriptor);
        this._um.player.AddDescriptor(difficultyDescriptor);
        this._um.player.Refresh();
    }
	
	void Update () 
    {
        if(GameStats.timer > 0)
        {
            GameStats.timer -= Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(GameStats.timer);
            string timeValue = String.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
            uiTimer.SetTimeValue(timeValue);
        }
        else
            GameOver();

        if (Input.GetKeyDown(KeyCode.F1))
        {
            Guard[] guards = UnitsManager.Instance.guards.ToArray();
            foreach (Guard g in guards)
            {
                g.Kill();
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            Peon[] peons = UnitsManager.Instance.peons.ToArray();
            foreach (Peon g in peons)
            {
                g.Kill();
            }
        }
	}

    private void ArenaCompleted()
    {
        if (GameStats.difficulty < 3)
        {
            Debug.Log("Arena " + GameStats.difficulty + " Completed !");
            GameStats.score += GameStats.nbPeon - GameStats.nbPeonKilled;
            GameStats.difficulty++;
            Application.LoadLevel("3C");
        }
        else
        {
            GameStats.score += GameStats.nbPeon - GameStats.nbPeonKilled;
            Debug.Log("Game Completed !");
            Debug.Log("Score = " + GameStats.score);
            if (GameStats.score >= GameStats.nbPeon / 2)
            {
                Debug.Log("Alignement = Good !");
                Application.LoadLevel("EndingGood");
            }
            else
            {
                Debug.Log("Alignement = Evil");
                Application.LoadLevel("EndingBad");
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER");
       StartCoroutine("WaitBeforeGameOver");
    }

    private IEnumerator WaitBeforeGameOver()
    {
        yield return new WaitForSeconds(4);
        Application.LoadLevel("GameOver");
    }

    void OnDisable()
    {
        UnitsManager.AllGuardRemoved -= ArenaCompleted;
        Player.PlayerIsDead -= GameOver;
    }
}
