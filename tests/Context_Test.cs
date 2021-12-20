using NUnit.Framework;
using Orbbec;

[TestFixture]
public class Context_Test
{
    private Context _context;
    private DeviceList _devList;

    [OneTimeSetUp]
    public void SetUp()
    {
        _context = new Context();
        _devList = _context.QueryDeviceList();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _context.Dispose();
        _devList.Dispose();
    }

    [Test]
    public void Context_DeviceCount()
    {
        int devCount = (int)_devList.DeviceCount();
        Assert.Greater(devCount, 0);
    }

    [Test]
    public void DeviceList_Name()
    {
        string name = _devList.Name(0);
        Assert.False(string.IsNullOrEmpty(name));
    }

    [Test]
    public void DeviceList_Pid()
    {
        int pid = _devList.Pid(0);
        Assert.Greater(pid, 0);
    }

    [Test]
    public void DeviceList_Vid()
    {
        int pid = _devList.Vid(0);
        Assert.AreEqual(pid, 11205);
    }

    [Test]
    public void DeviceList_Uid()
    {
        string uid = _devList.Uid(0);
        bool empty = string.IsNullOrEmpty(uid);
        Assert.IsFalse(empty);
    }
}