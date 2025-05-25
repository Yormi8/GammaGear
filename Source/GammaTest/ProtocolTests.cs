using GammaGear.Models.DML;

namespace GammaTest;

[TestClass]
public class ProtocolTests
{
    [TestMethod]
    public void TestReadXML()
    {
        string path = Path.Combine(TestUtils.GetCurrentDirectory(), "PatchMessages.xml");
        Console.WriteLine(File.ReadAllText(path));
        ProtocolFormat protocol = new ProtocolFormat(path);
        Assert.AreEqual("PatchMessages", protocol.Name.ToString());
        Assert.AreEqual(8, protocol.ServiceId);
        Assert.AreEqual("PATCH", protocol.Type.ToString());
        Assert.AreEqual(1, protocol.Version);
        Assert.AreEqual("Patch Server Messages", protocol.Description.ToString());

        Assert.AreEqual(3, protocol.Messages.Count);
    }
}
