namespace RocketLib.CustomTriggers
{
    public abstract class LevelStartTriggerAction<TInfo> : CustomTriggerAction<TInfo> where TInfo : LevelStartTriggerActionInfo
    {
        protected abstract void ExecuteAction(bool isLevelStart);

        public override TriggerActionInfo Info
        {
            get { return info; }
            set
            {
                info = (TInfo)value;
                if (info.RunAtLevelStart)
                {
                    ExecuteAction(isLevelStart: true);
                }
            }
        }

        public override void Start()
        {
            base.Start();
            if (!info.RunAtLevelStart)
            {
                ExecuteAction(isLevelStart: false);
            }
            this.state = TriggerActionState.Done;
        }

        public override void Update() { }
    }
}
