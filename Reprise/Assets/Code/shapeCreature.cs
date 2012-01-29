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
	

	// Use this for initialization
	void Start() {
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
     	counts[2] = 3;
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
		if (shapeManager.rotationCount % 1 == 0) {
			int maxIter = 0;
			for(int i = 0; i < 3; i++) {
				if (counts[maxIter] <= counts[i]) {
					maxIter = i;
				}
			}
			GetComponent<MeshFilter>().mesh = shapeManager.prefabs[maxIter];
		}
	}
	
	public void CollideWithCreature(GameObject obj) {
		if (obj.tag == "Ghost") {
				
		} else if (obj.tag == "Shape") {
			
		}
	}
}
