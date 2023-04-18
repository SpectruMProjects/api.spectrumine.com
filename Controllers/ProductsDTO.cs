namespace SpectruMineAPI.Controllers
{
    public record CreateProductDTO(string Name, string Description, string Category, string ImgUrl, string ObjUrl, string MatUrl, float Price);
}
