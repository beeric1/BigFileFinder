namespace BigFileFinder
{
    public record Arguments
    {

        public Arguments(
            Unit unit = Unit.Automatic,
            bool decimalScale = false,
            int maxDepth = 10,
            string? fileRegex = null,
            string? directoryRegex = null,
            long? minSize = null,
            bool help = false
            )
        {
            Unit = unit;
            DecimalScale = decimalScale;
            MaxDepth = maxDepth;
            FileRegex = fileRegex;
            DirectoryRegex = directoryRegex;
            MinSize = minSize;
            Help = help;

        }
        public Unit Unit { get; set; }
        public bool DecimalScale { get; set; }
        public int MaxDepth { get; set; }
        public string? FileRegex { get; set; }
        public string? DirectoryRegex { get; set; }
        public long? MinSize { get; set; }
        public bool Help { get; set; }
    }
}
