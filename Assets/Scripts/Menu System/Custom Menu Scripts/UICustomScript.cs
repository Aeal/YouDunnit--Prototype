using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIMenuItem))]
public class UICustomScript : MonoBehaviour
{
	UIMenuItem  mItemOwner;         // UI Menu item that owns this script
    bool        mBuiltOk = false,   // Was this custom script built properly?
                mLocalLock = false; // Local lock on the built boolean if this base class fails to get its owner.
	const int	mGUILayer = 31;
    bool mActive;
	public UIMenuItem Owner
	{
		get { return mItemOwner; }
		set { mItemOwner = value; }
	}
	public GameObject CreateTexturePlane(GameObject parent = null)
	{
		GameObject ret = GameObject.CreatePrimitive(PrimitiveType.Plane);
		
		if (ret == null)
		{
			// error
			Debug.LogError("Could not create plane!");
		}
		else if (parent == null)
		{
			// Nothing to transform to, just return normal plane
			ret.layer = mGUILayer;
		}
		else
		{
			// We have created a plane and have a parent

			ret.transform.localPosition = Vector3.zero;
            ret.transform.rotation = Owner.gameObject.transform.rotation;
            ret.transform.position = Owner.gameObject.transform.position;
			ret.transform.localScale = Owner.gameObject.transform.localScale;
			ret.transform.parent = Owner.gameObject.transform;
			ret.transform.localPosition += new Vector3(0,.1f,0);
			ret.layer = mGUILayer;
		}
		
		return (ret);	
	}
    public bool BuildOk
    {
        get { return mBuiltOk; }
        set
        {
			// This just verifies that this custom script was properly hooked up
            if (mLocalLock == false)
            {
                mBuiltOk = value;
                if (mBuiltOk == false)
                {
                    // If somehow this was ever built wrong, it can't be built ok again so lock it
                    mLocalLock = true;
                }

            }
        }
    }

    public bool Active
    {
        get
        {
            if (mItemOwner != null && mItemOwner.ParentScreen != null)
            {
                return (mItemOwner.ParentScreen.Active);
            }
            else
            {
                return false;
            }
        }
        set
        {
            if ( mBuiltOk == true )
                mActive = value;
            else
               Debug.LogError("Trying to set an custom script as active when it was not built properly.");
        }
    }
	// Use this for initialization
	public virtual void Start ()
	{
		if ( ( mItemOwner = this.gameObject.GetComponent<UIMenuItem>()) == null )
		{
			Debug.LogError("Error retrieving UIMenuItem!");
            BuildOk = false;
		}

	}

    public virtual void Update()
    {

    }

}
