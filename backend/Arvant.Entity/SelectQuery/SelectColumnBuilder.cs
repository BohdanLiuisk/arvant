using Arvant.Entity.SelectQuery.Models;
using Arvant.Entity.Structure;
using Arvant.Entity.Utils;

namespace Arvant.Entity.SelectQuery;

public class SelectColumnBuilder(List<SelectExpression> columns, EntityStructure structure, string tableAlias)
{
    public IEnumerable<string> BuildAliases() {
        SelectQueryUtils.EnsurePrimaryColumnIncluded(columns, structure);
        foreach (var column in columns) {
            column.SelectAlias = SelectQueryUtils.GetColumnAlias(tableAlias, column.Path);
        }
        var regularColumns = columns.Where(c => structure.Columns
            .Any(ec => ec.Name == c.Path && !ec.IsForeignKey));
        return regularColumns.Select(c => SelectQueryUtils.GetColumnAsSelect(tableAlias, c.Path));
    }
}
