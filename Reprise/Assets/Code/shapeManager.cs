using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class shapeManager : MonoBehaviour 
{
    static public float worldSize = 80;
    public float secondsForRotation = 2;

    public delegate void shapeCheck();
    public static shapeCheck shapeUpdate;

    public static List<Vector2> history = new List<Vector2>();

    public GameObject shapeSpace;
    public GameObject shapeMesh;
    public GameObject collectMesh;
    public GameObject gameCamera;
    public GameObject gameCameraSpace;

    public static List<shapeDropping> allShapes = new List<shapeDropping>();
    public GameObject playerMesh;
    public static GameObject playerShape;
    public static GameObject playerSpace;

    public static int rotationCount = 0;
	
	public static int cameraDistance = 10;
	
	public GameObject dustSphere;
	public GameObject skySphere;
	public Texture2D[] skyBackgrounds;
	public int currentBackground;
	public float backgroundFade;
	
	public Mesh[] prefabPars;
	static public Mesh[] prefabs;
	
	public Mesh[] shapePrefabPars;
	static public Mesh[] shapePrefabs;
	
	

	// Use this for initialization
	void Awake () 
    {
		prefabs = prefabPars;
		shapePrefabs = shapePrefabPars;
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
		if (Time.frameCount % 100 == 0) {
			CreateShape(Vector3.zero, 1);	
		}
		if (previousEulerY < gameCameraSpace.transform.eulerAngles.y && history.Count > 0) {
<<<<<<< HEAD
=======
			playerShape.GetComponent<shapeCreature>().ScaleUp(5);
>>>>>>> origin/master
      		gameCamera.transform.position = new Vector3(0, 0, -worldSize - cameraDistance);
            CreateGhost();
			ChangeBackground();
			playerShape.GetComponent<shapeCreature>().ScaleUp(0.25f);
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
	
    void CreateGhost()
    {
        GameObject temp = Instantiate(shapeMesh) as GameObject;
        temp.GetComponent<shapeDropping>().targetPos = Vector3.zero;
		Debug.Log(temp.GetComponent<shapeDropping>().targetPos);
        temp.transform.localScale = playerShape.transform.localScale;
        temp.transform.localRotation = playerShape.transform.localRotation;
        
		temp.GetComponent<MeshFilter>().mesh = playerShape.gameObject.GetComponent<MeshFilter>().mesh;
				
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
	
    void CreateShape(Vector3 pos, int prefab)
    {
        GameObject temp = Instantiate(collectMesh) as GameObject;
        temp.transform.localPosition = pos;
        temp.transform.localScale = playerShape.transform.localScale;
        temp.transform.localRotation = playerShape.transform.localRotation;

		temp.GetComponent<MeshFilter>().mesh = shapePrefabs[prefab];
				
		temp.transform.parent = gameCameraSpace.transform;
		
    }
}
