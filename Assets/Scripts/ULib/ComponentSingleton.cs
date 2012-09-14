using UnityEngine;

public class ComponentSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get 
		{ 
			if(instance == null)
			{		Debug.Log("Creating singletone type of" + typeof(T).ToString());
					instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
			}
			return instance;
		}
    }

    public virtual void Init()
    {
        
    }
}
