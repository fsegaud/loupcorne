using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Transform playerSpawn;
    [SerializeField] private List<Transform> guardSpawns;
    //[SerializeField] private List<Transform> peonSpawns;
    [SerializeField] private int difficulty;
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

        for (int i = 0; i < maxPeons; i++)
        {
            GameObject goPeon = Instantiate(peonPrefab) as GameObject;
            _um.AddPeon(goPeon.GetComponent<Peon>());
            float eulerY = Random.Range(0f, 359f);
            goPeon.transform.eulerAngles = new Vector3(goPeon.transform.eulerAngles.x, eulerY, goPeon.transform.eulerAngles.z);
            //goPeon.transform.position = peonSpawns[Random.Range(0, peonSpawns.Count)].position;
            NavMeshHit hit = new NavMeshHit();
            float posX;
            float posZ;
             do
             {
                 posX = Random.Range(-arenaSize / 2, arenaSize / 2);
                 posZ = Random.Range(-arenaSize / 2, arenaSize / 2);
             } while(!NavMesh.SamplePosition(new Vector3(posX, 0, posZ), out hit, 1, -1));
             goPeon.transform.position = new Vector3(posX, 1, posZ);
           
            goPeon.transform.parent = peonsParent;
        }

        for (int i = 0; i < maxGuards; i++)
        {
            GameObject goGuard = Instantiate(guardPrefab) as GameObject;
            _um.AddGuard(goGuard.GetComponent<Guard>());
            float eulerY = Random.Range(0f, 359f);
            goGuard.transform.eulerAngles = new Vector3(goGuard.transform.eulerAngles.x, eulerY, goGuard.transform.eulerAngles.z);
            goGuard.transform.position = guardSpawns[Random.Range(0, guardSpawns.Count)].position;
            goGuard.transform.parent = guardsParent;
        }

        Transform player = _um.player.transform;
        player.transform.position = playerSpawn.position;
        UnitsManager.AllGuardRemoved += ArenaCompleted;

        this.ApplyDifficulty(1);

        if (this.OnGameReady != null)
        {
            this.OnGameReady(this);
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
        Debug.Log("ARENA COMPLETED");
        //TODO: end arena
    }

    void OnDisable()
    {
        UnitsManager.AllGuardRemoved -= ArenaCompleted;
    }
}
