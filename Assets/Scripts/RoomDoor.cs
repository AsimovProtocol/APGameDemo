using UnityEngine;

public class RoomDoor : MonoBehaviour
{
    [SerializeField] private bool open;
    private Collider collider;
    private MeshRenderer mesh;

    // Start is called before the first frame update
    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
        if (open) setOpen();
        else setClosed();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void setOpen()
    {
        open = true;
        collider.enabled = false;
        mesh.enabled = false;
    }

    public void setClosed()
    {
        open = false;
        collider.enabled = true;
        mesh.enabled = true;
    }
}