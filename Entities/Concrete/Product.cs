namespace Entities.Concrete
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public int Gram { get; set; }

        public bool IsActive { get; set; }

        public int Stock { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateUser { get; set; }


    }
}
