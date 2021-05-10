using System;
using System.Collections.Generic;

public static class TrophyDico
{
    public static Dictionary<String, bool> trophyDone = new Dictionary<String, bool>()
    {
        {"Decapitated", false }
    };

    public static Dictionary<String, int> trophyMax = new Dictionary<String, int>()
    {
        {"Decapitated", 50 }
    };
}
