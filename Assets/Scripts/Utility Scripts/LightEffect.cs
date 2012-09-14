using UnityEngine;
using System.Collections;

//**********************************************************************
// LightEffect.cs
// Author: Tyler Ortiz
// Date: 12/16/2011
// Purpose: Apply a light flicker effect.
//**********************************************************************
public enum LightEffectType
{
    None,
    Flicker,
    Pulse,
};
//**********************************************************************
public class LightEffect : MonoBehaviour
{
    #region Members

    public LightEffectType _LightEffect = LightEffectType.None;
    public float    _Min = 0.06f,
                    _Max = 1.25f,
                    _Phase = 0.135f;
    public bool     _AffectsAmbient = false;
    Light[]         mLights;
    float[]         mOrigIntensity,
                    mOrigSpotAngle;
    Color[]         mOrigColor;
    int             mNumLights = 0;
    Random          rand;
    float           time = 0.0f;

    #endregion

    #region Methods
    //******************************************************************
	void Start()
    {
        mLights = gameObject.GetComponentsInChildren<Light>();
        mNumLights = mLights.GetLength(0);
        mOrigIntensity = new float[mNumLights];
        mOrigSpotAngle = new float[mNumLights];
        mOrigColor = new Color[mNumLights];
        for (int i = 0; i < mNumLights; i++)
        {
            mOrigIntensity[i] = mLights[i].intensity;
            mOrigSpotAngle[i] = mLights[i].spotAngle;
            mOrigColor[i] = mLights[i].color;
        }
        rand = new Random();
	}
    //******************************************************************
    void Update()
    {
        time += Time.deltaTime;
        if (time > _Phase)
        {
            time = 0.0f;

            switch (_LightEffect)
            {
                case LightEffectType.None:
                    break;
                case LightEffectType.Flicker:
                    float delta = Random.Range(_Min, _Max);
                    for (int i = 0; i < mNumLights; i++)
                    {
                        if (_AffectsAmbient == false)
                        {
                            if (mLights[i].type != LightType.Point)
                                mLights[i].intensity = mOrigIntensity[i] - (delta / 2) + delta;
                        }
                        else
                        {
                            mLights[i].intensity = mOrigIntensity[i] - (delta / 2) + delta;
                        }
                        mLights[i].spotAngle = mOrigSpotAngle[i] - (delta / 8) + (delta * 2);
                        //mLights[i].color = mOrigColor[i] + new Color(mOrigColor[i].r - (delta / 2.0f) + delta, mOrigColor[i].g - (delta / 2.0f) + delta, mOrigColor[i].b - (delta / 2.0f) + delta);// mOrigColor[i]. - (delta / 2) + delta;
                    }
                    break;
                case LightEffectType.Pulse:
                    mLights[0].intensity += _Min;
                    break;
                default:
                    break;
            }
        }
        else if (time < (_Phase / 2.0f))
        {
            for (int i = 0; i < mNumLights; i++)
            {
                mLights[i].intensity = mOrigIntensity[i];
                //mLights[i].spotAngle = mOrigSpotAngle[i];
                //mLights[i].color = mOrigColor[i] + new Color(mOrigColor[i].r - (delta / 2.0f) + delta, mOrigColor[i].g - (delta / 2.0f) + delta, mOrigColor[i].b - (delta / 2.0f) + delta);// mOrigColor[i]. - (delta / 2) + delta;
            }
        }
        else
        {

        }
	}
    //******************************************************************
    #endregion
}
//**********************************************************************
