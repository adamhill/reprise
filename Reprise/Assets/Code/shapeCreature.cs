using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class is instanced for every moveable/consumable object in the game, including the players object
/// </summary>

public class shapeCreature : MonoBehaviour 
{
    public Vector3 targetPos;
	public Vector3 targetScale;
	public float speed = 0.2f;
	public int[] counts;
	public GameObject[] prefabs;
	

	// Use this for initialization
	void Start() {
		counts[0] = 0; counts[1] = 0; counts[2] = 0;
		targetPos = this.transform.localPosition;
		targetScale = this.transform.localScale;
	}
	
	void OnTriggerEnter(Collider obj)
	{
		shapeDropping dropping = obj.GetComponent<shapeDropping>();
		if (!dropping.firstCollision) dropping.firstCollision = true;
		else CollideWithCreature(obj.gameObject);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKey("up"))
        {
            targetPos += new Vector3(0, speed, 0);
        }
        if (Input.GetKey("down"))
        {
            targetPos -= new Vector3(0, speed, 0);
        }
        if (Input.GetKey("right"))
        {
            targetPos += new Vector3(speed, 0, 0);
        }
        if (Input.GetKey("left"))
        {
            targetPos -= new Vector3(speed, 0, 0);
        }
		this.transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 0.02f);
		this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, targetPos, 0.2f);
	}
	
	
	public void ScaleUp(float dt) {
		targetScale = transform.localScale + new Vector3(dt,dt,dt);
		if (shapeManager.rotationCount % 5 == 0) {
			int maxIter = 0;
			int maxCount = 0;
			for(var i = 0; i < 2; i++) {
				if (maxCount < counts[i]) {
					maxCount = counts[i];
					maxIter = i;
				}
			}
			GetComponent<MeshFilter>().mesh = prefabs[maxIter].GetComponentInChildren<MeshFilter>().mesh;
		}
	}
	
	public void CollideWithCreature(GameObject obj) {
		if (obj.tag == "Ghost") {
				
		} else if (obj.tag == "Shape") {
			
		}
	}
}
