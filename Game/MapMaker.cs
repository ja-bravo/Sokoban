using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

class MapMaker
{
    private static MapMaker mapMaker;
    private List<Tile> tiles;
    private Dictionary<string,string> types;
    private Sprite cursor;
    private Sprite player;
    private Font font16;
    private string tileType;
    private string levelName;
    private string errorMessage;
    private int boxes;
    private int winningTiles;
    private bool showError;

    private MapMaker()
    {
        tiles = new List<Tile>();
        types = new Dictionary<string, string>();
        
        tileType = "Floor";
        cursor = new Sprite("Data/Images/cursor.png");
        cursor.MoveTo(0, 0);

        player = null;
        boxes = 0;
        winningTiles = 0;

        font16 = new Font("Data/Fonts/kenvector_future.ttf",16);
        showError = false;

        levelName = "";
        errorMessage = "";
        Load();
    }

    private void Load()
    {
        types.Add("Floor", "floor.png");
        types.Add("Door", "door.png");
        types.Add("Box", "box.png");
        types.Add("Plate", "plate.png");
        types.Add("WinTile", "winFloor.png");
        types.Add("Wall", "wall.png");
        types.Add("Player", "player.png");
    }

    public static MapMaker GetInstance()
    {
        if(mapMaker == null)
        {
            mapMaker = new MapMaker();
        }
        return mapMaker;
    }

    public void Run()
    {
        do
        {
            SdlHardware.ClearScreen();
            Draw();

            if (SdlHardware.KeyPressed(SdlHardware.KEY_F1))
            {
                if(boxes == winningTiles && boxes > 0)
                {
                    // TODO: Check for errors
                    Save();

                    do
                    {
                        SdlHardware.WriteHiddenText("Saved at: "+GetLevelName(),
                                                300, 200, 255, 255, 104, font16);

                        SdlHardware.WriteHiddenText("Press space key to continue",
                                                300, 232, 255, 255, 104, font16);

                        SdlHardware.ShowHiddenScreen();
                        Thread.Sleep(40);
                    }
                    while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC));
                    Thread.Sleep(250);
                }
                else
                {
                    errorMessage = "Not enough boxes/winning tiles.";
                    showError = true;
                }
            }

            if (SdlHardware.KeyPressed(SdlHardware.KEY_R))
            {
                tiles.Clear();
            }

            if (SdlHardware.KeyPressed(SdlHardware.KEY_1))
            {
                tileType = "Floor";
                showError = false;
            }

            if (SdlHardware.KeyPressed(SdlHardware.KEY_2))
            {
                tileType = "Wall";
                showError = false;
            }

            if (SdlHardware.KeyPressed(SdlHardware.KEY_3))
            {
                tileType = "WinTile";
                showError = false;
            }

            if (SdlHardware.KeyPressed(SdlHardware.KEY_4))
            {
                tileType = "Box";
                showError = false;
            }

            if (SdlHardware.KeyPressed(SdlHardware.KEY_5))
            {
                tileType = "Door";
                errorMessage = "Not supported yet.";
                showError = true;
            }

            if (SdlHardware.KeyPressed(SdlHardware.KEY_6))
            {
                tileType = "Plate";
                errorMessage = "Not supported yet.";
                showError = true;
            }

            if (SdlHardware.KeyPressed(SdlHardware.KEY_7))
            {
                tileType = "Player";
                showError = false;
            }

            if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
            {
                Spawn(tileType);
            }

            if (SdlHardware.KeyPressed(SdlHardware.KEY_X))
            {
                Remove();
            }

            if (SdlHardware.KeyPressed(SdlHardware.KEY_D) 
                && cursor.GetX() <= 1024 - 40)
            {
                cursor.SetX(cursor.GetX() + 40);
            }

            if (SdlHardware.KeyPressed(SdlHardware.KEY_A) 
                && cursor.GetX() >= 40)
            {
                cursor.SetX(cursor.GetX() - 40);
            }

            if (SdlHardware.KeyPressed(SdlHardware.KEY_W) 
                && cursor.GetY() >= 40)
            {
                cursor.SetY(cursor.GetY() - 40);
            }

            if (SdlHardware.KeyPressed(SdlHardware.KEY_S) 
                && cursor.GetY() <= 736 - 40)
            {
                cursor.SetY(cursor.GetY() + 40);
            }

