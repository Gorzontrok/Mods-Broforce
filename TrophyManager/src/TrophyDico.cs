using System;
using System.Collections.Generic;
using TrophyManager;


public class TrophyDico
{    //                      TrophyName              =   {"DESCRIPTION",                 "IMAGE PATH" ,                 bool,   int,    int progression                 };
    private static object[] WhoTurnOffTheLight      =   { "Decapitate 150 enemies",     "WhoTurnOffTheLight.png",       false,  150,    Main.settings.decapitatedCount  };
    private static object[] DoYouLikeMyMuscle       =   { "Make 50 enemies blind.",     "DoYouLikeMyMuscle.png",        false,  50 ,    Main.settings.blindCount        };
    private static object[] BOOMYouAreNowInvisible  =   { "Make 1500 enemies explode.", "BOOMYouAreNowInvisible.png",   false,  1500,   Main.settings.explodeCount      };
    private static object[] ForMURICA               =   { "Kill 1000 enemies.",         "ForMURICA.png",                false,  1000,   Main.settings.killCount         };
    private static object[] JesusWillBeProud        =   { "Kill 50 000 enemies.",       "JesusWillBeProud.png",         false,  50000,  Main.settings.killCount         };
    private static object[] Guerrilla               =   { "Rescue 50 villager.",        "Guerrilla.png",                false,  50,     Main.settings.villagerCount     };
    private static object[] DoorKill                =   { "Kill Someone with a door",   "DoorKill.png",                 false,  1,      Main.settings.doorKillCount     };

    public static Dictionary<String, object[]> trophyIntObjective = new Dictionary<String, object[]>()
    {
        //Can't put directly the info without declaring a variable.

        // { "NAME" ,                       info of the trophy      }
        {"Who Turn Off The Light ?!",       WhoTurnOffTheLight      },
        {"Do you like my muscle ?",         DoYouLikeMyMuscle       },
        { "*BOOM* you are now invisible.",  BOOMYouAreNowInvisible  },
        { "For MURICA !",                   ForMURICA               },
        { "Jesus will be proud.",           JesusWillBeProud        },
        { "Guerrilla.",                     Guerrilla               },
        { "D-D-D-DOOR KILL !",              DoorKill                }
    };
}
