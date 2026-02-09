namespace ProyectoPrueba.Entity
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Stock { get; set; }

        public decimal Price { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DeleteTime { get; set; }

        public string? ImageUrl { get; set; }

        public Product() { }

        public Product(int id, string name, string description, int stock, decimal price, bool isDeleted, DateTime deleteTime, string imageUrl)
        {
            Id = id;
            Name = name;
            Description = description;
            Stock = stock;
            Price = price;
            IsDeleted = isDeleted;
            DeleteTime = deleteTime;
            ImageUrl = imageUrl;
        }
    }
}
