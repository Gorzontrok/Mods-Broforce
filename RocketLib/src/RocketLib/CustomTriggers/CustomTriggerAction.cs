namespace RocketLib.CustomTriggers
{
    public abstract class CustomTriggerAction<TInfo> : TriggerAction where TInfo : CustomTriggerActionInfo
    {
        protected TInfo info;

        public override TriggerActionInfo Info
        {
            get { return info; }
            set { info = (TInfo)value; }
        }
    }
}
