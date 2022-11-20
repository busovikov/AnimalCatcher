using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform objectToFollow;
    public float minimalDistance;
    public float speed;
    private void Update()
    {
        if (objectToFollow)
        {
            Vector3 direction = ClampY(objectToFollow.transform.position - transform.position);
            TurnByDirection(direction);
            if (direction.sqrMagnitude > minimalDistance * minimalDistance)
            {
                transform.position += direction.normalized  * speed * Time.deltaTime ;
            }
        }
    }

    public Vector3 ClampY(Vector3 vec)
    {
        return new Vector3(vec.x, 0f, vec.z);
    }
    public void TurnByDirection(Vector3 direction)
    {
        var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }
}
