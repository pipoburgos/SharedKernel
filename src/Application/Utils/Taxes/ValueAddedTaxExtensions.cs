namespace SharedKernel.Application.Utils.Taxes;

/// <summary>
/// 
/// </summary>
public static class ValueAddedTaxExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="valueAddedTax"></param>
    /// <returns></returns>
    public static double CalculateValueAddedTax(this float value, double? valueAddedTax)
    {
        return ((double) value).CalculateValueAddedTax(valueAddedTax);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="valueAddedTax"></param>
    /// <returns></returns>
    public static double CalculateValueAddedTax(this double value, double? valueAddedTax)
    {
        if (valueAddedTax == null)
            return 0;

        return value * (valueAddedTax.Value / 100);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="valueAddedTax"></param>
    /// <returns></returns>
    public static double CalculateWithValueAddedTax(this float value, double? valueAddedTax)
    {
        return valueAddedTax == null ? value : ((double) value).CalculateWithValueAddedTax(valueAddedTax);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="valueAddedTax"></param>
    /// <returns></returns>
    public static double CalculateWithValueAddedTax(this double value, double? valueAddedTax)
    {
        if (valueAddedTax == null)
            return value;

        return value * (1 + valueAddedTax.Value / 100);
    }
}