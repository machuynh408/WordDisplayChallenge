using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WordDisplayServer.Database
{
    public class Commands
    {
        [Key]
        public int CommandId { get; set; }
        public string Words { get; set; }
    }

    public class States
    {
        [Key]
        public int StateId { get; set; }
        public bool CommandRunning { get; set; }
        public string CurrentWordSelected { get; set; }
        public int CommandTimeRemaining { get; set; }
    }

    public class LoggingContext : DbContext
    {
        public LoggingContext(DbContextOptions<LoggingContext> options) : base (options)
        {
     
        }

        public DbSet<Commands> Commands { get; set;}
        public DbSet<States> States { get; set;}
    }
}