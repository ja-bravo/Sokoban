/*
 * 
 * 06/03/2015 v0.01 - Jose Antonio Bravo:
 *                                Draw Level and Player.
 *                                Player can move.
 *                                Basic game loop.
 *                                
 * 06/03/2015 v0.02 - Jose Antonio Bravo:
 *                                 Added starting X and Y to the player and the 
 *                                 levelID to the level.
 *                                 Check if the player can move before doing it.
 *                                 
 * 27/03/2015 v0.03 - Jose Antonio Bravo:
 *                                  Improved player collision. 
 *                                  Start coords are read from the level file.
 *                                  
 * 21/05/2015 - Jose Manuel Gañan Escobar:
 *                                  Added listen to music in the level
 * */
using System;
using System.Threading;
using System.Collections.Generic;

class Game
{
    private Level level;
    private Player player;
    private Interface ui;
    private FileManager fileManager;
    private Menu menu;
    private static ScreenManager screenManager;
    private bool finished;
    private string levelID;
    private int boxesOnSpot;
    private int powerUps;
    private int score;

    const int WIDTH = 1024;
    const int HEIGHT = 736;
    const int LEFT_SCROLL_THRESHOLD = WIDTH / 8;
    const int RIGHT_SCROLL_THRESHOLD = WIDTH - LEFT_SCROLL_THRESHOLD;
    const int UP_SCROLL_THRESHOLD = HEIGHT / 5;
    const int DOWN_SCROLL_THRESHOLD = HEIGHT - UP_SCROLL_THRESHOLD;
    
    public Game()
    {
        bool fullScreen = false;
        menu = new Menu();
        SdlHardware.Init(WIDTH, HEIGHT, 24, fullScreen);
        PrepareGame();
    }

    private void WaitForNextFrame()
    {
        // 16 = 60 fps
        SdlHardware.Pause(40);
    }

    private void DrawElements()
    {
        //SdlHardware.Lock();
        SdlHardware.ClearScreen();
        level.Draw();
        player.Draw();
        ui.Draw(player.Steps, player.Pushes);
        ui.DrawLegend();

        if(player.HasPowerUp)
        {
            ui.DrawPowerUp(player.ActivePowerUP);
        }
        SdlHardware.ShowHiddenScreen();
        //SdlHardware.Unlock();
    }

    private void CheckInput()
    {
        if (SdlHardware.KeyPressed(SdlHardware.KEY_E))
        {
            LoadNextLevel();
            WaitForNextFrame(); // Wait so it doesn't skip a level
        }

        if (SdlHardware.KeyPressed(SdlHardware.KEY_Q))
        {
            LoadPrevLevel();
            WaitForNextFrame(); // Wait so it doesn't skip a level
        }

        if (SdlHardware.KeyPressed(SdlHardware.KEY_R))
        {
            Reload();
        }

        if (SdlHardware.KeyPressed(SdlHardware.KEY_ESC))
        {
            int option = menu.RunMenu();

            if (option != -1)
            {
                LoadLevel(option);
            }
            else
            {
                PrepareGame();
            }
        }

        if (SdlHardware.KeyPressed(SdlHardware.KEY_P))
        {
            screenManager.Pause();
        }

        if (SdlHardware.KeyPressed(SdlHardware.KEY_SPC))
        {
            if(player.CanSwap)
            {
                player.Swap();
            }
        }

        if (SdlHardware.KeyPressed(SdlHardware.KEY_H))
        {
            ui.ShowLegend = !ui.ShowLegend;
            WaitForNextFrame();
        }

        if (SdlHardware.KeyPressed(SdlHardware.KEY_W))
        {
            if (player.CanMove("up"))
            {
                player.MoveUp();
            }
            else if (player.CollisionsWithBox("up"))
            {
                player.PushBox("up");
            }
        }

        if (SdlHardware.KeyPressed(SdlHardware.KEY_S))
        {
            if (player.CanMove("down"))
            {
                player.MoveDown();
            }
            else if (player.CollisionsWithBox("down"))
            {
                player.PushBox("down");
            }
        }

        if (SdlHardware.KeyPressed(SdlHardware.KEY_A))
        {
            if (player.CanMove("left"))
            {
                player.MoveLeft();
            }
            else if (player.CollisionsWithBox("left"))
            {
                player.PushBox("left");
            }
        }

        if (SdlHardware.KeyPressed(SdlHardware.KEY_D))
        {
            if (player.CanMove("right"))
            {
                player.MoveRight();
            }
            else if (player.CollisionsWithBox("right"))
            {
                player.PushBox("right");
            }
        }
    }

