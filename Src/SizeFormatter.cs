namespace BigFileFinder
{
    public class SizeFormatter
    {
        private readonly Func<long, string> formattingFunction;

        public SizeFormatter(bool useDecimalScale = false, Unit unit = Unit.Automatic)
        {
            int scale = useDecimalScale ? 1000 : 1024;

            this.formattingFunction = unit switch
            {
                Unit.Automatic => size => DisplaySizeWithAutomaticUnit(size, scale),
                Unit.B => size => size + " B",
                _ => size => DisplaySize(size, unit, scale)
            };
        }

        public string DisplaySize(long size)
        {
            return formattingFunction.Invoke(size);
        }

        private static string DisplaySizeWithAutomaticUnit(long size, int scale)
        {
            if (size < scale) return size + " B";
            var unit = (Unit)Math.Log(size, scale);
            return DisplaySize(size, unit, scale);
            //return Math.Round(size / Math.Pow(scale, (double) unit), 4) + $" {unit}";        
        }

        private static string DisplaySize(long size, Unit unit, int scale)
        {
            return Math.Round((float)size / Math.Pow(scale, (double)unit), 4) + " " + unit;
        }

    }

    public enum Unit
    {
        Automatic = -1, B = 0, KB = 1, MB = 2, GB = 3, TB = 4
    }
}
