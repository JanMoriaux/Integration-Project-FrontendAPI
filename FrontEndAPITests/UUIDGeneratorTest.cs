using FrontEndAPI.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace FrontEndAPITests
{
    public class UUIDGeneratorTest
    {
        private readonly ITestOutputHelper _output;

        public UUIDGeneratorTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TestGenerateUserUUID()
        {
            _output.WriteLine(UUIDGenerator.GenerateUserUUID("test@test.com"));
        }

    }
}
