using System;
using System.Xml.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public enum NodeActionType
{
    OnDialogFocused,
    OnDialogLeave,
}
[Serializable]
public abstract class NodeAction
{
    [XmlIgnore]
    protected DialogItem mOwner;
    public DialogItem Owner
    {
        get { return mOwner; }
    }

    protected NodeActionType mActionType;
    public NodeActionType ActionType
    {
        get { return mActionType; }
    }

    public bool IsShowing;
    public virtual void Initilize(ref DialogItem owner, NodeActionType type)
    {
        mOwner = owner;
        mActionType = type;
    }

    protected NodeAction(){}
    /// <summary>
    /// Ovverride this method and perform the nodes logic in here
    /// </summary>
    public virtual void DoAction(){}
    /// <summary>
    /// Put your nodes initilization logic in here, be sure to call it in the runtime
    /// </summary>
    public virtual void Initilize(){}
    /// <summary>
    /// Do any logic in here that you may need to prepare your node for serialization
    /// </summary>
    public virtual void BuildForSerialize(){}

    public virtual void Update()
    {
    }

    /// <summary>
    /// Do anything here that needs to happen when the node is removed or destroyed
    /// </summary>
    public virtual void OnDestroy()
    {
    }

#if UNITY_EDITOR
    /// <summary>
    /// Override to suit your on inspector gui.
    /// Make sure to wrap this call with #if UNITY_EDITOR and #endif
    /// </summary>
    public virtual void OnInspectorGUI()
    {
    }

    public virtual void OnEditorDestroy()
    {
        
    }
#endif//end unity editor region
}

/// <summary>
/// Provides a simple implementaion for a node action that requires one gameobject hashed
/// </summary>
public abstract class SingleHash : NodeActionHash
{
    protected GameObject nodeTarget;
    protected GameObject prev;
    public string mHashID = "";
    public string objectTag = "";
   
    
    public override void Initilize(ref DialogItem owner, NodeActionType type)
    {
        base.Initilize(ref owner, type);

        //if (mHashID != "")
        //{
        //    Debug.Log("Loading Object hash");
        //    nodeTarget = LoadFromHash(mHashID);
        //    if (nodeTarget != null)
        //    {
        //        prev = nodeTarget;
        //    }
        //}
        if(nodeTarget == null)
        {
            //Debug.Log("Loading tag: " + objectTag);
            nodeTarget = LoadFromTag(objectTag);
            if (nodeTarget != null)
            {
                prev = nodeTarget;
            }
            
        }
        

    }

#if UNITY_EDITOR
    public void SetNodeTarget(ref GameObject target)
    {
        nodeTarget = target;
    }
    public override void OnInspectorGUI()
    {
        DrawNodeTargetGUI(ref nodeTarget, ref prev, objectTag);
        base.OnInspectorGUI();
    }

    public override void BuildForSerialize()
    {
        objectTag = nodeTarget.tag;
        base.BuildForSerialize();
    }
    
#endif
    
}

/// <summary>
/// Use this class when you node action needs to serialize a game object
/// </summary>
///
[Serializable] 
public abstract class NodeActionHash : NodeAction
{
    protected GameObject LoadFromTag(string tag)
    {
        GameObject objectRef = null;
        if (tag != "")
        {
            //Debug.Log("Loading tag: " + tag);
            if (CheckForUniqueTag(tag))
            {
                objectRef = GameObject.FindGameObjectWithTag(tag);
                //Debug.Log("Tag search results: " + objectRef.tag);
            }
            if (objectRef != null)
            {
                //Debug.Log("Node initilized");
                return objectRef;

            }
        }
        Debug.Log("Node not intilized");
        return null;
    }

    protected bool CheckForUniqueTag(string tag)
    {
        if (GameObject.FindGameObjectsWithTag(tag).Length > 2)
        {
            //Debug.Log("The Object you are trying to set does not have a unique tag. Please make sure you have a unique tag associated with this object");
            return false;
        }
        return true;
    }

    //protected GameObject LoadFromHash(string hash)
    //{
    //    Debug.Log("checking object: " + hash);
    //    GameObject objectRef = null;
    //    if (objectRef == null)
    //    {
    //        Debug.LogWarning("No target found. fetching node");
    //        objectRef = HashID.GetGameObjectFromHashID(hash);
    //    }
    //    if (objectRef == null)
    //    {
    //        Debug.LogError("FETCH FAIL: No Object with " + hash + "found. Please make sure IDs are loaded. ");
    //        return objectRef;
    //    }
    //    return objectRef;
    //}
#if UNITY_EDITOR

    
  
    protected void DrawNodeTargetGUI(ref GameObject nodeTarget, ref GameObject prev, string showString, string showStringLable = "Tag" ,string label = "Node Target: ")
    {
        prev = nodeTarget;
        EditorGUILayout.BeginHorizontal();
        nodeTarget = (GameObject)EditorGUILayout.ObjectField(label, nodeTarget, typeof(GameObject), true);
        EditorGUILayout.LabelField(showStringLable, showString);
        EditorGUILayout.EndHorizontal();
        if (prev == nodeTarget) return;
        if(CheckForUniqueTag(nodeTarget.tag))return;
        nodeTarget = null;
        //RemoveHashIDComponent(ref prev, ref nodeTarget, hashID);
        //hashID =  AddHashComponent(ref nodeTarget);
    }

