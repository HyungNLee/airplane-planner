using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Airline.Models;

namespace Airline.Tests
{
  [TestClass]
  public class FlightTests : IDisposable
  {
     public FlightTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=airline_test;";
    }

    public void Dispose()
    {
      City.DeleteAll();
      Flight.DeleteAll();
    }
    [TestMethod]
    public void Delete_DeletesFlightAssociationFromDatabase_FlightList()
    {
      //Arrange
      City testCity = new City("Seattle");
      testCity.Save();

      DateTime newDate = new DateTime(2018, 09, 25);
      Flight testFlight = new Flight("Portland", "Seattle", "On Time", newDate);
      testFlight.Save();
      //Act
      testFlight.AddCity(testCity);
      testFlight.Delete();

      List<Flight> resultCityFlights = testCity.GetFlight();
      List<Flight> testCityFlights = new List<Flight> {};
      //Assert
      CollectionAssert.AreEqual(testCityFlights, resultCityFlights);
    }
    [TestMethod]
    public void Test_AddCity_AddsCityToFlights()
    {
      //Arrange
      DateTime newDate = new DateTime(2018, 09, 25);
      Flight testFlight = new Flight("Portland", "Seattle", "On Time", newDate);
      testFlight.Save();

      City testCity1 = new City("Seattle");
      testCity1.Save();

      City testCity2 = new City("Portland");
      testCity2.Save();      
      //Act
      testFlight.AddCity(testCity1);
      testFlight.AddCity(testCity2);

      List<City> result = testFlight.GetCity();
      List<City> testList = new List<City>{testCity1, testCity2};
      //Assert
      CollectionAssert.AreEqual(result, testList);
    }
  }
}