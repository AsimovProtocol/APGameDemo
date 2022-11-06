using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SquarePuzzle : PuzzlePiece
{
    [SerializeField] private SquarePuzzleTile field00;
    [SerializeField] private SquarePuzzleTile field10;
    [SerializeField] private SquarePuzzleTile field20;
    [SerializeField] private SquarePuzzleTile field01;
    [SerializeField] private SquarePuzzleTile field11;
    [SerializeField] private SquarePuzzleTile field21;
    [SerializeField] private SquarePuzzleTile field02;
    [SerializeField] private SquarePuzzleTile field12;
    [SerializeField] private SquarePuzzleTile field22;

    private bool solved = false;

    // Start is called before the first frame update
    void Start()
    {
        SquarePuzzleTile[] tiles = { field00, field10, field20, field01, field11, field21, field02, field12, field22 };
        foreach (SquarePuzzleTile tile in tiles)
        {
            tile.init();
        }

        Random random = new Random();
        while (CheckSolved())
            foreach (SquarePuzzleTile tile in tiles)
            {
                if (random.Next(0, 2) == 0) tile.SilentClick();
            }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private bool CheckSolved()
    {
        bool on1 = field00.on;
        return on1 == field10.on && on1 == field20.on && on1 == field01.on &&
               on1 == field11.on && on1 == field21.on && on1 == field02.on &&
               on1 == field12.on && on1 == field22.on;
    }

    public bool PieceClicked(SquarePuzzleTile tile)
    {
        if (solved) return false;

        switch (tile.name)
        {
            case "f00":
                field10.SilentClick();
                field01.SilentClick();
                break;
            case "f10":
                field00.SilentClick();
                field11.SilentClick();
                field20.SilentClick();
                break;
            case "f20":
                field10.SilentClick();
                field21.SilentClick();
                break;
            case "f01":
                field00.SilentClick();
                field11.SilentClick();
                field02.SilentClick();
                break;
            case "f11":
                field01.SilentClick();
                field10.SilentClick();
                field21.SilentClick();
                field12.SilentClick();
                break;
            case "f21":
                field20.SilentClick();
                field11.SilentClick();
                field22.SilentClick();
                break;
            case "f02":
                field01.SilentClick();
                field12.SilentClick();
                break;
            case "f12":
                field02.SilentClick();
                field11.SilentClick();
                field22.SilentClick();
                break;
            case "f22":
                field12.SilentClick();
                field21.SilentClick();
                break;
            default:
                return false;
        }

        tile.SilentClick();

        this.solved = CheckSolved();
        if (this.solved) this.addProgress();
        return true;
    }
}