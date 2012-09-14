using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections;


[Serializable]
public class Motive
{
    private float     mSuspicionModifier =  0; //How much the Motive modifies the characters suspicion
    private string    mDialogText =  "PUT TEXT HERE"; //What the dialog system should say.
    private CharacterName mCharacter = CharacterName.Anyone; //Who this motive can belong too


    public string DialogText
    {
        get { return mDialogText; }
        set { mDialogText = value; }
    }

    public CharacterName Character
    {
        get { return mCharacter; }
        set { mCharacter = value; }
    }

    public float SuspicionModifier
    {
        get { return mSuspicionModifier; }
        set { mSuspicionModifier = value; }
    }

    public static  Motive Deserialize(string path)
    {
        using (StreamReader fs = new StreamReader(path))
        {
            return new XmlSerializer(typeof (Motive)).Deserialize(fs) as Motive;
        }
    }

    public static  void Serialize(string path, Motive target)
    {
        using (StreamWriter fs = new StreamWriter(path))
        {
            new XmlSerializer(typeof (Motive)).Serialize(fs, target);
        }
    }
}
