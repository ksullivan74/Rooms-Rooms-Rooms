using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Roommates.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace Roommates.Repositories
{
    public class RoomateRepository : BaseRepository
    {
        public RoomateRepository(string connectionString) : base(connectionString) { }

        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Roommate JOIN Room on Room.Id = RoomId WHERE Roommate.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;

                        // If we only expect a single row back from the database, we don't need a while loop.
                        if (reader.Read())
                        {
                            roommate = new Roommate
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("MaxOccupancy")),
                                Room = new Room { 
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                }
                                
                            };
                        }
                        return roommate;
                    }

                }
            }
        }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select * FROM Roommate JOIN Room on Room.Id = RoomId";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Roommate> roommates = new List<Roommate>();

                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");
                            int idValue = reader.GetInt32(idColumnPosition);

                            int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                            string firstNameValue = reader.GetString(firstNameColumnPosition);

                            int lastNameColumnPosition = reader.GetOrdinal("LastName");
                            string lastNameValue = reader.GetString(lastNameColumnPosition);

                            int renPortionColumnPosition = reader.GetOrdinal("RentPortion");
                            int rentPortionValue = reader.GetInt32(renPortionColumnPosition);

                            int moveInDateColumnPosition = reader.GetOrdinal("MoveInDate");
                            DateTime moveInDateValue = reader.GetDateTime(moveInDateColumnPosition);

                            Roommate roommateObject = new Roommate
                            {
                                Id = idValue,
                                FirstName = firstNameValue,
                                LastName = lastNameValue,
                                RentPortion = rentPortionValue,
                                MovedInDate = moveInDateValue,
                                Room = new Room
                                {
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                }
                            };

                            roommates.Add(roommateObject);
                        }
                        return roommates;

                    }
                }
            }
        }



    }
}
        