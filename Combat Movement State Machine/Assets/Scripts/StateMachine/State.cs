using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public abstract class State
{
	public abstract void Enter();
	public abstract void Tick(float deltaTime);
	public abstract void Exit();		
}


