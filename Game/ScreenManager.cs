/**
 * 
 * 21/05/2015 - Jose Manuel Gañan Escobar:
 *                                  Added controls screen
 *                                  When you press Escape, return to the menu
 * */

using System;
using System.Threading;
using System.Collections.Generic;
class ScreenManager
{
	private Sprite introScreen;
	private Sprite creditsScreen;
	private Sprite pauseScreen;
	private Sprite seeControls;
	private Font font32;
	private Font font18;

	public ScreenManager()
	{
		introScreen = new Sprite("Data/Images/intro.png");
		creditsScreen = new Sprite("Data/Images/credits.png");
		pauseScreen = new Sprite("Data/Images/pause.png");
		seeControls = new Sprite("Data/Images/controls.png");

		font32 = new Font("Data/Fonts/kenvector_future.ttf", 32);
		font18 = new Font("Data/Fonts/kenvector_future.ttf", 18);
	}


	public void EnterMapMaking()
	{
		MapMaker mapMaker = MapMaker.GetInstance();
		Thread.Sleep(250); // Wait so it doesnt select the sixth tile.
		mapMaker.Run();
	}

	public int ShowLevels()
	{
		SdlHardware.ClearScreen();
		short x = 460;
		short y = 200;
		int i = 0;
		foreach(string level in FileManager.GetLevels())
		{
			SdlHardware.WriteHiddenText("--------------", 
							 410, Convert.ToInt16(y-18),255, 255, 104, font18);

			SdlHardware.WriteHiddenText("Level "+(i+1),x,y,255,255,104,font18);

			SdlHardware.WriteHiddenText("--------------", 
								410, Convert.ToInt16(y+20),255, 255,104,font18);
			i++;
			y += 38;
		}
		SdlHardware.WriteHiddenText("Level List", 440, 100, 255, 255, 104, 
																	font18);
		SdlHardware.WriteHiddenText("Click on a level or press escape to exit", 
											270, 132, 255, 255, 104, font18);
		SdlHardware.ShowHiddenScreen();
		do
		{
			if (SdlHardware.MouseClicked())
			{
				y = 200;
				for(int j = 0; j < i; j++)
				{
					if(SdlHardware.GetMouseX() >= 400 && 
					   SdlHardware.GetMouseX() <= 600 && 
					   SdlHardware.GetMouseY() >= y-18 && 
					   SdlHardware.GetMouseY() <= y+30)
					{
						return j + 1; // The chosen level
					}
					y += 38;
				}
			}
			Thread.Sleep(40);
		}
		while (!SdlHardware.KeyPressed(SdlHardware.KEY_ESC));

		return -1; // To indicate that the user didnt choose a level
		
	}

	public void ShowControls()
	{
		SdlHardware.ClearScreen();
		seeControls.DrawOnHiddenScreen();
		SdlHardware.ShowHiddenScreen();
		do
		{
			Thread.Sleep(40);
		}
		while (!SdlHardware.KeyPressed(SdlHardware.KEY_ESC));
	}

	public void Pause()
	{
		SdlHardware.ClearScreen();
		pauseScreen.DrawOnHiddenScreen();
		SdlHardware.ShowHiddenScreen();
        Thread.Sleep(250);
		Sound.Pause(1);
		do
		{
			Thread.Sleep(40);
		}
		while (!SdlHardware.KeyPressed(SdlHardware.KEY_P));
		Thread.Sleep(80);
		Sound.Resume(1);
	}

	public void ShowScores()
	{
		SdlHardware.ClearScreen();
		Dictionary<string, int> scores = FileManager.GetScores();

		SdlHardware.WriteHiddenText("===== Top 5 players =====", 
			200, 100, 255, 255, 102, font32);
		int yStart = 120;
		int i = 0;
		foreach(string name in scores.Keys)
		{
			int score = 100 - scores[name];
			if(score >= 0 && i < 5)
			{
				SdlHardware.WriteHiddenText((i + 1) + " | " + name+"    -   "+
				score, 350, (short)(yStart + 32), 255, 255, 102, font32);
			}
			i++;
			yStart += 32;
		}

		SdlHardware.WriteHiddenText("ESCAPE TO EXIT",
			350, 525, 255, 255, 102, font32);

		SdlHardware.ShowHiddenScreen();
		do
		{
			Thread.Sleep(25);
		}
		while (!SdlHardware.KeyPressed(SdlHardware.KEY_ESC));
		Thread.Sleep(80);
	}

	public void ShowIntro()
	{
		introScreen.DrawOnHiddenScreen();
		SdlHardware.ShowHiddenScreen();
		do
		{
			if (SdlHardware.KeyPressed(SdlHardware.KEY_ESC))
			{
				SdlHardware.Exit();
			}
			
			Thread.Sleep(30);
		}
		while (!SdlHardware.KeyPressed(SdlHardware.KEY_SPC) && 
			!SdlHardware.KeyPressed(SdlHardware.KEY_1));
	}


	public void ShowCredits()
	{
		SdlHardware.ClearScreen();
		creditsScreen.DrawOnHiddenScreen();
		SdlHardware.ShowHiddenScreen();
		do
		{
			Thread.Sleep(40);
		}
		while (!SdlHardware.KeyPressed(SdlHardware.KEY_ESC));
	}
}