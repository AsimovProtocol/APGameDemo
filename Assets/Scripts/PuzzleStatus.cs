using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class PuzzleStatus : MonoBehaviour
{
    [SerializeField] private GameObject squarePuzzle;
    [SerializeField] public TileRoom TileRoom;

    private PuzzleStructure PM;
    private Text text;
    public bool generate = true;

    // Start is called before the first frame update
    void Start()
    {
        this.text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.generate)
        {
            this.generate = false;
            this.TileRoom.GenerateSquare();
            this.PM = new PuzzleStructure(genPuzzles());
        }

        this.text.color = PM.Completed ? Color.green : Color.red;
        this.text.text = $"Puzzle Progress: {PM.Progress}/{PM.MaxProgress}";
    }

    private List<Puzzle> genPuzzles()
    {
        int maxPuzzles = TileRoom.GenTiles.Count;
        Random random = new Random();
        List<PuzzlePiece> squarePuzzles = new List<PuzzlePiece>();
        for (int i = 0; i < random.Next(2, maxPuzzles / 3); i++)
        {
            TileMountPosition[] validTiles = TileRoom.GenTiles.Where(t => !t.Used).ToArray();
            GameObject pp = validTiles[random.Next(0, validTiles.Length)].Populate(squarePuzzle);
            squarePuzzles.Add(pp.GetComponent<PuzzlePiece>());
        }

        TileRoom.Populated();

        Puzzle p = new Puzzle(squarePuzzles, squarePuzzles.Count);
        foreach (PuzzlePiece piece in p.Pieces)
        {
            piece.Parent = p;
        }

        return new List<Puzzle>() { p };
    }
}