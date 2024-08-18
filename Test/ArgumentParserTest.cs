using BigFileFinder;

namespace FileInfoTest
{
    internal class ArgumentParserTest
    {

        [Test]
        public void EmptyArrayReturnsDefaultArguments()
        {
            Assert.That(ArgumentParser.Parse(Array.Empty<string>()),
                Is.EqualTo(new Arguments()));
        }

        [TestCase("--help")]
        [TestCase("-?")]
        [TestCase("-h")]
        [TestCase("--help", "-h")]
        public void HelpIsTrue(params string[] args)
        {
            Assert.That(ArgumentParser.Parse(args),
                Is.EqualTo(new Arguments(help: true)));
        }

        [TestCase(Unit.B, "--unit", "B")]
        [TestCase(Unit.B, "--unit", "b")]
        [TestCase(Unit.GB, "-u", "GB")]
        [TestCase(Unit.GB, "-u", "GB", "some", "other", "args")]
        [TestCase(Unit.Automatic, "--unit", "Automatic")]
        [TestCase(Unit.Automatic, "--unit", "automatic")]
        [TestCase(Unit.KB, "--unit", "automatic", "-u", "kb")]
        public void UnitIsRecognized(Unit unit, params string[] args)
        {
            Assert.That(ArgumentParser.Parse(args),
                Is.EqualTo(new Arguments(unit: unit)));
        }

        [Test]
        public void MissingUnitThrowsException()
        {
            var ex = Assert.Throws<InvalidArgumentsException>(() => ArgumentParser.Parse(new String[] { "--unit" }));
            Assert.That(ex.Message, Is.EqualTo("unit is missing"));
        }

        [Test]
        public void InvalidUnitThrowsException()
        {
            var ex = Assert.Throws<InvalidArgumentsException>(() => ArgumentParser.Parse(new String[] { "--unit", "abc" }));
            Assert.That(ex.Message, Is.EqualTo("invalid unit provided"));
        }

        [TestCase("--decimal-scale")]
        [TestCase("-d")]
        [TestCase("--decimal-scale", "-d")]
        public void DecimalScaleIsTrue(params string[] args)
        {
            Assert.That(ArgumentParser.Parse(args),
                Is.EqualTo(new Arguments(decimalScale: true)));
        }

        [TestCase("--binary-scale")]
        [TestCase("-b")]
        [TestCase("--binary-scale", "-b")]
        [TestCase("")]
        public void DecimalScaleIsFalse(params string[] args)
        {
            Assert.That(ArgumentParser.Parse(args),
                Is.EqualTo(new Arguments(decimalScale: false)));
        }

        [TestCase(1, "--max-depth", "1")]
        [TestCase(23, "-m", "23")]
        [TestCase(5, "--max-depth", "1", "-m", "5")]
        public void MaxDepthIsRecognized(int expected, params string[] args)
        {
            Assert.That(ArgumentParser.Parse(args),
                Is.EqualTo(new Arguments(maxDepth: expected)));
        }

        [Test]
        public void MissingMaxDepthThrowsException()
        {
            var ex = Assert.Throws<InvalidArgumentsException>(() => ArgumentParser.Parse(new String[] { "-m" }));
            Assert.That(ex.Message, Is.EqualTo("max depth missing"));
        }

        [Test]
        public void InvalidMaxDepthThrowsException()
        {
            var ex = Assert.Throws<InvalidArgumentsException>(() => ArgumentParser.Parse(new String[] { "--max-depth", "a" }));
            Assert.That(ex.Message, Is.EqualTo("invalid max depth provided"));
        }

        [TestCase("json", "--file-regex", "json")]
        [TestCase("[a-z]+", "--file", "[a-z]+")]
        [TestCase(".*", "-f", ".*")]
        public void ReadFileRegex(string expected, params string[] args)
        {
            Assert.That(ArgumentParser.Parse(args), Is.EqualTo(new Arguments(fileRegex: expected)));
        }

        [TestCase("folder", "--directory-regex", "folder")]
        [TestCase("[a-z]+", "--directory", "[a-z]+")]
        [TestCase(".*", "-e", ".*")]
        public void ReadDirectoryRegex(string expected, params string[] args)
        {
            Assert.That(ArgumentParser.Parse(args), Is.EqualTo(new Arguments(directoryRegex: expected)));
        }

        [TestCase(2342342, "--min-size", "2342342B")]
        [TestCase(12_288, "--size", "12KB")]
        [TestCase(2_097_152, "-s", "2MB")]
        [TestCase(2_000_000, "--decimal-scale", "-s", "2MB")]
        public void MinSizeIsRecognized(long? expected, params string[] args)
        {
            Assert.That(ArgumentParser.Parse(args).MinSize, Is.EqualTo(expected));
        }

        [TestCase(Unit.B, false, 5, null, null, null, false, "--unit", "B", "--binary-scale", "--max-depth", "5")]
        [TestCase(Unit.Automatic, true, 3, "txt", null, null, false, "-d", "-m", "3", "-u", "automatic", "--file", "txt")]
        [TestCase(Unit.MB, false, 10, "json", "dirName", null, false, "--directory-regex", "dirName", "-u", "b", "-u", "gb", "--file-regex", "json", "-u", "B", "-u", "mb")]
        public void ComplexArgumentsAreRecognized(
            Unit unit,
            bool useDecimal,
            int maxDepth,
            string? fileRegex,
            string? directoryRegex,
            int? minSize,
            bool help,
            params string[] args)
        {
            Assert.That(ArgumentParser.Parse(args),
                Is.EqualTo(new Arguments(unit, useDecimal, maxDepth, fileRegex, directoryRegex, minSize, help)));
        }
    }
}
