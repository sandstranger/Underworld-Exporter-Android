﻿using UnityEngine;
using System.Collections;

public class Cutscene_Splash : Cuts {

		public override void Awake()
		{
				base.Awake();

				switch(UWEBase._RES)
				{
				case UWEBase.GAME_UWDEMO:
						noOfImages=3;
						ImageTimes[0]=0.0f;
						ImageTimes[1]=2.0f;
						ImageTimes[2]=4.0f;
						ImageFrames[0]="SplashOriginDemo";
						ImageFrames[1]="cs011_n01";//UW splash screen.
						ImageFrames[2]="Anim_Base";//To finish.
						ImageLoops[0]=-1;
						ImageLoops[1]=-1;
						ImageLoops[2]=-1;
						noOfSubs=0;
						noOfAudioClips=0;
						break;
				case UWEBase.GAME_UW1:
						noOfImages=4;
						ImageTimes[0]=0.0f;
						ImageTimes[1]=1.0f;
						ImageTimes[2]=2.0f;
						ImageTimes[3]=5.0f;
						ImageFrames[0]="SplashOrigin";
						ImageFrames[1]="SplashBlueSky";
						ImageFrames[2]="cs011_n01";//UW splash screen.
						ImageFrames[3]="Anim_Base";//To finish.
						ImageLoops[0]=-1;
						ImageLoops[1]=-1;
						ImageLoops[2]=-1;
						ImageLoops[3]=-1;
						noOfSubs=0;
						noOfAudioClips=0;
						break;

				case UWEBase.GAME_UW2:
						noOfImages=4;
						ImageTimes[0]=0.0f;
						ImageTimes[1]=1.0f;
						ImageTimes[2]=2.0f;
						ImageTimes[3]=6.0f;
						ImageFrames[0]="SplashOriginEa";
						ImageFrames[1]="SplashLookingGlass";
						ImageFrames[2]="cs011_n01";//UW splash screen.
						ImageFrames[3]="Anim_Base";//To finish.
						ImageLoops[0]=-1;
						ImageLoops[1]=-1;
						ImageLoops[2]=-1;
						ImageLoops[3]=-1;
						noOfSubs=0;
						noOfAudioClips=0;
						break;

				}



		}


	public override void PostCutSceneEvent ()
	{
		GotoMainMenu();
	}
}
