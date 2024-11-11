namespace SharedKernel.Infrastructure.Redsys;

/// <summary> . </summary>
public class RedsysOptions
{
    /// <summary>
    /// 25/A-N Opcional: será el nombre del comercio que
    /// aparecerá en el ticket del cliente(opcional).
    /// </summary>
    public string? MerchantName { get; set; }

    /// <summary>
    /// 9/N. Obligatorio. Código FUC asignado al comercio.
    /// </summary>
    public string MerchantCode { get; set; } = null!;

    /// <summary>
    /// 4 /Núm. Obligatorio.Se debe enviar el código numérico
    /// de la moneda según el ISO-4217, por ejemplo:
    /// 978 euros
    /// 840 dólares
    /// 826 libras
    /// 392 yenes
    /// 4 se considera su longitud máxima
    /// </summary>
    public string Currency { get; set; } = null!;

    /// <summary>
    /// 1 / Num
    /// Obligatorio.para el comercio para indicar qué
    /// tipo de transacción es.Los posibles valores son:
    /// 0 – Autorización
    /// 1 – Preautorización
    /// 2 – Confirmación de preautorización
    /// 3 – Devolución Automática
    /// 5 – Transacción Recurrente
    /// 6 – Transacción Sucesiva
    /// 7 – Pre-autenticación
    /// 8 – Confirmación de pre-autenticación
    /// 9 – Anulación de Preautorización
    /// O – Autorización en diferido
    /// P– Confirmación de autorización en diferido
    /// Q - Anulación de autorización en diferido
    /// R – Cuota inicial diferido
    /// S – Cuota sucesiva diferido
    /// </summary>
    public string TransactionType { get; set; } = null!;

    /// <summary>
    /// Obligatorio. Número de terminal que le asignará
    /// su banco.Tres se considera su longitud máxima
    /// </summary>
    public string Terminal { get; set; } = null!;

    /// <summary>
    /// Sermepa url
    /// </summary>
    public bool Production { get; set; }

    /// <summary> Clave privada. </summary>
    public string Key { get; set; } = null!;
}