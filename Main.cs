using System;
using System.Collections.Generic;
using System.Linq;
using MCCsharp;
using MCCsharp.Enums;
using MCCsharp.Models;

namespace Flow.Launcher.Plugin.MinecraftRecipesBook;

/// <summary>
/// pl
/// </summary>
public class MinecraftRecipesBook : IPlugin, IPluginI18n
{
    private PluginInitContext _context;
    private MinecraftData _minecraftData;

    /// <inheritdoc />
    public void Init(PluginInitContext context)
    {
        _context = context;
        _minecraftData = new MinecraftData(Platform.Pc, "1.17");
    }

    /// <inheritdoc />
    public List<Result> Query(Query query)
    {
        var search = query.Search?.Trim();
        if (string.IsNullOrWhiteSpace(search))
        {
            return new List<Result>
            {
                new Result
                {
                    Title = "Please enter an item name",
                    SubTitle = "e.g. crafting_table, beacon, etc.",
                    IcoPath = "icon.png"
                }
            };
        }

        var mc = new MinecraftData(Platform.Pc, "1.17");

        Item? item = null;
        try
        {
            item = mc.GetItemByIdOrName(search);
        }
        catch
        {
            // Aucun item trouvé
        }

        if (item == null)
        {
            return new List<Result>
            {
                new Result
                {
                    Title = $"Item not found: {search}",
                    SubTitle = "Try with a valid Minecraft item name",
                    IcoPath = "icon.png"
                }
            };
        }

        Recipe? recipe = null;
        try
        {
            recipe = mc.GetRecipe(item);
        }
        catch
        {
            // Aucune recette trouvée
        }

        if (recipe == null)
        {
            return new List<Result>
            {
                new Result
                {
                    Title = item.DisplayName,
                    SubTitle = "No crafting recipe found for this item",
                    IcoPath = "icon.png"
                }
            };
        }

        var formatted = FormatRecipe(recipe);

        return new List<Result>
        {
            new Result
            {
                Title = $"{item.DisplayName} x{recipe.ResultCount}",
                SubTitle = formatted,
                IcoPath = "icon.png"
            }
        };
    }


    private string FormatRecipe(Recipe recipe)
    {
        var ingredientsCount = new Dictionary<string, int>();

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                var ingredient = recipe.Matrix[y, x];
                if (ingredient == null) continue;

                var name = ingredient.Name.ToLower();
                if (!ingredientsCount.ContainsKey(name))
                    ingredientsCount[name] = 0;

                ingredientsCount[name]++;
            }
        }

        return string.Join(" ", ingredientsCount.Select(kvp => $"{kvp.Value}x{kvp.Key}"));
    }

    private string GetTranslation(string key)
    {
        if (!string.IsNullOrEmpty(key))
            return _context.API.GetTranslation(key) ?? "undefined";
        return "undefined";
    }

    /// <inheritdoc />
    public string GetTranslatedPluginTitle() => GetTranslation("mrb_title");

    /// <inheritdoc />
    public string GetTranslatedPluginDescription() => GetTranslation("mrb_description");
}
