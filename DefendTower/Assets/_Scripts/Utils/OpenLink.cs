using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLink : MonoBehaviour{
	public void OpenWebLink() {
		System.Diagnostics.Process.Start("https://www.linkedin.com/in/christoph-maureder/");
	}
}
