using Microsoft.VisualStudio.TestTools.UnitTesting;
using SRVN_time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRVN_time.Tests
{
    [TestClass()]
    public class RaceTests
    {
        [TestMethod()]
        public void RaceTest()
        {
            var rc = new Race("R101V-1");
            Assert.IsTrue(rc.IsValid());
        }
    }
}