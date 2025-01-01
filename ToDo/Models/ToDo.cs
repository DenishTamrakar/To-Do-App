using System.ComponentModel.DataAnnotations;

public class TODO{
    [Key]
    public int TDID{get; set;}
    public string? TDName{get; set;}
    public bool TDStatus{get; set;}
    public int UserID{get; set;}
}