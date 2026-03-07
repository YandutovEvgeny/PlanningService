using PlanningService.Application.Contracts.Planner.Enums;

namespace PlanningService.Application.Contracts.Planner;

/// <summary>
/// Represents the column metdata.<br/>
/// Data types, titles, styles and etc.
/// </summary>
public class ColumnMetadata
{
    /// <summary>
    /// Represents the data type of column.
    /// </summary>
    public string DataType { get; set; } = string.Empty;

    /// <summary>
    /// Represents the title of column.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Represents the display style.
    /// </summary>
    public Alignment Style { get; set; }

    /// <summary>
    /// True if column is editable, otherwise false.
    /// </summary>
    public bool IsEditable { get; set; }
}
