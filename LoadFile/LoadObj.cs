namespace XMLReading_WPF
{
    class LoadObj
    {
        public long Length { get; set; }
        public string Text { get; set; }
        public LoadObj()
        {
        }
        public virtual int Read(int bufferSize = 10000000)
        {
            return 0;
        }

        public virtual void Close()
        {
        }
    }
}
