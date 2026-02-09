namespace ProyectoPrueba.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stock {  get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public ProductDTO() { }

        public ProductDTO(int id, string name, string description, int stock, decimal price, string imageUrl) {
            Id = id;
            Name = name;
            Description = description;
            Stock = stock;
            Price = price;
            ImageUrl = imageUrl;
        
        }
    }
}
