using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


	class ActionCancelAccuse : ActionBase
	{
        protected override void DoActualAction()
        {
            GameManager.Instance.CancelAccuse();
            base.DoActualAction();
        }
	}

