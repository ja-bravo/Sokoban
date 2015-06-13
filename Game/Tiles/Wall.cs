/*
 * 
 * 06/03/2015 v0.01 - Jose Antonio Bravo:
 *                                 Almost empty skeleton.
 * */
class Wall : Tile
{
    public Wall(int x, int y)
        : base(x, y)
    {
        width = 40;
        height = 40;
        LoadImage("data/Images/wall.png");
        Type = types.NOWALKABLE;
    }

    public override string GetType()
    {
        return "Wall";
    }
}