namespace MarathonEvents.Models;
using System.Collections.Generic;
using MarathonEvents.Enums;



public class Account
{
    public int AccountId { get; set; }
    public string Name { get; set; }
    public Role role { get; set; } 
    public string   Email { get; set; }
    public string Password { get; set; }


    public ICollection<Event> CreatedEvents { get; set; } = new List<Event>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();


   }


