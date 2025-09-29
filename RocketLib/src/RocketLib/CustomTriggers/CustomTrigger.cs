using System;

namespace RocketLib.CustomTriggers
{
    public class CustomTrigger
    {
        public Type CustomTriggerActionType;
        public Type CustomTriggerActionInfoType;
        public string ActionName;
        public string Tag;
        public int Priority = 0;

        public CustomTrigger(Type customTriggerActionType, Type customTriggerActionInfoType, string actionName, string tag, int priority = 0)
        {
            CustomTriggerActionType = customTriggerActionType;
            CustomTriggerActionInfoType = customTriggerActionInfoType;
            ActionName = actionName;
            Tag = tag;
            Priority = priority;
        }
    }
}
