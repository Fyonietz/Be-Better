using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Productive_Maxxing.Pages;

public class MoneyModel:PageModel{
  private readonly ILogger<MoneyModel> _logger;
  public string Greet { get; set; } = "";
 public MoneyModel(ILogger<MoneyModel> logger)
    {
        _logger = logger;
    }
  public void OnGet(){
    Greet= "Welcome To Money Management Page";
  }
}

