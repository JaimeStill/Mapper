namespace Mapper.Scripts
{
    public class StateData
    {
        public string Display { get; set; }
        public string Name { get; set; }
        public string Fips { get; set; }
        public string Option => $"{this.Display} - {this.Fips}";
    }
}