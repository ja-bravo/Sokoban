/*
 * 
 * 06/03/2015 v0.01 - Jose Antonio Bravo:
 *                                 Almost empty skeleton.
 * */
class Floor : Tile
{
    public Floor(int x, int y)
        : base(x, y)
    {
        width = 40;
        height = 40;
        LoadImage("data/Images/floor.png");
        Type = types.WALKABLE;
    }

    public override string GetType()
    {
        return "Floor";
    }
}