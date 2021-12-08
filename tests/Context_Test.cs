using NUnit.Framework;
using Orbbec;

[TestFixture]
public class Context_Test
{
    private Context _context;

    [SetUp]
    public void SetUp()
    {
        _context = new Context();
    }

    [Test]
    public void Context_DeviceCount()
    {
        DeviceList devList = _context.QueryDeviceList();
        int devCount = (int)devList.DeviceCount();
        Assert.Greater(devCount, 0);
    }

    [Test]
    public void DeviceList_Name()
    {
        DeviceList devList = _context.QueryDeviceList();
        string name = devList.Name(0);
        StringAssert.AreEqualIgnoringCase("Astra+", name);
    }

    [Test]
    public void DeviceList_Pid()
    {
        DeviceList devList = _context.QueryDeviceList();
        int pid = devList.Pid(0);
        Assert.AreEqual(pid, 1590);
    }

    [Test]
    public void DeviceList_Vid()
    {
        DeviceList devList = _context.QueryDeviceList();
        int pid = devList.Vid(0);
        Assert.AreEqual(pid, 11205);
    }
}