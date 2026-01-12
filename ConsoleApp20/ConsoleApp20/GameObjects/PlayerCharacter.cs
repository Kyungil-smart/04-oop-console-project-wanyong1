using ConsoleApp20.GameObjects;
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

    public event Action<string> OnDialogueOpened;
    public event Action OnDialogueClosed;
    public event Action OnStepConsumed;

    public Action OnEnterExit;
    public Func<Vector, Vector, bool> TryPushBlockHandler { get; set; }

    private string _healthGauge;
    private string _manaGauge;

    private const int HUD_X = 1; // 추가
    private const int HUD_Y = 1; // 추가

    private DialogueBox _dialogue;// 대사창


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
        //대사
        _dialogue = new DialogueBox();
        _dialogue.OnOpened += () =>
        {
            IsActiveControl = false;
            // 열릴 때 speaker는 StartDialogue에서 이벤트로 쏴줄 거라 여기선 생략 가능
        };

        _dialogue.OnClosed += () =>
        {
            IsActiveControl = !_inventory.IsActive;
            if (OnDialogueClosed != null) OnDialogueClosed();
        };
    }

    public void Update()
    {
        if (_dialogue != null && _dialogue.IsActive)
        {
            _dialogue.Update();
            return;
        }

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
        //if (InputManager.GetKey(ConsoleKey.A))
        //{
        //    if (_inventory.TrySelectSkill())
        //    {
        //        Mana.Value--;
        //        
        //    }
        //}

        if (InputManager.GetKey(ConsoleKey.Spacebar))
        {
            if(Mana.Value <=0)
            {
                return;
            }

            if (_inventory.TrySelectAttack())
            {
                Attack(1);
            }
            if (_inventory.TrySelectSkill())
            {
                Mana.Value--;

            }
        }
        if (InputManager.GetKey(ConsoleKey.F))
        {
            TryTalk();
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
            return;

        // 2. 벽인지?
        Tile nextTile = Field[nextPos.Y, nextPos.X];
        if (nextTile.isWall) return;

        GameObject nextTileObject = Field[nextPos.Y, nextPos.X].OnTileObject;
        if (nextTileObject != null)
        {

            var exit = nextTileObject as ExitDevice;
            if (exit != null)
            {
                if (exit.IsActive)
                {
                    if (OnEnterExit != null) OnEnterExit();
                }
                return;
            }

            // ✅ 블록이면 “밀기” 먼저 시도
            if (nextTileObject is ConsoleApp20.GameObjects.PushBlock)
            {
                bool pushed = TryPushBlockHandler != null && TryPushBlockHandler(Position, direction);
                if (!pushed) return;

                // 밀었으면, 이제 앞칸이 비었는지 다시 확인
                nextTileObject = Field[nextPos.Y, nextPos.X].OnTileObject;
                if (nextTileObject != null) return;
            }

            // ✅ NPC는 통과 불가
            if (nextTileObject is ITalkable) return;

            // ✅ 아이템 상호작용
            if (nextTileObject is IInteractable)
                (nextTileObject as IInteractable).Interact(this);
            if(nextTileObject is Monster m)
            {
                return;
            }
        }

        Field[Position.Y, Position.X].OnTileObject = null;
        Field[nextPos.Y, nextPos.X].OnTileObject = this;
        Position = nextPos;
        if (OnStepConsumed != null) OnStepConsumed();

    }


    public void Render()
    {
        DrawHealthGauge();
        DrawManaGauge();
        _inventory.Render();
        if (_dialogue != null && Field != null)
        {
            int x = 1;
            int y = Field.GetLength(0) + 1; // 맵(10줄) 아래
            _dialogue.Render(x, y);
        }

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
             case 0:
                _manaGauge = "□□□□□";
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
    public void StartDialogue(string speaker, string[] pages)
    {
        if (_dialogue == null) return;
        if (_inventory.IsActive) return;
        if (_dialogue.IsActive) return;

        if (OnDialogueOpened != null) OnDialogueOpened(speaker);
        _dialogue.Open(speaker, pages);
    }

    private void TryTalk()
    {
        if (Field == null) return;
        if (_inventory.IsActive) return;

        Vector[] dirs = { Vector.Up, Vector.Down, Vector.Left, Vector.Right };
        int h = Field.GetLength(0);
        int w = Field.GetLength(1);

        foreach (var dir in dirs)
        {
            Vector p = Position + dir;
            if (p.X < 0 || p.Y < 0 || p.X >= w || p.Y >= h) continue;

            GameObject obj = Field[p.Y, p.X].OnTileObject;
            if (obj is ITalkable talkable)
            {
                talkable.Talk(this);
                return;
            }
        }
    }



}


