using NUnit.Framework;

namespace VL.NUnit
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            System.Console.WriteLine("11");
        }

        [Test]
        public void Test1()
        {
            System.Console.WriteLine("22");
            Assert.Pass();
        }
    }
}