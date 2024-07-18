namespace DOCTORLOAN.Models.Products;

public class AttributeGroup
{
    public string Name { get; set; }
    public virtual ICollection<Attribute> Attributes { get; set; }
}
public enum AttributeGroupEnum
{
    Size = 1, //Kích thước
    SpecificWeight = 2, // Khối lượng
    Designs = 3, //Kiểu dáng
    CoreMaterial = 4, //Chất liệu lõi
    UpholsteryMaterial = 5, //Chất liệu bọc
    ProductionTechnology = 6, //Công nghệ sản xuất
    EfficiencyOFUse = 7, //Hiệu quả sử dụng
    UserManual = 8, //Hướng dẫn sử dụng
    Guarantee = 9, //Bảo hành
    YearOFManufacture = 10, //Năm sản xuất
    MadeIn = 11, // Sản xuất tại
}