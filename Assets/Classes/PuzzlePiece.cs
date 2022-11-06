using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public Puzzle Parent { get; set; }

    public PuzzlePiece(Puzzle parent)
    {
        Parent = parent;
    }

    protected PuzzlePiece()
    {
        Parent = null;
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