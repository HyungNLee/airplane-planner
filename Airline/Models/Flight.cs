using Airline;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Airline.Models
{
  public class Flight 
  {
    private string _departureCity;
    private string _arrivalCity;
    private string _status;
    private DateTime _departureTime;
    private int _id;

    public Flight (string newDepartureCity, string newArrivalCity, string newStatus, DateTime newDepartureTime, int newId = 0)
    {
      _departureCity = newDepartureCity;
      _arrivalCity = newArrivalCity;
      _status = newStatus;
      _departureTime = newDepartureTime;
      _id = newId;
    }
    public string GetDepartureCity()
    {
      return _departureCity;
    }
    public string GetArrivalCity()
    {
      return _arrivalCity;
    }
    public string GetStatus()
    {
      return _status;
    }
    public DateTime GetDepartureTime()
    {
      return _departureTime;
    }
    public int GetId()
    {
      return _id;
    }

    public override bool Equals(System.Object otherFlight)
    {
      if (!(otherFlight is Flight))
      {
        return false;
      }
      else
      {
        Flight newFlight = (Flight) otherFlight;
        bool idEquality = this.GetId() == newFlight.GetId();
        bool departureTimeEquality = this.GetDepartureTime() == newFlight.GetDepartureTime();
        bool departureCityEquality = this.GetDepartureCity() == newFlight.GetDepartureCity();
        bool arrivalCityEquality = this.GetArrivalCity() == newFlight.GetArrivalCity();
        bool statusEquality = this.GetStatus() == newFlight.GetStatus();
        return (idEquality && departureTimeEquality && departureCityEquality && arrivalCityEquality && statusEquality);
      }
    }

    public override int GetHashCode()
    {
      string allHash = this.GetDepartureCity() + this.GetArrivalCity() + this.GetStatus();
      return allHash.GetHashCode();
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO flights (departure_city, arrival_city, status, departure_time) VALUES (@departureCity, @arrivalCity, @status, @departureTime);";

      MySqlParameter newDepartureCity = new MySqlParameter();
      newDepartureCity.ParameterName = "@departureCity";
      newDepartureCity.Value = this._departureCity;
      cmd.Parameters.Add(newDepartureCity);

      MySqlParameter newArrivalCity = new MySqlParameter();
      newArrivalCity.ParameterName = "@arrivalCity";
      newArrivalCity.Value = this._arrivalCity;
      cmd.Parameters.Add(newArrivalCity);

      MySqlParameter newStatus = new MySqlParameter();
      newStatus.ParameterName = "@status";
      newStatus.Value = this._status;
      cmd.Parameters.Add(newStatus);

      MySqlParameter newDepartureTime = new MySqlParameter();
      newDepartureTime.ParameterName = "@departureTime";
      newDepartureTime.Value = this._departureTime;
      cmd.Parameters.Add(newDepartureTime);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Flight> GetAll()
    {
      List<Flight> allFlights = new List<Flight>() {};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM flights;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int flightId = rdr.GetInt32(0);
        string flightDepartureCity = rdr.GetString(1);
        string flightArrivalCity = rdr.GetString(2);
        string flightStatus = rdr.GetString(3);
        DateTime flightDepartureTime = rdr.GetDateTime(4);
        Flight newFlight = new Flight(flightDepartureCity, flightArrivalCity, flightStatus, flightDepartureTime, flightId);
        allFlights.Add(newFlight);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allFlights;
    }

    public static Flight Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM flights WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int flightId = 0;
      string flightDepartureCity = "";
      string flightArrivalCity = "";
      string flightStatus = "";
      DateTime flightDepartureTime = new DateTime(0000, 00, 00);

      while (rdr.Read())
      {
        flightId = rdr.GetInt32(0);
        flightDepartureCity = rdr.GetString(1);
        flightArrivalCity = rdr.GetString(2);
        flightStatus = rdr.GetString(3);
        flightDepartureTime = rdr.GetDateTime(4);
      }

      Flight newFlight = new Flight(flightDepartureCity, flightArrivalCity, flightStatus, flightDepartureTime, flightId);
      
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newFlight;
    }

    public void UpdateFlight(string newDepartureCity, string newArrivalCity, string newStatus, DateTime newDepartureTime)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE flights SET departure_city = @newDepartureCity, arrival_city = @newArrivalCity, status = @newStatus, departure_time = @newDepartureTime WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter departureCity = new MySqlParameter();
      departureCity.ParameterName = "@newDepartureCity";
      departureCity.Value = newDepartureCity;
      cmd.Parameters.Add(departureCity);

      MySqlParameter arrivalCity = new MySqlParameter();
      arrivalCity.ParameterName = "@newArrivalCity";
      arrivalCity.Value = newArrivalCity;
      cmd.Parameters.Add(arrivalCity);

      MySqlParameter status = new MySqlParameter();
      status.ParameterName = "@newStatus";
      status.Value = newStatus;
      cmd.Parameters.Add(status);

      MySqlParameter departureTime = new MySqlParameter();
      departureTime.ParameterName = "@newDepartureTime";
      departureTime.Value = newDepartureTime;
      cmd.Parameters.Add(departureTime);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      _departureCity = newDepartureCity;
      _arrivalCity = newArrivalCity;
      _status = newStatus;
      _departureTime = newDepartureTime;
      
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
      cmd.CommandText = @"DELETE FROM flights WHERE id = @searchId; DELETE FROM flights_cities WHERE flight_id = @searchId;";

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
      cmd.CommandText = @"DELETE FROM flights;";

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void AddCity(City newCity)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO flights_cities (flight_id, city_id) VALUES (@FlightId, @CityId);";

      MySqlParameter flight_id = new MySqlParameter();
      flight_id.ParameterName = "@FlightId";
      flight_id.Value = _id;
      cmd.Parameters.Add(flight_id);

      MySqlParameter city_id = new MySqlParameter();
      city_id.ParameterName = @"CityId";
      city_id.Value = newCity.GetId();
      cmd.Parameters.Add(city_id);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<City> GetCity()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      
      cmd.CommandText = @"SELECT cities.* FROM flights
      JOIN flights_cities ON (flights.id = flights_cities.flight_id)
      JOIN cities ON (flights_cities.city_id = cities.id)
      WHERE flights.id = @FlightId;";

      MySqlParameter flightIdParameter = new MySqlParameter();
      flightIdParameter.ParameterName = "@FlightId";
      flightIdParameter.Value = _id;
      cmd.Parameters.Add(flightIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<City> cities = new List<City> {};

      while(rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string cityName = rdr.GetString(1);
        City newCity = new City(cityName, cityId);
        cities.Add(newCity); 
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return cities;
    }
  }
}