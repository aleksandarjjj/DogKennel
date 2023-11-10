using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogKennel.Model
{
    public enum DataReaderColumn
    {
        PedigreeID = 1,
        Tattoo,
        Name,
        Breeder,
        Father,
        Mother,
        DKTitles,
        Titles,
        LastVisit,
        Count,
        DateOfBirth,
        HD,
        AD,
        HZ,
        SP,
        IndexDate,
        HDIndex,
        Sex,
        Colour,
        Alive,
        AK,
        BreedStatus,
        MB,
        Picture,
        Owner,
        Address,
        PostalNumber,
        City,
        Phone,
        Email,
        Url,
        Log,
        Username,
        Password,
        Updated,
        UserCookie,
        UserIP,
        AddedBy,
        DateAdded
    }

    public enum Dogs
    {
        PedigreeID = DataReaderColumn.PedigreeID,
        DateOfBirth = DataReaderColumn.DateOfBirth,
        Alive = DataReaderColumn.Alive,
        Sex = DataReaderColumn.Sex,
        Colour = DataReaderColumn.Colour,
        AK = DataReaderColumn.AK,
        BreedStatus = DataReaderColumn.BreedStatus,
        DKTitles = DataReaderColumn.DKTitles,
        Titles = DataReaderColumn.Titles,
        Name = DataReaderColumn.Name,
        Picture = DataReaderColumn.Picture
    }
}
