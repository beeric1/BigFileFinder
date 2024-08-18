using BigFileFinder;

namespace FileInfoTest
{
    public class SizeFormatterTest
    {

        [TestCase(0, false, Unit.Automatic, ExpectedResult = "0 B")]
        [TestCase(123, false, Unit.Automatic, ExpectedResult = "123 B")]
        [TestCase(1, false, Unit.Automatic, ExpectedResult = "1 B")]
        [TestCase(5 * 1024, false, Unit.Automatic, ExpectedResult = "5 KB")]
        [TestCase(7000, false, Unit.Automatic, ExpectedResult = "6.8359 KB")]
        [TestCase(7_000_000, true, Unit.Automatic, ExpectedResult = "7 MB")]
        [TestCase(7_000_000, true, Unit.GB, ExpectedResult = "0.007 GB")]
        [TestCase(7000, true, Unit.Automatic, ExpectedResult = "7 KB")]
        [TestCase(7000, true, Unit.B, ExpectedResult = "7000 B")]
        [TestCase(7000, false, Unit.B, ExpectedResult = "7000 B")]
        [TestCase(90_000_000_000, true, Unit.Automatic, ExpectedResult = "90 GB")]
        [TestCase(1_000_000_000_000, true, Unit.Automatic, ExpectedResult = "1 TB")]
        [TestCase(1_000_000_000_000, false, Unit.Automatic, ExpectedResult = "931.3226 GB")]
        public string DisplaySize(long size, bool useDecimalScale, Unit unit)
        {
            return new BigFileFinder.SizeFormatter(useDecimalScale, unit).DisplaySize(size);
        }

    }
}