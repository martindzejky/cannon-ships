﻿using System;
using System.Reflection;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Conditions{

	[Category("✫ Script Control/Common")]
	[Description("Will subscribe to a public event of Action type and return true when the event is raised.\n(eg public event System.Action [name])")]
	public class CheckCSharpEvent : ConditionTask {

		[SerializeField]
		private System.Type targetType = null;
		[SerializeField]
		private string eventName = null;

		public override Type agentType{
			get {return targetType != null? targetType : typeof(Transform);}
		}
		
		protected override string info{
			get
			{
				if (string.IsNullOrEmpty(eventName))
					return "No Event Selected";
				return string.Format("'{0}' Raised", eventName);
			}
		}


		protected override string OnInit(){
			
			if (eventName == null)
				return "No Event Selected";

			var eventInfo = agentType.RTGetEvent(eventName);
			if (eventInfo == null){
				return "Event was not found";
			}

			var methodInfo = this.GetType().RTGetMethod("Raised");
			var handler = methodInfo.RTCreateDelegate(eventInfo.EventHandlerType, this);
			eventInfo.AddEventHandler(agent, handler);
			return null;
		}

		public void Raised(){
			YieldReturn(true);
		}

		protected override bool OnCheck(){
			return false;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnTaskInspectorGUI(){

			if (!Application.isPlaying && GUILayout.Button("Select Event")){
				Action<EventInfo> Selected = (e)=> {
					targetType = e.DeclaringType;
					eventName = e.Name;
				};

				var menu = new UnityEditor.GenericMenu();
				if (agent != null){
					foreach(var comp in agent.GetComponents(typeof(Component)).Where(c => c.hideFlags == 0) ){
						menu = EditorUtils.GetEventSelectionMenu(comp.GetType(), null, Selected, menu);
					}
					menu.AddSeparator("/");
				}
				foreach (var t in UserTypePrefs.GetPreferedTypesList(typeof(Component))){
					menu = EditorUtils.GetEventSelectionMenu(t, null, Selected, menu);
				}

				if ( NodeCanvas.Editor.NCPrefs.useBrowser){ menu.ShowAsBrowser("Select Event", this.GetType()); }
				else { menu.ShowAsContext(); }
				Event.current.Use();
			}

			if (targetType != null){
				GUILayout.BeginVertical("box");
				UnityEditor.EditorGUILayout.LabelField("Selected Type", agentType.FriendlyName());
				UnityEditor.EditorGUILayout.LabelField("Selected Event", eventName);
				GUILayout.EndVertical();
			}
		}
		
		#endif
	}



	[Category("✫ Script Control/Common")]
	[Description("Will subscribe to a public event of Action<T> type and return true when the event is raised.\n(eg public event System.Action<T> [name])")]
	public class CheckCSharpEvent<T> : ConditionTask {

		[SerializeField]
		private System.Type targetType = null;
		[SerializeField]
		private string eventName = null;
		[SerializeField]
		private BBParameter<T> saveAs = null;

		public override Type agentType{
			get {return targetType ?? typeof(Transform);}
		}
		
		protected override string info{
			get
			{
				if (string.IsNullOrEmpty(eventName))
					return "No Event Selected";
				return string.Format("'{0}' Raised", eventName);
			}
		}


		protected override string OnInit(){

			if (eventName == null)
				return "No Event Selected";			

			var eventInfo = agentType.RTGetEvent(eventName);
			if (eventInfo == null){
				return "Event was not found";
			}

			var methodInfo = this.GetType().RTGetMethod("Raised");
			var handler = methodInfo.RTCreateDelegate(eventInfo.EventHandlerType, this);
			eventInfo.AddEventHandler(agent, handler);
			return null;
		}

		public void Raised(T eventValue){
			saveAs.value = eventValue;
			YieldReturn(true);
		}

		protected override bool OnCheck(){
			return false;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnTaskInspectorGUI(){

			if (!Application.isPlaying && GUILayout.Button("Select Event")){
				Action<EventInfo> Selected = (e)=> {
					targetType = e.DeclaringType;
					eventName = e.Name;
				};


				var menu = new UnityEditor.GenericMenu();
				if (agent != null){
					foreach(var comp in agent.GetComponents(typeof(Component)).Where(c => c.hideFlags == 0) ){
						menu = EditorUtils.GetEventSelectionMenu(comp.GetType(), typeof(T), Selected, menu);
					}
					menu.AddSeparator("/");
				}
				foreach (var t in UserTypePrefs.GetPreferedTypesList(typeof(Component))){
					menu = EditorUtils.GetEventSelectionMenu(t, typeof(T), Selected, menu);
				}

				if ( NodeCanvas.Editor.NCPrefs.useBrowser){ menu.ShowAsBrowser("Select Event", this.GetType()); }
				else { menu.ShowAsContext(); }
				Event.current.Use();
			}

			if (targetType != null){
				GUILayout.BeginVertical("box");
				UnityEditor.EditorGUILayout.LabelField("Selected Type", agentType.FriendlyName());
				UnityEditor.EditorGUILayout.LabelField("Selected Event", eventName);
				GUILayout.EndVertical();

				EditorUtils.BBParameterField("Save Value As", saveAs, true);
			}
		}
		
		#endif
	}
}