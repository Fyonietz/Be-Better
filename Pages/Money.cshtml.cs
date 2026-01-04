using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
namespace Productive_Maxxing.Pages;

public class MoneyModel:PageModel{
  private readonly ILogger<MoneyModel> _logger;
  private readonly Db _db;

  public List<Kebutuhan> Kebutuhan_List {get;private set;} = new();
  public MoneyModel(Db db,ILogger<MoneyModel> logger){
    _db = db;
    _logger = logger;
  }
  public string Greet { get; set; } = "";
 
  public async Task OnGetAsync(){

    Greet= "Welcome To Money Management Page";
    using var conn = _db.GetConnection();
    await conn.OpenAsync();

    using var cmd = new MySqlCommand("SELECT * FROM Kebutuhan",conn);

    using var reader = await cmd.ExecuteReaderAsync();

    while(await reader.ReadAsync()){
      Kebutuhan_List.Add(new Kebutuhan{
        id = reader.GetInt32("id"),
        nama = reader.GetString("nama"),
        nominal = reader.GetDouble("nominal")
      });
    }
  }
}