    private void CheckCollisions()
    {
        List<PressurePlate> plates = level.GetPlates();
        List<Box> boxes = level.GetBoxes();

        for (int i = 0; i < boxes.Count; i++)
        {
            Box box = boxes[i];
            for (int j = 0; j < plates.Count; j++)
            {
                PressurePlate plate = plates[j];
                if (plate.IsSteppedBy(box) || player.CollisionsWith(plate))
                {
                    plate.Activate();
                    plates.RemoveAt(j);
                }
                else
                {
                    plate.Deactivate();
                }
            }
        }

        if (Level.HasDoors())
        {
            List<Door> doors = Level.GetDoors();

            foreach (Door door in doors)
            {
                if (door.PlateIsActivated())
                {
                    door.Open();
                }
                else if (door.State == Tile.states.OPEN)
                {
                    door.Close();
                }
            }
        }
    }
    private void CheckConditions()
    {
        // Scrolling
        if(player.GetX() > RIGHT_SCROLL_THRESHOLD )
        {
            level.ScrollRight();
            player.MoveLeft();
        }

        if (player.GetX() < LEFT_SCROLL_THRESHOLD)
        {
            level.ScrollLeft();
            player.MoveRight();
        }

        if (player.GetY() < UP_SCROLL_THRESHOLD)
        {
            level.ScrollUp();
            player.MoveDown();
        }

        if (player.GetY() > DOWN_SCROLL_THRESHOLD)
        {
            level.ScrollDown();
            player.MoveUp();
        }

        List<WinTile> wTiles = level.GetWinTiles();
        List<Box> boxes = level.GetBoxes();

        int count = 0;
        foreach (Box b in boxes)
        {
            foreach (WinTile wt in wTiles)
            {
                if (wt.IsInSpot(b))
                {
                    count++;
                }
            }
        }

        if(count > boxesOnSpot)
        {
            boxesOnSpot = count;
        }

        if (count == boxes.Count)
        {
            score = 1000 - player.Pushes - player.Steps;
            ui.ShowWinScreen();

            Dictionary<String, int> scores = FileManager.GetScores();
            if (scores.ContainsKey(ui.GetUserName())
                && score > scores[ui.GetUserName()])
            {
                ui.ShowHigherScore();
            }

            SaveScore(ui.GetUserName());

            
            PickChoice();
        }


        if(boxesOnSpot == boxes.Count/2 && !player.HasPowerUp
            && powerUps == 0)
        {
            string option = ui.DrawPowerUpSelection();
            powerUps++;
            
            PowerUp pUp;
            switch(option)
            {
                case "speed":
                    pUp = new PowerUp(PowerUp.PowUpType.SPEED);
                    player.ActivatePowerUp(pUp);
                    break;

                case "open":
                    pUp = new PowerUp(PowerUp.PowUpType.OPEN);
                    player.ActivatePowerUp(pUp);
                    break;

                case "swap":
                    pUp = new PowerUp(PowerUp.PowUpType.SWAP);
                    player.ActivatePowerUp(pUp);
                    break;
            }

        }
    }

    private void PickChoice()
    {
        string choice = ui.GetChoice();

        switch (choice)
        {
            case "restart":
                Reload();
                break;

            case "next":
                LoadNextLevel();
                break;

            case "prev":
                LoadPrevLevel();
                break;
        }
    }

    private void SaveScore(string userName)
    {
        fileManager.SaveScore(userName, levelID, score);
    }

