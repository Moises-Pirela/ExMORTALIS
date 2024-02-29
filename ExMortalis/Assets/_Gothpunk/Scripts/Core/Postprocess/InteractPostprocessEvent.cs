namespace Transendence.Core.Postprocess
{
    public struct InteractPostprocessEvent : IPostProcessEvent
    {
        public enum InteractionType { Open, Close, Inspect, Use }

        public int TargetEntityId;
        public int InteractDealerEntityId;
        public InteractionType Type;
    }
}


