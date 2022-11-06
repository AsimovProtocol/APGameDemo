using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraLooking : MonoBehaviour
{
    [SerializeField] public GameObject looking = null;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit, 5))
        {
            this.looking = hit.collider.gameObject;
        }
        else
        {
            this.looking = null;
        }
    }

    public bool IsLooking(GameObject gameObject1)
    {
        return this.looking != null && this.looking.GetInstanceID() == gameObject1.GetInstanceID();
    }
}