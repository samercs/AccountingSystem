namespace AccountingSystem.Entity
{
    public class Account : EntityBase
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int Level { get; set; }
        public decimal InitBalance { get; set; }
    }
}
