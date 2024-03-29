﻿using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace FlowCanvas.Nodes{

	[Name("Character Controller")]
	[Category("Events/Object")]
	[Description("Called when the Character Controller hits a collider while performing a Move")]
	public class CharacterControllerEvents : EventNode<CharacterController> {

		private ControllerColliderHit hitInfo;
		private FlowOutput hit;

		protected override string[] GetTargetMessageEvents(){
			return new string[]{ "OnControllerColliderHit" };
		}

		protected override void RegisterPorts(){
			hit = AddFlowOutput("Collider Hit");
			AddValueOutput<GameObject>("Other", ()=> { return hitInfo.gameObject; });
			AddValueOutput<Vector3>("Collision Point", ()=> { return hitInfo.point; });
			AddValueOutput<ControllerColliderHit>("Collision Info", ()=> { return hitInfo; });
		}

		void OnControllerColliderHit(ControllerColliderHit hitInfo){
			this.hitInfo = hitInfo;
			hit.Call(new Flow(1));
		}
	}
}