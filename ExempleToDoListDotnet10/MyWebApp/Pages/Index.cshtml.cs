using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyWebApp.Helpers;
using MyWebApp.Models;
using System.Linq;

namespace MyWebApp.Pages;

public class IndexModel : PageModel
{
    private const string SessionKey = "TodoItems";

    [BindProperty]
    public string NewTitle { get; set; } = string.Empty;

    public List<TaskItem> Items { get; private set; } = new();

    public void OnGet()
    {
        Items = HttpContext.Session.GetObject<List<TaskItem>>(SessionKey) ?? new();
    }

    public IActionResult OnPostAdd()
    {
        Items = HttpContext.Session.GetObject<List<TaskItem>>(SessionKey) ?? new();
        if (!string.IsNullOrWhiteSpace(NewTitle))
        {
            Items.Add(new TaskItem
            {
                Title = NewTitle.Trim(),
                IsDone = false,
            });
            HttpContext.Session.SetObject(SessionKey, Items);
            ModelState.Clear();
        }
        return RedirectToPage();
    }

    public IActionResult OnPostToggle(string id)
    {
        Items = HttpContext.Session.GetObject<List<TaskItem>>(SessionKey) ?? new();
        var item = Items.FirstOrDefault(t => t.Id == id);
        if (item is not null)
        {
            item.IsDone = !item.IsDone;
            HttpContext.Session.SetObject(SessionKey, Items);
        }
        return RedirectToPage();
    }

    public IActionResult OnPostDelete(string id)
    {
        Items = HttpContext.Session.GetObject<List<TaskItem>>(SessionKey) ?? new();
        if (Items.RemoveAll(t => t.Id == id) > 0)
        {
            HttpContext.Session.SetObject(SessionKey, Items);
        }
        return RedirectToPage();
    }

    public IActionResult OnPostClear()
    {
        HttpContext.Session.Remove(SessionKey);
        return RedirectToPage();
    }
}
