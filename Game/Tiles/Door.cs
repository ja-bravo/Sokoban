using System;


class Door : Tile
{
    public states State { get; set; }
    private PressurePlate plate;
    public Door(int x, int y)
        : base(x, y)
    {
        width = 40;
        height = 40;
        LoadImage("data/Images/door.png");
        Type = types.NOWALKABLE;
        State = states.CLOSED;
    }

    public void AssignPlate(PressurePlate p)
    {
        plate = p;
    }

    public void PowerOpen()
    {
        State = states.POWEROPEN;
        Type = types.WALKABLE;
        LoadImage("Data/Images/floor.png");
    }

    public bool PlateIsActivated()
    {
        if( plate != null && plate.State == Tile.states.ACTIVATED)
        {
            return true;
        }

        return false;
    }

    public void Open()
    {
        if(State == states.CLOSED || State == states.POWEROPEN)
        {
            State = states.OPEN;
            Type = types.WALKABLE;
            LoadImage("Data/Images/floor.png");
        }
    }

    public void Close()
    {
        if (State == states.OPEN)
        {
            State = states.CLOSED;
            Type = types.NOWALKABLE;
            LoadImage("Data/Images/door.png");
        }
    }

    public override string GetType()
    {
        return "Door";
    }
}
