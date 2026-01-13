using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp20.Utils;
using System.ComponentModel.Design;

public struct Tile
{
    // 타일 위에 뭐가 올라와있는지?
    public GameObject OnTileObject { get; set; }
    public bool isWall;
    public GameObject FloorObject { get; set; }
    // 타일 위에 올라서면 발생해야 하는 이벤트
    public event Action OnStepPlayer;
    // 자신의 좌표
    public Vector Position { get; set; }
    

    public Tile(Vector position)
    {
        Position = position;
        OnTileObject = null;
        OnStepPlayer = null;
        isWall = false;
        FloorObject = null;
    }

    public void Print()
    {
        if (OnTileObject != null)
        {
            OnTileObject.Symbol.Print();
        }

        else if (FloorObject != null)
        {
            FloorObject.Symbol.Print();
        }

        else
        {
            ' '.Print();
        }
    }
}