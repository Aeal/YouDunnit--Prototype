using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotiveManager //: Singleton<MotiveManager>
{


    private static MotiveManager instance;

    public static MotiveManager Instance
    {
        get { return instance ?? (instance = new MotiveManager()); }
    }

    public Dictionary<CharacterName,List<Motive>> Motives = new Dictionary<CharacterName, List<Motive>>();

    public void Init()
    {

        Object[] motiveFiles = Resources.LoadAll("Motives");
        List<Motive> motives = new List<Motive>();
        foreach (TextAsset motiveFile in motiveFiles)
        {
            using(StringReader reader = new StringReader(motiveFile.text) )
            {
               motives.Add(new XmlSerializer(typeof(Motive)).Deserialize(reader) as Motive);
            }
        }

        foreach (Motive motive in motives)
        {
            if(!Motives.ContainsKey(motive.Character))
            {
                Motives.Add(motive.Character,new List<Motive>());
            }
//           //Debug.Log("Added motive for " + motive.Character);
            Motives[motive.Character].Add(motive);
        }
        
    }
    
    public static Motive GetMotive(CharacterName person = CharacterName.Anyone)
    {
        System.Random ran = new System.Random();

        return  Instance.Motives[person][0];// :
            //Instance.Motives[CharacterName.Anyone][0];
    }



}
