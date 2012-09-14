using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



class AccusationResults 
{

    public static CharacterName CurrentCharacterAccusing;
    public static CharacterItem CharacterItem1,
                         CharacterItem2;
    public static Motive CharacterMotive;
	
	public static void Clear()
	{
		CharacterMotive = null;
	}
}

