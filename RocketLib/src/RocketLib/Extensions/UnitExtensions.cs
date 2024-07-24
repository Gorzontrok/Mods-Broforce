

namespace RocketLib
{
    public static class UnitExtensions
    {
        public static void SetIsZombie(this Unit unit, bool isZombie)
        {
            unit.SetFieldValue("isZombie", isZombie);
        }
    }
}
