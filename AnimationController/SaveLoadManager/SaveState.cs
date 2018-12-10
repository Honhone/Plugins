namespace AnimationController
{
    public struct SaveHolder
    {
        public SerializableDictionary<int, SaveState> Buffer { get; set; }
    }
    public struct SaveState
    {
        public OCICtrlType CtrlType;
        public float X;
        public float Y;
    }
    public enum OCICtrlType //probably pointless
    {
        Character = 0,
        Object = 1
    };
} 
