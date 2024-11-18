﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Quest variables.
/// </summary>
/// 	 * per uw-specs.txt
/// 7.8.1 UW1 quest flags
/// 
/// flag   description
/// 
/// 0    Dr. Owl's assistant Murgo freed
/// 1    talked to Hagbard
/// 2    met Dr. Owl?
/// 3    permission to speak to king Ketchaval
/// 4    Goldthirst's quest to kill the gazer (1: gazer killed)
/// 5    Garamon, find talismans and throw into lava
/// 6    friend of Lizardman folk
/// 7    ?? (conv #24, Murgo)
/// 8    book from Bronus for Morlock
/// 9    "find Gurstang" quest
/// 10    where to find Zak, for Delanrey
/// 11    Rodrick killed
/// 
/// 32    status of "Knight of the Crux" quest
/// 0: no knight
/// 1: seek out Dorna Ironfist
/// 2: started quest to search the "writ of Lorne"
/// 3: found writ
/// 4: door to armoury opened
/// 
/// 
/// 
/// UW2 Quest flags
/// 0: PT related - The troll is released
/// 1: PT related - Bishop is free?
/// 2: PT related - Borne wants you to kill bishop
/// 3: PT related - Bishop is dead
/// 
/// 6: Helena asks you to speak to praetor loth
/// 7: Loth is dead
/// 8:  Kintara tells you about Javra
/// 9:  Lobar tells you about the "virtues"
/// 10: PT related
/// 11: Listener under the castle is dead.
/// 12: used in Ice caverns to say the avatar can banish the guardians presence. Wand of altara?
/// 13: Mystral wants you to spy on altara
/// 
/// 15: Altara tells you about the listener
/// 
/// 18: You learn that the Trikili can talk.
/// 19: You know Relk has found the black jewel (josie tells you)
/// 20: You've met Mokpo
/// 22: Blog is now your friend(?)
/// 23: You have used Blog to defeat dorstag
/// 24: You have won a duel in the arena
/// 25: You have defeated Zaria in the pits
/// 26: You know about the magic scepter (for the Zoranthus)
/// 27: You know about the sigil of binding/got the djinn bottle(by bringing the scepter to zorantus)
/// 28: Took Krilner as a slave
/// 29: You know Dorstag has the gem(?)
/// 30: Dorstag refused your challenge(?)
/// 32: Met a Talorid
/// 33: You have agreed to help the talorid (historian)
/// 34: Met or heard of Eloemosynathor
/// 35: The vortz are attacking!
/// 36: Quest to clarify question for Data Integrator
/// 37: talorus related (checked by futurian)
/// 38: talorus related *bliy scup is regenerated
/// 39: talorus related
/// 40: Dialogian has helped with data integrator
/// 43: Patterson has gone postal
/// 45: Janar has been met and befriended
/// 
/// 47: You have recieve the quest from the triliki
/// 48: You have dreamed about the void
/// 49: Bishop tells you about the gem.
/// 50: The keep is going to crash.
/// 51: You have visited the ice caves (britannia becomes icy)
/// 54: Checked by Mors Gotha? related to keep crashing
/// 55: Banner of Killorn returned (based on Scd.ark research)
/// 58: Set when meeting bishop. Bishop tells you about altara
/// 60: Possible prison tower variable. used in check trap on second highest level
/// 61: You've brought water to Josie
/// 
/// 63: Blog has given you the gem
/// 64: Is mors dead
/// 65: Pits related (checked by dorstag)
/// 68: You have given the answers to nystrul and the invasion (endgame) has begun.
/// 104: Set when you enter scintilus level 5 (set by variable trap)
/// 106: Meet mors gothi and got the book
/// 107: Set after freeing praetor loth and you/others now know about the horn.
/// 109: Set to 1 after first LB conversation. All castle occupants check this on first talk.
/// 110: Checked when talking to LB and Dupre. The guardians forces are attacking
/// 112: checked when talking to LB. You have been fighting with the others
/// 114: checked when talking to LB. The servants are restless
/// 115: checked when talking to LB. The servants are on strike
/// 116: The strike has been called off.
/// 117: Mors has been defeated in Kilorn
/// 118: The wisps tell you about the trilikisssss2
/// 119: Fizzit the thief surrenders
/// 120: checked by miranda?
/// 121: You have defeated Dorstag
/// 122: You have killed the bly scup ductosnore
/// 123: Relk is dead
/// 128: 0-128 bit field of where the lines of power have been broken.
/// 129: How many enemies killed in the pits (also xclock 14)
/// 131: You are told that you are in the prison tower =1  
/// 	You are told you are in kilhorn keep =3
/// 	You are told you are in scintilus = 19
/// 	(this is probably a bit field.)
/// 132: Set to 2 during Kintara conversation
/// 133: How much Jospur owes you for fighting in the pits
/// 134: The password for the prison tower (random value)
/// 135: Checked by goblin in sewers  (no of worms killed on level. At more than 8 they give you fish)
/// 143: Set to 33 after first LB conversation. Set to 3 during endgame (is this what triggers the cutscenes?)
public class Quest : UWEBase
{
    /// <summary>
    /// The quest variable integers
    /// </summary>
    /// Typically these are for story advancement and conversations.
    public int[] QuestVariables = new int[256];

