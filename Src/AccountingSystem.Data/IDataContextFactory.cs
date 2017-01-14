namespace AccountingSystem.Data
{
    public interface IDataContextFactory
    {
        IDataContext GetContext();
    }
}
