namespace GammaItems
{
    [Flags]
    public enum ItemFlags
    {
        FLAG_NoAuction = 0b_0000_0000_0000_0001,
        FLAG_NoBargain = 0b_0000_0000_0000_0010,
        FLAG_NoSell = FLAG_NoBargain,
        FLAG_NoTrade = 0b_0000_0000_0000_0100,
        FLAG_Retired = 0b_0000_0000_0000_1000,
        FLAG_CrownsOnly = 0b_0000_0000_0001_0000,
        FLAG_NoDrop = 0b_0000_0000_0010_0000,
        FLAG_NoGift = 0b_0000_0000_0100_0000,
        FLAG_Crafted = 0b_0000_0000_1000_0000,
        FLAG_PVPOnly = 0b_0000_0001_0000_0000,
        FLAG_NoPVP = 0b_0000_0010_0000_0000,
        FLAG_DevItem = 0b_0000_0100_0000_0000,
    }
}
