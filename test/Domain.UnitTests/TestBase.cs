using AutoFixture;
using NUnit.Framework;

namespace Domain.UnitTests;

public class TestBase
{
    protected Fixture _fixture;

    [SetUp]
    public virtual void TestSetUp()
    {
        _fixture = new Fixture();
    }
}