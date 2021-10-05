namespace XMLReading_WPF
{
    class SaveObj
    {
        public bool IsReady { get; protected set; } = false;
        public SaveObj()
        {
        }
        public virtual void Add(string text)
        {
        }
        public virtual void Save()
        {

        }
        public virtual void Close()
        {
        }
    }
}
