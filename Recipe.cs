using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace RecipeApp_POE
{
    //This is the recipe class.
    //All the lists used to store information in the progrram belong to this class
    //Various methods used in this program are also part of this class
    public class Recipe
    {
        // Declare the caloriesExceed300 event using the CaloriesExceed300Handler delegate. This event is triggered
        // when the calorie count exceeds 300. The event can be null if no handlers are attached.
        public static event CaloriesExceed300Handler? caloriesExceed300;

        // Define a delegate named CaloriesExceed300Handler that specifies the method signature for handlers of the
        // caloriesExceed300 event. The delegate takes a nullable double (double?) parameter that represents the total calories
        public delegate void CaloriesExceed300Handler(double? totalCalories);


        //____________________ CODE ATTRIBUTION _______________________________________
        //The following layout  of using automatic properties (shorthand) was taken from W3 Schools 
        // Link: https://www.w3schools.com/cs/cs_properties.php (Accessed 15 April 2024)
        public string Name { get; set; }
        public int Num_ingredients { get; set; }
        public int Num_steps { get; set; }
        public double ScaleFactor { get; set; }
        public double? totalCalories { get; set; }


        //____________________ CODE ATTRIBUTION _______________________________________
        //The following layout of how to use Lists in C# was taken from GeeksForGeeks 
        // Link: https://www.geeksforgeeks.org/c-sharp-list-class/ (Accessed 12 May 2024)
        public List<string> Ingredients = new List<string>();
        public List<double> IngredientQuantities = new List<double>();
        public List<double> IngredientQuantitiesScaled = new List<double>();
        public List<string> IngredientUnitsOfMeasurement = new List<string>();
        public List<double> IngredientCalories = new List<double>();
        public List<string> recipeUnitsToDisplay = new List<string>();
        public List<string> Steps = new List<string>();
        public List<string> FoodGroups = new List<string>();

        public Recipe()
        {
            totalCalories = 0;
        }

        public static Dictionary<string, Tuple<double, string>> conversionsDict = new Dictionary<string, Tuple<double, string>>() {
                {"tablespoon", Tuple.Create(16.0, "cup") },
                {"tablespoons", Tuple.Create(16.0, "cup") },
                {"teaspoon", Tuple.Create(48.0, "cup") },
                {"teaspoons", Tuple.Create(48.0, "cup") },
            };

        //METHOD 1:
        //This method validates user input. When a user is required to enter an int input, it ensures they have done accordingly.
        public static int? checkIntInput(string userInput)
        {
            int? userInputInt = null;
            try
            {
                userInputInt = Convert.ToInt32(userInput);
            }
            catch (Exception ex)
            {
                //____________________ CODE ATTRIBUTION _______________________________________
                //The following layout of changing the console foreground color was taken from GeeksForGeeks 
                // Link: https://www.geeksforgeeks.org/c-sharp-how-to-change-foreground-color-of-text-in-console/ (Accessed 15 April 2024)
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please enter a valid input");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            return userInputInt;
        }

        //METHOD 2:
        //This method validates user input. When a user is required to enter an double input, it ensures they have done accordingly.
        public static double? checkDoubleInput(string userInput)
        {
            double? userInputDouble = null;
            try
            {
                userInputDouble = Convert.ToDouble(userInput);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please enter a valid input");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            return userInputDouble;
        }

        //METHOD 3:
        //This method allows the user to enter all the recipe details, captures it and saves it in lists
        //as well as saves the recipe object in the allRecipes dictionary of the RecipeManager class
        public void enterRecipeDetails(Recipe recipe)
        {

            Console.WriteLine("Enter the name of the recipe: ");
            recipe.Name = Console.ReadLine().ToUpper();

            //A do-while loop has been used to ensure the user enters an int value for the number of ingredients
            int? userInputChecked = null;
            do
            {
                Console.Write("\nEnter the number of ingredients for this recipe: ");
                userInputChecked = checkIntInput(Console.ReadLine());
            } while (userInputChecked == null);
            recipe.Num_ingredients = userInputChecked.Value;

            //Get all the necessary information regarding each ingredient from the user
            for (int i = 0; i < recipe.Num_ingredients; i++)
            {
                Console.Write("\nName of Ingredient " + (i + 1) + ": ");
                recipe.Ingredients.Add(Console.ReadLine());

                double? userDoubleChecked = null;
                do
                {
                    Console.Write("Quantity of Ingredient " + (i + 1) + ": ");
                    userDoubleChecked = checkDoubleInput(Console.ReadLine());
                } while (userDoubleChecked == null);
                recipe.IngredientQuantities.Add(userDoubleChecked.Value);
                recipe.IngredientQuantitiesScaled.Add(userDoubleChecked.Value);

                Console.Write("Unit of measurement for Ingredient " + (i + 1) + ": ");
                string? unitOfMeasurement = Console.ReadLine().ToLower();

                double? caloriesChecked = null;
                do
                {
                    Console.Write("Number of calories for Ingredient " + (i + 1) + ": ");
                    caloriesChecked = checkDoubleInput(Console.ReadLine());
                } while (caloriesChecked == null);
                addCalories(recipe, caloriesChecked);
                recipe.IngredientCalories.Add(caloriesChecked.Value);

                if (recipe.totalCalories > 300)
                {
                    caloriesExceed300?.Invoke(recipe.totalCalories);
                }
                ProvideCalorieContextPerIngredient(caloriesChecked);

                bool foodGroupEntered = false;
                string foodGroup = null;
                do
                {
                    Console.Write("\nWhat is the food group of Ingredient " + (i + 1) + "?\n1. Starchy foods\n2. Vegetables and fruits\n3. Dry beans, peas, lentils and soya\n4. Chicken, fish, meat and eggs\n5. Milk and dairy products\n6. Fats and oil\n7. Water\nEnter the corresponding number: ");
                    string userChosenFoodGroup = Console.ReadLine();
                    switch (userChosenFoodGroup)
                    {
                        case "1":
                            foodGroup = "Starchy foods";
                            Console.WriteLine("Starchy foods are a good source of energy and the main source of a range of nutrients in our diet.");
                            foodGroupEntered = true;
                            break;
                        case "2":
                            foodGroup = "Vegetables and fruits";
                            Console.WriteLine("Rich in vitamins, minerals, and fiber, vegetables and fruits are essential for good health and may reduce the risk of disease.");
                            foodGroupEntered = true;
                            break;
                        case "3":
                            foodGroup = "Dry beans, peas, lentils and soya";
                            Console.WriteLine("These are great sources of protein and fiber, which help in muscle repair and can aid digestion.");
                            foodGroupEntered = true;
                            break;
                        case "4":
                            foodGroup = "Chicken, fish, meat and eggs";
                            Console.WriteLine("Important sources of high-quality protein and other essential nutrients like omega-3 fatty acids (from fish).");
                            foodGroupEntered = true;
                            break;
                        case "5":
                            foodGroup = "Milk and dairy products";
                            Console.WriteLine("These provide calcium, which is essential for healthy bones and teeth, as well as being a source of protein and vitamins.");
                            foodGroupEntered = true;
                            break;
                        case "6":
                            foodGroup = "Fats and oil";
                            Console.WriteLine("Essential for long-term energy, absorption of certain vitamins, and cell function. Use in moderation.");
                            foodGroupEntered = true;
                            break;
                        case "7":
                            foodGroup = "Water";
                            Console.WriteLine("Essential for life, water plays a critical role in every bodily function, from digestion to temperature regulation.");
                            foodGroupEntered = true;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Please enter a valid input");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                    }
                } while (!foodGroupEntered);
                recipe.FoodGroups.Add(foodGroup);

                recipe.recipeUnitsToDisplay.Add(unitOfMeasurement);
                recipe.IngredientUnitsOfMeasurement.Add(unitOfMeasurement);

            }
            userInputChecked = null;
            do
            {
                Console.Write("\nEnter the number of steps for this recipe: ");
                userInputChecked = checkIntInput(Console.ReadLine());
            } while (userInputChecked == null);
            recipe.Num_steps = userInputChecked.Value;
            for (int i = 0; i < recipe.Num_steps; i++)
            {
                Console.Write("\nEnter a description of what the user should do for Step " + (i + 1) + ": ");
                recipe.Steps.Add(Console.ReadLine());
            }
            RecipeManager.allRecipes.Add(recipe.Name, recipe);
            Console.WriteLine("\nYour recipe has been added!");
        }

        //METHOD 4:
        //This method displays all the recipes to the user in alphabetical order
        public void displayAllRecipes()
        {
            var sortedKeys = RecipeManager.allRecipes.Keys.OrderBy(key => key);

            // Printing the sorted keys
            foreach (var key in sortedKeys)
            {
                Console.WriteLine(key);
            }
        }

        //METHOD 5:
        //This method displays the full recipe details in a neat format to the user 
        public void displayRecipe(Recipe recipe)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n############# FULL RECIPE #################\n");
            Console.WriteLine("");
            Console.WriteLine(recipe.Name);
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("Number of ingredients required: " + recipe.Num_ingredients);
            Console.WriteLine("Number of steps: " + recipe.Num_steps);

            Console.WriteLine("\nINGREDIENTS:");
            for (int i = 0; i < recipe.Num_ingredients; i++)
            {
                Console.WriteLine("- " + recipe.IngredientQuantitiesScaled[i] + " " + recipe.recipeUnitsToDisplay[i] + " " + recipe.Ingredients[i] + " (" + recipe.FoodGroups[i] + ")  -  " + recipe.IngredientCalories[i] + " calories");
            }
            Console.WriteLine("\nSTEPS:");
            for (int i = 0; i < recipe.Num_steps; i++)
            {
                Console.WriteLine((i + 1) + ". " + recipe.Steps[i]);
            }
            Console.WriteLine("\nTotal calories: " + recipe.totalCalories);
            if (recipe.totalCalories > 300)
            {
                caloriesExceed300?.Invoke(recipe.totalCalories);
            }
            ProvideCalorieExplanation(recipe);
        }

        //METHOD 6:
        //This method calculates the totalCalories by adding the calories for each ingredient to the totalCalories property of the recipe object
        public void addCalories(Recipe recipe, double? calories)
        {
            recipe.totalCalories += calories;
        }

        //METHOD 7:
        //This method provides an explanation based on the calorie count for the whole recipe
        public void ProvideCalorieExplanation(Recipe recipe)
        {
            if (recipe.totalCalories <= 200)
            {
                Console.WriteLine("This is a low-calorie dish, ideal for weight loss or maintenance diets.");
            }
            else if (recipe.totalCalories <= 300)
            {
                Console.WriteLine("This dish has a moderate calorie count, suitable for everyday meals.");
            }
            else if (recipe.totalCalories <= 400)
            {
                Console.WriteLine("While slightly above typical daily meal recommendations, it's still manageable.");
            }
            else if (recipe.totalCalories <= 600)
            {
                Console.WriteLine("This is a high-calorie dish, best suited for post-workout meals or if you're trying to gain weight.");
            }
            else
            {
                Console.WriteLine("This dish is very high in calories and should be consumed sparingly unless under specific dietary needs.");
            }
        }

        //METHOD 8:
        //This method provides an explanation based on the calorie count for each ingredient of the recipe
        private void ProvideCalorieContextPerIngredient(double? calorie)
        {
            if (calorie < 50)
                Console.WriteLine("This is a low-calorie ingredient, helpful for weight management.");
            else if (calorie < 200)
                Console.WriteLine("This ingredient has a moderate calorie count, fitting well into balanced meals.");
            else
                Console.WriteLine("This is a high-calorie ingredient, best used sparingly unless energy needs are high.");
        }

        //METHOD 9:
        //This method halves the ingredient quantities 
        public void halfRecipe(Recipe recipe)
        {
            for (int i = 0; i < recipe.Num_ingredients; i++)
            {
                recipe.IngredientQuantitiesScaled[i] = recipe.IngredientQuantitiesScaled[i] * 0.5;
            }
        }

        //METHOD 10:
        //This method doubles the ingredient quantities 
        public void doubleRecipe(Recipe recipe)
        {
            for (int i = 0; i < recipe.Num_ingredients; i++)
            {
                recipe.IngredientQuantitiesScaled[i] = recipe.IngredientQuantitiesScaled[i] * 2;
            }
        }

        //METHOD 11:
        //This method triples the ingredient quantities 
        public void tripleRecipe(Recipe recipe)
        {
            for (int i = 0; i < recipe.Num_ingredients; i++)
            {
                recipe.IngredientQuantitiesScaled[i] = recipe.IngredientQuantitiesScaled[i] * 3;
            }
        }

        //METHOD 12:
        /*
       * The `ScaleAndConvertUnits` method in the Recipe application is designed to adjust the quantities of
       * ingredients based on a specified scaling factor—either 'half', 'double', or 'triple'.
       * Once the quantities are scaled, the method checks each ingredient to determine if the new quantity 
       * reaches a threshold that necessitates a conversion to a different unit of measurement, based on 
       * predefined conversion rules. If so, the method performs the conversion, ensuring that both the 
       * quantities and their corresponding units are appropriately adjusted. After scaling and converting 
       * the units, the updated recipe is automatically displayed to the user. 
       */
        public void ScaleAndConvertUnits(Recipe recipe, string scaleFactor)
        {
            switch (scaleFactor)
            {
                case "half":
                    recipe.halfRecipe(recipe);
                    Console.WriteLine("The recipe has been halved.");
                    break;
                case "double":
                    recipe.doubleRecipe(recipe);
                    Console.WriteLine("The recipe has been doubled.");
                    break;
                case "triple":
                    recipe.tripleRecipe(recipe);
                    Console.WriteLine("The recipe has been tripled.");
                    break;
                //If the user enters an invalid scale factor, they will be informed
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a valid scale factor");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }

            for (int i = 0; i < recipe.Num_ingredients; i++)
            {
                string currentUnit = recipe.recipeUnitsToDisplay[i];
                double quantity = recipe.IngredientQuantities[i];

                if (Recipe.conversionsDict.ContainsKey(currentUnit) && quantity >= Recipe.conversionsDict[currentUnit].Item1)
                {
                    recipe.IngredientQuantities[i] = quantity / Recipe.conversionsDict[currentUnit].Item1;
                    recipe.recipeUnitsToDisplay[i] = Recipe.conversionsDict[currentUnit].Item2;
                }
            }
            displayRecipe(recipe);
        }
        //____________________ CODE ATTRIBUTION _______________________________________
        //The layout above of how to convert units of measurements when they reach a certain threshold was taken from OpenAI - ChatGPT 
        // I looked at the logic, adapted it to suit my code and implemented it (Accessed 15 April 2024)

        //METHOD 13:
        //This method informs the user that the recipe doesn't exist
        public void recipeNonExistent()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("That recipe doesn't exist");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }

}