    protected string AddHashComponent(ref GameObject nodeTarget)
    {
        if(nodeTarget == null) return string.Empty;
        //Debug.Log("Adding new Hash ID");
        HashID newid = nodeTarget.AddComponent<HashID>();
        return newid.Initilize();
    }

    protected void RemoveHashIDComponent(ref GameObject prev, ref GameObject nodeTarget, string hashID)
    {
        if (prev == null) return;

        HashID[] hashIDs = prev.GetComponents<HashID>();
        if (hashID == null) return;

        //Debug.Log("Removing old hash ID");
        foreach (var id in hashIDs)
        {
            if (id.HashId != hashID) continue;
            GameObject.DestroyImmediate(id);
            break;
        }
    }
	
	protected void RemoveHashIDComponent(GameObject target, string objectHashID)
    {
		if(target == null) return;
        HashID[] ids = target.GetComponents<HashID>();
        foreach (HashID hashID in ids)
        {
            if(hashID.mHashID != objectHashID)continue;

            GameObject.DestroyImmediate(hashID);
            break;

        }
    }
    
#endif
}

[Serializable]
public class LookAtGameObject : NodeActionHash
{
    private GameObject looker,
                       looktAtTarget,
                       prevLooker,
                       prevTarget;

    public string lookerHashID,
                  lookAtTargetHashID,
                  lookerTag,
                  targetTag;

    public float   lookTime;

    public void SetLooker(ref GameObject target)
    {
        looker = target;
    }

    public void SetTarget(ref GameObject target)
    {
        looktAtTarget = target;
    }

    public override void  Initilize(ref DialogItem owner, NodeActionType type)
    {

        looker = LoadFromTag(lookerTag);
        prevLooker = looker;

        looktAtTarget = looktAtTarget = LoadFromTag(targetTag);
        prevTarget = looktAtTarget;
        base.Initilize(ref owner, type);
    }
    public override void DoAction()
    {
        //Debug.Log(looker.name +" is looking at "+ looktAtTarget.name);
        Vector3 pos = looktAtTarget.transform.position;
        pos.y = looker.transform.position.y;
        iTween.LookTo(looker, pos, lookTime);
        base.DoAction();
    }
#if UNITY_EDITOR

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
        lookTime = EditorGUILayout.FloatField("Look time: ", lookTime);
        DrawNodeTargetGUI(ref looker, ref prevLooker, lookerTag, "Looker: ");
        DrawNodeTargetGUI(ref looktAtTarget, ref prevTarget, targetTag, "Look At Target");
        EditorGUILayout.EndVertical();
    }
    public override void BuildForSerialize()
    {
        lookerTag = looker.tag;
        targetTag = looktAtTarget.tag;
        base.BuildForSerialize();
    }
#endif


}

[Serializable]
public class StopNav : SingleHash
{
    public override void DoAction()
    {
        //Debug.Log("Stopping navigation on: " + nodeTarget.name);
        NavMeshAgent agent = nodeTarget.GetComponent<NavMeshAgent>();
        if(agent != null)
        {
            Debug.Log("Stopping Agent");
            agent.Stop();
            agent.enabled = false;
        }
        base.DoAction();
    }
}

[Serializable]
public class StartNav : SingleHash
{
    public override void DoAction()
    {
        //Debug.Log("Resuming Navigation");
        NavMeshAgent agent = nodeTarget.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = true;
            agent.Resume();
        }
        base.DoAction();
    }
}

[Serializable]
public class PlayAnimation : SingleHash
{
    public string AnimationName = "";
   
    public override void DoAction()
    {
        //Debug.Log("Node Target: " + nodeTarget.name + " animaion: " + AnimationName);
		Animation animationComponent = nodeTarget.GetComponent<Animation>();
		if(animationComponent == null)
		{
			animationComponent = nodeTarget.GetComponentInChildren<Animation>();
			if(animationComponent == null)
			{
				Debug.LogError("No animation components found");
				return;
			}
		}
        animationComponent.Play(AnimationName);
        base.DoAction();
    }
#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
        AnimationName = EditorGUILayout.TextField("Animation name: ", AnimationName);
        base.OnInspectorGUI();
        EditorGUILayout.EndVertical();
    }

#endif
}


[Serializable]
public class  JumpToNode : SingleHash
{
    public int nodeNumberToJumpTo = 0;

    public override void DoAction()
    {
        //Debug.Log("Jumping To Node " + nodeNumberToJumpTo);
        nodeTarget.GetComponent<Conversation>().SetDialogIndex(nodeNumberToJumpTo);
        base.DoAction();
    }

#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
        nodeNumberToJumpTo = EditorGUILayout.IntField("Node to jump to index (base 0): ", nodeNumberToJumpTo);
        DrawNodeTargetGUI(ref nodeTarget, ref prev, objectTag, "Conversation owner");
        
        EditorGUILayout.EndVertical();
    }

#endif
}