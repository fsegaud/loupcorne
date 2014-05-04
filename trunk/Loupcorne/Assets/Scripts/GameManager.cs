using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
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
        transform.position = playerSpawn.position;
        UnitsManager.AllGuardRemoved += ArenaCompleted;
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
