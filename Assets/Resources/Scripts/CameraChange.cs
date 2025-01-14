using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraChange : MonoBehaviour
{
    // Start is called before the first frame update

    public CinemachineVirtualCamera followCamera;
    public CinemachineFreeLook freeCamera;

    public void CameraSwitch(bool takeOver)
    {
        if (takeOver)
        {
            followCamera.enabled = true;
            freeCamera.enabled = false;
        }
        else
        {
            followCamera.enabled = false;
            freeCamera.enabled = true;
        }
    }

    public void CameraFollow(Transform follower, Transform target, Vector3 offset)
    {
        if (follower == null)
            follower = followCamera.transform;
        //var follower = followCamera.transform;
        float distance = Vector3.Distance(follower.position, target.position);
        follower.position = Vector3.MoveTowards(follower.position, target.position + offset, Time.deltaTime * 100);
        
        var direction = target.position - follower.position;
        var rotation = Quaternion.LookRotation(direction);
        follower.rotation = Quaternion.LookRotation(target.forward);
    }

    void Start()
    {
        CameraSwitch(false);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
