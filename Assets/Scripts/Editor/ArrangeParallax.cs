using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ArrangeParallaxScript))]
public class ArrangeParallax : Editor
{

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		ArrangeParallaxScript myScript = (ArrangeParallaxScript)target;
		if (GUILayout.Button("Change Z Values"))
		{
			
				myScript.PopulateLists();
		}
	}



}
