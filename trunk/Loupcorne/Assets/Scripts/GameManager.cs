using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private Transform guardSpawnsParent;
    private List<SpawnPoint> guardSpawns;
    [SerializeField] private int maxGuards; 
    [SerializeField] private int maxPeons;
    [SerializeField] Object guardPrefab;
    [SerializeField] Object peonPrefab;
    [SerializeField] Transform guardsParent;
    [SerializeField] Transform peonsParent;
    [SerializeField] private int arenaSize;

    private UnitsManager _um;

    public delegate void GameReadyEventHandler(GameManager sender);
    public event GameReadyEventHandler OnGameReady;

	void Start () 
    {
        _um = UnitsManager.Instance;

        maxGuards *= GameStats.difficulty;
        GameStats.nbPeon += maxPeons;

        CreateGuardSpawns();

        CreatePeons();

        CreateGuards();

        Transform player = _um.player.transform;
        player.position = playerSpawn.position;
        
        UnitsManager.AllGuardRemoved += ArenaCompleted;
        Player.PlayerIsDead += GameOver;

        this.ApplyDifficulty(GameStats.difficulty);

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
            float eulerY = Random.Range(0f, 359f);
            goPeon.transform.eulerAngles = new Vector3(goPeon.transform.eulerAngles.x, eulerY, goPeon.transform.eulerAngles.z);
            NavMeshHit hit = new NavMeshHit();
            float posX;
            float posZ;
            do
            {
                posX = Random.Range(-arenaSize / 2, arenaSize / 2);
                posZ = Random.Range(-arenaSize / 2, arenaSize / 2);
            } while (!NavMesh.SamplePosition(new Vector3(posX, 0, posZ), out hit, 1, -1));
            goPeon.transform.position = new Vector3(posX, 1, posZ);

            goPeon.transform.parent = peonsParent;
        }
    }

    private void CreateGuardSpawns()
    {
        guardSpawns = new List<SpawnPoint>();
        for (int i = 0; i < guardSpawnsParent.childCount; i++)
        {
            SpawnPoint sp = guardSpawnsParent.GetChild(i).GetComponent<SpawnPoint>();
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
            Debug.Log("create guards");
            GameObject goGuard = Instantiate(guardPrefab) as GameObject;
            _um.AddGuard(goGuard.GetComponent<Guard>());
            float eulerY = Random.Range(0f, 359f);
            goGuard.transform.eulerAngles = new Vector3(goGuard.transform.eulerAngles.x, eulerY, goGuard.transform.eulerAngles.z);

            foreach (SpawnPoint sp in guardSpawns)
            {
                if (sp.AddGuard(goGuard))
                    break;
            }
            goGuard.transform.parent = guardsParent;
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
	
	}

    private void ArenaCompleted()
    {
        Debug.Log("Total Peon : " + GameStats.nbPeon);
        Debug.Log("Peon Killed : " + GameStats.nbPeonKilled);
        if (GameStats.difficulty < 3)
        {
            Debug.Log("Arena Completed !");
            GameStats.difficulty++;
            Application.LoadLevel("3C");
        }
        else
        {
            Debug.Log("Game Completed !");
        }
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER");
    }

    void OnDisable()
    {
        UnitsManager.AllGuardRemoved -= ArenaCompleted;
        Player.PlayerIsDead -= GameOver;
    }
}
