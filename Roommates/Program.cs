using Microsoft.VisualBasic;
using Roommates.Models;
using Roommates.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=true;";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepository = new ChoreRepository(CONNECTION_STRING);
            RoomateRepository roommaterepo = new RoomateRepository(CONNECTION_STRING);
            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all Unassigned Chores"):
                        List<Chore> chores = choreRepository.GetUnassigned();
                        foreach (Chore c in chores)
                        {
                                Console.WriteLine($"{c.Name} is unassigned.");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for a Chore"):
                        Console.Write("Chore Id: ");
                        id = int.Parse(Console.ReadLine());

                        Chore chore = choreRepository.GetById(id);

                        Console.WriteLine($"{chore.Id} - {chore.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.Write("Chore name: ");
                        name = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = name
                        };

                        choreRepository.Insert(choreToAdd);

                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for a roommate"):
                        Console.Write("Roommate Id: ");
                        id = int.Parse(Console.ReadLine());

                        Roommate roommate = roommaterepo.GetById(id);

                        Console.WriteLine($"{roommate.Id} - {roommate.FirstName} Room:({roommate.Room.Name})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all Roommates"):
                        List<Roommate> roommates = roommaterepo.GetAll();
                        foreach (Roommate r in roommates)
                        {
                            Console.WriteLine($"{r.FirstName} lives in the {r.Room.Name}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a Chore to a Roommate"):
                        
                        List<Roommate> roommates1 = roommaterepo.GetAll();
                        foreach (Roommate r in roommates1)
                        {
                            Console.WriteLine($"{r.Id}{r.FirstName}");
                        }
                        Console.Write("Choose a Roomate Number:");
                        int choreForRoommate = int.Parse(Console.ReadLine());

                        List<Chore> chores1 = choreRepository.GetAll();
                        foreach(Chore c in chores1)
                        {
                            Console.WriteLine($"{c.Id}{c.Name}");
                        }
                        Console.Write("Enter a chore Id:");
                        int roommateForChore = int.Parse(Console.ReadLine());

                        choreRepository.AssignChore(roommateForChore, choreForRoommate);
                        break;
                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Delete a room"):
                        List<Room> deleteRoomOptions = roomRepo.GetAll();
                        foreach (Room r in deleteRoomOptions)
                        {
                            Console.WriteLine($"{r.Id}{r.Name}");
                        }
                        Console.Write("Which Room would you like to delete?");
                        int selectedRoomIdForDelete = int.Parse(Console.ReadLine());
                        roomRepo.Delete(selectedRoomIdForDelete);
                        break;
                    case ("Update a Chore"):
                        List<Chore> updateChore = choreRepository.GetAll();
                        foreach(Chore c in updateChore)
                        {
                            Console.WriteLine($"{c.Id} {c.Name}");
                        }
                        Console.Write("Which Chore number to update?");
                        int selectedChoreId = int.Parse(Console.ReadLine());
                        Chore selectedChore = updateChore.FirstOrDefault(c => c.Id == selectedChoreId);
                        Console.Write("New Chore Name:");
                        selectedChore.Name = Console.ReadLine();

                        choreRepository.Update(selectedChore);
                        break;
                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Show all Unassigned Chores",
                "Search for a Chore",
                "Add a chore",
                "Search for a roommate",
                "Show all Roommates",
                "Add a Chore to a Roommate",
                "Update a room",
                "Delete a room",
                "Update a Chore",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}
