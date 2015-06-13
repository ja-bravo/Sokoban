/*
 * 
 * 06/03/2015 v0.01 - Jose Antonio Bravo:
 *                                 Hardcoded map
 *                                 
 * 06/03/2015 v0.02 - Jose Antonio Bravo:
 *                                 Removed the hardcoded(array) map and load it 
                                   from a file.
 *                                 Added levelID to the constructor so it know 
                                   which map to load.
 *                                 Added a method to return the list of tiles.
 *                                 
 * 27/03/2015 v0.03 - Jose Antonio Bravo:
 *                                  Start coords are read from the level file.
 *                                  
 * 21/05/2015 - Jose Manuel Gañan Escobar:
 *                                  Added listen to music in the level
 * */

using System;
using System.IO;
using System.Collections.Generic;
class Level
{
    private static List<Tile> tiles;
    private Door[] doors;
    private PressurePlate[] plates;
    private int xStart;
    private int yStart;
    private const int TILESIZE = 40;
    private int maxDoors;

    public static int Count
    {
        get
        {
            return tiles.Count;
        }
    }

    public void ScrollRight()
    {
        foreach(Tile t in tiles)
        {
            t.MoveTo(t.GetX() - 5, t.GetY());
        }
    }

    public void ScrollLeft()
    {
        foreach (Tile t in tiles)
        {
            t.MoveTo(t.GetX() + 5, t.GetY());
        }
    }

    public void ScrollUp()
    {
        foreach (Tile t in tiles)
        {
            t.MoveTo(t.GetX(), t.GetY()+5);
        }
    }

    public void ScrollDown()
    {
        foreach (Tile t in tiles)
        {
            t.MoveTo(t.GetX(), t.GetY() - 5);
        }
    }

    public Level(string levelID)
    {
        tiles = new List<Tile>();
        LoadFile(levelID);
        Link();
    }

    public static Tile GetTile(int i)
    {
        return tiles[i];
    }

    public void Draw()
    {
        // Draw first the non-boxes tiles so the boxes are
        // on top when they move.
        foreach(Tile t in tiles)
        {
            if(t.GetType() != "Box" && t.IsOnScreen())
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
    }

    private void Link()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            Door door = doors[i];
            PressurePlate plate = plates[i];
            if (door != null && plate != null)
            {
                door.AssignPlate(plate);
                tiles.Add(door);
                tiles.Add(plate);
            }
        }
    }

    public static List<Door> GetDoors()
    {
        List<Door> doors = new List<Door>();

        foreach (Tile t in tiles)
        {
            if (t.GetType() == "Door")
            {
                doors.Add((Door)t);
            }
        }

        return doors;
    }

    public static bool HasDoors()
    {
        foreach (Tile t in tiles)
        {
            if (t.GetType() == "Door")
            {
                //player.Stop();
                return true;
            }
        }
        return false;
    }

    public List<WinTile> GetWinTiles()
    {
        List<WinTile> wTiles = new List<WinTile>();

        foreach(Tile t in tiles)
        {
            if(t.GetType() == "WinTile")
            {
                wTiles.Add((WinTile)t);
            }
        }

        return wTiles;
    }

    public List<Box> GetBoxes()
    {
        List<Box> boxes = new List<Box>();

        foreach (Tile t in tiles)
        {
            if (t.GetType() == "Box")
            {
                boxes.Add((Box)t);
            }
        }

        return boxes;
    }

    public List<PressurePlate> GetPlates()
    {
        List<PressurePlate> plates = new List<PressurePlate>();

        foreach (Tile t in tiles)
        {
            if (t.GetType() == "Plate")
            {
                plates.Add((PressurePlate)t);
            }
        }

        return plates;
    }
    public int GetXStart()
    {
        return xStart;
    }

    public int GetYStart()
    {
        return yStart;
    }

    public void ChangeLevel(string levelID)
    {
        tiles.Clear();
        LoadFile(levelID);
        Link();
    }

    private void LoadFile(string levelID)
    {
        try
        {
            StreamReader file = File.OpenText("Data/Levels/" + levelID);

            int width = 0;
            int heightCount = 0;

            string line;
            line = file.ReadLine();
            string[] firstLine = line.Split(' ');

            xStart = Convert.ToInt32(firstLine[0]);
            yStart = Convert.ToInt32(firstLine[1]);
            maxDoors = Convert.ToInt32(firstLine[2]);

            doors = new Door[maxDoors];
            plates = new PressurePlate[maxDoors];
            do
            {
                line = file.ReadLine();

                if (line != null)
                {
                    string[] parts = line.Split(' ');

                    foreach (string part in parts)
                    {
                        int height = heightCount * TILESIZE;
                        if (part == "F")
                        {
                            tiles.Add(new Floor(width * TILESIZE, height));
                        }
                        else if (part == "W")
                        {
                            tiles.Add(new Wall(width * TILESIZE, height));
                        }
                        else if (part == "T")
                        {
                            tiles.Add(new WinTile(width * TILESIZE, height));
                        }
                        else if (part.StartsWith("P"))
                        {
                            // Add the plate to the plates array with the index
                            // indicated on the level file. Ej: P5, the index
                            // would be 5
                            if (maxDoors > 0)
                            {
                                int index = (int)Char.GetNumericValue(part[1]);
                                plates[index] = new PressurePlate(width * TILESIZE, height);
                            }
                            else
                            {
                                tiles.Add(new PressurePlate(width * TILESIZE, height));
                            }
                        }
                        else if (part.StartsWith("D"))
                        {
                            // Add the door to the doors array with the index
                            // indicated on the level file. Ej: D5, the index 
                            // would be 5
                            if(maxDoors > 0)
                            {
                                int index = (int)Char.GetNumericValue(part[1]);
                                doors[index] = new Door(width * TILESIZE, height);
                            }
                            else
                            {
                                tiles.Add(new Door(width * TILESIZE, height));
                            }
                        }
                        else if (part == "B")
                        {
                            /* Add a floor tile so when the box moves
                             * there's something below
                             * */
                            tiles.Add(new Floor(width * TILESIZE, height));
                            tiles.Add(new Box(width * TILESIZE, height));   
                        }
                        width++;
                    }
                    width = 0;
                    heightCount++;
                }
            }
            while (line != null);
            file.Close();
        }
        catch(Exception e)
        {
            SdlHardware.Log("Error at loading the level: " + levelID);
        }
    }
}