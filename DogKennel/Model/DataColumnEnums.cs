﻿namespace DogKennel.Model
{
    //Enum defining columns from Excel file
    public enum TblColumnsExcel
    {
        PedigreeID = 1,
        TattooNo,
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

    //Enum defining columns for database table TblDogs
    public enum TblDogs
    {
        PedigreeID = TblColumnsExcel.PedigreeID,
        DateOfBirth = TblColumnsExcel.DateOfBirth,
        Alive = TblColumnsExcel.Alive,
        Sex = TblColumnsExcel.Sex,
        Colour = TblColumnsExcel.Colour,
        AK = TblColumnsExcel.AK,
        BreedStatus = TblColumnsExcel.BreedStatus,
        DKTitles = TblColumnsExcel.DKTitles,
        Titles = TblColumnsExcel.Titles,
        Name = TblColumnsExcel.Name,
        Picture = TblColumnsExcel.Picture
    }

    //Enum defining columns for database table TblDogHealth
    public enum TblDogHealth
    {
        PedigreeID = TblColumnsExcel.PedigreeID,
        HD = TblColumnsExcel.HD,
        AD = TblColumnsExcel.AD,
        HZ = TblColumnsExcel.HZ,
        SP = TblColumnsExcel.SP
    }

    //Enum defining columns for database table TblDogPedigree
    public enum TblDogPedigree
    {
        PedigreeID = TblColumnsExcel.PedigreeID,
        Father = TblColumnsExcel.Father,
        Mother = TblColumnsExcel.Mother,
        TattooNo = TblColumnsExcel.TattooNo,
        Owner = TblColumnsExcel.Owner,
    }
}
