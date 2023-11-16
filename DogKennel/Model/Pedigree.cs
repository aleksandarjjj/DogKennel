namespace DogKennel.Model
{
    public class Pedigree
    {
        public string? PedigreeID { get; set; }
        public string? Father { get; set; }
        public string? Mother { get; set; }
        public string? TattooNo { get; set; }
        public string? Owner { get; set; }

        public Pedigree() { }
        public Pedigree(string? pedigreeID, string? father, string? mother, string? tattooNo, string? owner)
        {
            PedigreeID = pedigreeID;
            Father = father;
            Mother = mother;
            TattooNo = tattooNo;
            Owner = owner;
        }
    }
}
