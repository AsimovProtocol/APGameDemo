using System;
using System.Collections;
using System.Collections.Generic;
using Classes;
using UnityEngine;
using UnityEngine.UIElements;

public class TileRoom : MonoBehaviour
{
    [SerializeField] private Vector3 size = Vector3.one;
    [SerializeField] private float angle = 0;
    [SerializeField] private Vector2 doorSize = Vector2.zero;
    [SerializeField] private bool doorFront = true;
    [SerializeField] private bool doorBack = false;
    [SerializeField] private bool doorLeft = false;
    [SerializeField] private bool doorRight = false;
    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject wallTile;
    [SerializeField] private GameObject roofTile;

    public List<TileMountPosition> GenTiles = new();

    private Transform tr;
    private Vector3 pos;
    private float offset = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        pos = tr.position;
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Generates a tile with the given parameters and adds it to the parent object
    void GenerateTile(GameObject prefabTile, float x, float y, float z, float tileAngle = 0f)
    {
        GameObject tile = Instantiate(prefabTile, new Vector3(x + pos.x, y + pos.y, z + pos.z), Quaternion.Euler(0, tileAngle, 0));
        tile.transform.parent = tr;
    }

    void AddGenTile(float x, float y, float z, float tileAngle)
    {
        GenTiles.Add(new TileMountPosition(new Vector3(x + pos.x, y + pos.y + 0.5f, z + pos.z), Quaternion.Euler(0, tileAngle + angle, 0), this));
    }

    // Generates a square room
    public void GenerateSquare()
    {
        GenerateFloorSquare();
        GenerateRoofSquare();
        GenerateWallSquareFront(doorFront, false);
        GenerateWallSquareFront(doorBack, true);
        GenerateWallSquareSide(doorLeft, true);
        GenerateWallSquareSide(doorRight, false);
    }

    public void Populated()
    {
        tr.Rotate(Vector3.up, angle);
    }

    // Generates the floor for a square room
    void GenerateFloorSquare()
    {
        float startz = 0 - size.z / 2;
        for (float z = startz; z < startz + size.z; z++)
        {
            for (float x = 0.5f; x < size.x; x++)
            {
                GenerateTile(floorTile, x, 0, z + offset);
            }
        }
    }

    // Generates the roof for a square room
    void GenerateRoofSquare()
    {
        float startz = 0 - size.z / 2;
        for (float z = startz; z < startz + size.z; z++)
        {
            for (float x = 0.5f; x < size.x; x++)
            {
                GenerateTile(roofTile, x, size.y, z + offset);
            }
        }
    }

    // Generates the left and right sides for a square room
    void GenerateWallSquareSide(bool door, bool left)
    {
        int tileAngle = left ? 270 : 90;
        float z = (left ? 1 : -1) * (size.z / 2 - offset);
        for (float x = 0.5f; x < size.x; x++)
        {
            for (float y = 0; y < size.y; y++)
            {
                if (door && !(y >= doorSize.y || (x >= doorSize.x / 2 + (size.x / 2) || x < doorSize.x / 2 * -1 + (size.x / 2))))
                {
                    continue;
                }

                GenerateTile(wallTile, x, y, z, tileAngle);
                if (Math.Floor(x) % 3 == 0 && y == 1 && Math.Floor(x) < size.x - 2)
                    AddGenTile(x, y, z, tileAngle);
            }
        }
    }

    // Generates the front and back sides for a square room
    void GenerateWallSquareFront(bool door, bool back)
    {
        int tileAngle = back ? 0 : 180;
        float x = (back ? size.x - 0.5f : 0.5f);
        float startz = 0 - size.z / 2;
        for (float z = startz; z < startz + size.z; z++)
        {
            for (float y = 0; y < size.y; y++)
            {
                if (door && !(y >= doorSize.y || (z >= doorSize.x / 2 || z < doorSize.x / 2 * -1)))
                {
                    continue;
                }

                GenerateTile(wallTile, x, y, z + offset, tileAngle);
                if (Math.Floor(z) % 3 == 0 && y == 1 && Math.Floor(z) < size.z - 2)
                    AddGenTile(x, y, z, tileAngle);
            }
        }
    }
}