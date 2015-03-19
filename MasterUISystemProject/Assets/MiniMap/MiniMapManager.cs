using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MiniMapManager : MonoBehaviour {
	
	public Camera miniMapCam;
	public RectTransform compassImage;
	public Text zoomLabel;
	public GameObject miniMapCanvas;
	public RectTransform miniMapRender;
	public RectTransform miniMapMask;
	public float mapProportionOfScreen = 0.2f;
	public bool isMiniActive = false;
	public float orthoCamRadius = 5;

	private bool minimapRotate = false;
	private float orthoCamIncrement = 0.303f;
	private Vector2 relativeClickPostion = Vector2.zero;
	private Vector2 miniMapCenter = Vector2.zero;
	private Vector3 rayStartPostion = Vector2.zero;
	private int mapDim;
	
	void Start()
	{
		SetMiniMapCam ();
		//SetCompassTransform ();
		zoomLabel.text = "Diameter: " + (2 * orthoCamRadius * 3.3f).ToString("F1")  + " ft";
	}

	void Update()
	{
		miniMapCam.transform.position = new Vector3 (POI_ReferenceHub.Instance.Avatar.transform.position.x, POI_ReferenceHub.Instance.Avatar.transform.position.y + 2.6f, POI_ReferenceHub.Instance.Avatar.transform.position.z);
		if (Input.GetKeyDown ("m")) 
		{
			isMiniActive = !isMiniActive;
			miniMapCanvas.SetActive (isMiniActive);
		}
		if (Input.GetKeyDown ("r")) 
		{
			minimapRotate = !minimapRotate;
		}
	
		if (minimapRotate) 
		{
			miniMapCam.transform.eulerAngles = new Vector3 (90, POI_ReferenceHub.Instance.Avatar.transform.eulerAngles.y, 0);
			compassImage.transform.eulerAngles = new Vector3(0,0, POI_ReferenceHub.Instance.Avatar.transform.eulerAngles.y);
		} 
		else 
		{
			miniMapCam.transform.eulerAngles = new Vector3 (90, 0, 0);
			compassImage.transform.eulerAngles = Vector3.zero;
		}
	}

	public void SetRotateTrue(){
		minimapRotate = true;
	}

	public void SetRotatefalse(){
		minimapRotate = false;
	}

	public void Teleport()
	{
		relativeClickPostion.x = (Input.mousePosition.x - miniMapCenter.x) / (0.5f*mapDim);
		relativeClickPostion.y = (Input.mousePosition.y - miniMapCenter.y) / (0.5f*mapDim);
		rayStartPostion = miniMapCam.transform.position + (relativeClickPostion.x*miniMapCam.transform.right + relativeClickPostion.y*miniMapCam.transform.up) * miniMapCam.orthographicSize;
		RaycastHit hit = new RaycastHit();
		Physics.Raycast(rayStartPostion,Vector3.down,out hit, Mathf.Infinity);
		POI_ReferenceHub.Instance.Avatar.transform.position = hit.point;
	}
	
	void SetMiniMapCam()
	{
		miniMapCenter = new Vector2 (miniMapMask.position.x - 0.5f*miniMapMask.sizeDelta.x, miniMapMask.position.y -0.5f*miniMapMask.sizeDelta.y);
		mapDim = (int)(Screen.height * mapProportionOfScreen);
		miniMapMask.sizeDelta = new Vector2 (mapDim, mapDim);
		miniMapRender.sizeDelta = new Vector2 (mapDim, mapDim);
		compassImage.sizeDelta = new Vector2 (mapDim + 10, mapDim + 10);
		compassImage.localPosition = new Vector3 (miniMapMask.localPosition.x - 0.5f*mapDim, miniMapMask.localPosition.y - 0.5f*mapDim, 0);
		miniMapCam.orthographicSize = orthoCamRadius;
	}

	public void ChangeSize(bool isIncrease)
	{
		if (isIncrease) 
		{
			mapProportionOfScreen += 0.05f;
		} 
		else 
		{
			mapProportionOfScreen -= 0.05f;
		}

		if (mapProportionOfScreen < .2f)
						mapProportionOfScreen = .2f;

		if (mapProportionOfScreen > .9f)
						mapProportionOfScreen = .9f;

		SetMiniMapCam ();
	}

	public void ChangeZoom(bool isIncrease)
	{
		if (isIncrease) 
			orthoCamRadius += orthoCamIncrement;
		else
			orthoCamRadius -= orthoCamIncrement;
		
		// This is where we will reset the zoom of the ortho camera that is used to capture the minimap.
		SetMiniMapCam ();
		//SetCompassTransform ();
		
		// set zoom label
		zoomLabel.text = "Diameter: " + (2 * orthoCamRadius * 3.3f).ToString("F1")  + " ft";
	}

}
