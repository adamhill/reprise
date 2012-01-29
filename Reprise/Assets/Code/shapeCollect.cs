using UnityEngine;
using System.Collections;

public class shapeCollect : MonoBehaviour {
	public Vector3 targetPos;
	public float speed;

	// Use this for initialization
	void Start () {
		targetPos = this.transform.localPosition;
		speed = Random.Range(4, 10);
	}
	
	// Update is called once per frame
	void Update () {
		targetPos = new Vector3(transform.position.x - speed, Mathf.Sin(Time.time) * speed, transform.position.z);
		targetPos = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
		//this.transform.position = Vector3.Lerp(this.transform.position, targetPos, 0.2f);
		//transform.position -= new Vector3(speed, 0, 0);
	}
}
