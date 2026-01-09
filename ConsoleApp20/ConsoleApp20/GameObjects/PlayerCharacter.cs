using ConsoleApp20.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class PlayerCharacter : GameObject
{
    public ObservableProperty<int> Health = new ObservableProperty<int>(5);
    public ObservableProperty<int> Mana = new ObservableProperty<int>(5);
    private string _healthGauge;
    private string _manaGauge;

    private const int HUD_X = 1; // 추가
    private const int HUD_Y = 1; // 추가


    public Tile[,] Field { get; set; }
    private Inventory _inventory;
    public bool IsActiveControl { get; private set; }

    public PlayerCharacter() => Init();

    public void Init()
    {
        Symbol = 'P';
        IsActiveControl = true;
        Health.AddListener(SetHealthGauge);
        Mana.AddListener(SetManaGauge);
        _healthGauge = "■■■■■";
        _manaGauge = "■■■■■";
        _inventory = new Inventory(this);
    }

    public void Update()
    {
        if (InputManager.GetKey(ConsoleKey.I))
        {
            HandleControl();
        }

        if (InputManager.GetKey(ConsoleKey.UpArrow))
        {
            Move(Vector.Up);
            _inventory.SelectUp();
        }

        if (InputManager.GetKey(ConsoleKey.DownArrow))
        {
            Move(Vector.Down);
            _inventory.SelectDown();
        }

        if (InputManager.GetKey(ConsoleKey.LeftArrow))
        {
            Move(Vector.Left);
        }

        if (InputManager.GetKey(ConsoleKey.RightArrow))
        {
            Move(Vector.Right);
        }

        if (InputManager.GetKey(ConsoleKey.Enter))
        {
            _inventory.Select();
        }

        if (InputManager.GetKey(ConsoleKey.T))
        {
            Health.Value--;
        }
        if (InputManager.GetKey(ConsoleKey.Spacebar))
        {
            Attack(1);

        }
        if (InputManager.GetKey(ConsoleKey.A))
        {
            Mana.Value--;

        }
    }

    public void HandleControl()
    {
        _inventory.IsActive = !_inventory.IsActive;
        IsActiveControl = !_inventory.IsActive;
        Debug.LogWarning($"{_inventory._itemMenu.CurrentIndex}");
    }

    private void Move(Vector direction)
    {
        if (Field == null || !IsActiveControl) return;

        Vector current = Position;
        Vector nextPos = Position + direction;

        // 1. 맵 바깥은 아닌지?
        if (nextPos.X < 0 || nextPos.Y < 0 || nextPos.X >= Field.GetLength(1) || nextPos.Y >= Field.GetLength(0))
        {
            return;
        }
        // 2. 벽인지?
        Tile nextTile = Field[nextPos.Y, nextPos.X];

        if (nextTile.isWall)
        {
            return;
        }

        GameObject nextTileObject = Field[nextPos.Y, nextPos.X].OnTileObject;

        if (nextTileObject != null)
        {
            if (nextTileObject is IInteractable)
            {
                (nextTileObject as IInteractable).Interact(this);
            }
        }

        Field[Position.Y, Position.X].OnTileObject = null;
        Field[nextPos.Y, nextPos.X].OnTileObject = this;
        Position = nextPos;
    }

    public void Render()
    {
        DrawHealthGauge();
        DrawManaGauge();
        _inventory.Render();

    }

    public void AddItem(Item item)
    {
        _inventory.Add(item);
    }

    public void DrawManaGauge()
    {
        //Console.SetCursorPosition(Position.X - 2, Position.Y - 1);
        //_healthGauge.Print(ConsoleColor.Blue);
        int x = 0;
        int y = Console.WindowHeight - 1;

        if (x < 0 || y < 0 ||
            x >= Console.WindowWidth ||
            y >= Console.WindowHeight)
            return;

        Console.SetCursorPosition(x, y);
        "MP ".Print(ConsoleColor.White);
        _manaGauge.Print(ConsoleColor.Blue);
    }

    public void DrawHealthGauge()
    {
        //Console.SetCursorPosition(Position.X - 2, Position.Y - 2);
        //_healthGauge.Print(ConsoleColor.Red);
        int x = 0;
        int y = Console.WindowHeight - 2;

        if (x < 0 || y < 0 ||
            x >= Console.WindowWidth ||
            y >= Console.WindowHeight)
            return;

        Console.SetCursorPosition(x, y);
        "HP ".Print(ConsoleColor.White);
        _healthGauge.Print(ConsoleColor.Red);
    }

    public void SetHealthGauge(int health)
    {
        switch (health)
        {
            case 5:
                _healthGauge = "■■■■■";
                break;
            case 4:
                _healthGauge = "■■■■□";
                break;
            case 3:
                _healthGauge = "■■■□□";
                break;
            case 2:
                _healthGauge = "■■□□□";
                break;
            case 1:
                _healthGauge = "■□□□□";
                break;
        }
    }

    public void SetManaGauge(int mana)
    {
        switch (mana)
        {
            case 5:
                _manaGauge = "■■■■■";
                break;
            case 4:
                _manaGauge = "■■■■□";
                break;
            case 3:
                _manaGauge = "■■■□□";
                break;
            case 2:
                _manaGauge = "■■□□□";
                break;
            case 1:
                _manaGauge = "■□□□□";
                break;
        }
    }

    public void Heal(int value)
    {
        Health.Value += value;
    }
    public void ManaHeal(int value)
    {
        Mana.Value += value;
    }
    public void Attack(int damage)
    {
        if (Field == null) return;
        List<(int x, int y)> redraw = new List<(int, int)>();
        int height = Field.GetLength(0);
        int width = Field.GetLength(1);

        for (int dy = -1; dy <= 1; dy++)
        {
            
            for (int dx = -1; dx <= 1; dx++)
            {
                
                if (dx == 0 && dy == 0) continue;

                int tx = Position.X + dx;
                int ty = Position.Y + dy;

                if (tx < 0 || tx >= width || ty < 0 || ty >= height) continue;

                redraw.Add((tx, ty));
                Console.SetCursorPosition(tx, ty);
                '*'.Print();

                var target = Field[ty, tx].OnTileObject;
                
                if (target is IDamageable d)
                {
                    d.TakeDamage(damage); //
                }
            }
        }
        Thread.Sleep(200);
    }

    
}


