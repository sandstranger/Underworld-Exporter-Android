using System.Collections;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicController : UWEBase
{
	public enum UWMusicTracks
	{
		UW1introduction = 1,
		UW1darkabyss = 2,
		UW1descent = 3,
		UW1wanderer = 4,
		UW1battlefield = 5,
		UW1combat = 6,
		UW1injured = 7,
		UW1armed = 10,
		UW1victory = 11,
		UW1death = 12,
		UW1fleeing = 13,
		UW1mapslegends = 15,
		UW2theme = 1,
		UW2enemywounded = 2,
		UW2combat = 3,
		UW2dangeroussituation = 4,
		UW2armed = 5,
		UW2victory = 6,
		UW2sewers = 7,
		UW2talorus = 10,
		UW2prisontower = 11,
		UW2death = 12,
		UW2killorn = 13,
		UW2icecaverns = 14,
		UW2scintillus = 15,
		UW2castle = 16,
		UW2themeagain = 17,
		UW2introduction = 30,
		UW2trap = 31
	}

	public static string UW1Path;

	public static string UW2Path;

	private int currenttrack = -1;

	public const int SOUND_EFFECT_FOOT_1 = 1;

	public const int SOUND_EFFECT_FOOT_2 = 2;

	public const int SOUND_EFFECT_PUNCH_1 = 3;

	public const int SOUND_EFFECT_PUNCH_2 = 4;

	public const int SOUND_EFFECT_WATER_LAND_1 = 5;

	public const int SOUND_EFFECT_NPC_DEATH_1 = 6;

	public const int SOUND_EFFECT_MELEE_HIT_1 = 7;

	public const int SOUND_EFFECT_MELEE_HIT_2 = 8;

	public const int SOUND_EFFECT_RANGED_STRIKE = 9;

	public const int SOUND_EFFECT_MELEE_MISS_1 = 10;

	public const int SOUND_EFFECT_DOOR_MOVE = 11;

	public const int SOUND_EFFECT_DOOR_FINISH = 12;

	public const int SOUND_EFFECT_RUMBLE = 18;

	public const int SOUND_EFFECT_PORTCULLIS = 20;

	public const int SOUND_EFFECT_SPLASH_1 = 24;

	public const int SOUND_EFFECT_SPLASH_2 = 25;

	public const int SOUND_EFFECT_MELEE_MISS_2 = 28;

	public const int SOUND_EFFECT_ICE_SLIDE = 29;

	public const int SOUND_EFFECT_DRINK = 30;

	public const int SOUND_EFFECT_EAT_1 = 31;

	public const int SOUND_EFFECT_EAT_2 = 33;

	public const int SOUND_EFFECT_NPC_DEATH_2 = 34;

	public const int SOUND_EFFECT_NPC_DEATH_3 = 35;

	public const int SOUND_EFFECT_ZAP = 36;

	public const int SOUND_EFFECT_EAT_3 = 37;

	public const int SOUND_EFFECT_BANG = 40;

	public const int SOUND_EFFECT_FOOT_GRAVELLY = 47;

	public const int SOUND_EFFECT_FOOT_ICE = 48;

	public const int SOUND_EFFECT_GUARDIAN_LAUGH_1 = 49;

	public const int SOUND_EFFECT_GUARDIAN_LAUGH_2 = 50;

	public static bool PlayMusic = true;

	private const int MUS_DEATH = 10;

	private const int MUS_MAP = 9;

	private const int MUS_INJURED = 8;

	private const int MUS_FLEEING = 7;

	private const int MUS_COMBAT = 6;

	private const int MUS_WEAPON = 4;

	private const int MUS_IDLE = 1;

	private const int MUS_INTRO = 0;

	private const int MUS_VICTORY = 5;

	private const int MUS_ENDGAME = 11;

	public static float LastAttackCounter = 0f;

	public bool Stopped;

	public bool InIntro;

	public bool Combat;

	public bool InMap;

	public bool EndGame;

	private int playing;

	public bool SpecialClip;

	public AudioClip[] MainTrackList = new AudioClip[32];

	public UWMusicTracks[] IntroTracks = new UWMusicTracks[1] { UWMusicTracks.UW1introduction };

	public UWMusicTracks[] IdleTracks = new UWMusicTracks[3]
	{
		UWMusicTracks.UW1darkabyss,
		UWMusicTracks.UW1descent,
		UWMusicTracks.UW1wanderer
	};

	public UWMusicTracks[] CombatTracks = new UWMusicTracks[2]
	{
		UWMusicTracks.UW1battlefield,
		UWMusicTracks.UW1combat
	};

	public UWMusicTracks[] InjuredTracks = new UWMusicTracks[1] { UWMusicTracks.UW1injured };

	public UWMusicTracks[] WeaponTracks = new UWMusicTracks[1] { UWMusicTracks.UW1armed };

	public UWMusicTracks[] VictoryTracks = new UWMusicTracks[1] { UWMusicTracks.UW1victory };

	public UWMusicTracks[] DeathTracks = new UWMusicTracks[1] { UWMusicTracks.UW1death };

	public UWMusicTracks[] FleeingTracks = new UWMusicTracks[1] { UWMusicTracks.UW1fleeing };

	public UWMusicTracks[] MapTracks = new UWMusicTracks[1] { UWMusicTracks.UW1mapslegends };

	public UWMusicTracks[] EndGameTracks = new UWMusicTracks[1] { UWMusicTracks.UW1wanderer };

	private bool StopProcessing;

	private AudioSource Aud;

	public AudioSource MusicalInstruments;

	public AudioClip[] SoundEffects = new AudioClip[1];

	private static bool ready;

	public static MusicController instance;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		Aud = GetComponent<AudioSource>();
	}

	public IEnumerator Begin()
	{
		yield return LoadGameSoundTracks(UWEBase._RES);
		ready = true;
		yield return 0;
	}

	public IEnumerator LoadGameSoundTracks(string GAME)
	{
		if (GAME != null && GAME == "UW2")
		{
			SetTrackListsUW2();
			for (int j = 1; j <= 32; j++)
			{
				yield return LoadAudioFileFromWWW(UW2Path, j);
			}
		}
		else
		{
			SetTrackListsUW1();
			for (int i = 1; i <= 15; i++)
			{
				yield return LoadAudioFileFromWWW(UW1Path, i);
			}
		}
		yield return 0;
	}

	private IEnumerator LoadAudioFileFromWWW(string AudioBank, int FileTrackNumber)
	{
		string Path = AudioBank + FileTrackNumber.ToString("d2") + ".ogg";
		if (!File.Exists(Path))
		{
			yield break;
		}
		using (WWW download = new WWW("file://" + Path))
		{
			yield return download;
			AudioClip clip = download.GetAudioClip(false);
			if (clip != null)
			{
				MainTrackList[FileTrackNumber] = clip;
			}
		}
	}

	private void SetTrackListsUW1()
	{
		IntroTracks = new UWMusicTracks[1] { UWMusicTracks.UW1introduction };
		IdleTracks = new UWMusicTracks[3]
		{
			UWMusicTracks.UW1darkabyss,
			UWMusicTracks.UW1descent,
			UWMusicTracks.UW1wanderer
		};
		CombatTracks = new UWMusicTracks[2]
		{
			UWMusicTracks.UW1battlefield,
			UWMusicTracks.UW1combat
		};
		InjuredTracks = new UWMusicTracks[1] { UWMusicTracks.UW1injured };
		WeaponTracks = new UWMusicTracks[1] { UWMusicTracks.UW1armed };
		VictoryTracks = new UWMusicTracks[1] { UWMusicTracks.UW1victory };
		DeathTracks = new UWMusicTracks[1] { UWMusicTracks.UW1death };
		FleeingTracks = new UWMusicTracks[1] { UWMusicTracks.UW1fleeing };
		MapTracks = new UWMusicTracks[1] { UWMusicTracks.UW1mapslegends };
	}

	private void SetTrackListsUW2()
	{
		IdleTracks = new UWMusicTracks[1] { UWMusicTracks.UW2castle };
		CombatTracks = new UWMusicTracks[1] { UWMusicTracks.UW1descent };
		InjuredTracks = new UWMusicTracks[1] { UWMusicTracks.UW1wanderer };
		WeaponTracks = new UWMusicTracks[1] { UWMusicTracks.UW1battlefield };
		VictoryTracks = new UWMusicTracks[1] { UWMusicTracks.UW1combat };
		DeathTracks = new UWMusicTracks[1] { UWMusicTracks.UW1death };
		FleeingTracks = new UWMusicTracks[1] { UWMusicTracks.UW2trap };
		MapTracks = new UWMusicTracks[1] { UWMusicTracks.UW2castle };
	}

	public void UpdateMusicState()
	{
		if (LastAttackCounter <= 0f)
		{
			Combat = false;
		}
		else
		{
			LastAttackCounter -= Time.deltaTime;
			InIntro = false;
			Combat = true;
		}
		if (UWCharacter.Instance.CurVIT <= 10 && Combat)
		{
			UWCharacter.Instance.Injured = true;
		}
		else
		{
			UWCharacter.Instance.Injured = false;
		}
		if (Combat && !UWCharacter.Instance.WeaponDrawn)
		{
			UWCharacter.Instance.Fleeing = true;
		}
		else
		{
			UWCharacter.Instance.Fleeing = false;
		}
		if (SpecialClip)
		{
			if (!Aud.isPlaying)
			{
				SpecialClip = false;
			}
			return;
		}
		if (!Aud.isPlaying && !StopProcessing)
		{
			playing = -1;
		}
		switch (GetMaxPriority())
		{
		case 11:
			ChangeTracklist(EndGameTracks, 10);
			break;
		case 0:
			ChangeTracklist(IntroTracks, 10);
			break;
		case 10:
			ChangeTracklist(DeathTracks, 10);
			break;
		case 9:
			ChangeTracklist(MapTracks, 9);
			break;
		case 6:
			ChangeTracklist(CombatTracks, 6);
			break;
		case 8:
			ChangeTracklist(InjuredTracks, 8);
			break;
		case 7:
			ChangeTracklist(FleeingTracks, 7);
			break;
		case 4:
			ChangeTracklist(WeaponTracks, 4);
			break;
		case 1:
			ChangeTracklist(IdleTracks, 1);
			break;
		case 2:
		case 3:
		case 5:
			break;
		}
	}

	private void Update()
	{
		if (ready)
		{
			UpdateMusicState();
		}
	}

	private void PlayRandom(UWMusicTracks[] tracklist)
	{
		if (PlayMusic)
		{
			int num = Random.Range(0, tracklist.GetUpperBound(0) + 1);
			if (tracklist[num] != (UWMusicTracks)currenttrack)
			{
				Aud.clip = MainTrackList[(int)tracklist[num]];
				if (!Stopped)
				{
					Aud.Play();
				}
			}
			currenttrack = (int)tracklist[num];
		}
		else
		{
			Aud.Stop();
		}
	}

	public void Stop()
	{
		StopProcessing = true;
		Aud.Stop();
	}

	public void StopAll()
	{
		Stop();
		Stopped = true;
	}

	public void ResumeAll()
	{
		Stopped = false;
		Resume();
	}

	public void Resume()
	{
		StopProcessing = false;
		PlayRandom(IdleTracks);
	}

	private int GetMaxPriority()
	{
		if (UWCharacter.Instance.Death)
		{
			return 10;
		}
		if (EndGame)
		{
			return 11;
		}
		if (InMap)
		{
			return 9;
		}
		if (UWCharacter.Instance.Injured)
		{
			return 8;
		}
		if (UWCharacter.Instance.Fleeing)
		{
			return 7;
		}
		if (Combat)
		{
			return 6;
		}
		if (UWCharacter.Instance.WeaponDrawn)
		{
			return 4;
		}
		if (InIntro)
		{
			return 0;
		}
		return 1;
	}

	private void ChangeTracklist(UWMusicTracks[] newTrackList, int playingMode)
	{
		if (playing != playingMode)
		{
			if (!GameWorldController.instance.AtMainMenu)
			{
				InIntro = false;
			}
			PlayRandom(newTrackList);
			playing = playingMode;
		}
	}

	public void PlaySpecialClip(UWMusicTracks[] newTrackList)
	{
		PlayRandom(newTrackList);
		SpecialClip = true;
		playing = -1;
	}

	public void ChangeTrackListForUW2(int levelNo)
	{
		switch ((GameWorldController.UW2_LevelNos)levelNo)
		{
		case GameWorldController.UW2_LevelNos.Academy0:
		case GameWorldController.UW2_LevelNos.Academy1:
		case GameWorldController.UW2_LevelNos.Academy2:
		case GameWorldController.UW2_LevelNos.Academy3:
		case GameWorldController.UW2_LevelNos.Academy4:
		case GameWorldController.UW2_LevelNos.Academy5:
		case GameWorldController.UW2_LevelNos.Academy6:
		case GameWorldController.UW2_LevelNos.Academy7:
			IdleTracks = new UWMusicTracks[2]
			{
				UWMusicTracks.UW1mapslegends,
				UWMusicTracks.UW1introduction
			};
			break;
		case GameWorldController.UW2_LevelNos.Prison0:
		case GameWorldController.UW2_LevelNos.Prison1:
		case GameWorldController.UW2_LevelNos.Prison2:
		case GameWorldController.UW2_LevelNos.Prison3:
		case GameWorldController.UW2_LevelNos.Prison4:
		case GameWorldController.UW2_LevelNos.Prison5:
		case GameWorldController.UW2_LevelNos.Prison6:
		case GameWorldController.UW2_LevelNos.Prison7:
			IdleTracks = new UWMusicTracks[2]
			{
				UWMusicTracks.UW1victory,
				UWMusicTracks.UW1introduction
			};
			break;
		case GameWorldController.UW2_LevelNos.Ice0:
		case GameWorldController.UW2_LevelNos.Ice1:
			IdleTracks = new UWMusicTracks[2]
			{
				UWMusicTracks.UW2icecaverns,
				UWMusicTracks.UW1introduction
			};
			break;
		case GameWorldController.UW2_LevelNos.Killorn0:
		case GameWorldController.UW2_LevelNos.Killorn1:
		case GameWorldController.UW2_LevelNos.Pits0:
		case GameWorldController.UW2_LevelNos.Pits1:
			IdleTracks = new UWMusicTracks[2]
			{
				UWMusicTracks.UW1fleeing,
				UWMusicTracks.UW1introduction
			};
			break;
		case GameWorldController.UW2_LevelNos.Talorus0:
		case GameWorldController.UW2_LevelNos.Talorus1:
		case GameWorldController.UW2_LevelNos.Ethereal0:
		case GameWorldController.UW2_LevelNos.Ethereal1:
		case GameWorldController.UW2_LevelNos.Ethereal2:
		case GameWorldController.UW2_LevelNos.Ethereal3:
		case GameWorldController.UW2_LevelNos.Ethereal4:
		case GameWorldController.UW2_LevelNos.Ethereal5:
		case GameWorldController.UW2_LevelNos.Ethereal6:
		case GameWorldController.UW2_LevelNos.Ethereal7:
		case GameWorldController.UW2_LevelNos.Ethereal8:
			IdleTracks = new UWMusicTracks[1] { UWMusicTracks.UW1armed };
			break;
		case GameWorldController.UW2_LevelNos.Tomb0:
		case GameWorldController.UW2_LevelNos.Tomb1:
		case GameWorldController.UW2_LevelNos.Tomb2:
		case GameWorldController.UW2_LevelNos.Tomb3:
			IdleTracks = new UWMusicTracks[2]
			{
				UWMusicTracks.UW2castle,
				UWMusicTracks.UW2icecaverns
			};
			break;
		case GameWorldController.UW2_LevelNos.Britannia2:
		case GameWorldController.UW2_LevelNos.Britannia3:
			IdleTracks = new UWMusicTracks[2]
			{
				UWMusicTracks.UW1injured,
				UWMusicTracks.UW1introduction
			};
			break;
		default:
			IdleTracks = new UWMusicTracks[2]
			{
				UWMusicTracks.UW2castle,
				UWMusicTracks.UW1introduction
			};
			break;
		}
	}
}
