using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrangeParallaxScript : MonoBehaviour {

	[SerializeField]
	public ParallaxLayer[] layers;


	public void PopulateLists()
	{

		for (int i = 0; i < layers.Length; i++)
		{

			ParallaxLayer parallaxLayer = new ParallaxLayer();

			

			int max = layers[i].layer.transform.childCount;
		


			//layers[i].Sprites.Capacity = max;
			for (int j = 0; j < max;  j++)
			{
				Debug.Log(j);
				parallaxLayer.Sprites.Add(layers[i].layer.transform.GetChild(j).transform);
				parallaxLayer.Sprites[j].transform.position = new Vector3(parallaxLayer.Sprites[j].transform.position.x,
									parallaxLayer.Sprites[j].transform.position.y, layers[i].Zvalue);
			
			}
				
			//Debug.Log(string.Format("{0} {1}","number of sprites is: ", parallaxLayer.Sprites.Count));
			//}
		}

	}
	[System.Serializable]
	public class ParallaxLayer
	{
		public GameObject layer;
		public int Zvalue;
		[SerializeField]
		public List<Transform> Sprites = new List<Transform>();
	}

}
