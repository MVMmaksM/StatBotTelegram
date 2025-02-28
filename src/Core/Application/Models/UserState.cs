using Application.Constants;

namespace Application.Models;

public class UserState
{
    public MenuItems MenuItem { get; set; }
    public OperationCode? OperationItem { get; set; }
}