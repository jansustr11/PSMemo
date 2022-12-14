using Moq;
using PSMemo.Cmdlets.Common;
using PSMemo.Repository;
using System.Linq;
using PSMemo.Tests;
using PSMemo.Cmdlets;
using static PSMemo.Tests.TestUtils;
using System.IO.Abstractions;
using FluentAssertions;

namespace PSMemo.Tests.Integration.Cmdlets;

public class AddMemoTests
{
    [Fact]
    public void AddOneItem()
    {
        var mockRuntime = new MockCommandRuntime<string>();
        var mockFileSystem = new Mock<IFileSystem>();

        string[]? writtenLines = null;

        mockFileSystem.Setup(x => x.File.Exists(It.IsAny<string>()))
            .Returns(true);
        mockFileSystem.Setup(x => x.File.ReadAllLines(It.IsAny<string>()))
            .Returns(new string[] { "item1", "item2" });
        mockFileSystem.Setup(x => x.File.WriteAllLines(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
            .Callback((string _, IEnumerable<string> _lines) => writtenLines = _lines.ToArray());

        var repo = new MemoFileSystemRepository(mockFileSystem.Object, "X:\\test");

        var cmdlet = new AddMemo()
        {
            CommandRuntime = mockRuntime,
            Repository = repo,
            Key = "a.b.c",
            Value = "newItem"
        };

        Execute(cmdlet);

        string expectedPath = @"X:\test\a.b.c.memo";
        mockFileSystem.Verify(x => x.File.Exists(expectedPath), Times.Once);
        mockFileSystem.Verify(x => x.File.ReadAllLines(expectedPath), Times.Once);
        mockFileSystem.Verify(x => x.File.WriteAllLines(expectedPath, It.IsAny<IEnumerable<string>>()), Times.Once);

        mockRuntime.Output.Should().HaveCount(0);

        writtenLines.Should().Equal("newItem", "item1", "item2");
    }
}