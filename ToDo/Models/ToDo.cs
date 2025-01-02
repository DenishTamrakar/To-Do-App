    using System.ComponentModel.DataAnnotations;

    namespace ToDo.Models;
    public class TODO{
        [Key]
        public int TDID{get; set;}
        public string? TDName{get; set;}
        public bool TDStatus{get; set;}
        public int UserID{get; set;}
    }