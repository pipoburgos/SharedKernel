using Dapper;

namespace SharedKernel.Infrastructure.Dapper.Data.Queries;

/// <summary>  </summary>
public partial class QueryBuilder
{
    private readonly List<string> _condiciones;
    private readonly DynamicParameters _dynamicParameters;

    private string GetFilters(bool isAnd)
    {
        var resultado = _tabs[0];
        // El primer "tab" son datos fijos que siempre vienen aunque no se haya puesto nada:
        // "Expdte.AmbitoId NOT IN (10, 11)" y "appmain.ModeloExpediente.Id IS NULL"
        _tabs.RemoveAt(0);

        if (_tabs.Any())
            resultado += " AND (" + GetResult(isAnd, _tabs) + ")";

        return Where + resultado;
    }

    /// <summary>  </summary>
    public QueryBuilder AddFilter(string filter)
    {
        _condiciones.Add(filter);
        return this;
    }

    /// <summary>  </summary>
    public QueryBuilder AddFilter(Func<QueryBuilder, bool> expression)
    {
        var esAnd = expression(this);
        NewTab(esAnd);
        return this;
    }

    /// <summary>  </summary>
    public QueryBuilder AddFilter<T>(string table, string column, T? desde, T? hasta,
        bool negar)
    {
        var id = Guid.NewGuid().ToString().Substring(0, 5);

        object? hastaObj = hasta;

        if (hasta != null && hasta is DateTime fechaHasta)
            hastaObj = fechaHasta.AddDays(1).AddMinutes(-1);

        AddFilter(desde != null && hasta != null, table, column, negar,
            QueryConditions.Between, $"desde{id}", desde, $"hasta{id}", hastaObj);

        AddFilter(desde != null && hasta == null, table, column, negar,
            QueryConditions.HigherOrEquals, $"desde{id}", desde);

        AddFilter(desde == null && hasta != null, table, column, negar,
            QueryConditions.LowerOrEquals, $"hasta{id}", hastaObj);

        return this;
    }

    /// <summary>  </summary>
    public QueryBuilder AddFilter(string table, string column, object? filter, bool negar = false,
        QueryConditions? condition = null)
    {
        if (filter == default)
            return this;

        if (filter is string texto && string.IsNullOrWhiteSpace(texto))
            return this;

        if (filter is bool estaCheckeado && !estaCheckeado)
            return this;

        var type = filter.GetType();

        var typeNotNullable = Nullable.GetUnderlyingType(type) ?? type;

        var isString = typeNotNullable == typeof(string);

        var isDate = typeNotNullable == typeof(DateTime);

        //var isEnumerable = !isString && typeof(IEnumerable).IsAssignableFrom(type);

        var defaultCondition = condition ?? (isDate ? QueryConditions.DateEquals :
            isString ? QueryConditions.Like : QueryConditions.Equals);

        if (filter is bool booleano)
            filter = booleano ? "1" : "0";

        return AddFilter(table, column, negar, defaultCondition, null, filter);
    }

    /// <summary>  </summary>
    public QueryBuilder AddFilter(bool must, string table, string column, bool negar, QueryConditions condicion,
        string paramName, object? value, string? nombreParametro2 = null, object? value2 = null, string? campo = null,
        string paramConst = "@", string? table2 = null)
    {
        if (!must)
            return this;

        return AddFilter(table, column, negar, condicion, paramName, value, nombreParametro2, value2, campo,
            paramConst, table2);
    }

    /// <summary>  </summary>
    public QueryBuilder AddFilter(string table, string column, bool negar, QueryConditions condicion,
        string? paramName, object? value, string? nombreParametro2 = null, object? value2 = null, string? campo = null,
        string paramConst = "@", string? table2 = null)
    {
        if (paramName == default || string.IsNullOrWhiteSpace(paramName))
            paramName = $"{table}{column}";

        if (ExistsParameter(paramName))
            paramName = GetNextParameter(paramName, 1);

        if (condicion != QueryConditions.Like)
            AddParameter(paramName, value);
        else if (value != default)
            AddParameter(paramName, value.ToString()!.Replace("%", "$%").Replace("*", "%"));

        if (nombreParametro2 != null)
            AddParameter(nombreParametro2, value2);

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
                    ? string.Format(
                        $"{campo} NOT BETWEEN {paramConst}{paramName} AND {paramConst}{nombreParametro2}")
                    : string.Format($"{campo} BETWEEN {paramConst}{paramName} AND {paramConst}{nombreParametro2}"));
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
                    ? string.Format(
                        $"({table}.{column} <> {paramConst}{paramName} OR {table}.{campo} <> {paramConst}{paramName})")
                    : string.Format(
                        $"({table}.{column} = {paramConst}{paramName} OR {table}.{campo} = {paramConst}{paramName})"));
                break;
            case QueryConditions.LikeAnd:
                _condiciones.Add(negar
                    ? string.Format(
                        $"({table}.{column} NOT LIKE '%' + {paramConst}{paramName} + '%' AND {table2}.{campo} <> {paramConst}{nombreParametro2})")
                    : string.Format(
                        $"({table}.{column} LIKE '%' + {paramConst}{paramName} + '%' AND {table2}.{campo} = {paramConst}{nombreParametro2})"));
                break;
            case QueryConditions.HigherEqualsAndOr:
                // and (Matricula_Anio >= 2020 and Matricula_Contador >=2 or Matricula_Anio > 2020)
                _condiciones.Add(negar
                    ? string.Format(
                        $"({table}.{column} <= {paramConst}{paramName} AND {table}.{campo} <= {paramConst}{nombreParametro2} OR {table}.{column} < {paramConst}{paramName})")
                    : string.Format(
                        $"({table}.{column} >= {paramConst}{paramName} AND {table}.{campo} >= {paramConst}{nombreParametro2} OR {table}.{column} > {paramConst}{paramName})"));
                break;
            case QueryConditions.LowerEqualsAndOr:
                // and (Matricula_Anio <= 2021 and Matricula_Contador <=1 or Matricula_Anio < 2021)
                _condiciones.Add(negar
                    ? string.Format(
                        $"({table}.{column} >= {paramConst}{paramName} AND {table}.{campo} >= {paramConst}{nombreParametro2} OR {table}.{column} > {paramConst}{paramName})")
                    : string.Format(
                        $"({table}.{column} <= {paramConst}{paramName} AND {table}.{campo} <= {paramConst}{nombreParametro2} OR {table}.{column} < {paramConst}{paramName})"));
                break;
            case QueryConditions.DateEquals:
                _condiciones.Add(negar
                    ? string.Format($"CAST({campo} as Date) <> CAST({paramConst}{paramName} As Date)")
                    : string.Format($"CAST({campo} as Date) = CAST({paramConst}{paramName} As Date)"));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(condicion), condicion, null);
        }

        return this;
    }

    /// <summary>  </summary>
    public QueryBuilder AddParameter(string nombre, object? obj)
    {
        if (obj == default)
            return this;

        if (ExistsParameter(nombre))
            throw new DapperException("El nombre del parámetro introducido (" + nombre + ") ya existe.");

        _dynamicParameters.Add(nombre, obj);
        return this;
    }

    private bool ExistsParameter(string? nombre)
    {
        return _dynamicParameters.ParameterNames.Contains(nombre);
    }

    private string GetNextParameter(string nombre, int indice)
    {
        var key = nombre;
        if (indice > 1)
            key = $"{key}{indice}";

        if (!ExistsParameter(key))
            return key;

        indice++;
        return GetNextParameter(nombre, indice);
    }
}
