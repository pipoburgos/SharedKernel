namespace SharedKernel.Application.Utils.Iva
{
    public static class IvaExtensions
    {
        public static double CalculateIva(this float value, double? iva)
        {
            return ((double) value).CalculateIva(iva);
        }

        public static double CalculateIva(this double value, double? iva)
        {
            if (iva == null)
                return 0;

            return value * (iva.Value / 100);
        }

        public static double CalculateWithIva(this float value, double? iva)
        {
            return iva == null ? value : ((double) value).CalculateWithIva(iva);
        }

        public static double CalculateWithIva(this double value, double? iva)
        {
            if (iva == null)
                return value;

            return value * (1 + iva.Value / 100);
        }
    }
}