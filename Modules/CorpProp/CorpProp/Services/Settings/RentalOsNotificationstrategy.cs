namespace CorpProp.Services.Settings
{
    public class RentalOsNotificationstrategy : DefaultNotificationStrategy
    {
        protected override string GetSuccessCode()
        {
            return $"RentalOS{base.GetSuccessCode()}";
        }

        protected override string GetFailCode()
        {
            return $"RentalOS{base.GetFailCode()}";
        }
    }
}