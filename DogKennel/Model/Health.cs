namespace DogKennel.Model
{
    public class Health
    {
        public string? PedigreeID { get; set; }
        public string? HD { get; set; }
        public string? AD { get; set; }
        public string? HZ { get; set; }
        public string? SP { get; set; }

        public Health() { }
        public Health(string? pedigreeID, string? hD, string? aD, string? hZ, string? sP)
        {
            PedigreeID = pedigreeID;
            HD = hD;
            AD = aD;
            HZ = hZ;
            SP = sP;
        }
    }
}
