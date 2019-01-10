using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestModuleProject
{
    [TestClass]
    public class UnitTestHID
    {
        [TestMethod]
        public void TestBrowseHID()
        {
            var result = HIDLib.HIDAPIs.BrowseHID();
            Assert.IsNotNull(result);
        }
    }
}
