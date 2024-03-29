﻿using FlowCanvas;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using UnityEngine;

public class EnergyPod : MonoBehaviour {

    public AudioClip soundEffect;

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.GetComponent<Ship>() == null) {
            return;
        }

        var myBlackboard = GetComponent<Blackboard>();
        var myEnergy = myBlackboard.GetVariable<float>("energy");

        var otherBlackboard = other.gameObject.GetComponent<Blackboard>();
        var otherEnergy = otherBlackboard.GetVariable<float>("energy");
        var otherMaxEnergy = otherBlackboard.GetVariable<float>("maxEnergy");

        var newEnergy = Mathf.Min(otherEnergy.GetValue() + myEnergy.GetValue(), otherMaxEnergy.GetValue());
        otherEnergy.SetValue(newEnergy);

        var particles = transform.GetChild(0);
        particles.parent = null;
        particles.GetComponent<ParticleSystem>().Play();
        Destroy(particles.gameObject, 1);

        // re-activate ship
        foreach (var flowScriptController in other.gameObject.GetComponents<FlowScriptController>()) {
            flowScriptController.enabled = true;
        }
        foreach (var behaviourTreeOwner in other.gameObject.GetComponents<BehaviourTreeOwner>()) {
            behaviourTreeOwner.enabled = true;
        }

        // play sound
        GameObject.Find("AudioPlayer").GetComponent<FlowScriptController>().SendEvent<Vector3>("Position", transform.position);
        GameObject.Find("AudioPlayer").GetComponent<FlowScriptController>().SendEvent<AudioClip>("Play", soundEffect);

        Destroy(gameObject);
    }

}
