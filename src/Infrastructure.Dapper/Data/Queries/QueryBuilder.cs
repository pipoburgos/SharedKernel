using Dapper;
using SharedKernel.Application.Cqrs.Queries.Entities;
using SharedKernel.Infrastructure.Dapper.Data.ConnectionFactory;

namespace SharedKernel.Infrastructure.Dapper.Data.Queries;

/// <summary> </summary>
public partial class QueryBuilder
{
    #region Miembros

    private const string Select = "SELECT";
    private const string InnerJoin = "\n\tINNER JOIN ";
    private const string LeftJoin = "\n\tLEFT JOIN ";
    private const string Where = "\nWHERE\n\t";
    private const string And = "\n\tAND ";
    private const string Or = "\n\tOR ";
    private readonly List<string> _tabs;
    private readonly List<string> _joinsTab;
    private readonly List<string> _joins;
    private readonly IDbConnectionFactory _dbConnectionFactory = null!;
    private readonly PageOptions _state = null!;
    private readonly bool _isAnd;
    private string _select;
    private string _tempTables = null!;

    #endregion

    #region Constructores

    /// <summary> . </summary>
    public QueryBuilder(IDbConnectionFactory dbConnectionFactory, PageOptions state, bool isAnd)
    {
        _dynamicParameters = new DynamicParameters();
        _condiciones = new List<string>();
        _tabs = new List<string>();
        _joinsTab = new List<string>();
        _joins = new List<string>();
        _dbConnectionFactory = dbConnectionFactory;
        _state = state;
        _isAnd = isAnd;
        _select = string.Empty;
    }

    /// <summary> . </summary>
    private QueryBuilder(DynamicParameters dynamicParameters)
    {
        _dynamicParameters = dynamicParameters;
        _condiciones = new List<string>();
        _tabs = new List<string>();
        _joinsTab = new List<string>();
        _joins = new List<string>();
        _isAnd = true;
        _select = string.Empty;
    }

    /// <summary> . </summary>
    public QueryBuilder CreateSubQuery()
    {
        return new QueryBuilder(_dynamicParameters);
    }

    #endregion

    #region Métodos públicos

    /// <summary> . </summary>
    public QueryResult Build()
    {
        var consulta = _select;

        consulta += GetJoins();

        consulta += GetFilters(_isAnd);

        return new QueryResult(_dynamicParameters, consulta, _tempTables, _state, _dbConnectionFactory);
    }

    /// <summary> . </summary>
    public string GetSubConsulta()
    {
        NewTab(true);

        var consulta = _select;

        consulta += GetJoins();

        consulta += GetFilters(_isAnd);

        return consulta;
    }

    /// <summary> . </summary>
    public int TotalCondiciones => _condiciones.Count;

    /// <summary> . </summary>
    public QueryBuilder AddSubQueryExists(QueryBuilder subQuery)
    {
        _condiciones.Add($"\n\t EXISTS (\n\t\t {subQuery.GetSubConsulta()} )");
        return this;
    }

    /// <summary> . </summary>
    public QueryBuilder AddSubQueryNotExists(QueryBuilder subQuery)
    {
        _condiciones.Add($"\n\t NOT EXISTS (\n\t\t {subQuery.GetSubConsulta()} )");
        return this;
    }

    /// <summary> . </summary>
    public QueryBuilder AddSubQuery(string subQuery)
    {
        _condiciones.Add(subQuery);
        return this;
    }

    /// <summary> . </summary>
    public QueryBuilder AddTempTables(string tables)
    {
        _tempTables = tables;
        return this;
    }

    /// <summary> . </summary>
    public QueryBuilder AddSelect(string select)
    {
        _select = Select + select;
        return this;
    }

    /// <summary> . </summary>
    public QueryBuilder AddSelect(string selectTableNameAlias, QueryTable table)
    {
        _select = $"SELECT '{selectTableNameAlias}' FROM {table}";
        return this;
    }

    /// <summary> . </summary>
    public QueryBuilder AddLeftJoin(QueryTable table1, QueryTable table2)
    {
        _joinsTab.Add($"{LeftJoin}{table1}\n\t\tON {table1.Join} = {table2.Join}");
        return this;
    }

    /// <summary> . </summary>
    public QueryBuilder AddInnerJoin(bool must, QueryTable table1, QueryTable table2, string? more = null)
    {
        if (must)
        {
            _joinsTab.Add($"{InnerJoin}{table1}\n\t\tON {table1.Join} = {table2.Join}{more}");
        }

        return this;
    }

    /// <summary> . </summary>
    public QueryBuilder AddInnerJoin(QueryTable table1, QueryTable table2, string? more = null)
    {
        _joinsTab.Add($"{InnerJoin}{table1}\n\t\tON {table1.Join} = {table2.Join}{more}");
        return this;
    }

    #endregion

    #region Private Methods

    private void NewTab(bool esAnd)
    {
        if (!_condiciones.Any())
        {
            _joinsTab.Clear();
            return;
        }

        var resultado = GetResult(esAnd, _condiciones);
        _tabs.Add(resultado);
        _joins.AddRange(_joinsTab);
        _joinsTab.Clear();
        _condiciones.Clear();
    }

    private string GetResult(bool isAnd, List<string> lista)
    {
        var resultado =
            (isAnd ? string.Empty : "(") +
            string.Join(isAnd ? And : Or, lista) +
            (isAnd ? string.Empty : ")");

        return resultado;
    }

    private string GetJoins()
    {
        return string.Join(string.Empty, _joins.Distinct());
    }

    #endregion
}