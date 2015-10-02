using UnityEngine;
using System.Collections;

public class CloudCrafter : MonoBehaviour {

	//fields set in the Unity Inspector pane
	public int numClouds = 40; // the number of clouds to make
	public GameObject [] cloudPrefabs; // the prefabs for the clouds
	public Vector3 cloudPosMin; // min position of each cloud
	public Vector3 cloudPosMax; // max position of each cloud
	public float cloudScaleMin = 1; // min scale of each cloud
	public float cloudScaleMax = 5; // max scale of each cloud
	public float cloudSpeedMult = 0.5f; //adjusts speed of clouds

	public bool _________________;

	//fields set dynamically
	public GameObject[] cloudInstances;

	void Awake(){
		//make an array large enough to hold all the Cloud_Instances
		cloudInstances = new GameObject[numClouds];
		//find the CloudAnchor parent GameObject
		GameObject anchor = GameObject.Find ("CloudAnchor");
		//iterate through and make Cloud_s
		GameObject cloud;
		for (int i=0; i<numClouds; i++) {
			//pick an int between 0 and cloudPrefabs.Length - 1
			//Random.Range will not ever pick as high as the top number
			int prefabNum = Random.Range (0, cloudPrefabs.Length);
			//make an instance
			cloud = Instantiate (cloudPrefabs [prefabNum]) as GameObject;
			//position cloud
			Vector3 cPos = Vector3.zero;
			cPos.x = Random.Range (cloudPosMin.x, cloudPosMax.x);
			cPos.y = Random.Range (cloudPosMin.y, cloudPosMax.y);
			//scale cloud
			float scaleU = Random.value;
			float scaleVal = Mathf.Lerp (cloudScaleMin, cloudScaleMax, scaleU);
			//smaller clouds (with smaller scaleU) should be nearer the ground)
			cPos.y = Mathf.Lerp (cloudPosMin.y, cPos.y, scaleU);
			//smaller cloud should be further away
			cPos.z = 100 - 90 * scaleU;
			//Apply these transforms to the cloud
			cloud.transform.position = cPos;
			cloud.transform.localScale = Vector3.one * scaleVal;
			//Make coud a child of the anchor
			cloud.transform.parent = anchor.transform;
			//add the cloud to cloudInstances
			cloudInstances [i] = cloud; 
		}
	}

	void Update(){
		//Iterate over each cloud that was created
		foreach (GameObject cloud in cloudInstances){
			//get the cloud scale and position
			float scaleVal = cloud.transform.localScale.x;
			Vector3 cPos = cloud.transform.position;
			//move larger clouds faster
			cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
			//if a cloud has mved too far left
			if (cPos.x <= cloudPosMin.x){
				//move it to the far right
				cPos.x = cloudPosMax.x;
			}
			//apply the new position to cloud
			cloud.transform.position = cPos;
		}
	}
}
