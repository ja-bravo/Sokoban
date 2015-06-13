using System;
using System.Timers;

class PowerUp : Sprite
{
    public enum PowUpType { OPEN, SPEED, SWAP }
    public PowUpType Type { get; set; }
    private Timer timer;
    private Player player;
    public PowerUp(PowUpType type)
    {
        this.Type = type;

        timer = new Timer();
        timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        switch (Type)
        {
            case PowUpType.OPEN:
                timer.Interval = 7500; // Time that the power up is active.
                break;


            case PowUpType.SPEED:
                timer.Interval = 5000; // Time that the power up is active.
                break;

            case PowUpType.SWAP:
                break;
        }
    }

    private void OnTimedEvent(object source, ElapsedEventArgs e)
    {
        Deactivate();
    }

    public void Activate(Player player)
    {
        this.player = player;
        switch (Type)
        {
            case PowUpType.OPEN:
                player.HasPowerUp = true;
                if(Level.HasDoors())
                {
                    foreach (Door door in Level.GetDoors())
                    {
                        door.PowerOpen();
                    }

                    timer.Start();
                }
                break;

            case PowUpType.SPEED:
                player.SetSpeed(player.GetSpeedX() * 2, player.GetSpeedY() * 2);
                player.HasPowerUp = true;
                timer.Start();
                break;

            case PowUpType.SWAP:
                player.CanSwap = true;
                player.HasPowerUp = true;
                break;
        }
    }

    private void Deactivate()
    {
        player.HasPowerUp = false;
        switch (Type)
        {
            case PowUpType.OPEN:
                player.HasPowerUp = false;
                if(Level.HasDoors())
                {
                    foreach (Door door in Level.GetDoors())
                    {
                        door.State = Tile.states.OPEN;
                        door.Close();
                    }
                    timer.Stop();
                }
                break;

            case PowUpType.SPEED:
                player.SetSpeed(player.InitialSpeed, player.InitialSpeed);
                timer.Stop();
                break;

            case PowUpType.SWAP:
                player.CanSwap = false;
                player.HasPowerUp = false;
                break;
        }
    }
}

