using UnityEngine;
using System.Collections;

public class StageBuilderController : MonoBehaviour {

    public GameObject[] stagePrefabs;
    public GameObject fullStage;

    private GameObject viewRoot;
    private ConcertController concertController;

    private GameObject currentStage;

	void Awake()
    {
        viewRoot = GameObject.Find("View");
        concertController = FindObjectOfType<ConcertController>();
    }

    void OnEnable()
    {
        concertController.StartOfConcert += ChangeStagePrefab;
    }

    void OnDisable()
    {
        concertController.StartOfConcert -= ChangeStagePrefab;
    }

    void Start () {
        ConcertData startingConcertData = GameState.instance.Concert.CurrentConcert;
        ChangeStagePrefab(startingConcertData);
    }

    void Update () {
	
	}

    private void ChangeStagePrefab(ConcertData data)
    {
        int stageIndex = data.id;
        if (stageIndex < stagePrefabs.Length)
        {
            foreach (GameObject tempStagePrefab in stagePrefabs)
            {
                if (tempStagePrefab.name == ("Stage" + stageIndex))
                {
                    CreateStage(tempStagePrefab);
                    break;
                }
            }
        } else
        {
            CreateStage(fullStage);
        }
    }

    private void CreateStage(GameObject tempStagePrefab)
    {
        if (currentStage)
        {
            Destroy(currentStage);
        }
        GameObject instantiated = Instantiate<GameObject>(tempStagePrefab);
        instantiated.transform.SetParent(viewRoot.transform);

        currentStage = instantiated;
    }
}
