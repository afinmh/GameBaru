using UnityEngine;

public class SimpleTeleport : MonoBehaviour
{
    public Transform player;       // Drag player ke sini
    public Transform pointA;       // Titik awal
    public Transform pointB;       // Titik tujuan

    private bool atPointA = true;  // Status posisi sekarang

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (atPointA)
            {
                player.position = pointB.position;
            }
            else
            {
                player.position = pointA.position;
            }

            atPointA = !atPointA; // toggle status
            Debug.Log("Teleported to " + (atPointA ? "Point A" : "Point B"));
        }
    }
}
