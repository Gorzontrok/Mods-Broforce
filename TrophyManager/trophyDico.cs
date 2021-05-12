using System;
using System.Collections.Generic;
using TrophyManager;


public class Order
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public bool IsComplete { get; set; }
    public int Objective { get; set; }
    public int Progression { get; set; }

    public Order(string Name, string Description, string ImagePath, bool IsComplete, int Objective, int Progression)
    {
        this.Name = Name;
        this.Description = Description;
        this.ImagePath = ImagePath;
        this.IsComplete = IsComplete;
        this.Objective = Objective;
        this.Progression = Progression;
    }
}

public class TrophyDico
{

    List<TrophyDico> Trophy_dico = new List<TrophyDico>();
    public void t()
    {
        //var WhoTurnOffTheLight = new Order("Decapitate 150 enemies", "WhoTurnOffTheLight", false, 150, (Main.settings.decapitatedCount));
    }

    //                      TrophyName              =   {"DESCRIPTION",                 "IMAGE PATH" ,              bool,   int,    int progression                 };
    private static object[] WhoTurnOffTheLight      =   { "Decapitate 150 enemies",     "WhoTurnOffTheLight",       false,  150,    Main.settings.decapitatedCount  };
    private static object[] DoYouLikeMyMuscle       =   { "Make 50 enemies blind.",     "DoYouLikeMyMuscle",        false,  50 ,    Main.settings.blindCount        };
    private static object[] BOOMYouAreNowInvisible  =   { "Make 1500 enemies explode.", "BOOMYouAreNowInvisible",   false,  1500,   Main.settings.explodeCount      };
    private static object[] ForMURICA               =   { "Kill 1000 enemies.",         "ForMURICA",                false,  1000,   Main.settings.killCount         };

    public static Dictionary<String, bool> trophyDone = new Dictionary<String, bool>()
    {
        {"Who Turn Off The Light", false },
        {"Do you like my muscle ?", false },
        {"*BOOM* you are now invisible.", false },
        {"For MURICA !", false }
    };

    public static Dictionary<String, int> trophyMax = new Dictionary<String, int>()
    {
        {"Who Turn Off The Light", 150 },
        {"Do you like my muscle ?", 50 },
        {"*BOOM* you are now invisible.", 1500 },
        {"For MURICA !", 1000 }
    };

    public static Dictionary<String, int> trophyProgress = new Dictionary<String, int>()
    {
        {"Who Turn Off The Light", Main.settings.decapitatedCount},
        {"Do you like my muscle ?", Main.settings.blindCount },
        { "*BOOM* you are now invisible.", Main.settings.explodeCount },
        { "For MURICA !", Main.settings.killCount }
    };

    public static Dictionary<String, object[]> trophyIntObjective = new Dictionary<String, object[]>()
    {
        // { "NAME" , info of the trophy }
        {"Who Turn Off The Light ?!",       WhoTurnOffTheLight      },
        {"Do you like my muscle ?",         DoYouLikeMyMuscle       },
        { "*BOOM* you are now invisible.",  BOOMYouAreNowInvisible  },
        { "For MURICA !",                   ForMURICA               }
    };

    //Jesus will be proud,  Kill 50 000 terrorist
    //Guerrilla, rescue 50 villager
    //D-D-D-DOOR KILL !   , Kill someone with a door
}
