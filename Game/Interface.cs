using System;
using System.Threading;
using System.Collections.Generic;

class Interface
{
    private Font font18;
    private Sprite bottomBar;
    private string userName;
    private string levelName;
    public bool ShowLegend { get; set; }
    

    public Interface(string levelID)
    {
        userName = " ";
        levelName = levelID;
        font18 = new Font("Data/Fonts/kenvector_future.ttf", 18);
        bottomBar = new Sprite("Data/Images/ui.png");
        ShowLegend = true;
        
    }

    public void ShowHigherScore()
    {
        Thread.Sleep(500);
        SdlHardware.WriteHiddenText("New high score!", 360, 380, 
                                    0, 180, 102, font18);

        SdlHardware.WriteHiddenText("Press enter to continue", 360, 400,
                                    0, 180, 102, font18);
        SdlHardware.ShowHiddenScreen();
        do
        {
            Thread.Sleep(50);
        }
        while (!SdlHardware.KeyPressed(SdlHardware.KEY_RETURN));
    }

    public void DrawLegend()
    {
        if(ShowLegend)
        {
            List<string> types = new List<string>();
            for (int i = 0; i < Level.Count; i++)
            {
                Tile t = Level.GetTile(i);

                if (!types.Contains(t.GetType()))
                {
                    types.Add(t.GetType());
                }
            }

            DrawLegend(types);
        }
    }

    public string DrawPowerUpSelection()
    {
        int backgroundX = 512 - 140 / 2;
        string option = "";

        SdlHardware.WriteHiddenText("Click to select a power-up",
                                            360, 320, 255, 255, 255, font18);

        SdlHardware.DrawHiddenImage(new Image("Data/Images/pUPScreen.png"),
                                            backgroundX, 350);

        SdlHardware.DrawHiddenImage(new Image("Data/Images/pUP-Speed.png"),
                                            backgroundX + 10, 350 + 10);

        SdlHardware.DrawHiddenImage(new Image("Data/Images/pUp-Swap.png"),
                                            backgroundX + 60, 350 + 10);

        SdlHardware.DrawHiddenImage(new Image("Data/Images/pUp-Open.png"),
                                            backgroundX + 110, 350 + 10);
        SdlHardware.ShowHiddenScreen();

        do
        {
            if(SdlHardware.MouseClicked())
            {
                if(SdlHardware.GetMouseX() >= backgroundX+10 &&
                    SdlHardware.GetMouseX() <= backgroundX+50 &&
                    SdlHardware.GetMouseY() >= 350 && 
                    SdlHardware.GetMouseY() <= 350+50)
                {
                    option = "speed";
                }

                if (SdlHardware.GetMouseX() >= backgroundX + 60 &&
                    SdlHardware.GetMouseX() <= backgroundX + 100 &&
                    SdlHardware.GetMouseY() >= 350 && 
                    SdlHardware.GetMouseY() <= 350 + 50)
                {
                    option = "swap";
                }

                if (SdlHardware.GetMouseX() >= backgroundX + 110 &&
                    SdlHardware.GetMouseX() <= backgroundX + 150 &&
                    SdlHardware.GetMouseY() >= 350 && 
                    SdlHardware.GetMouseY() <= 350 + 50)
                {
                    option = "open";
                }
            }
            Thread.Sleep(80);
        }
        while (option == "");

        return option;
    }

    public void DrawPowerUp(PowerUp.PowUpType type)
    {
        Image image;
        switch (type)
        {
            case PowerUp.PowUpType.SPEED:
                image = new Image("Data/Images/pUp-Speed.png");
                SdlHardware.DrawHiddenImage(image, 40, 40);
                break;

            case PowerUp.PowUpType.OPEN:
                SdlHardware.WriteHiddenText("The doors are open for: 7.5 secs", 
                    0, 22, 255, 255, 205, font18);
                image = new Image("Data/Images/pUp-Speed.png");
                SdlHardware.DrawHiddenImage(image,40, 40);
                break;

            case PowerUp.PowUpType.SWAP:
                SdlHardware.WriteHiddenText("Press space to activate", 
                    0, 22, 255, 255, 205, font18);
                image = new Image("Data/Images/pUp-Swap.png");
                SdlHardware.DrawHiddenImage(image, 40, 40);
                break;
        }
    }

