/*
 * 
 * 06/03/2015 v0.01 - Jose Antonio Bravo:
 *                                 Almost empty skeleton.
 * */
class Tile : Sprite
{
    public enum types { WALKABLE, NOWALKABLE};
    public enum states { OPEN, POWEROPEN, CLOSED, ACTIVATED, DEACTIVATED };
    public types Type { get; set; }

    public Tile(int x, int y)
    {
        this.x = x;
        this.y = y;
        width = 40;
        height = 40;
    }

    public bool IsOnScreen()
    {
        if ((x < 0 && x + width < 0) || (x > 1024 && x + width > 1024))
        {
            return false;
        }

        if ((y < 0 && y + height < 0) || (y > 736 && y + height > 736))
        {
            return false;
        }
        return true;
    }
    public virtual string GetType()
    {
        return "";
    }
}