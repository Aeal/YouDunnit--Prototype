using UnityEngine;
using System.Collections.Generic;

public class NavigationNode : MonoBehaviour
{
    public static List<GameObject> Nodes = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
	    Nodes.Add(this.gameObject);
	}

	public static GameObject GetRandomNode()
    {
        return Nodes[new System.Random().Next(0,Nodes.Count-1)];
    }

    public static List<GameObject> returnNodes()
    {
        return Nodes;
    }

    public static int returnNodeCount()
    {
        return Nodes.Count;
    }
    public static void Clear()
    {
        Nodes = new List<GameObject>();
    }
}
