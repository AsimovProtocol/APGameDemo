using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleStructure
{
    private List<Puzzle> Puzzles { get; }

    public int Progress => Puzzles.Sum(p => p.Progress);
    public int MaxProgress => Puzzles.Sum(p => p.MaxProgress);
    public bool Completed => Puzzles.All(p => p.Solved);

    public PuzzleStructure(List<Puzzle> puzzles)
    {
        Puzzles = puzzles;
    }
}