/*
 * 
 * 06/03/2015 v0.01 - Jose Antonio Bravo:
 *                                  Player can move.
 *                                  
 * 06/03/2015 v0.02 - Jose Antonio Bravo:     
 *                                  Added x and y to constructor, values will be
                                    read from the level file.
 *                                  Check if the player collisions with another 
                                    sprite before moving.
 *                                  Use Level.GetTiles() to check if it 
                                    collisions with a tile.
 *                                  
 * 27/03/2015 v0.03 - Jose Antonio Bravo:
 *                                  Improved player collision.
 *                                  Start coords are read from the level file.
 * */

using System.Collections.Generic;
class Player : Sprite
{
    private Box adjacentBox;

    public int Steps { get; set; }
    public int Pushes { get; set; }

    public int InitialSpeed { get; set; }
    public bool HasPowerUp { get; set; }
    public bool CanSwap { get; set; }
    public PowerUp.PowUpType ActivePowerUP { get; set; }

    public Player(int x, int y)
    {
        this.x = x;
        this.y = y;

        InitialSpeed = 5;
        xSpeed = InitialSpeed;
        ySpeed = InitialSpeed;
        height = 35;
        width = 35;
        HasPowerUp = false;

        Steps = 0;
        Pushes = 0;
        LoadImage("Data/Images/player.png");
    }

    public void Draw()
    {
        DrawOnHiddenScreen();
    }

    public void Swap()
    {
        int oldX = x;
        int oldY = y;

        if (!CanMove("right"))
        {
            oldX -= 5;
        }

        if (!CanMove("down"))
        {
            oldY -= 2;
        }

        x = adjacentBox.GetX();
        y = adjacentBox.GetY();


        adjacentBox.MoveTo(oldX, oldY-2);
        HasPowerUp = false;
        CanSwap = false;
    }

    public void Reload(int x, int y)
    {
        MoveTo(x, y);
        Steps = 0;
        Pushes = 0;
        CanSwap = false;
        HasPowerUp = false;
    }

    public void PushBox(string dir)
    {
        Pushes++;

        adjacentBox.SetSpeed(GetSpeedX(), GetSpeedY());
        adjacentBox.Move(dir);
        Move(dir);
    }

    public bool CanMove(string dir)
    {
        for (int i = 0; i < Level.Count; i++)
        {
            Tile t = Level.GetTile(i);
            if (this.CollisionsWith(t, dir) && 
                t.Type == Tile.types.NOWALKABLE)
            {
                return false;
            }
        }
        return true;
    }

    public bool CollisionsWithBox(string dir)
    {
        for (int i = 0; i < Level.Count; i++)
        {
            Tile tile = Level.GetTile(i);

            if (CollisionsWith(tile, dir) && tile.GetType() == "Box")
            {
                adjacentBox = (Box)tile;
                return true;
            }
        }
        return false;
    }

    private bool CollisionsWith(Sprite otherSprite, string dir)
    {
        return (visible && otherSprite.IsVisible() &&
            CollisionsWith(otherSprite.GetX(),
                otherSprite.GetY(),
                otherSprite.GetX() + otherSprite.GetWidth(),
                otherSprite.GetY() + otherSprite.GetHeight(), dir));
    }

    private bool CollisionsWith(int xStart, int yStart, int xEnd, int yEnd,
                                string dir)
    {
        if (dir == "right")
        {
            if (visible &&
                (x + xSpeed < xEnd) &&
                (x + width + xSpeed > xStart) &&
                (y < yEnd) &&
                (y + height > yStart)
                )
                return true;
        }

        if (dir == "left")
        {
            if (visible &&
                (x - xSpeed < xEnd) &&
                (x + width - xSpeed > xStart) &&
                (y < yEnd) &&
                (y + height > yStart)
                )
                return true;
        }

        if (dir == "up")
        {
            if (visible &&
                (x < xEnd) &&
                (x + width > xStart) &&
                (y - ySpeed < yEnd) &&
                (y - ySpeed + height > yStart)
                )
                return true;
        }

        if (dir == "down")
        {
            if (visible &&
                (x < xEnd) &&
                (x + width > xStart) &&
                (y + ySpeed < yEnd) &&
                (y + height + ySpeed > yStart))
                return true;
        }
        return false;


    }

    private void Move(string dir)
    {
        switch (dir)
        {
            case "left":
                if (CanMove("left"))
                {
                    MoveLeft();
                }
                break;

            case "right":
                if (CanMove("right"))
                {
                    MoveRight();
                }
                break;

            case "up":
                if (CanMove("up"))
                {
                    MoveUp();
                }
                break;

            case "down":
                if (CanMove("down"))
                {
                    MoveDown();
                }
                break;
        }
    }

    public void MoveLeft()
    {
        Steps++;
        x -= xSpeed;

        if (Steps % 5 == 4)
        {
            Sound.PlayWav(Sound.STEP, 2, 0);
        }
    }

    public void MoveRight()
    {
        Steps++;
        x += xSpeed;

        if (Steps % 5 == 4)
        {
            Sound.PlayWav(Sound.STEP, 2, 0);
        }
    }

    public void MoveUp()
    {
        Steps++;
        y -= ySpeed;

        if (Steps % 5 == 4)
        {
            Sound.PlayWav(Sound.STEP, 2, 0);
        }
    }

    public void MoveDown()
    {
        Steps++;
        y += ySpeed;
        
        if(Steps % 5 == 4)
        {
            Sound.PlayWav(Sound.STEP, 2, 0);
        }
    }

    public void ActivatePowerUp(PowerUp pUp)
    {
        ActivePowerUP = pUp.Type;
        pUp.Activate(this);
    }
}
