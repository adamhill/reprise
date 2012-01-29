using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class is instanced for every moveable/consumable object in the game, including the players object
/// </summary>

public class shapeDropping : MonoBehaviour 
{
    public enum Shapes
    {
        Square,
        Cirlce,
        Triangle,
    }

    public GameObject shapeSpace;
    public bool ghost;
	public Vector3 targetPos;
	public float speed = 0.1f;
	
    //use this to access proper playerHistory list
    public List<Vector2> history = new List<Vector2>();
    public int historyIndex;
    public int historyCounter = 0;
	
	public bool firstCollision = false;

	// Use this for initialization
	void Start() 
    {
		targetPos = this.transform.localPosition;
	}
		
	// Update is called once per frame
	void Update () 
    {
        if (Time.frameCount % 2 == 0)
        {
            if (historyCounter >= historyIndex)
            {
                historyCounter = 0;
            }
            targetPos = new Vector3(this.transform.position.x + speed, history[historyCounter].y, this.transform.position.z);
            historyCounter++;
        }
		this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, targetPos, 0.2f);
	}
	
    public void depthCheck()
    {
		if (Vector3.Distance(shapeManager.playerShape.transform.localScale, transform.localScale) > 4)
	    {
	        kill();
	    }
    }
	
    public void kill()
    {
        shapeManager.allShapes.Remove(this);
        shapeManager.shapeUpdate -= this.depthCheck;
        Destroy(gameObject);
		Destroy(shapeSpace);
    }
}
