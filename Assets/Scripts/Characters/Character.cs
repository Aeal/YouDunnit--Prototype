using UnityEngine;
using System.Collections;

public enum CharacterName
{
    Deborah,
    Doc,
    Bennington,
    Butler,
    Malory,
    Elizabeth,
    Cunningham,
    Sinclair,
    Dorothy,
    Wellsworth,
    Quinn,
    Bouchez,
    Kurtz,
    Anyone,
    MainCharacter,
}
public class Character : MonoBehaviour
{
    public CharacterName mCharacterName;
    protected const int MAX_SUSPICION_RATING = 100;
    protected const int MIN_SUSPICION_RATING = 0;
    protected float mSuspicionAmount;
    public float CharacterSuspicion
    {
        get { return mSuspicionAmount; }
    }
    protected void Start()
    {
        //Debug.Log("Registering " + mCharacterName);
        GameManager.Instance.RegisterCharacter(mCharacterName.ToString(), this);
    }
    public void ModifySuspicion(float amount)
    {
        mSuspicionAmount = Mathf.Clamp(mSuspicionAmount + amount, MIN_SUSPICION_RATING, MAX_SUSPICION_RATING);
    }
}
