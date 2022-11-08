using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class PuzzleStatus : MonoBehaviour
{
    [SerializeField] private GameObject squarePuzzle;
    [SerializeField] public TileRoom TileRoom;
    public bool generate = true;
    public int lastProgress;

    private PuzzleStructure PM;
    private Text text;

    // Start is called before the first frame update
    private void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (generate)
        {
            generate = false;
            TileRoom.GenerateSquare();
            PM = new PuzzleStructure(genPuzzles());
            lastProgress = -1;
        }

        if (lastProgress != PM.Progress)
        {
            UpdateProgress();
            lastProgress = PM.Progress;
        }
    }

    private void UpdateProgress()
    {
        text.color = PM.Completed ? Color.green : Color.red;
        text.text = $"Puzzle Progress: {PM.Progress}/{PM.MaxProgress}";
        if (PM.Completed && TileRoom.backDoor != null) TileRoom.backDoor.setOpen();
    }

    private List<Puzzle> genPuzzles()
    {
        var maxPuzzles = TileRoom.GenTiles.Count;
        var random = new Random();
        var squarePuzzles = new List<PuzzlePiece>();
        for (var i = 0; i < random.Next(maxPuzzles / 4, maxPuzzles / 2); i++)
        {
            var validTiles = TileRoom.GenTiles.Where(t => !t.Used).ToArray();
            var pp = validTiles[random.Next(0, validTiles.Length)].Populate(squarePuzzle);
            squarePuzzles.Add(pp.GetComponent<PuzzlePiece>());
        }

        TileRoom.Populated();

        var p = new Puzzle(squarePuzzles, squarePuzzles.Count);
        foreach (var piece in p.Pieces) piece.Parent = p;

        return new List<Puzzle> { p };
    }
}