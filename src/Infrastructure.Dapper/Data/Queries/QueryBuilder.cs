using SharedKernel.Application.Cqrs.Queries.Entities;
using System.Diagnostics;

namespace SharedKernel.Infrastructure.Dapper.Data.Queries;

/// <summary> </summary>
public class QueryBuilder
{
    #region Miembros

    private const string SelectText = "SELECT";
    private const string GroupByText = "\nGROUP BY";
    private const string InnerJoinText = "\n\tINNER JOIN ";
    private const string LeftJoinText = "\n\tLEFT JOIN ";
    private const string WhereText = "\nWHERE\n\t";
    private const string AndText = "\n\tAND ";
    private const string OrText = "\n\tOR ";
    private readonly List<string> _condiciones;
    private readonly List<string> _tabs;
    private readonly List<string> _subTabs;
    private readonly List<string> _leftJoinsTab;
    private readonly List<string> _leftJoins;
    private readonly string _connectionString;
    private readonly PageOptions _state;
    private bool _isAnd;
    private string _select;
    private string _tempTables = null!;
    private string _groupBy;
    private readonly CustomDynamicParameters _dynamicParameters;

    #endregion

    #region Constructores

    /// <summary> </summary>
    public QueryBuilder(string connectionString, PageOptions state)
    {
        _dynamicParameters = new CustomDynamicParameters();
        _condiciones = new List<string>();
        _tabs = new List<string>();
        _subTabs = new List<string>();
        _leftJoinsTab = new List<string>();
        _leftJoins = new List<string>();
        _connectionString = connectionString;
        _state = state;
        _isAnd = true;
        _select = string.Empty;
        _groupBy = string.Empty;
    }

    #endregion

    #region Métodos públicos

    /// <summary> </summary>
    public void IsNotAnd()
    {
        _isAnd = false;
    }

    /// <summary> </summary>
    public QueryResult Build()
    {
        var query = _select;

        query += GetLeftJoins();

        query += GetFilters(_isAnd);

        query += _groupBy;

        Debug.WriteLine("-------------------------------------------------------------------------------");
        Debug.WriteLine(query);
        Debug.WriteLine("-------------------------------------------------------------------------------");

        return new QueryResult(_dynamicParameters, query, _tempTables, _connectionString, _state);
    }

    /// <summary> </summary>
    public QueryBuilder AddTempTables(string tables)
    {
        _tempTables = tables;
        return this;
    }

    /// <summary> </summary>
    public QueryBuilder Select(string select)
    {
        _select = SelectText + select;
        return this;
    }

    /// <summary> </summary>
    public void Where(string filter)
    {
        _condiciones.Add(filter);
    }

    /// <summary> </summary>
    public QueryBuilder Where(Func<QueryBuilder, bool> expression)
    {
        var esAnd = expression(this);
        NewTab(esAnd);
        return this;
    }

    /// <summary> </summary>
    public QueryBuilder Where(bool esAnd, params Func<QueryBuilder, bool>[] expressions)
    {
        foreach (var expression in expressions)
        {
            var esAndDos = expression(this);
            NewTab(esAndDos, true);
        }
        return AddSubTabsToTab(esAnd);
    }

    /// <summary> </summary>
    public QueryBuilder GroupBy(string groupBy)
    {
        _groupBy = GroupByText + groupBy;
        return this;
    }

    /// <summary> </summary>
    public QueryBuilder LeftJoin(QueryTable table1, QueryTable table2)
    {
        _leftJoinsTab.Add($"{LeftJoinText}{table1}\n\t\tON {table1.Join} = {table2.Join}");
        return this;
    }

    /// <summary> </summary>
    public QueryBuilder InnerJoin(QueryTable table1, QueryTable table2, string? more = default)
    {
        _leftJoinsTab.Add($"{InnerJoinText}{table1}\n\t\tON {table1.Join} = {table2.Join}{more}");
        return this;
    }

    /// <summary> </summary>
    public QueryBuilder InnerJoin(string query, string paramName, object value)
    {
        _dynamicParameters.AddParameter(paramName, value);
        _leftJoinsTab.Add(query);
        return this;
    }

    /// <summary> </summary>
    public void AddSubQuery(string subQuery)
    {
        _condiciones.Add(subQuery);
    }

    /// <summary> </summary>
    public void AddParameter(string nombre, object obj)
    {
        _dynamicParameters.AddParameter(nombre, obj);
    }

