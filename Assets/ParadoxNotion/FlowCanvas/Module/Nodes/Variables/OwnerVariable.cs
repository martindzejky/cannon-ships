﻿using ParadoxNotion.Design;
using UnityEngine;


namespace FlowCanvas.Nodes{

	[Name("Self")]
	[Description("Returns the Owner GameObject")]
	public class OwnerVariable : VariableNode {
		
		public override string name{
			get {return "<size=20>SELF</size>";}
		}

		protected override void RegisterPorts(){
			AddValueOutput<GameObject>("Value", ()=> { return graphAgent? graphAgent.gameObject : null; });
		}

		public override void SetVariable(object o){
			//Owner variable can't be set...
		}
	}
}