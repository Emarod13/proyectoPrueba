namespace ProyectoPrueba.Wrappers
{
    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; } // Total de productos en la BD
        public int TotalPages { get; set; }   // Total de páginas calculadas
        public List<T> Data { get; set; }     // La lista de productos de ESTA página

        public PagedResponse(List<T> data, int pageNumber, int pageSize, int totalRecords)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            // Cálculo mágico de páginas: (Total / Tamaño) redondeado hacia arriba
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        }
    }
}
