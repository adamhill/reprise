using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class shapeManager : MonoBehaviour 
{
    public float worldSize = 280;
    public float secondsForRotation = 2;

    public delegate void shapeCheck();
    public static shapeCheck shapeUpdate;

    public static List<Vector2> history = new List<Vector2>();

    public GameObject shapeSpace;
    public GameObject shapeMesh;
    public GameObject gameCamera;
    public GameObject gameCameraSpace;

    public static List<shapeDropping> allShapes = new List<shapeDropping>();
    public GameObject playerMesh;
    public static GameObject playerShape;
    public static GameObject playerSpace;

    public static int rotationCount = 0;
	
	public static int cameraDistance = 20;
	
	public GameObject skySphere;
	public GameObject dustSphere;

	// Use this for initialization
	void Awake () 
    {
        playerSpace = Instantiate(shapeSpace, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        playerShape = Instantiate(playerMesh) as GameObject;
        
        gameCameraSpace = Instantiate(shapeSpace, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        gameCamera.transform.parent = gameCameraSpace.transform;
        gameCamera.transform.position = new Vector3(0, 0, -worldSize - cameraDistance);

        playerShape.transform.parent = gameCameraSpace.transform;
        playerShape.transform.position = new Vector3(0, 0, -worldSize);
		
		skySphere = Instantiate(skySphere, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		skySphere.transform.localScale *= worldSize * 3;
		
		dustSphere = Instantiate(dustSphere, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		dustSphere.transform.localScale *= worldSize * 2;
	}
	
	void Update ()
    {
		float previousEulerY = gameCameraSpace.transform.eulerAngles.y;
		
		gameCameraSpace.transform.Rotate(-Vector3.up * ((worldSize * Time.deltaTime) / secondsForRotation));
        
		if (Time.frameCount % 2 == 0)
        {
            history.Add(playerShape.transform.localPosition);
        }
		if (previousEulerY < gameCameraSpace.transform.eulerAngles.y && history.Count > 0) {
			playerShape.GetComponent<shapeCreature>().ScaleUp(0.2f);
      		gameCamera.transform.position = new Vector3(0, 0, -worldSize - cameraDistance);
            createShape();
		}
	}

    void createShape()
    {
        GameObject objectSpace = Instantiate(shapeSpace, playerSpace.transform.localEulerAngles, Quaternion.identity) as GameObject;
        GameObject temp = Instantiate(shapeMesh) as GameObject;
        temp.transform.localPosition = playerShape.transform.localPosition;
        temp.transform.localScale = playerShape.transform.localScale;
        temp.GetComponent<MeshFilter>().mesh = playerShape.GetComponent<MeshFilter>().mesh;
		temp.transform.parent = objectSpace.transform;
        temp.GetComponent<shapeDropping>().shapeSpace = objectSpace;
        foreach (Vector2 v in history) 
            temp.GetComponent<shapeDropping>().history.Add(v);
 
        temp.GetComponent<shapeDropping>().historyIndex = history.Count;
        history.Clear();
        history = new List<Vector2>();
        
		if (shapeUpdate == null)
			shapeUpdate = new shapeCheck(temp.GetComponent<shapeDropping>().depthCheck);	
		
        shapeUpdate += temp.GetComponent<shapeDropping>().depthCheck;
        allShapes.Add(temp.GetComponent<shapeDropping>());

		shapeUpdate();
        rotationCount++;
    }
}