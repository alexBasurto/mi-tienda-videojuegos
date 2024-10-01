﻿namespace WebApiTiendaVideojuegos.Interface

{

    public interface IGestorArchivos
    {
        Task<string> EditarArchivo(byte[] contenido, string extension, string carpeta, string ruta,
            string contentType);
        Task BorrarArchivo(string ruta, string carpeta);
        Task<string> GuardarArchivo(byte[] contenido, string extension, string carpeta, string contentType);
    }

}
