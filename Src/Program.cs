namespace BigFileFinder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var arguments = GetArgumentsOrExit(args);
            if (arguments.Help)
            {
                PrintHelpPage();
            }
            else
            {
                Run(arguments);
            }

        }

        private static void Run(Arguments arguments)
        {
            try
            {
                var sizeFormatter = new SizeFormatter(arguments.DecimalScale, arguments.Unit);
                var printer = new Printer(sizeFormatter, arguments.MaxDepth, arguments.FileRegex, arguments.DirectoryRegex, arguments.MinSize);

                var currentDir = Directory.GetCurrentDirectory();
                var dirInfo = new DirectoryInfo(currentDir);
                printer.PrintDirectory(dirInfo);

            }
            catch (InvalidArgumentsException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private static Arguments GetArgumentsOrExit(string[] args)
        {
            try
            {
                return ArgumentParser.Parse(args);
            }
            catch (InvalidArgumentsException ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
                return null;
            }
        }

        private static void PrintHelpPage()
        {
            Console.WriteLine(helpText);
        }

        private static readonly String helpText = """
Optional arguments:
     unit:  
        Defines in which unit files sizes are displayed.
        Possible values (not case sensitive): Automatic, B, KB, MB, GB, TB
        Default value: Automatic
        Name: --unit
        Alias: -u
        Sample: --unit KB

     scale: 
        Determines wheter to treat 1 KB as 1024 or 1000 Bytes
        Default: binary scale (same as Windows Explorer does it)
        Name: --binary-scale | --decimal-scale
        Alias: -b            |  -d
            
     minSize:   
        Ignores all files smaller than the provided file size
        Possible values: Number + unit (from unit argument except automatic)
        Considers the scale (binary/decimal) argument for the conversion if it comes before the minSize argument
        Name: --min-size
        Aliases: --size, -s
        Sample: --min-size 1GB

     depth: 
        Defines the maximal directory depth that is explored (starting from the current directoy)
        Value: integer number
        Default value: 10
        Name: --max-depth
        Alias: -m
        Sample: --max-depth 3

     fileRegex: 
        Regular expression that is applied on the file name (only matches are displayed)
        Value: regex (as a string)
        Default: no filtering
        Name: --file-regex
        Aliases: --file, -f
        Sample: --file-regex txt

     directoryRegex:    
        Regular expression that is applied on the file name (only matches are displayed)
        Value: regex (as a string)
        Default: no filtering
        Name: --directory-regex
        Aliases: --directory, -e
        Sample: -e "^.{1,5}$"

    help:   
        Displays this help page (takes priority over any other argument)
        Name: --help
        Aliases: -?, -h
""";
    }
}