    /// <summary> </summary>
    public void AddParameter(string table, string column, bool negar, QueryConditions condicion, string paramName,
        object? value, string? secondParameter = default, object? value2 = default, string? campo = default,
        string paramConst = "@", string? table2 = default)
    {
        if (_dynamicParameters.ParameterNames.Contains(paramName))
            throw new Exception("Parameter (" + paramName + ") already exists.");

        if (condicion != QueryConditions.Like && value != default)
            _dynamicParameters.AddParameter(paramName, value);
        else if (value != default)
            // ReSharper disable once RedundantSuppressNullableWarningExpression
            _dynamicParameters.AddParameter(paramName, value.ToString()!.Replace("%", "$%").Replace("*", "%"));

        if (secondParameter != default && value2 != default)
            _dynamicParameters.AddParameter(secondParameter, value2);

        if (campo == null)
            campo = $"{table}.{column}";

        switch (condicion)
        {
            case QueryConditions.IsNull:
                _condiciones.Add(negar
                    ? string.Format($"{campo} IS NOT NULL")
                    : string.Format($"{campo} IS NULL"));
                break;
            case QueryConditions.Like:
                _condiciones.Add(negar
                    ? string.Format($"{campo} NOT LIKE '%' + {paramConst}{paramName} + '%' ESCAPE '$'")
                    : string.Format($"{campo} LIKE '%' + {paramConst}{paramName} + '%' ESCAPE '$'"));
                break;
            case QueryConditions.Equals:
                _condiciones.Add(negar
                    ? string.Format($"{campo} <> {paramConst}{paramName}")
                    : string.Format($"{campo} = {paramConst}{paramName}"));
                break;
            case QueryConditions.In:
                _condiciones.Add(negar
                    ? string.Format($"{campo} NOT IN {paramConst}{paramName}")
                    : string.Format($"{campo} IN {paramConst}{paramName}"));
                break;
            case QueryConditions.Between:
                _condiciones.Add(negar
                    ? string.Format($"{campo} NOT BETWEEN {paramConst}{paramName} AND {paramConst}{secondParameter}")
                    : string.Format($"{campo} BETWEEN {paramConst}{paramName} AND {paramConst}{secondParameter}"));
                break;
            case QueryConditions.HigherOrEquals:
                _condiciones.Add(negar
                    ? string.Format($"{campo} < {paramConst}{paramName}")
                    : string.Format($"{campo} >= {paramConst}{paramName}"));
                break;
            case QueryConditions.LowerOrEquals:
                _condiciones.Add(negar
                    ? string.Format($"{campo} > {paramConst}{paramName}")
                    : string.Format($"{campo} <= {paramConst}{paramName}"));
                break;
            case QueryConditions.EqualsOr:
                _condiciones.Add(negar
                    ? string.Format($"({table}.{column} <> {paramConst}{paramName} OR {table}.{campo} <> {paramConst}{paramName})")
                    : string.Format($"({table}.{column} = {paramConst}{paramName} OR {table}.{campo} = {paramConst}{paramName})"));
                break;
            case QueryConditions.LikeAnd:
                _condiciones.Add(negar
                    ? string.Format($"({table}.{column} NOT LIKE '%' + {paramConst}{paramName} + '%' AND {table2}.{campo} <> {paramConst}{secondParameter})")
                    : string.Format($"({table}.{column} LIKE '%' + {paramConst}{paramName} + '%' AND {table2}.{campo} = {paramConst}{secondParameter})"));
                break;
            case QueryConditions.HigherEqualsAndOr:
                _condiciones.Add(negar
                    ? string.Format($"({table}.{column} <= {paramConst}{paramName} AND {table}.{campo} <= {paramConst}{secondParameter} OR {table}.{column} < {paramConst}{paramName})")
                    : string.Format($"({table}.{column} >= {paramConst}{paramName} AND {table}.{campo} >= {paramConst}{secondParameter} OR {table}.{column} > {paramConst}{paramName})"));
                break;
            case QueryConditions.LowerEqualsAndOr:
                _condiciones.Add(negar
                    ? string.Format($"({table}.{column} >= {paramConst}{paramName} AND {table}.{campo} >= {paramConst}{secondParameter} OR {table}.{column} > {paramConst}{paramName})")
                    : string.Format($"({table}.{column} <= {paramConst}{paramName} AND {table}.{campo} <= {paramConst}{secondParameter} OR {table}.{column} < {paramConst}{paramName})"));
                break;
            case QueryConditions.DateEquals:
                _condiciones.Add(negar
                    ? string.Format($"CAST({campo} as Date) <> CAST({paramConst}{paramName} As Date)")
                    : string.Format($"CAST({campo} as Date) = CAST({paramConst}{paramName} As Date)"));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(condicion), condicion, null);
        }
    }

    #endregion

    #region Private Methods

    private void NewTab(bool esAnd, bool esSubTab = false)
    {
        if (!_condiciones.Any())
        {
            _leftJoinsTab.Clear();
            return;
        }

        var resultado = GetResult(esAnd, _condiciones);

        if (esSubTab)
        {
            _subTabs.Add(resultado);
        }
        else
        {
            _tabs.Add(resultado);
        }

        _leftJoins.AddRange(_leftJoinsTab);
        _leftJoinsTab.Clear();
        _condiciones.Clear();
    }

    private string GetFilters(bool isAnd)
    {
        var resultado = _tabs[0];
        _tabs.RemoveAt(0);

        if (_tabs.Any())
            resultado += " AND (" + GetResult(isAnd, _tabs) + ")";

        return WhereText + resultado;
    }

    /// <summary> </summary>
    public QueryBuilder AddSubTabsToTab(bool isAnd)
    {
        if (!_subTabs.Any())
            return this;

        _tabs.Add(GetResult(isAnd, _subTabs));
        _subTabs.Clear();
        return this;
    }

    /// <summary> </summary>
    public void AddParameters(string nombre, object obj)
    {
        _dynamicParameters.AddParameter(nombre, obj);
    }

    private string GetResult(bool isAnd, List<string> lista)
    {
        var resultado =
            (isAnd ? string.Empty : "(") +
            string.Join(isAnd ? AndText : OrText, lista) +
            (isAnd ? string.Empty : ")");

        return resultado;
    }

    private string GetLeftJoins()
    {
        return string.Join(string.Empty, _leftJoins.Distinct());
    }

    #endregion
}