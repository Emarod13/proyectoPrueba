namespace ProyectoPrueba.DTOs
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string Details { get; set; } // Opcional: para mostrar el StackTrace solo en desarrollo

        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }
}
