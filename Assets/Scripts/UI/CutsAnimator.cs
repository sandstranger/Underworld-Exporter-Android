﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CutsAnimator : GuiBase
{
    public bool Reset;
    public int frameIndex;
    public Texture2D anim_base;//Transparent image
    public Texture2D black;
    public Texture2D red;
    public Texture2D orange;
    public RawImage TargetControl;
    public string SetAnimation = "";
    public string PrevAnimation = "";
    bool mode;//True if using cs file
    public bool looping;
    CutsLoader cuts;
    public Material UI_UNLIT;//Special material for transparency issues.

    //public Texture2D test;

    public override void Start()
    {
        base.Start();
        TargetControl.texture = anim_base;
        mode = false;
    }

    public override void Update()
    {
        {
            if (PrevAnimation != SetAnimation)
            {
                PrevAnimation = SetAnimation;
                PlayAnimFile(SetAnimation);
            }
        }
    }

    IEnumerator cutscenerunner()
    {
        while (Reset == false)
        {
            if (mode == true)//Playing a cuts file
            {
                if (frameIndex > cuts.ImageCache.GetUpperBound(0))
                {
                    if (looping)
                    {
                        frameIndex = 0;
                        TargetControl.texture = cuts.ImageCache[frameIndex++];
                    }
                    else
                    {//fade to black
                        TargetControl.texture = black;
                        PostAnimPlay();
                    }
                }
                else
                {
                    TargetControl.texture = cuts.ImageCache[frameIndex++];
                }

            }
            else
            {//Showing a static image
             //Do nothing/quit out of routine.
                Reset = true;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }


    void PostAnimPlay()
    {//Code to call at the end of some animations.
        switch (SetAnimation.ToLower())
        {
            case "uw2resurrecttransition":
            case "death_with_sapling"://Player will resurrect,
            case "cs402.n01":
                {
                    SetAnimation = "Anim_Base";
                    //Clears out the animation.
                    PrevAnimation = "x";
                    switch (_RES)
                    {
                        case GAME_UW2:
                            UWCharacter.ResurrectPlayerUW2(); break;
                        default:
                            UWCharacter.ResurrectPlayerUW1(); break;
                    }

                    break;
                }
            case "death"://PLayer is dying and will not resurrect.
            case "cs403.n01":
                SetAnimation = "cs403.n02";
                break;
            case "death_final"://Player has finally died.
            case "cs403.n02":
                SetAnimation = "Anim_Base";
                GameWorldController.instance.AtMainMenu = true;
                //Clear out game objects
                foreach (Transform child in GameWorldController.instance.LevelModel.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                foreach (Transform child in GameWorldController.instance.DynamicObjectMarker())
                {
                    GameObject.Destroy(child.gameObject);
                }
                foreach (Transform child in GameWorldController.instance.SceneryModel.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                for (int x = 0; x <= GameWorldController.instance.Tilemaps.GetUpperBound(0); x++)
                {
                    GameWorldController.instance.Tilemaps[x] = null;
                }
                for (int x = 0; x <= GameWorldController.instance.objectList.GetUpperBound(0); x++)
                {
                    GameWorldController.instance.objectList[x] = null;
                }
                UWHUD.instance.mainmenu.gameObject.SetActive(true);
                UWHUD.instance.mainmenu.MenuMode = 0;
                UWHUD.instance.mainmenu.OpScr.SetActive(true);
                UWHUD.instance.mainmenu.CharGen.SetActive(false);
                UWHUD.instance.mainmenu.ButtonClickMainMenu(4);//reset menu
                GameWorldController.instance.LevelNo = -1;
                WindowDetectUW.SwitchFromMouseLook();
                break;
            default:
                UWCharacter.Instance.playerCam.cullingMask = HudAnimation.NormalCullingMask;
                SetAnimation = "Anim_Base";
                break;
        }
    }


    void PlayAnimFile(string animName)
    {
        UWCharacter.Instance.playerCam.cullingMask = 0;
        Reset = true;
        frameIndex = 0;
        StopAllCoroutines();
        switch (SetAnimation.ToLower())
        {
            case "fadetored":
                TargetControl.material = UI_UNLIT;
                TargetControl.texture = red;
                break;
            case "uw2resurrecttransition"://UW2 death by ally or in alternate dimension
                TargetControl.material = UI_UNLIT;
                TargetControl.texture = black;
                mode = false;looping = false;
                StartCoroutine(CutsceneWait(0.5f));
                break;
            case "fadetoblacksleep":
                TargetControl.material = UI_UNLIT;
                TargetControl.texture = black;
                break;
            case "cuts_blank":
            case "":
            case "anim_base":
                TargetControl.material = null;
                UWCharacter.Instance.playerCam.cullingMask = HudAnimation.NormalCullingMask;
                TargetControl.texture = anim_base;
                break;
            case "splashlookingglass":
                TargetControl.material = UI_UNLIT;
                TargetControl.texture = (Texture2D)GameWorldController.instance.bytloader.LoadImageAt(7);
                break;
            case "splashoriginea":
                TargetControl.material = UI_UNLIT;
                TargetControl.texture = (Texture2D)GameWorldController.instance.bytloader.LoadImageAt(6);
                break;
            case "splashorigin":
            case "pres1_0000":
                TargetControl.material = UI_UNLIT;
                TargetControl.texture = (Texture2D)GameWorldController.instance.bytloader.LoadImageAt(BytLoader.PRES1_BYT);
                break;
            case "splashbluesky":
            case "pres2_0000":
                TargetControl.material = UI_UNLIT;
                TargetControl.texture = (Texture2D)GameWorldController.instance.bytloader.LoadImageAt(BytLoader.PRES2_BYT);
                break;
            case "splashorigindemo":
                TargetControl.material = UI_UNLIT;
                TargetControl.texture = (Texture2D)GameWorldController.instance.bytloader.LoadImageAt(BytLoader.PRESD_BYT);
                break;
            case "almricsitting"://special case
                TargetControl.material = UI_UNLIT;
                cuts = new CutsLoader("cs000.n11");
                TargetControl.texture = cuts.ImageCache[0];
                break;
            case "death_with_sapling":
                TargetControl.material = UI_UNLIT;
                cuts = new CutsLoader("cs402.n01");
                mode = true; Reset = false;
                TargetControl.texture = cuts.ImageCache[0];
                looping = false;
                StartCoroutine(cutscenerunner());
                break;
            case "death":
                TargetControl.material = UI_UNLIT;
                cuts = new CutsLoader("cs403.n01");
                mode = true; Reset = false;
                looping = false;
                TargetControl.texture = cuts.ImageCache[0];
                StartCoroutine(cutscenerunner());
                break;
            case "death_final":
                TargetControl.material = UI_UNLIT;
                cuts = new CutsLoader("cs403.n02");
                TargetControl.texture = cuts.ImageCache[0];
                looping = false;
                mode = true; Reset = false;
                StartCoroutine(cutscenerunner());
                break;
            case "ChasmMap":
                TargetControl.material = UI_UNLIT;
                cuts = new CutsLoader("cs410.n01");
                TargetControl.texture = cuts.ImageCache[0];
                break;
            case "Anvil":
                cuts = new CutsLoader("cs404.n01");
                TargetControl.texture = cuts.ImageCache[0];
                StartCoroutine(cutscenerunner());
                break;
            //case "c401.n01":

            default:
                if (SetAnimation.Substring(0, 7) == "Volcano")
                {
                    cuts = new CutsLoader("cs400.n01");
                    int index = int.Parse(SetAnimation.Substring(SetAnimation.Length - 1, 1));
                    TargetControl.texture = cuts.ImageCache[index];
                    TargetControl.material = UI_UNLIT;
                }
                else if ((SetAnimation.Substring(0, 5) == "Grave"))
                {//Graves
                    TargetControl.material = UI_UNLIT;
                    cuts = new CutsLoader("cs401.n01");
                    //TargetControl.texture = cuts.ImageCache[0];
                    //break;	
                    string regexForNumber = "([-+]?[0-9]*\\.?[0-9]+)";
                    Match GraveID = Regex.Match(SetAnimation, regexForNumber);
                    if (GraveID.Success)
                    {
                        int value = int.Parse(GraveID.Groups[0].Value);
                        TargetControl.texture = cuts.ImageCache[value];
                    }
                }
                else
                {
                    TargetControl.material = UI_UNLIT;
                    mode = true;
                    Reset = false;
                    cuts = new CutsLoader(animName.Replace("_", "."));
                    StartCoroutine(cutscenerunner());
                }

                break;
        }
    }

    /// <summary>
    /// Hold on a cutscene for a period before calling postanimplay()
    /// </summary>
    /// <param name="waittime"></param>
    /// <returns></returns>
    IEnumerator CutsceneWait(float waittime)
    {
        yield return new WaitForSeconds(0.2f);
        PostAnimPlay();
    }
}
