namespace TaskService.Models;
public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool Completed { get; set; }
    public int UserId{get;set;}
}