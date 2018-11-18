namespace Tasks.DataLayer.Models.Base
{
    public interface IEntityKeyProvider
    {
        object[] GetKeyValues();
    }
}
