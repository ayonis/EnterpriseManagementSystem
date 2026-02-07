using EnterpriseManagementSystem.Domain.Enums;

namespace EnterpriseManagementSystem.Application.Common.Dtos
{
    public class BasicDto
    {
        public string SortBy { get; set; } = string.Empty;
        public string SortOrder { get; set; } = Sort.NoSorting.ToString();
        public DateDto? FromDate { get; set; }
        public DateDto? ToDate { get; set; }

    }
}
