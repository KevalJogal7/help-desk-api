namespace HelpDesk.Services.DTOs.Common;

public class DropdownOption
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
}

public class SubCategoryResponse : DropdownOption
{
    public int CategoryId { get; set; }
}