    /// <summary>
    /// The game Variables for the check/set variable traps
    /// </summary>
    /// Typically these are used for traps/triggers/switches.
    public int[] variables = new int[128];

    /// <summary>
    /// Additional variables in UW2. Possibly these are all bit fields hence the name Only known usage is the scintillus 5 switch puzzle
    /// </summary>
    public int[] BitVariables = new int[128];

    /// <summary>
    /// The x clocks tracks progress during the game and is used in firing events.
    /// </summary>
    /// The xclock is a range of 16 variables. When references by traps they index is -16 to get the below values.
    /// The X Clock is tied closely to SCD.ark and the scheduled events within that file.
    /// 1=Miranda conversations & related events in the castle
    ///     1 - Nystrul is curious about exploration.Set after entering lvl 1 from the route downwards. (set variable traps 17 may be related)
    ///     2 - After you visited another world.  (variable 17 is set to 16), dupre is tempted
    ///     3 - servants getting restless
    ///     4 - concerned about water, dupre is annoyed by patterson
    ///     5 - Dupre is bored / dupre is fetching water
    ///     7 - Miranda wants to talk to you pre tori murder
    ///     8 - tori is murdered
    ///     9 - Charles finds a key
    ///     11 - Go see Nelson
    ///     12 - Patterson goes postal
    ///     13 - Patterson is dead
    ///     14 - Gem is weak/Mors is in killorn(?)
    ///     15 - Nystrul wants to see you again re endgame
    ///     16 - Nystrul questions have been answered Mars Gotha comes
    /// 2=Nystrul and blackrock gems treated
    /// 3=Djinn Capture
    ///     2 = balisk oil is in the mud
    ///     3 = bathed in oil
    ///     4 = baked in lava
    ///     5 = iron flesh cast (does not need to be still on when you break the bottle)
    ///     6 = djinn captured in body
    /// 14=Tracks no of enemies killed in pits. Does things like update graffiti.
    /// 15=Used in multiple convos. Possibly tells the game to process a change when updated?
    public int[] x_clocks = new int[16];


    /// <summary>
    /// Item ID for the sword of justice
    /// </summary>
    public const int TalismanSword = 10;
    /// <summary>
    /// Item ID for the shield of valor
    /// </summary>
    public const int TalismanShield = 55;
    /// <summary>
    /// Item id for the Taper of sacrifice
    /// </summary>
    public const int TalismanTaper = 147;
    public const int TalismanTaperLit = 151;
    /// <summary>
    /// Item Id for the cup of wonder.
    /// </summary>
    public const int TalismanCup = 174;
    /// <summary>
    /// Item ID for the book of honesty
    /// </summary>
    public const int TalismanBook = 310;
    /// <summary>
    /// Item Id for the wine of compassion.
    /// </summary>
    public const int TalismanWine = 191;
    /// <summary>
    /// Item ID for the ring of humility
    /// </summary>
    public const int TalismanRing = 54;
    /// <summary>
    /// Item ID for the standard of honour.
    /// </summary>
    public const int TalismanHonour = 287;

    /// <summary>
    /// The no of talismans to still be cast into abyss in order to complete the game.
    /// </summary>
    public int TalismansRemaining; //= new bool[8];

    /// <summary>
    /// Tracks which garamon dream we are at.
    /// </summary>
    public int GaramonDream;//The next dream to play

    /// <summary>
    /// Tracks which incense dream we are at
    /// </summary>
    public int IncenseDream;

    /// <summary>
    /// Tracks the last day that there was a garamon dream.
    /// </summary>
    public int DayGaramonDream = -1;

    /// <summary>
    /// Is the orb on tybals level destroyed.
    /// </summary>
    public bool isOrbDestroyed;

    /// <summary>
    /// Has Garamon been buried. If so talismans can now be sacrificed.
    /// </summary>
    public bool isGaramonBuried;

    /// <summary>
    /// Is the cup of wonder found.
    /// </summary>
    public bool isCupFound;

    /// <summary>
    /// Is the player fighting in arena.
    /// </summary>
    public bool FightingInArena = false;

    /// <summary>
    /// The arena opponents item ids
    /// </summary>
    public int[] ArenaOpponents = new int[5];

    /// <summary>
    /// Has the player eaten a dream plant.
    /// </summary>
    public bool DreamPlantEaten = false;

    /// <summary>
    /// Is the player in the dream world
    /// </summary>
    public bool InDreamWorld = false;

    public static Quest instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        switch (_RES)
        {
            case GAME_UW2:
                QuestVariables = new int[147];
                break;
            default:
                QuestVariables = new int[36];
                break;
        }
    }


    /// <summary>
    /// Gets the next incense dream
    /// </summary>
    /// <returns>The incense dream.</returns>
    public int getIncenseDream()
    {
        if (IncenseDream >= 3)
        {//Loop around
            IncenseDream = 0;
        }
        return IncenseDream++;
    }
}
