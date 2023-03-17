using Microsoft.Data.SqlClient;
using Roommates.Models;
using System;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    public class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }


        public List<Chore> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Chore";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Chore> Chores = new List<Chore>();

                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");

                            int idValue = reader.GetInt32(idColumnPosition);

                            int nameColumnPosition = reader.GetOrdinal("Name");
                            string nameValue = reader.GetString(nameColumnPosition);

                            Chore ChoreObject = new Chore
                            {
                                Id = idValue,
                                Name = nameValue,
                            };

                            Chores.Add(ChoreObject);
                        }
                        return Chores;

                    }
                }
            }
        }


        // Below get's a list of unassigned chores
        public List<Chore> GetUnassigned()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Chore FULL OUTER JOIN RoommateChore on RoommateChore.ChoreId = Chore.Id WHERE ChoreId Is NULL";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Chore> Chores = new List<Chore>();

                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");

                            int idValue = reader.GetInt32(idColumnPosition);

                            int nameColumnPosition = reader.GetOrdinal("Name");
                            string nameValue = reader.GetString(nameColumnPosition);

                            Chore ChoreObject = new Chore
                            {
                                Id = idValue,
                                Name = nameValue,
                            };

                            Chores.Add(ChoreObject);
                        }
                        return Chores;

                    }
                }
            }
        }

        public Chore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select * FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Chore chore = null;

                        if (reader.Read())
                        {
                            chore = new Chore
                            {
                                Id = id,
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            };
                        }
                        return chore;
                    }

                }
            }
        }


        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name) 
                                         OUTPUT INSERTED.Id 
                                         VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    // cmd.Parameters.AddWithValue("@maxOccupancy", room.MaxOccupancy);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                }
            }
        }


        public void AssignChore(int roommateId, int choreId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO RoommateChore(ChoreId, RoommateId) 
                                         OUTPUT INSERTED.Id 
                                         VALUES (@choreId, @RoommateId)";
                    cmd.Parameters.AddWithValue("@RoommateId", roommateId);
                    cmd.Parameters.AddWithValue("@choreId", choreId);
                    int id = (int)cmd.ExecuteScalar();
                }
            }

        }

        public void Update(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Chore
                    SET Name = @name
                    WHERE Chore.Id = @id";
                    cmd.Parameters.AddWithValue("@id", chore.Id);
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        ///  Delete the room with the given id
        /// </summary>
        /*
        public void Delete()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // What do you think this code will do if there is a roommate in the room we're deleting???
                    cmd.CommandText = "DELETE FROM Room WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        */

    }
}