    private void DrawLegend(List<string> types)
    {
        short textY = 8;
        foreach (string type in types)
        {
            switch (type)
            {
                case "Box":
                    SdlHardware.WriteHiddenText("Box", 933, textY, 255, 255, 204, 
                                                                        font18);
                    SdlHardware.DrawHiddenImage(new Image("Data/Images/box.png"),
                                                980, textY - 8);

                    textY += 40;
                    break;

                case "Door":
                    SdlHardware.WriteHiddenText("Door", 920, textY, 
                                                255, 255, 204, font18);
                    SdlHardware.DrawHiddenImage(new Image("Data/Images/door.png"),
                                                980, textY - 8);
                    textY += 40;
                    break;

                case "Plate":
                    SdlHardware.WriteHiddenText("Pressure plate", 780, textY, 
                                                255, 255, 204, font18);
                    SdlHardware.DrawHiddenImage(new Image("Data/Images/plate.png"),
                                                980, textY - 8);
                    textY += 40;
                    break;

                case "Wall":
                    SdlHardware.WriteHiddenText("Wall", 916, textY, 
                                                255, 255, 204, font18);
                    SdlHardware.DrawHiddenImage(new Image("Data/Images/wall.png"),
                                                980, textY - 8);
                    textY += 40;
                    break;

                case "Floor":
                    SdlHardware.WriteHiddenText("Floor", 905, textY, 
                                                255, 255, 204, font18);
                    SdlHardware.DrawHiddenImage(new Image("Data/Images/floor.png"),
                                                980, textY - 8);
                    textY += 40;
                    break;

                case "WinTile":
                    SdlHardware.WriteHiddenText("Winning tile", 833, textY, 
                                                255, 255, 204, font18);
                    SdlHardware.DrawHiddenImage(new Image("Data/Images/winFloor.png"),
                                                980, textY-8);
                    textY += 40;
                    break;
            }
        }
    }

    public void Draw(int steps, int pushes)
    {
        bottomBar.DrawOnHiddenScreen();
        SdlHardware.WriteHiddenText("Steps:  " + steps, 20, 692, 0, 51, 10,
                                                                    font18);
        SdlHardware.WriteHiddenText("Pushes: " + pushes, 20, 710, 0, 51, 10,
                                                                    font18);

        SdlHardware.WriteHiddenText("Level " + levelName.Replace("level",""), 
                                            910, 700, 0, 51, 102,font18);

    }

    public void Reload(string levelID)
    {
        levelName = levelID;
    }

    public void ShowWinScreen()
    {
        Sprite winScreen = new Sprite("Data/Images/win.png");

        do
        {
            SdlHardware.ClearScreen();
            winScreen.DrawOnHiddenScreen();
            ReadKeyboard();
            
            // If you try to write an empty string, the games crashes with out 
            // an error
            if(userName != "")
            {
                SdlHardware.WriteHiddenText(userName, 440, 318, 0, 180, 102, 
                                                                    font18);
            }

            SdlHardware.ShowHiddenScreen();
            Thread.Sleep(50);
        }
        while (!SdlHardware.KeyPressed(SdlHardware.KEY_RETURN));
    }

    public string GetChoice()
    {
        Sprite choiceScreen = new Sprite("Data/Images/choice.png");
        SdlHardware.ClearScreen();
        choiceScreen.DrawOnHiddenScreen();
        SdlHardware.ShowHiddenScreen();

        string choice = "";
        do
        { 
            if(SdlHardware.KeyPressed(SdlHardware.KEY_R))
            {
                choice = "restart";
            }

            if(SdlHardware.KeyPressed(SdlHardware.KEY_E))
            {
                choice = "next";
            }

            if (SdlHardware.KeyPressed(SdlHardware.KEY_Q))
            {
                choice = "prev";
            }
            Thread.Sleep(100);
        }
        while (choice != "restart" && choice != "next" && choice != "prev");

        return choice;

    }

    public string GetUserName()
    {
        return userName;
    }

    public string GetLevelName()
    {
        return levelName;

    }

    private void ReadKeyboard()
    {
        if (SdlHardware.KeyPressed(SdlHardware.KEY_RM))
        {
            if(userName.Length >= 1)
            {
                userName = userName.Substring(0, userName.Length - 1);
            }
        }
            
        if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
            userName += " ";
        
        if(SdlHardware.KeyPressed(SdlHardware.KEY_A))
            userName += "a";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_B))
            userName += "b";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_C))
            userName += "c";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_D))
            userName += "d";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_E))
            userName += "e";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_F))
            userName += "f";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_G))
            userName += "g";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_H))
            userName += "h";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_I))
            userName += "i";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_J))
            userName += "j";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_K))
            userName += "k";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_L))
            userName += "l";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_M))
            userName += "m";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_N))
            userName += "n";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_O))
            userName += "o";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_P))
            userName += "p";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_Q))
            userName += "q";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_R))
            userName += "r";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_S))
            userName += "s";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_T))
            userName += "t";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_U))
            userName += "u";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_V))
            userName += "v";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_W))
            userName += "w";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_X))
            userName += "x";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_Y))
            userName += "y";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_Z))
            userName += "z";


        if (SdlHardware.KeyPressed(SdlHardware.KEY_1))
            userName += "1";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_2))
            userName += "2";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_3))
            userName += "3";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_4))
            userName += "4";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_5))
            userName += "5";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_6))
            userName += "6";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_7))
            userName += "7";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_8))
            userName += "8";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_9))
            userName += "9";
        if (SdlHardware.KeyPressed(SdlHardware.KEY_0))
            userName += "0";
    }
}