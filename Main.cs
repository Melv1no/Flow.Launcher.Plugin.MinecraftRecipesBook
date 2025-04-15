using System;
using System.Collections.Generic;

namespace Flow.Launcher.Plugin.MinecraftRecipesBook
{
    /// <summary>
    /// Minecraft Recipes Book 
    /// </summary>
    public class MinecraftRecipesBook : IPlugin, IPluginI18n
    {
        private PluginInitContext _context;
        /// <inheritdoc />
        public void Init(PluginInitContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public List<Result> Query(Query query)
        {
            var result = new Result()
            {
                Title = $"{GetTranslation("mrb_title")}",
                SubTitle = $"{GetTranslation("mrb_usage")}",
            };
            return new List<Result> {result};
        }
        
        private string GetTranslation(string key)
        {
            if(!String.IsNullOrEmpty(key))
                return _context.API.GetTranslation(key) != null ? _context.API.GetTranslation(key) : "undefined";
            return "undefined";
        }

        /// <inheritdoc />
        public string GetTranslatedPluginTitle()
        {
            return GetTranslation("mrb_title");
        }

        /// <inheritdoc />
        public string GetTranslatedPluginDescription()
        {
            return GetTranslation("mrb_description");
        }
    }
}