using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public Transform player;
    // Late Update is called once per every other frame
    void LateUpdate()
    {
        transform.position = new Vector3(player.position.x,transform.position.y,player.position.z );
    }
}
