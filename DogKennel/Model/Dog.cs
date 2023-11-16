namespace DogKennel.Model
{
    public class Dog
    {
        public string? PedigreeID { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Alive { get; set; }
        public string? Sex { get; set; }
        public string? Colour { get; set; }
        public string? AK { get; set; }
        public string? BreedStatus { get; set; }
        public string? DKTitles { get; set; }
        public string? Titles { get; set; }
        public string? Name { get; set; }
        public string? Picture { get; set; }

        public Dog() { }
        public Dog(string? pedigreeID, string? dateOfBirth, string? alive, string? sex, string? colour, string? aK, string? breedStatus, string? dKTitles, string? titles, string? name, string? picture)
        {
            PedigreeID = pedigreeID;
            DateOfBirth = dateOfBirth;
            Alive = alive;
            Sex = sex;
            Colour = colour;
            AK = aK;
            BreedStatus = breedStatus;
            DKTitles = dKTitles;
            Titles = titles;
            Name = name;
            Picture = picture;
        }
        public override string ToString()
        {
            return PedigreeID;
        }
    }
}
