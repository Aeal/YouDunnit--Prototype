using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour {
	
public Texture2D normalReticleTexture;
public Texture2D reticleTextureOverFlasbackObject;
private Texture2D currentTexture;
private float minSize = 30;
private float maxSize = 60;
private float maxDistance = 20;
private bool isVisible = true;

private RaycastHit hit;
private Rect rectangle;

private int screenHeight;
private int screenWidth;

public Texture2D[] animationArray;

private Color mRecticleColor;
private int currentAnimationIndex;
	
	void Start()
	{
		screenHeight = Screen.height;
		screenWidth = Screen.width;
		rectangle = new Rect(screenWidth/2,screenHeight/2,minSize,minSize);//Sets draw location for reticle
	}
	
	void Update()
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenWidth/2,screenHeight/2)); //Checks center of screen for raycast.
		if (Physics.Raycast(ray, out hit,  maxDistance)) //Runs check to see if there is a raycast collision within our maximum distance.
		{
			/*
			 if(hit.collider.gameObject.GetComponent<FlashbackObject>())
			{
				currentTexture = animationArray[currentAnimationIndex];
				if(currentAnimationIndex == animationArray.Length - 1)
					currentAnimationIndex = 0;
				else
					currentAnimationIndex++;
			}
			else
				currentTexture = normalReticleTexture; */
			currentTexture = normalReticleTexture;
			rectangle.width = rectangle.height = Map(hit.distance, 0, maxDistance, maxSize, minSize); //this is a maping system to resize values to a different size. 5 out 10 would be resized to 50 out of 100 if it were map(5, 0, 10, 0, 100);
															//Without the above line, our reticle wouldn't resize hardly ever, and it also only scales the size between are max and min value. Max is first otherwise the reticle would get larger with the further distance.
			if(rectangle.width < minSize)
			{
				rectangle.width = rectangle.height = minSize; //if distance draws it smaller then our minimum size, then resize it to minimum
			}
			if(rectangle.width > maxSize)
			{
				rectangle.width = rectangle.height = maxSize; //if distance draws it larger then our maximum size, then resize it to maximum
			}
		}else{
			rectangle.width = rectangle.height = minSize; //if no collision within the maximum distance is found then we simply draw the smallest size.
			currentTexture = normalReticleTexture;
		}
		rectangle.x = screenWidth/2 - rectangle.width/2; //These two lines change the position of the reticle to be based on the center of the texture rather then the top left.
		rectangle.y = screenHeight/2 - rectangle.height/2; //They are always needed, so it's outside of the if statements above.

        //Se the color of the reticle based on the object it hit.
        if (hit.collider != null)
        {
            switch (hit.collider.gameObject.tag)
            {
                case "NPC":
                    mRecticleColor = XKCDColors.DarkForestGreen;
                    break;
                case "InteractiveItem":
                    mRecticleColor = XKCDColors.DarkGold;
                    break;
                default:
                    mRecticleColor = XKCDColors.PaleGold;
                    break;
            }
        }
        else
        {
            mRecticleColor = Color.white;
        }
	} 
	
	void OnGUI()
	{
		if(isVisible)
		{
		    GUI.color = mRecticleColor;
			GUI.DrawTexture(rectangle, currentTexture);
            GUI.color = Color.white;

		}
	}
	
	float Map(float val, float fromLow, float fromHigh, float toLow, float toHigh)
	{
		   return toLow + (toHigh - toLow) * ((val - fromLow) / (fromHigh - fromLow));
	}
}
