using UnityEngine;
using System.Collections;

public class WebCamMaterialRenderer : MonoBehaviour {

	void Start () {
        WebCamTexture webcamTexture = new WebCamTexture();
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }
	
	void Update () {
	
	}
}
