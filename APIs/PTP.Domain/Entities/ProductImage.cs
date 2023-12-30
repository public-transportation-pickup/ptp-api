namespace PTP.Domain.Entities;
public class ProductImage
{
    public int Id { get; set; } = default!;
    public string ImageName { get; set; } = default!;
    public string ImageURL { get; set; } = default!;

    #region Relationship
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;
    #endregion
}