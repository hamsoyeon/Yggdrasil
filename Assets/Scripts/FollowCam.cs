using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{


	public GameObject Target;
    [SerializeField]
	private Vector3 camOffset;

    // Start is called before the first frame update
    void Start()
    {
		//Target = GameObject.Find("Player");
		//camOffset = new Vector3(0,45, -26.5f);

	}

    // Update is called once per frame
    void Update()
    {
        transform.position = Target.transform.position+camOffset;
    }
}
