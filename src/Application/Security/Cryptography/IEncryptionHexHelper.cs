namespace SharedKernel.Application.Security.Cryptography
{
    public interface IEncryptionHexHelper
    {
        /// <summary>
        ///     Funcion para el Encriptado de Cadenas de Texto
        ///     <param name="texto">Texto a encriptar</param>
        ///     <returns>Cadena encriptada</returns>
        /// </summary>
        string Encrypt(string texto);

        /// <summary>
        ///     Funcion para el Desencriptado de Cadenas de Texto
        ///     <param name="texto">Texto a desencriptar</param>
        ///     <returns>Cadena desencriptada</returns>
        /// </summary>
        string Decrypt(string texto);
    }
}
