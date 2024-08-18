

using System.Text.RegularExpressions;

namespace BigFileFinder
{
    public partial class ArgumentParser
    {

        static readonly List<string> helpArgs = ["--help", "-?", "-h"];
        static readonly List<string> unitArgs = ["--unit", "-u"];
        static readonly List<string> binaryScaleArgs = ["--binary-scale", "-b"];
        static readonly List<string> decimalScaleArgs = ["--decimal-scale", "-d"];
        static readonly List<string> depthArgs = ["--max-depth", "-m"];
        static readonly List<string> fileRegexArgs = ["--file-regex", "--file", "-f"];
        static readonly List<string> directoryRegexArgs = ["--directory-regex", "--directory", "-e"];
        static readonly List<string> minSizeArgs = ["--min-size", "--size", "-s"];

        static readonly Regex sizeRegex = SizeRegex();

        [GeneratedRegex(@"(\d+)([a-zA-Z]+)")]
        private static partial Regex SizeRegex();

        public static Arguments Parse(string[] args)
        {
            var arguments = new Arguments();

            for (int i = 0; i < args.Length; i++)
            {
                var currentArg = args[i];
                if (helpArgs.Contains(currentArg))
                {
                    arguments.Help = true;
                    return arguments;
                }
                if (binaryScaleArgs.Contains(currentArg))
                {
                    arguments.DecimalScale = false;
                }
                else if (decimalScaleArgs.Contains(currentArg))
                {
                    arguments.DecimalScale = true;
                }
                else if (depthArgs.Contains(currentArg))
                {
                    arguments.MaxDepth = ParseInt(args, ++i);
                }
                else if (unitArgs.Contains(currentArg))
                {
                    arguments.Unit = ParseUnit(args, ++i);
                }
                else if (fileRegexArgs.Contains(currentArg))
                {
                    arguments.FileRegex = GetString(args, ++i, "fileRegex");
                }
                else if (directoryRegexArgs.Contains(currentArg))
                {
                    arguments.DirectoryRegex = GetString(args, ++i, "directoryRegex");
                }
                else if (minSizeArgs.Contains(currentArg))
                {
                    arguments.MinSize = GetSize(args, ++i, arguments.DecimalScale);
                }
            }
            return arguments;
        }

        private static Unit ParseUnit(string[] args, int i)
        {
            try
            {
                var nextArg = args[i];
                return (Unit)Enum.Parse(typeof(Unit), nextArg, true);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new InvalidArgumentsException("unit is missing", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidArgumentsException("invalid unit provided", ex);
            }
        }

        private static int ParseInt(string[] args, int i)
        {
            try
            {
                var nextArg = args[i];
                return Int32.Parse(nextArg);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new InvalidArgumentsException("max depth missing", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidArgumentsException($"invalid max depth provided", ex);
            }
        }

        private static string GetString(string[] args, int i, string argument)
        {
            if (args.Length <= i)
            {
                throw new InvalidArgumentsException($"value for {argument} is missing");
            }
            return args[i];
        }

        private static long GetSize(string[] args, int i, bool decimalScale)
        {
            try
            {
                var nextArg = args[i];
                var match = sizeRegex.Match(nextArg);
                string sizeNr = match.Groups[1].Value;
                string sizeUnit = match.Groups[2].Value;
                int size = Int32.Parse(sizeNr);
                Unit unit = (Unit)Enum.Parse(typeof(Unit), sizeUnit, true);
                if (unit == Unit.B || unit == Unit.Automatic)
                {
                    return size;
                }
                else
                {
                    return (long)Math.Pow(decimalScale ? 1000 : 1024, (double)unit) * size;
                }

            }
            catch (IndexOutOfRangeException ex)
            {
                throw new InvalidArgumentsException("minSize is missing", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidArgumentsException("invalid minSize provided", ex);
            }
        }


    }
}
