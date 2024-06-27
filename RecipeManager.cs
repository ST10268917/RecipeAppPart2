using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp_POE
{
    internal class RecipeManager
    {
        //This dictionary stores all the recipe names and their corresponding recipe objects as key-value pairs
        public static Dictionary<string, Recipe> allRecipes = new Dictionary<string, Recipe>();
    }
}
