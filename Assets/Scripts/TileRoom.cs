using System;
using System.Collections.Generic;
using Classes;
using UnityEngine;

public class TileRoom : MonoBehaviour
{
    [SerializeField] public Vector3 size = Vector3.one;
    [SerializeField] public float angle;
    [SerializeField] private Vector2 doorSize = Vector2.zero;
    [SerializeField] private bool doorFront = true;
    [SerializeField] private bool doorBack;
    [SerializeField] private bool doorLeft;
    [SerializeField] private bool doorRight;
    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject wallTile;
    [SerializeField] private GameObject roofTile;
    [SerializeField] public RoomDoor backDoorTile;
    [SerializeField] private TileRoom thisPrefab;
    [SerializeField] private RoomCompletion roomCompletionPrefab;
    [SerializeField] private bool mainRoom;
    public RoomDoor backDoor;
    public RoomCompletion roomCompletion;
    public TileRoom corridor;
    private readonly float offset = 0.5f;


    public List<TileMountPosition> GenTiles = new();
    private Vector3 pos;
    private ValueTuple<TileRoom, TileRoom> scorridors;
    private ValueTuple<TileRoom, TileRoom> srooms;

    private Transform tr;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    // Generates a tile with the given parameters and adds it to the parent object
    private void GenerateTile(GameObject prefabTile, float x, float y, float z, float tileAngle = 0f)
    {
        var tile = Instantiate(prefabTile, new Vector3(x + pos.x, y + pos.y, z + pos.z), Quaternion.Euler(0, tileAngle, 0));
        tile.transform.parent = tr;
    }

    private void AddGenTile(float x, float y, float z, float tileAngle)
    {
        GenTiles.Add(new TileMountPosition(new Vector3(x + pos.x, y + pos.y + 0.5f, z + pos.z), Quaternion.Euler(0, tileAngle + angle, 0), this));
    }

    // Generates a square room
    public void GenerateSquare()
    {
        tr = GetComponent<Transform>();
        pos = tr.position;
        GenerateFloorSquare();
        GenerateRoofSquare();
        GenerateWallSquareFront(doorFront, false);
        GenerateWallSquareFront(doorBack, true);
        GenerateWallSquareSide(doorLeft, true);
        GenerateWallSquareSide(doorRight, false);
        if (mainRoom)
        {
            if (corridor != null)
            {
                foreach (Transform t in corridor.transform) Destroy(t.gameObject);
                corridor.GenerateSquare();
                corridor.transform.parent = tr;
            }

            if (scorridors.Item1 != null)
            {
                foreach (Transform t in scorridors.Item1.transform) Destroy(t.gameObject);
                scorridors.Item1.GenerateSquare();
                scorridors.Item1.transform.parent = tr;
            }

            if (scorridors.Item2 != null)
            {
                foreach (Transform t in scorridors.Item2.transform) Destroy(t.gameObject);
                scorridors.Item2.GenerateSquare();
                scorridors.Item2.transform.parent = tr;
            }

            if (srooms.Item1 != null)
            {
                foreach (Transform t in srooms.Item1.transform) Destroy(t.gameObject);
                srooms.Item1.GenerateSquare();
                srooms.Item1.transform.parent = tr;
                foreach (var genTile in srooms.Item1.GenTiles) GenTiles.Add(genTile);
            }

            if (srooms.Item2 != null)
            {
                foreach (Transform t in srooms.Item2.transform) Destroy(t.gameObject);
                srooms.Item2.GenerateSquare();
                srooms.Item2.transform.parent = tr;
                foreach (var genTile in srooms.Item2.GenTiles) GenTiles.Add(genTile);
            }
        }
    }

    public void Populated()
    {
        tr.Rotate(Vector3.up, angle);
    }

    // Generates the floor for a square room
    private void GenerateFloorSquare()
    {
        var startz = 0 - size.z / 2;
        for (var z = startz; z < startz + size.z; z++)
        for (var x = 0.5f; x < size.x; x++)
            GenerateTile(floorTile, x, 0, z + offset);
    }

    // Generates the roof for a square room
    private void GenerateRoofSquare()
    {
        var startz = 0 - size.z / 2;
        for (var z = startz; z < startz + size.z; z++)
        for (var x = 0.5f; x < size.x; x++)
            GenerateTile(roofTile, x, size.y, z + offset);
    }

