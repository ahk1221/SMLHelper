﻿namespace SMLHelper.Handlers
{    
    using SMLHelper.Patchers.EnumPatching;
    using SMLHelper.Utility;
    using System.Collections.Generic;

    /// <summary>
    /// A handler class for everything related to creating new TechCategories.
    /// </summary>
    public static class TechCategoryHandler
    {

        /// <summary>
        /// Adds a new <see cref="TechCategory" /> into the game.
        /// </summary>
        /// <param name="techCatagoryName">The name of the TechCategory. Should not contain special characters.</param>
        /// <param name="displayName">The display name of the TechCategory. Can be anything.</param>
        /// <returns>
        /// The new <see cref="TechCategory" /> that is created.
        /// </returns>
        public static TechCategory AddTechCategory(string techCatagoryName, string displayName)
        {
            TechCategory techCategory = TechCategoryPatcher.AddTechCategory(techCatagoryName);

            Dictionary<TechCategory, string> valueToString = uGUI_BlueprintsTab.techCategoryStrings.valueToString;
            valueToString[techCategory] = "TechCategory" + techCatagoryName;
            

            LanguageHandler.SetLanguageLine("TechCategory" + techCatagoryName, displayName);

            return techCategory;
        }

        /// <summary>
        /// Safely looks for a modded category from another mod in the SMLHelper TechCategoryCache.
        /// </summary>
        /// <param name="techCategoryString">The string used to define the techcategory.</param>
        /// <returns>
        ///   <c>True</c> if the item was found; Otherwise <c>false</c>.
        /// </returns>
        public static bool ModdedTechCategoryExists(string techCategoryString)
        {
            EnumTypeCache cache = TechCategoryPatcher.cacheManager.RequestCacheForTypeName(techCategoryString, false);

            if(cache != null) // Item Found
            {
                return true;
            }
            else // Mod not present or not yet loaded
            {
                return false;
            }
        }

        /// <summary>
        /// Safely looks for a modded category from another mod in the SMLHelper TechCategoryCache and outputs its <see cref="TechCategory" /> value when found.
        /// </summary>
        /// <param name="techCategoryString">The string used to define the techcategory.</param>
        /// <param name="modTechCategory">The TechCategory enum value of the modded. Defaults to <see cref="TechCategory.Misc" /> when the item was not found.</param>
        /// <returns>
        ///   <c>True</c> if the item was found; Otherwise <c>false</c>.
        /// </returns>
        public static bool TryGetModdedTechCategory(string techCategoryString, out TechCategory modTechCategory)
        {
            EnumTypeCache cache = TechCategoryPatcher.cacheManager.RequestCacheForTypeName(techCategoryString, false);

            if(cache != null) // Item Found
            {
                modTechCategory = (TechCategory)cache.Index;
                return true;
            }
            else // Mod not present or not yet loaded
            {
                modTechCategory = TechCategory.Misc;
                return false;
            }
        }

        /// <summary>
        /// Registers the TechCategory to a TechGroup in CraftData.groups.
        /// </summary>
        /// <param name="techGroup">The tech group.</param>
        /// <param name="techCategory">The tech category.</param>
        /// <returns></returns>
        public static bool TryRegisterTechCategoryToTechGroup(TechGroup techGroup, TechCategory techCategory)
        {
            if(!CraftData.groups.TryGetValue(techGroup, out Dictionary<TechCategory, List<TechType>> techCategories))
            {
                //Should not even really be possible but just incase.
                return false;
            }

            if(techCategories.ContainsKey(techCategory))
            {
                return true;
            }

            techCategories[techCategory] = new List<TechType>();
            return true;
        }
    }
}