    private void LoadPrevLevel()
    {
        levelID = fileManager.GetPrevLevelID();
        level.ChangeLevel(levelID);
        player.Reload(level.GetXStart(), level.GetYStart());
        ui.Reload(levelID);
        boxesOnSpot = 0;
        score = 0;
        powerUps = 0;
    }

    private void LoadLevel(int id)
    {

        if (!Sound.IsLoaded)
        {
            Sound.Init(12000, 2, 1024);
        }

        Sound.Stop(1);
        Sound.PlayWav(Sound.BACKGROUND, 1, -1);
        Sound.SetVolume(1, 64);

        levelID = "level" + id;
        fileManager.SetLevelID(id - 1);
        level.ChangeLevel(levelID);
        player.Reload(level.GetXStart(), level.GetYStart());
        ui.Reload(levelID);
        boxesOnSpot = 0;
        powerUps = 0;
        score = 0;
    }

    private void LoadNextLevel()
    {
        levelID = fileManager.GetNextLevelID();
        level.ChangeLevel(levelID);
        player.Reload(level.GetXStart(), level.GetYStart());
        ui.Reload(levelID);
        boxesOnSpot = 0;
        powerUps = 0;
        score = 0;
    }

    public void PrepareGame()
    {
        screenManager = new ScreenManager();
        fileManager = new FileManager();
        levelID = fileManager.GetLevelName();

        level = new Level(levelID);
        player = new Player(level.GetXStart(), level.GetYStart());
        ui = new Interface(levelID);
        finished = false;
        boxesOnSpot = 0;
        powerUps = 0;
        score = 0;
    }

    private void Reload()
    {
        level.ChangeLevel(levelID);
        player.Reload(level.GetXStart(), level.GetYStart());
        boxesOnSpot = 0;
        powerUps = 0;
        score = 0;
    }

    public void Run()
    {
        screenManager.ShowIntro();
        int option = menu.RunMenu();

        if(option != -1)
        {
            LoadLevel(option);
        }

        if (!Sound.IsLoaded)
        {
            Sound.Init(12000, 2, 1024);
        }

        Sound.PlayWav(Sound.BACKGROUND, 1, -1);
        Sound.SetVolume(1, 64);
        do
        {
            if (!finished)
            {
                DrawElements();

                CheckInput();

                CheckCollisions();

                CheckConditions();

                WaitForNextFrame();
            }
        }
        while (!finished);
    }

    struct Menu
    {
        private Sprite screen;
        private int option;
        public int RunMenu()
        {
            screen = new Sprite("Data/Images/menu.png");
            option = 1;
            if(Sound.IsOn)
            {
                Sound.Stop(1);
                Sound.Stop(2);
            }
            Thread.Sleep(100);
            do
            {
                SdlHardware.ClearScreen();
                screen.DrawOnHiddenScreen();
                SdlHardware.ShowHiddenScreen();

                if (SdlHardware.KeyPressed(SdlHardware.KEY_2))
                {
                    option = screenManager.ShowLevels();
                    Thread.Sleep(500); // Wait so the game does not exit
                    
                    if(option != -1)
                    {
                        return option;
                    }
                }

                if (SdlHardware.KeyPressed(SdlHardware.KEY_3))
                {
                    screenManager.ShowControls();
                    Thread.Sleep(500); // Wait so the game does not exit
                }

                if (SdlHardware.KeyPressed(SdlHardware.KEY_4))
                {
                    screenManager.ShowScores();
                    Thread.Sleep(500); // Wait so the game does not exit
                }

                if (SdlHardware.KeyPressed(SdlHardware.KEY_5))
                {
                    screenManager.ShowCredits();
                    Thread.Sleep(500); // Wait so the game does not exit
                }

                if (SdlHardware.KeyPressed(SdlHardware.KEY_6))
                {
                    screenManager.EnterMapMaking();
                    Thread.Sleep(500); // Wait so the game does not exit
                }

                if (SdlHardware.KeyPressed(SdlHardware.KEY_ESC))
                {
                    SdlHardware.Exit();
                }

                Thread.Sleep(30);
            }
            while (!SdlHardware.KeyPressed(SdlHardware.KEY_1));
            
            return option;
           
        }
    }
}