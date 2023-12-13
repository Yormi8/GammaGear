namespace GammaItems
{
    public enum IllegalityReason
    {
        Legal = 0,
        LevelRequirementConflict        = 0b0000_0001,
        SchoolRequirementConflict       = 0b0000_0010,
        SchoolRestrictionConflict       = 0b0000_0100,
        PvpArenaRankRequirementConflict = 0b0000_1000,
        PetArenaRankRequirementConflict = 0b0001_0000,
        DuplicateItemTypeConflict       = 0b0010_0000,
        BadgeRequirementConflict        = 0b0100_0000
    }
}
