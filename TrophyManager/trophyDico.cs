using System;
using System.IO;
using System.Collections.Generic;
using TrophyManager;

public static class TrophyDico
{
    public static Dictionary<String, bool> trophyDone = new Dictionary<String, bool>()
    {
        {"Who Turn Off The Light", false },
        {"Do you like my muscle ?", false },
        {"*BOOM* you are now invisible.", false },
        {"For MURICA !", false }
    };

    public static Dictionary<String, int> trophyMax = new Dictionary<String, int>()
    {
        {"Who Turn Off The Light", 50 },
        {"Do you like my muscle ?", 20 },
        {"*BOOM* you are now invisible.", 150 },
        {"For MURICA !", 100 }
    };

    public static Dictionary<String, int> trophyProgress = new Dictionary<String, int>()
    {
        {"Who Turn Off The Light", Main.settings.decapitatedCount},
        {"Do you like my muscle ?", Main.settings.blindCount },
        { "*BOOM* you are now invisible.", Main.settings.explodeCount },
        { "For MURICA !", Main.settings.killCount }
    };
}
