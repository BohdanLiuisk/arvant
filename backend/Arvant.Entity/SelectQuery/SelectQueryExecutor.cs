﻿using Arvant.Entity.Abstract;
using Arvant.Entity.SelectQuery.Enums;
using Arvant.Entity.SelectQuery.Models;
using Arvant.Entity.Structure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlKata;
using SqlKata.Execution;

namespace Arvant.Entity.SelectQuery;

public class SelectQueryExecutor(EntityStructureManager structureManager, QueryFactory queryFactory, 
        AliasStorage aliasStorage) 
    : ISelectQueryExecutor
{
    public async Task<string> ExecuteAsync(SelectQueryConfig selectConfig) {
        var jsonResult = await ExecuteInternal(selectConfig);
        return jsonResult.ToString(Formatting.Indented);
    }
    
    public async Task<SelectQueryResult<T>?> ExecuteAsync<T>(SelectQueryConfig selectConfig) {
        var jsonResult = await ExecuteInternal(selectConfig);
        return jsonResult.ToObject<SelectQueryResult<T>>();
    }

    private async Task<JObject> ExecuteInternal(SelectQueryConfig selectConfig) {
        var rootQuery = new Query();
        try {
            var rootStructure = await structureManager.FindEntityStructureByName(selectConfig.EntityName);
            selectConfig.RootStructure = rootStructure;
            BuildRootQuery(rootQuery, selectConfig, rootStructure);
            return await BuildJsonResult(selectConfig, rootQuery);
        } catch (Exception e) {
            return new SelectResultBuilder(selectConfig)
                .WithQuery(rootQuery).WithError(e.Message).Build();
        } finally {
            aliasStorage.Clear();
        }
    }

    private async Task<JObject> BuildJsonResult(SelectQueryConfig selectConfig, Query rootQuery) {
        var resultBuilder = new SelectResultBuilder(selectConfig).WithQuery(rootQuery);
        if (selectConfig.IsPaginated) {
            var paginationResult = await queryFactory.PaginateAsync<dynamic>(rootQuery,
                selectConfig.PaginationConfig!.Page, selectConfig.PaginationConfig!.PerPage);
            return resultBuilder.WithPagination(paginationResult).WithRows(paginationResult.List).Build();
        }
        var rows = await queryFactory.GetAsync(rootQuery);
        return resultBuilder.WithRows(rows).Build();
    }

    private Query BuildRootQuery(Query query, SelectQueryConfig selectConfig, EntityStructure rootStructure) {
        var rootTableAlias = aliasStorage.GetTableAlias(selectConfig.EntityName);
        var rootQuery = query.From($"{selectConfig.EntityName} as {rootTableAlias}");
        if (selectConfig.AllColumns == true) {
            selectConfig.AddAllColumns(rootStructure);
        }
        var joinsStorage = new JoinsStorage(aliasStorage, rootTableAlias, rootStructure);
        var joinBuilder = new JoinBuilder(rootQuery, aliasStorage, joinsStorage, structureManager);
        if (selectConfig.Expressions.All(e => e.Type != ExpressionType.Function)) {
            var columnExpressions = selectConfig.Expressions.Where(e => e.Type == ExpressionType.Column).ToList();
            var selectBuilder = new SelectColumnBuilder(columnExpressions, rootStructure, rootTableAlias);
            var queryColumns = selectBuilder.BuildAliases();
            rootQuery.Select(queryColumns);
            joinBuilder.AppendLeftJoins(selectConfig.Expressions, rootTableAlias, rootStructure);
        } else {
            new AggrFuncBuilder(query, joinBuilder).AppendAggrFunctions(selectConfig.Expressions);
        }
        var subQueryExpressions = selectConfig.Expressions.Where(e => e.Type == ExpressionType.SubQuery).ToList();
        var subQueryBuilder = new SubQueryBuilder(aliasStorage, structureManager);
        subQueryBuilder.AppendSubQueries(rootQuery, subQueryExpressions, rootTableAlias, rootStructure);
        var filterBuilder = new FilterBuilder(rootQuery, aliasStorage, joinsStorage, structureManager, joinsStorage);
        filterBuilder.AppendFilter(selectConfig.Filter);
        var sortBuilder = new SortBuilder(rootQuery, aliasStorage, joinsStorage, structureManager);
        sortBuilder.AppendSorting(selectConfig);
        if (selectConfig.Limit != null && selectConfig.Limit != 0) {
            rootQuery.Limit(selectConfig.Limit.Value);
        }
        return rootQuery;
    }
}