            SdlHardware.ShowHiddenScreen();
            Thread.Sleep(90);
        }
        while (!SdlHardware.KeyPressed(SdlHardware.KEY_ESC));
    }

    private void Remove()
    {
        for (int i = 0; i < tiles.Count; i++ )
        {
            Tile t = tiles[i];
            if (t.GetX() == cursor.GetX() &&
                t.GetY() == cursor.GetY())
            {
                if(t.GetType() == "Box")
                {
                    boxes--;
                }
                if (t.GetType() == "WinTile")
                {
                    winningTiles--;
                }
                tiles.RemoveAt(i);
            }
        }
    }

    private void Draw()
    {
        SdlHardware.DrawHiddenImage(new Image("Data/Images/" + types[tileType]),
                                                                        984, 0);
        // Draw first the non-boxes tiles so the boxes are
        // on top when they move.
        foreach (Tile t in tiles)
        {
            if (t.GetType() != "Box" && t.IsOnScreen())
            {
                t.DrawOnHiddenScreen();
            }
        }

        foreach (Tile t in tiles)
        {
            if (t.GetType() == "Box" && t.IsOnScreen())
            {
                t.DrawOnHiddenScreen();
            }
        }

        if(player != null)
        {
            player.DrawOnHiddenScreen();
        }

        if(showError)
        {
            SdlHardware.WriteHiddenText(errorMessage,
                                                300, 200, 255, 255, 104, font16);
        }

        SdlHardware.WriteHiddenText("Press 1-7 to cycle through the tiles.",
                                                50, 2, 255, 255, 104, font16);
        SdlHardware.WriteHiddenText("Press R to reload the map, F1 to save it and ESC to exit.",
                                                50, 18, 255, 255, 104, font16);
        SdlHardware.WriteHiddenText("Press space to add a tile and X to remove.",
                                                50, 32, 255, 255, 104, font16);

        cursor.DrawOnHiddenScreen();
    }
    private void Spawn(string type)
    {
        // Remove the tile that was below the cursor
        for (int i = 0; i < tiles.Count; i++ )
        {
            Tile t = tiles[i];
            if (t.GetX() == cursor.GetX() &&
               t.GetY() == cursor.GetY())
            {
                if (t.GetType() == "Box")
                {
                    boxes--;
                }
                if (t.GetType() == "WinTile")
                {
                    winningTiles--;
                }

                tiles.RemoveAt(i);
            }
        }

        if(showError && errorMessage != "Not supported yet.")
        {
            showError = false;
        }

        switch (type)
        {
            case "Player":
                if(player == null)
                {
                    player = new Sprite("Data/Images/player.png");
                    player.MoveTo(cursor.GetX(), cursor.GetY());
                }
                else
                {
                    player.MoveTo(cursor.GetX(), cursor.GetY());
                }
                tiles.Add(new Floor(cursor.GetX(), cursor.GetY()));
                break;

            case "Box":
                tiles.Add(new Box(cursor.GetX(), cursor.GetY()));
                boxes++;
                break;

            case "Door":
                //tiles.Add(new Door(cursor.GetX(), cursor.GetY()));
                break;

            case "Plate":
                //tiles.Add(new PressurePlate(cursor.GetX(), cursor.GetY()));
                break;

            case "Wall":
                tiles.Add(new Wall(cursor.GetX(), cursor.GetY()));
                break;

            case "Floor":
                tiles.Add(new Floor(cursor.GetX(), cursor.GetY()));
                break;

            case "WinTile":
                tiles.Add(new WinTile(cursor.GetX(), cursor.GetY()));
                winningTiles++;
                break;
        }
    }

    private void Save()
    {
        int maxHeight = 1;
        int maxWidth = 1;
        foreach(Tile t in tiles)
        {
            if(t.GetY() > maxHeight)
            {
                maxHeight = t.GetY();
            }

            if (t.GetX() > maxWidth)
            {
                maxWidth = t.GetX();
            }
        }

        maxHeight /= 40;
        maxWidth /= 40;

        maxHeight++;
        maxWidth++;

        string[,] map = new string[maxHeight, maxWidth];

        foreach(Tile t in tiles)
        {
            int posX = t.GetX() / 40;
            int posY = t.GetY() / 40;
            string character = "";

            switch (t.GetType())
            {
                case "Box":
                    character = "B";
                    break;

                case "Door":
                    character = "D"; //TODO: LINK WITH PLATE
                    break;

                case "Plate":
                    character = "P"; //TODO: LINK WITH DOOR
                    break;

                case "Wall":
                    character = "W";
                    break;

                case "Floor":
                    character = "F";
                    break;

                case "WinTile":
                    character = "T";
                    break;
            }

            map[posY, posX] = character;

            for(int i = 0; i < maxHeight; i++)
            {
                for(int j = 0; j < maxWidth; j++)
                {
                    if(map[i,j] == null)
                    {
                        map[i, j] = "0";
                    }
                }
            }
        }

        string levelName = GetLevelName();
        StreamWriter sw = new StreamWriter(levelName);
        
        if(player != null)
        {
            // TODO: Get number of doors.
            sw.WriteLine(player.GetX()+" "+player.GetY()+" 0");
        }
        else
        {
            // TODO: Get number of doors.
            sw.WriteLine("0 0 0");
        }

        string line = "";
        for (int i = 0; i < maxHeight; i++)
        {
            for (int j = 0; j < maxWidth; j++)
            {
                line += map[i, j] + " ";
            }
            sw.WriteLine(line);
            line = "";
        }

        sw.Close();
    }

    private string GetLevelName()
    {
        if(levelName == "")
        {
            List<string> levels = FileManager.GetLevels();
            levelName = "Data/levels/level" + (levels.Count + 1);
        
        }

        return levelName;
    }
}