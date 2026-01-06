using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
namespace Productive_Maxxing.Pages;
public class MoneyFunctions{
  private readonly Db _db;
  public MoneyFunctions(Db db){
    _db = db;
  }
  //Get
  public async Task<List<Kebutuhan>> DisplayKebutuhanLists(){
    var Kebutuhan_List = new List<Kebutuhan>();
    using var conn = _db.GetConnection();
    await conn.OpenAsync();

    using var cmd = new MySqlCommand("SELECT * FROM Kebutuhan",conn);

    using var reader = await cmd.ExecuteReaderAsync();

    while(await reader.ReadAsync()){
      Kebutuhan_List.Add(new Kebutuhan{
        id = reader.GetInt32("id"),
        nama = reader.GetString("nama"),
        nominal = reader.GetDouble("nominal"),
        notes = reader.GetString("notes")
      });
    }
    return Kebutuhan_List;
  }
  //Post 
  public async Task TambahKebutuhan(Kebutuhan kebutuhan){
     using var conn = _db.GetConnection();
    await conn.OpenAsync();

    using var cmd = new MySqlCommand(
        "INSERT INTO Kebutuhan(nama,nominal,notes) VALUES(@nama,@nominal,@notes)"
        ,conn);
    cmd.Parameters.AddWithValue("@nama",kebutuhan.nama);
    cmd.Parameters.AddWithValue("@nominal",kebutuhan.nominal);
    cmd.Parameters.AddWithValue("@notes",kebutuhan.notes ?? "");

    await cmd.ExecuteNonQueryAsync();
  }
}
public class MoneyModel:PageModel{
  private readonly ILogger<MoneyModel> _logger;
  private readonly MoneyFunctions _moneyFunctions;
  public List<Kebutuhan> Kebutuhan_List {get;private set;} = new();
  [BindProperty]
  public Kebutuhan kebutuhan {get;set;}=new();
  public MoneyModel(
  ILogger<MoneyModel> logger,
  MoneyFunctions moneyfunctions
  ){
  _logger = logger;
  _moneyFunctions = moneyfunctions;
  }
  public string Greet { get; set; } = "";
 
  public async Task OnGetAsync(){

    Greet= "Welcome To Money Management Page";
    Kebutuhan_List = await _moneyFunctions.DisplayKebutuhanLists();
  }
  [IgnoreAntiforgeryToken]
   public async Task<JsonResult> OnGetKebutuhanList()
    {
        var list = await _moneyFunctions.DisplayKebutuhanLists();
        return new JsonResult(list);
    }
public async Task<JsonResult> OnPostTambahKebutuhan([FromBody] Kebutuhan kebutuhan)
    {
        try
        {
            // Validate data
            if (kebutuhan == null || string.IsNullOrEmpty(kebutuhan.nama))
            {
                return new JsonResult(new 
                { 
                    success = false, 
                    error = "Invalid data" 
                });
            }

            // Save to database
            await _moneyFunctions.TambahKebutuhan(kebutuhan);

            // Return success response
            return new JsonResult(new 
            { 
                success = true, 
                message = "Data berhasil ditambahkan" 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding kebutuhan");
            return new JsonResult(new 
            { 
                success = false, 
                error = ex.Message 
            });
        }
    }

}

