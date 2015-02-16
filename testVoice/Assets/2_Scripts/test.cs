using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
	public Vector2 InitScale;
	public Vector3 min;
	public Vector3 max;
	// Use this for initialization
	void Start () {		
        Debug.Log (gameObject.transform.localScale);
		Debug.Log (gameObject.renderer.bounds.min);
		Debug.Log (gameObject.renderer.bounds.max);
		min = gameObject.renderer.bounds.min;
		max = gameObject.renderer.bounds.max;
		gameObject.renderer.bounds.SetMinMax (gameObject.renderer.bounds.min,max);
		InitScale = new Vector2 (gameObject.transform.localScale.x*(Mathf.Abs(max.x - min.x)),
		                         gameObject.transform.localScale.y*(Mathf.Abs(max.y - min.y)));
		

	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.localScale = new Vector3(InitScale.x/Mathf.Abs(max.x - min.x),InitScale.y/Mathf.Abs(max.y - min.y),1);
	}
}
