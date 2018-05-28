using FrontEndAPI.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace FrontEndAPITests
{
    public class DateUtilityTest
    {
        private readonly ITestOutputHelper _output;

        public DateUtilityTest(ITestOutputHelper output)
        {
            _output = output;
        }


        [Fact]
        public void TestUserInitialize()
        {
            DateTime dt = DateTime.ParseExact("15/05/2018 19:38", "dd/MM/yyyy HH:mm", null);
            _output.WriteLine(DateUtility.ConvertToStringWithZoneOffset(dt));
        }
    }
}
