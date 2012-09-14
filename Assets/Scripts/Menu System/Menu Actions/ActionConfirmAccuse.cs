using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ActionConfirmAccuse : ActionBase
{

    protected override void DoActualAction()
    {
        MansionResults.ConfirmAccuse(AccusationResults.CurrentCharacterAccusing.ToString(), AccusationResults.CharacterItem1.ToString(),
                                     AccusationResults.CharacterItem2.ToString(), AccusationResults.CharacterMotive);
        base.DoActualAction();
    }


}