    // Generates the left and right sides for a square room
    private void GenerateWallSquareSide(bool door, bool left)
    {
        var tileAngle = left ? 270 : 90;
        var z = (left ? 1 : -1) * (size.z / 2 - offset);
        for (var x = 0.5f; x < size.x; x++)
        for (float y = 0; y < size.y; y++)
        {
            if (door && !(y >= doorSize.y || x >= doorSize.x / 2 + size.x / 2 || x < doorSize.x / 2 * -1 + size.x / 2))
            {
                if (doorSize.y < 4 && doorSize.x < 4 && y == 2 && x - size.x / 2 + offset == doorSize.x / 2 - 2 && mainRoom)
                {
                    var scorridor = Instantiate(thisPrefab,
                        new Vector3(x + pos.x - offset, y + pos.y - size.y / 2f + offset, z + pos.z - 3.5f * (left ? -1 : 1)),
                        Quaternion.identity);
                    scorridor.size = new Vector3(3, 3, 6);
                    scorridor.angle = angle;
                    scorridor.doorFront = false;
                    scorridor.doorBack = false;
                    scorridor.doorLeft = true;
                    scorridor.doorRight = true;
                    scorridor.doorSize = new Vector2(3, 3);
                    scorridor.mainRoom = false;
                    if (left) scorridors.Item1 = scorridor;
                    else scorridors.Item2 = scorridor;

                    var sroom = Instantiate(thisPrefab,
                        new Vector3(x + pos.x - offset - 4, y + pos.y - size.y / 2f + offset, z + pos.z - 11 * (left ? -1 : 1)),
                        Quaternion.identity);
                    sroom.size = new Vector3(12, 5, 9);
                    sroom.angle = angle;
                    sroom.doorFront = false;
                    sroom.doorBack = false;
                    sroom.doorLeft = !left;
                    sroom.doorRight = left;
                    sroom.doorSize = new Vector2(3, 3);
                    sroom.mainRoom = false;
                    if (left) srooms.Item1 = sroom;
                    else srooms.Item2 = sroom;
                }

                continue;
            }

            ;

            GenerateTile(wallTile, x, y, z, tileAngle);
            if (Math.Floor(x) % 3 == 0 && y == 1 && Math.Floor(x) < size.x - 2 && x > 1)
                AddGenTile(x, y, z, tileAngle);
        }
    }

    // Generates the front and back sides for a square room
    private void GenerateWallSquareFront(bool door, bool back)
    {
        var tileAngle = back ? 0 : 180;
        var x = back ? size.x - 0.5f : 0.5f;
        var startz = 0 - size.z / 2;
        for (var z = startz; z < startz + size.z; z++)
        for (float y = 0; y < size.y; y++)
        {
            if (door && !(y >= doorSize.y || z >= doorSize.x / 2 || z < doorSize.x / 2 * -1))
            {
                if (back && doorSize.y < 4 && doorSize.x < 4 && y == 2 && z == doorSize.x / 2 - 1 && mainRoom)
                {
                    backDoor = Instantiate(backDoorTile, new Vector3(x + pos.x + offset + 0.05f, y + pos.y - offset, z + pos.z - offset), Quaternion.identity);
                    backDoor.transform.parent = tr;
                    var b1 = Instantiate(backDoorTile, new Vector3(x + pos.x + offset + 0.05f + 6, y + pos.y - offset, z + pos.z - offset),
                        Quaternion.identity);
                    b1.transform.parent = tr;
                    roomCompletion = Instantiate(roomCompletionPrefab, new Vector3(x + pos.x + offset + 3.05f, y + pos.y - offset, z + pos.z - offset),
                        Quaternion.identity);
                    roomCompletion.transform.parent = tr;
                    corridor = Instantiate(thisPrefab, new Vector3(x + pos.x + offset, y + pos.y - size.y / 2f + offset, z + pos.z - offset),
                        Quaternion.identity);
                    corridor.size = new Vector3(6, 3, 3);
                    corridor.angle = angle;
                    corridor.doorFront = true;
                    corridor.doorBack = true;
                    corridor.doorLeft = false;
                    corridor.doorRight = false;
                    corridor.doorSize = new Vector2(3, 3);
                    corridor.mainRoom = false;
                }

                continue;
            }

            GenerateTile(wallTile, x, y, z + offset, tileAngle);
            if (Math.Floor(z) % 3 == 0 && y == 1 && Math.Floor(z) < size.z - 2 && z > 1)
                AddGenTile(x, y, z, tileAngle);
        }
    }
}