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
    public void City_DBStartsEmpty_Empty()
    {
      //Arrange
      int count = City.GetAll().Count;

      //Assert
      Assert.AreEqual(0, count);
    }
    
  }
}