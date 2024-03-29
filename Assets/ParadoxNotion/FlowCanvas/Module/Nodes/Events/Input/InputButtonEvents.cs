﻿using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace FlowCanvas.Nodes{

	[Name("Input Button")]
	[Category("Events/Input")]
	[Description("Calls respective outputs when the defined Button is pressed down, held down or released.\nButtons are configured in Unity Input Manager.")]
	public class InputButtonEvents : EventNode, IUpdatable {

		[RequiredField]
		public string buttonName = "Fire1";
		private FlowOutput down;
		private FlowOutput up;
		private FlowOutput pressed;

		public override string name{
			get {return string.Format("<color=#ff5c5c>➥ Button '{0}'</color>", buttonName).ToUpper();}
		}

		protected override void RegisterPorts(){
			down    = AddFlowOutput("Down");
			pressed = AddFlowOutput("Pressed");
			up      = AddFlowOutput("Up");
		}

		public void Update(){
			
			if (Input.GetButtonDown(buttonName)){
				down.Call(new Flow(1));
			}

			if (Input.GetButton(buttonName)){
				pressed.Call(new Flow(1));
			}
			
			if (Input.GetButtonUp(buttonName)){
				up.Call(new Flow(1));
			}
		}
	}
}