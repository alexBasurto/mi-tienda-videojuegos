using System.ComponentModel.DataAnnotations;

namespace WebApiTiendaVideojuegos.Validators
{
    public class PesoArchivoValidacion : ValidationAttribute
    {
        private readonly int pesoMaximoEnMegaBytes;

        public PesoArchivoValidacion(int PesoMaximoEnMegaBytes)
        {
            pesoMaximoEnMegaBytes = PesoMaximoEnMegaBytes;
        }

        // value representa al archivo
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            // IFormFile es el dato tal y como entra desde la post
            IFormFile formFile = value as IFormFile;

            // Si es null damos el ok para que se guarde (en este caso) el producto sin imagen
            if (formFile == null)
            {
                return ValidationResult.Success;
            }

            // Si sobrepasa el tamaño devolvemos un error
            if (formFile.Length > pesoMaximoEnMegaBytes * 1024 * 1024)
            {
                return new ValidationResult($"El peso del archivo no debe ser mayor a {pesoMaximoEnMegaBytes}mb");
            }

            // Si hemos llegado hasta aquí es que todo ha ido bien y el archivo cumple con el tamaño especificado en el DTO
            return ValidationResult.Success;
        }
    }

}
