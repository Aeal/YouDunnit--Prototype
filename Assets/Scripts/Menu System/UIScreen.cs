using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// ****************************************************************************
[System.Serializable]
public class UIScreen : MonoBehaviour
{

    protected Menu  m_RefMenuSystem;
    public List<UIMenuItem> mMenuItems;
	public bool mPausesGame = false;
    public bool mIsViewing,
                mIsStatic = false,
                mActive = false,
				mModifyTime = false;
	public bool wasSetasActive = false;
	protected bool m_ScreenInTransition = false;
    private int m_layer = 1;
    #region Accessors / Modifiers
    // ************************************************************************
    public int ScreenLayer
    {
        get { return m_layer; }
        set { m_layer = value; }
    }
    // ************************************************************************
    public bool Active
    {
        get 
        { 
            if ( m_ScreenInTransition == false )
			{
				return mActive; 
			}
			else
			{
				
				return false;
			}
		}
        set { mActive = value; }
    }
    // ************************************************************************
    public Menu ScreenMenu
    {
        get { if (m_RefMenuSystem == null) { m_RefMenuSystem = this.gameObject.transform.parent.parent.GetComponent<Menu>(); } return m_RefMenuSystem; }
        set { m_RefMenuSystem = value; }
    }
    // ************************************************************************
    #endregion

    #region Methods
    // ************************************************************************
    public void Start()
    {
        // Subscribe this UIscreen's onactive / oninactive 
		/*if ( mPauseGame )
		{
			GameManager.OnGamePaused += OnScreenActive;	
			GameManager.OnGameResume += OnScreenActive;
		}*/
		
        if (mActive == true)
        {

            OnScreenActive();
        }
        else
        {

            OnScreenInactive();
        }

    }
    // ************************************************************************
    public void Update()
    {
		if ( m_ScreenInTransition == true )
		{
			mActive = false;
			bool switchit = true;
			foreach( UIMenuItem item in mMenuItems)
			{
				if ( item.IsCustomMenuItem == false )
				{
					if ( item.GetTransitionFinished() == false )
					{
						switchit = false;
					}
				}
			}

			if ( switchit )
			{
				//Debug.Log (gameObject.name + " is done");
				m_ScreenInTransition = false;
				if ( wasSetasActive == true )
				{
					mActive = true;
					wasSetasActive = false;
				}
			}
		}
	

    }
    // ************************************************************************
    public void OnScreenClick()
    {
		if ( Active )
		{
        RaycastHit hitInfo;
        UIMenuItem itemHit;
		
        if (Physics.Raycast( ScreenMenu.camera.ScreenPointToRay(Input.mousePosition), out hitInfo))
        {
            itemHit = hitInfo.collider.gameObject.GetComponent<UIMenuItem>();
            if ( itemHit != null )
            {
                if ( itemHit.ParentScreen == this)
                {
                    //Debug.Log("Active Hit: Screen " + this.gameObject.name + " hit item -" + hitInfo.collider.gameObject.name);
                    if (itemHit.ParentScreen.mActive == true)
                    itemHit.OnClick();
                }
                else
                {
                    //Debug.Log("Inactive Hit: Hit another screen's menu item -" + hitInfo.collider.gameObject.name);
                }
            }
            
        }
		}


    }
    // ************************************************************************
    public void OnScreenActive()
    {
		wasSetasActive = true;
		//ScreenMenu.mInTransition = true;
        //Debug.Log(gameObject.name + " is active");
		m_ScreenInTransition = true;

        foreach (UIMenuItem item in mMenuItems)
        {
			if (item.IsCustomMenuItem == false)
                item.StartTransitionOn();

        }
		BasicMenuInput.Instance.OnMenuClickPressHandle += OnScreenClick;
    }
    // ************************************************************************
    public void OnScreenInactive()
    {
		m_ScreenInTransition = true;

        foreach (UIMenuItem item in mMenuItems)
        {
			if (item.IsCustomMenuItem == false)
			    item.StartTransitionOff();
        }

        BasicMenuInput.Instance.OnMenuClickPressHandle -= OnScreenClick;
    }
    // ************************************************************************
    public void OnDestroy()
    {
        BasicMenuInput.Instance.OnMenuClickPressHandle -= OnScreenClick;
    }
    // ************************************************************************
    #endregion
}
// ****************************************************************************