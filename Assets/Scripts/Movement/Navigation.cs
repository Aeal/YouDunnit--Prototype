using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Navigation : MonoBehaviour {
	
	
	public GameObject[] navPoints;
	NavMeshAgent me;
    float time = 0.0f;
    float timeToSwap = 0.5f;
	private bool t_Mode = false;
	
	// Use this for initialization
	void Start () 
    {
        me = this.GetComponent<NavMeshAgent>();
       	calcNewPoint();
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        if (me.enabled)
        {
           ////Debug.Log("Lawl: " + me.pathStatus + gameObject);
			
			
            if (me.remainingDistance <= 5.0f)
            {
				
                time += Time.deltaTime;
                if (time >= timeToSwap)
                {
                    time = 0.0f;
                    calcNewPoint();
                }

            }
        

        }
     


	}

    public void StopPath()
    {
        me.Stop();
    }

    public  void Resume()
    {
        me.Resume();
    }

    
    private void calcNewPoint()
    {
        me.SetDestination(NavigationNode.GetRandomNode().transform.position);
    }
}
