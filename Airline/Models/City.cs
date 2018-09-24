using Airline;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Airline.Models
{
  public class City
  {
    private int _id;
    private string _name;

    public City (string newName, int cityId = 0)
    {
      _name = newName;
      _id = cityId;
    }
    public string GetName()
    {
      return _name;
    }
    public int GetId()
    {
      return _id;
    }

    public override bool Equals(System.Object otherItem)
    {
      if (!(otherItem is City))
      {
        return false;
      }
      else
      {
        City newCity = (City) otherItem;
        bool idEquality = this.GetId() == newCity.GetId();
        bool nameEquality = this.GetName() == newCity.GetName();
        return (idEquality && nameEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO cities (name) VALUES (@name);";

      MySqlParameter newName = new MySqlParameter();
      newName.ParameterName = "@name";
      newName.Value = this._name;
      cmd.Parameters.Add(newName);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<City> GetAll()
    {
      List<City> allCities = new List<City>() {};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cities;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string cityName = rdr.GetString(1);
        City newCity = new City(cityName, cityId);
        allCities.Add(newCity);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCities;
    }

    public static City Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cities WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int cityId = 0;
      string cityName = "";

      while (rdr.Read())
      {
        cityId = rdr.GetInt32(0);
        cityName = rdr.GetString(1);
      }

      City newCity = new City(cityName, cityId);
      
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newCity;
    }

    public void UpdateName(string newName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE cities SET name = @newName WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@newName";
      name.Value = newName;
      cmd.Parameters.Add(name);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      _name = newName;
      
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM cities WHERE id = @searchId; DELETE from flights_cities WHERE city_id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = @"searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM cities;";

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void AddFlight(Flight newFlight)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO flights_cities (flight_id, city_id) VALUES (@flightId, @cityId);";

      MySqlParameter flight_id = new MySqlParameter();
      flight_id.ParameterName = "@flightId";
      flight_id.Value = newFlight.GetId();
      cmd.Parameters.Add(flight_id);

      MySqlParameter city_id = new MySqlParameter();
      city_id.ParameterName = "@cityId";
      city_id.Value = _id;
      cmd.Parameters.Add(city_id);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public List<Flight> GetFlight()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

      cmd.CommandText = @"SELECT flights.* FROM cities
      JOIN flights_cities ON (cities.id = flights_cities.city_id)
      JOIN flights ON (flights_cities.flight_id = flights.id)
      WHERE city.id = @CityId;";

      MySqlParameter cityIdParameter = new MySqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = _id;
      cmd.Parameters.Add(cityIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Flight> flights = new List<Flight> {};

      int flightId = 0;
      string flightDepartureCity = "";
      string flightArrivalCity = "";
      string flightStatus = "";
      DateTime flightDepartureTime = new DateTime(0000, 00, 00);

      while(rdr.Read())
      {
        flightId = rdr.GetInt32(0);
        flightDepartureCity = rdr.GetString(1);
        flightArrivalCity = rdr.GetString(2);
        flightStatus = rdr.GetString(3);
        flightDepartureTime = rdr.GetDateTime(4);
        Flight newFlight = new Flight(flightDepartureCity, flightArrivalCity, flightStatus, flightDepartureTime, flightId);
        flights.Add(newFlight);
      }
     
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return flights;
    }
  }
}