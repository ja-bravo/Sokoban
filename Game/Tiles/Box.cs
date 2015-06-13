/*
 * 
 * 06/03/2015 v0.01 - Jose Antonio Bravo:
 *                                 Almost empty skeleton.
 * */
class Box : Tile
{
    public Box(int x, int y)
        : base(x, y)
    {
        width = 40;
        height = 40;

        xSpeed = 5;
        ySpeed = 5;
        LoadImage("data/Images/box.png");
        Type = types.NOWALKABLE;
    }

    public void Move(string dir)
    {
        switch (dir)
        {
            case "left":
                if (CanMove("left"))
                {
                    x -= xSpeed;
                }
                break;

            case "right":
                if (CanMove("right"))
                {
                    x += xSpeed;
                }
                break;

            case "up":
                if (CanMove("up"))
                {
                    y -= xSpeed;
                }
                break;

            case "down":
                if (CanMove("down"))
                {
                    y += xSpeed;
                }
                break;
        }
    }

    public bool CanMove(string dir)
    {
        for (int i = 0; i < Level.Count; i++)
        {
            Tile t = Level.GetTile(i);
            if (CollisionsWith(t, dir) && t.Type == Tile.types.NOWALKABLE
                && t != this)
            {
                return false;
            }
        }
        return true;
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
    public override string GetType()
    {
        return "Box";
    }
}