using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    private Puzzle Parent { get; }

    public PuzzlePiece(Puzzle parent)
    {
        Parent = parent;
    }

    public void addProgress()
    {
        Parent.Progress++;
    }

    public void removeProgress()
    {
        Parent.Progress--;
    }
}