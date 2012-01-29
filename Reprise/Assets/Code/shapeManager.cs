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
	
	public GameObject dustSphere;
	public GameObject skySphere;
	public Texture2D[] skyBackgrounds;
	public int currentBackground;
	public float backgroundFade;

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
			playerShape.GetComponent<shapeCreature>().ScaleUp(5);
      		gameCamera.transform.position = new Vector3(0, 0, -worldSize - cameraDistance);
            CreateShape();
			ChangeBackground();
		}
		
		if (backgroundFade > 0) {
			Material skyMaterial = skySphere.renderer.material;
			skyMaterial.SetFloat("_Blend", 1.0f - backgroundFade);
			backgroundFade -= Time.deltaTime * 0.25f;
			if (backgroundFade < 0) {
				skyMaterial.SetFloat("_Blend", 1.0f);
			}
		}
	}
	
	void ChangeBackground() {
		backgroundFade = 1.0f;
		Material skyMaterial = skySphere.renderer.material;
		skyMaterial.SetTexture("_TexMat1", skyBackgrounds[currentBackground]);
		currentBackground = (currentBackground + 1) % skyBackgrounds.Length;
		skyMaterial.SetTexture("_TexMat2", skyBackgrounds[currentBackground]);
	}

    void CreateShape()
    {
        GameObject temp = Instantiate(shapeMesh) as GameObject;
        temp.transform.localPosition = playerShape.transform.localPosition;
        temp.transform.localScale = playerShape.transform.localScale;
        temp.transform.localRotation = playerShape.transform.localRotation;
        
		temp.GetComponent<MeshFilter>().mesh = playerShape.gameObject.GetComponentInChildren<MeshFilter>().mesh;
				
		temp.transform.parent = gameCameraSpace.transform;
        temp.GetComponent<shapeDropping>().shapeSpace = playerSpace;
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
