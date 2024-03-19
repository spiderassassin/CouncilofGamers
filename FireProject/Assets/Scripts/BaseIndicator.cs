using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BaseIndicator : MonoBehaviour
{

    public GameObject Base;
    public GameObject Player;

    Vector3 screenPos;
    Vector2 onScreenPos;
    float max;
    Camera camera;

    RectTransform rt;
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
        /*
        Vector3 objectsRelativeScreenPos = Base.transform.position; 
        Vector3 direction = (objectsRelativeScreenPos - Player.transform.position).normalized;

        float angleRad = Mathf.Atan2(direction.y, direction.x);
        float angleDeg = angleRad * 57.2957795f;
        transform.localEulerAngles = new Vector3(0, 0, angleDeg);*/
        /*
        Vector3 dir = Base.transform.position - transform.position;
        Vector2 twodimDir = new Vector2(dir.x, dir.z);
        twodimDir.Normalize();
        float angle = Mathf.Atan2(twodimDir.y, twodimDir.x);
        Quaternion originRot = transform.rotation;
        transform.LookAt(Base.transform.position);
        Quaternion lookRot = transform.rotation;*/


        /*
        
        // Get the position of the object in screen space
        Vector3 objScreenPos = Camera.main.WorldToScreenPoint(Base.transform.position);

        // Get the directional vector between your arrow and the object
        Vector3 dir = (objScreenPos - rt.position).normalized;

        // Calculate the angle 
        // We assume the default arrow position at 0° is "up"
        float angle = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(dir, Vector3.down));

        // Use the cross product to determine if the angle is clockwise
        // or anticlockwise
        Vector3 cross = Vector3.Cross(dir, Vector3.up);
        angle = -Mathf.Sign(cross.z) * angle;

        // Update the rotation of your arrow
        rt.localEulerAngles = new Vector3(rt.localEulerAngles.x, rt.localEulerAngles.y, angle);

        Debug.Log("updating angle" + Base.transform.position.ToString());*/




        /*
        screenPos = camera.WorldToViewportPoint(Base.transform.position); //get viewport positions

        if (screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1)
        {
            Debug.Log("already on screen, don't bother with the rest!");
            return;
        }

        onScreenPos = new Vector2(screenPos.x - 0.5f, screenPos.y - 0.5f) * 2; //2D version, new mapping
        max = Mathf.Max(Mathf.Abs(onScreenPos.x), Mathf.Abs(onScreenPos.y)); //get largest offset
        onScreenPos = (onScreenPos / (max * 2)) + new Vector2(0.5f, 0.5f); //undo mapping

        */



    }
}
