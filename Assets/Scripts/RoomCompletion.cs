using UnityEngine;

public class RoomCompletion : MonoBehaviour
{
    private bool popped;
    private PuzzleStatus status;

    // Start is called before the first frame update
    private void Start()
    {
        status = GameObject.Find("PuzzleStatus").GetComponent<PuzzleStatus>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!popped && other.gameObject.layer == 3)
        {
            popped = true;
            var old = status.TileRoom;
            status.TileRoom = Instantiate(old, old.transform.position - new Vector3(0, 0, old.size.z + 6), Quaternion.identity);
            foreach (Transform t in status.TileRoom.transform) Destroy(t.gameObject);
            old.corridor.transform.RotateAround(status.TileRoom.transform.position, Vector3.up, old.angle * -1);
            old.corridor.transform.parent = status.TileRoom.transform;
            Destroy(old.gameObject);
            var b1 = Instantiate(status.TileRoom.backDoorTile, old.transform.position - new Vector3(0, -1.5f, old.size.z), Quaternion.Euler(0, old.angle, 0));
            b1.transform.RotateAround(status.TileRoom.transform.position, Vector3.up, old.angle * -1);
            b1.transform.parent = status.TileRoom.transform;
            status.TileRoom.name = "Room";
            status.generate = true;
        }
    }
}