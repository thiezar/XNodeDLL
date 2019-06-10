using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class ExampleNode : Node {

	[Input] public Connection input;
	[Output] public Connection output;

	public string title;

	protected override void Init() {
		base.Init();
		
	}

	public override object GetValue(NodePort port) {
		return null;
	}

	[Serializable] public class Connection { }
}