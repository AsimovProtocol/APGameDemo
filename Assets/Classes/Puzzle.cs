using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle
{
    public List<PuzzlePiece> Pieces { get; }
    public int Progress { get; set; }
    public int MaxProgress { get; }

    public bool Solved => Progress >= MaxProgress;

    public Puzzle(List<PuzzlePiece> pieces, int maxProgress)
    {
        Pieces = pieces;
        MaxProgress = maxProgress;
    }
}