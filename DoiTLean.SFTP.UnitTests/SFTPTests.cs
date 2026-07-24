using NUnit.Framework;
using DoiTLean.SFTP.Structures;
using System;
using System.Reflection;

namespace DoiTLean.SFTP.UnitTests;

/// <summary>
/// Tests for the RemoteItem structure exposed to OutSystems consumers.
/// </summary>
public class RemoteItemTests
{
    [Test]
    public void Constructor_SetsAllFields()
    {
        var created = new DateTime(2026, 1, 1);
        var modified = new DateTime(2026, 1, 2);

        var item = new RemoteItem("file.txt", 123, false, true, created, modified, 123L);

        Assert.Multiple(() =>
        {
            Assert.That(item.ssFilename, Is.EqualTo("file.txt"));
            Assert.That(item.ssSizeInBytes, Is.EqualTo(123));
            Assert.That(item.ssIsDir, Is.False);
            Assert.That(item.ssIsLink, Is.True);
            Assert.That(item.ssCreated, Is.EqualTo(created));
            Assert.That(item.ssModified, Is.EqualTo(modified));
            Assert.That(item.ssSizeInBytesLong, Is.EqualTo(123L));
        });
    }

    [Test]
    public void Constructor_NullFilename_DefaultsToEmptyString()
    {
        var item = new RemoteItem(null!, 0, false, false, DateTime.MinValue, DateTime.MinValue);

        Assert.That(item.ssFilename, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Constructor_OmittedSizeInBytesLong_DefaultsToZero()
    {
        var item = new RemoteItem("file.txt", 100, false, false, DateTime.MinValue, DateTime.MinValue);

        Assert.That(item.ssSizeInBytesLong, Is.EqualTo(0L));
    }
}

/// <summary>
/// Tests for the size clamping behavior of the private Long2Int helper in SFTP,
/// which keeps ssSizeInBytes backward compatible for files over 2 GB.
/// </summary>
public class SFTPLong2IntTests
{
    private static int InvokeLong2Int(long value)
    {
        var method = typeof(SFTP).GetMethod("Long2Int", BindingFlags.NonPublic | BindingFlags.Static);
        return (int)method!.Invoke(null, new object[] { value })!;
    }

    [Test]
    public void Long2Int_ValueWithinIntRange_ReturnsExactValue()
    {
        Assert.That(InvokeLong2Int(1024L), Is.EqualTo(1024));
    }

    [Test]
    public void Long2Int_ValueAtIntMaxValue_ReturnsIntMaxValue()
    {
        Assert.That(InvokeLong2Int(int.MaxValue), Is.EqualTo(int.MaxValue));
    }

    [Test]
    public void Long2Int_ValueAboveIntMaxValue_ClampsToIntMaxValue()
    {
        long overTwoGb = (long)int.MaxValue + 1_000_000L;

        Assert.That(InvokeLong2Int(overTwoGb), Is.EqualTo(int.MaxValue));
    }

    [Test]
    public void Long2Int_ZeroValue_ReturnsZero()
    {
        Assert.That(InvokeLong2Int(0L), Is.EqualTo(0));
    }
}
