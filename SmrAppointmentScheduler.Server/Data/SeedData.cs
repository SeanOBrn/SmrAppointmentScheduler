using System;
using System.Linq;
using System.Collections.Generic;
using SmrAppointmentScheduler.Server.Models;

namespace SmrAppointmentScheduler.Server.Data;

public static class SeedData
{
    public static void Seed(AppDbContext db)
    {
        if (db.Branches.Any())
        {
            return; // DB has been seeded
        }

        // Create branches
        var branches = new List<Branch>
        {
            new Branch { Name = "Central Branch", Address = "123 Main St" },
            new Branch { Name = "North Branch", Address = "456 North Ave" }
        };

        db.Branches.AddRange(branches);

        // Create mechanics and assign to branches
        var mechanics = new List<Mechanic>
        {
            new Mechanic { FirstName = "John", LastName = "Doe", Branch = branches[0] },
            new Mechanic { FirstName = "Jane", LastName = "Smith", Branch = branches[0] },
            new Mechanic { FirstName = "Bob", LastName = "Brown", Branch = branches[1] }
        };

        db.Mechanics.AddRange(mechanics);

        // Create service types
        var serviceTypes = new List<ServiceType>
        {
            new ServiceType { Name = "Inspection", Description = "Vehicle inspection" },
            new ServiceType { Name = "Service", Description = "Regular service" },
            new ServiceType { Name = "Repair", Description = "Mechanical repair" },
            new ServiceType { Name = "Diagnostics", Description = "Diagnostics and troubleshooting" }
        };

        db.ServiceTypes.AddRange(serviceTypes);

        db.SaveChanges();

        // Generate appointment slots for the next 7 days, 9am-5pm, 1-hour slots
        var slots = new List<AppointmentSlot>();
        var startDate = DateTime.Today;
        var mechanicsList = db.Mechanics.ToList();
        var branchesList = db.Branches.ToList();
        var servicesList = db.ServiceTypes.ToList();

        int slotCounter = 0;
        for (int day = 0; day < 7; day++)
        {
            for (int hour = 9; hour < 17; hour++)
            {
                var start = startDate.AddDays(day).AddHours(hour);
                var end = start.AddHours(1);

                var mechanic = mechanicsList[slotCounter % mechanicsList.Count];
                var branch = branchesList[slotCounter % branchesList.Count];
                var service = servicesList[slotCounter % servicesList.Count];

                slots.Add(new AppointmentSlot
                {
                    BranchId = branch.Id,
                    MechanicId = mechanic.Id,
                    ServiceTypeId = service.Id,
                    Start = start,
                    End = end
                });

                slotCounter++;
            }
        }

        db.AppointmentSlots.AddRange(slots);
        db.SaveChanges();
    }
}
