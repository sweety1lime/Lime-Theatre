namespace Theatre.Core
{
    interface ICRUD
    {

        public void CreateAsync();
        public void DeleteAsync();
        public void ReadAsync();
        public void UpdateAsync();

        public void LogicalDelete();

      

    }
}
