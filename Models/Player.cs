using System;

namespace PlayerManagementAPI.Models
{
    public class Player
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Age { get; set; }
        public required string Position { get; set; }
        public required string Team { get; set; }
        public DateTime JoinDate { get; set; }
        public int JerseyNumber { get; set; }
    }
}
