/*
 * 
 * 06/03/2015 v0.01 - Jose Antonio Bravo:
 *                                 Almost empty skeleton.
 * */
class PressurePlate : Tile
{
    public states State { get; set; }
    public PressurePlate(int x, int y)
        : base(x, y)
    {
        width = 40;
        height = 40;
        LoadImage("Data/Images/plate.png");
        Type = types.WALKABLE;
        State = states.DEACTIVATED;
    }

    public void Activate()
    {
        if(State == states.DEACTIVATED)
        {
            State = states.ACTIVATED;
            LoadImage("Data/Images/plate2.png");
        }
    }

    public void Deactivate()
    {
        if (State == states.ACTIVATED)
        {
            State = states.DEACTIVATED;
            LoadImage("Data/Images/plate.png");
        }
    }

    public bool IsSteppedBy(Box b)
    {
        int xEnd = b.GetX() + b.GetWidth();
        int xStart = b.GetX();

        int yEnd = b.GetY() + b.GetHeight();
        int yStart = b.GetY();

        // Adding the size/2 to have some leeway on where the boxes 
        // are counted as in.
        if ((x + width / 2 < xEnd) &&
            (x + width > xStart + b.GetWidth() / 2) && (y + height / 2 < yEnd) 
            && (y + height > yStart + b.GetHeight() / 2))
        {
            return true;
        }

        return false;
    }

    public override string GetType()
    {
        return "Plate";
    }
}