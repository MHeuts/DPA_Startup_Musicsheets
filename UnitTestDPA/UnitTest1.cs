using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DPA_Musicsheets.Converters.LilyPond;
using DPA_Musicsheets.Models;

namespace UnitTestDPA
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ConvertLilyPondBack()
        {
            LilyPondConverter converter = new LilyPondConverter();
            Staff song = converter.ConvertBack("");
            Assert.AreEqual(song, new Staff());
        }
    }
}
