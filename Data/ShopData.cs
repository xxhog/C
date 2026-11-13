using HomeCakes.Models;
using System.Security.Cryptography;

namespace HomeCakes.Data;

// Для учебного примера данные хранятся в памяти и сбрасываются при перезапуске.
public class ShopData
{
    public List<Cake> Cakes { get; } = new()
    {
        new(){Id=1,Name="Медовик",Description="Нежные медовые коржи и сметанный крем",Image="https://images.unsplash.com/photo-1578985545062-69928b1d9587?auto=format&fit=crop&w=900&q=80",Price=1800,Category="Классические",IsPopular=true},
        new(){Id=2,Name="Ягодное облако",Description="Ванильный бисквит, сливочный крем и ягоды",Image="https://images.unsplash.com/photo-1565958011703-44f9829ba187?auto=format&fit=crop&w=900&q=80",Price=2300,Category="Ягодные",IsPopular=true},
        new(){Id=3,Name="Шоколадный",Description="Шоколадный бисквит и насыщенный ганаш",Image="https://images.unsplash.com/photo-1606313564200-e75d5e30476c?auto=format&fit=crop&w=900&q=80",Price=2100,Category="Шоколадные"},
        new(){Id=4,Name="Морковный",Description="Пряный бисквит с орехами и сырным кремом",Image="https://images.unsplash.com/photo-1621303837174-89787a7d4729?auto=format&fit=crop&w=900&q=80",Price=1950,Category="Классические"},
        new(){Id=5,Name="Лимонный",Description="Лёгкий мусс, лимонный курд и бисквит",Image="https://images.unsplash.com/photo-1519915028121-7d3463d20b13?auto=format&fit=crop&w=900&q=80",Price=2200,Category="Фруктовые"},
        new(){Id=6,Name="Красный бархат",Description="Бархатные коржи и нежный крем-чиз",Image="https://images.unsplash.com/photo-1586788680434-30d324b2d46f?auto=format&fit=crop&w=900&q=80",Price=2400,Category="Классические"}
    };
    public List<string> Fillings { get; } = new() { "Клубничная прослойка", "Солёная карамель", "Ореховый крем", "Лимонный курд" };
    public List<Review> Reviews { get; } = new()
    {
        new(){Id=1,Name="Анна из Ижевска",Text="Очень вкусный торт, красивое оформление и доставка по Ижевску вовремя!",Rating=5,IsApproved=true},
        new(){Id=2,Name="Ирина",Text="Заказывали торт на день рождения в Ижевске. Гости попросили контакты кондитера.",Rating=5,IsApproved=true}
    };
    public List<ShopOrder> Orders { get; } = new();
    public List<ShopUser> Users { get; } = new();

    public ShopData(IConfiguration configuration, IWebHostEnvironment environment)
    {
        var adminEmail = configuration["Admin:Email"] ?? "admin@cakes.local";
        var adminPassword = configuration["Admin:Password"];
        if (string.IsNullOrWhiteSpace(adminPassword) && environment.IsDevelopment())
            adminPassword = "Admin123!";
        if (!string.IsNullOrWhiteSpace(adminPassword))
            Users.Add(new ShopUser { Name="Администратор", Email=adminEmail, PasswordHash=HashPassword(adminPassword), Role="Администратор" });
    }
    public static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 120000, HashAlgorithmName.SHA256, 32);
        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }
    public static bool VerifyPassword(string password, string saved)
    {
        var parts=saved.Split('.'); if(parts.Length!=2) return false;
        try { var salt=Convert.FromBase64String(parts[0]); var oldHash=Convert.FromBase64String(parts[1]); var newHash=Rfc2898DeriveBytes.Pbkdf2(password,salt,120000,HashAlgorithmName.SHA256,oldHash.Length); return CryptographicOperations.FixedTimeEquals(oldHash,newHash); }
        catch { return false; }
    }
}

