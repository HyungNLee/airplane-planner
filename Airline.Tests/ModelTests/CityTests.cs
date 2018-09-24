using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Airline.Models;

namespace Airline.Tests
{
  [TestClass]
  public class CityTests : IDisposable
  {
    public CityTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=airline_test;";
    }

    public void Dispose()
    {
      City.DeleteAll();
      Flight.DeleteAll();
    }

    [TestMethod]
    public void GetAll_DBStartsEmpty_Empty()
    {
      //Arrange
      int count = City.GetAll().Count;

      //Assert
      Assert.AreEqual(0, count);
    }

    [TestMethod]
    public void Equals_TrueForSameName_City()
    {
      //Arrange
      City cityOne = new City("Seattle");
      City cityTwo = new City("Seattle");

      //Assert
      Assert.AreEqual(cityOne, cityTwo);
    }
    
    [TestMethod]
    public void Save_CitiesSaveToDatabase_CitiesList()
    {
      //Arrange
      City testCity = new City("Seattle");
      testCity.Save();

      //Act
      List<City> result = City.GetAll();
      List<City> testlist = new List<City>{testCity};

      //Assert
      CollectionAssert.AreEqual(testlist, result);
    }

    [TestMethod]
    public void Save_AssignsIdToObject_id()
    {
      //Arrange
      City testCity = new City("Seattle");
      testCity.Save();

      //Act
      City savedCity = City.GetAll()[0];

      int result = savedCity.GetId();
      int testId = testCity.GetId();

      //Assert 
      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void Find_FindsItemInDatabase_City()
    {
      //Arrange
      City testCity = new City("Seattle");
      testCity.Save();

      //Act
      City result = City.Find(testCity.GetId());

      //Assert

      Assert.AreEqual(testCity, result);

    }
  }
}