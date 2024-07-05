namespace BlumClickWinForm
{
    internal struct Offset
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        public int[] baseOffset;

        public void SaveBaseOffset()
        {
            baseOffset = new int[4] { Left, Top, Right, Bottom };
        }
    }
}
