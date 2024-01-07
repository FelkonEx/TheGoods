using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CPHInline
{
    public static Random rnd = new Random();

    public List<string> hairUrlList = new List<string>{
        "C:/Users/Tim/Pictures/TWITCH/VT00BA/V2.6/FULL/HEAD/HEAD_UP.png",
        "C:/Users/Tim/Pictures/TWITCH/VT00BA/V2.6/FULL/HEAD/HEAD_HALF.png",
        "C:/Users/Tim/Pictures/TWITCH/VT00BA/V2.6/FULL/HEAD/HEAD_DOWN.png"
    };

    public string hairSceneSrc = "[S] PNGTuber | Hair";
    public string hairImgSrc = "[I] PNGTuber | Head - Hair";

    public bool Execute()
    {
        int currentHairVersionNum;
        Int32.TryParse(args["pngHairVer"].ToString(), out currentHairVersionNum);

        if (currentHairVersionNum + 1 == hairUrlList.Count)
        {
            currentHairVersionNum = 0;
        }
        else
        {
            currentHairVersionNum++;
        }

        CPH.ObsSetImageSourceFile(hairSceneSrc, hairImgSrc, hairUrlList[currentHairVersionNum], 0);
        CPH.SetGlobalVar("pngHairVer", currentHairVersionNum);
        return true;
    }
}