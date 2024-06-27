namespace RecipeApp_POE
{

    internal class Program
    {

        //--------------------------  MAIN METHOD  --------------------------------------------
        static void Main(string[] args)
        {
            Console.Title = "Recipe App";
            //loop control variable
            bool shouldContinue = true;

            Recipe recipe = null;

            //Subscribing to the event
            Recipe.caloriesExceed300 += OnCaloriesExceed300;

            //while loop to control the main menu of the program 
            while (shouldContinue)
            {

                displayMenu();
                string userChoice = Console.ReadLine();

                //____________________ CODE ATTRIBUTION _______________________________________
                //The following layout of using clearing the console was taken from TutorialsPoint 
                // Link: https://www.tutorialspoint.com/how-to-clear-screen-using-chash#:~:text=Use%20the%20Console.,left%20corner%20of%20the%20window. (Accessed 15 April 2024)
                Console.Clear();

                //Option 1: Enter a Recipe
                if (userChoice == "1")
                {
                    //Creating an object from the recipe class
                    recipe = new Recipe();
                    recipe.enterRecipeDetails(recipe);
                }

                //Option 2: Display Recipe
                else if (userChoice == "2")
                {
                    //This checks if there is a recipe to display. If not, the program will tell the user to enter a recipe first
                    if (recipe == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please add a recipe first.");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        recipe.displayAllRecipes();
                        Console.WriteLine("Which recipe would you like to display? ");
                        string recipeChosen = Console.ReadLine().ToUpper();
                        if (RecipeManager.allRecipes.ContainsKey(recipeChosen) == true)
                        {
                            Recipe recipeToDisplay = RecipeManager.allRecipes[recipeChosen];
                            recipe.displayRecipe(recipeToDisplay);
                        }
                        else
                        {
                            recipe.recipeNonExistent();
                        }

                    }
                }

                //Option 3: Scale the recipe
                else if (userChoice == "3")
                {
                    if (recipe == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please add a recipe first.");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        recipe.displayAllRecipes();
                        Console.WriteLine("Which recipe would you like to scale? ");
                        string recipeChosen = Console.ReadLine().ToUpper();
                        if (RecipeManager.allRecipes.ContainsKey(recipeChosen) == true)
                        {
                            Recipe recipeToScale = RecipeManager.allRecipes[recipeChosen];
                            Console.Write("How do you wish to scale the recipe? Enter 'half', 'double' or 'triple': ");
                            string scaleFactor = Console.ReadLine().ToLower();
                            recipeToScale.ScaleAndConvertUnits(recipeToScale, scaleFactor);
                        }
                        else
                        {
                            recipe.recipeNonExistent();
                        }
                    }
                }

                //Option 4: Reset to original values

                else if (userChoice == "4")
                {
                    if (recipe == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please add a recipe first.");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        recipe.displayAllRecipes();
                        Console.WriteLine("Which recipe would you like to reset? ");
                        string recipeChosen = Console.ReadLine().ToUpper();
                        if (RecipeManager.allRecipes.ContainsKey(recipeChosen) == true)
                        {
                            Recipe recipeToReset = RecipeManager.allRecipes[recipeChosen];
                            recipeToReset.recipeUnitsToDisplay = recipeToReset.IngredientUnitsOfMeasurement;
                            for (int i = 0; i < recipeToReset.Num_ingredients; i++)
                            {
                                recipeToReset.IngredientQuantitiesScaled[i] = recipeToReset.IngredientQuantities[i];
                            }
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("The quantities have been reset to their original values.");
                            recipe.displayRecipe(recipe);
                        }
                        else
                        {
                            recipe.recipeNonExistent();
                        }
                    }
                }

                //Option 5: Clear Recipe
                else if (userChoice == "5")
                {
                    if (recipe == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Please add a recipe first.");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        recipe.displayAllRecipes();
                        Console.WriteLine("Which recipe would you like to clear? ");
                        string recipeChosen = Console.ReadLine().ToUpper();
                        if (RecipeManager.allRecipes.ContainsKey(recipeChosen) == true)
                        {
                            Console.Write("Are you sure you want to clear the recipe? Enter y or n: ");
                            if (Console.ReadLine().ToLower() == "y")
                            {
                                RecipeManager.allRecipes.Remove(recipeChosen);
                                Console.WriteLine("The recipe has been cleared. You can select menu item 1 if you wish to add another recipe.");
                            }
                            else
                            {
                                Console.WriteLine("The recipe hasn't been removed.");
                            }
                        }
                        else
                        {
                            recipe.recipeNonExistent();
                        }
                    }
                }

                //Option 6: Exit
                else if (userChoice == "6")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Goodbye! See you soon");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    shouldContinue = false;
                }

                //If the  user enters an invalid menu number, they will be informed
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a valid menu number\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

            }
        }

        // ------------------------------  METHODS -----------------------------------------   

        //Method 1:
        //This method displays the menu 
        public static void displayMenu()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n*******************************");
            Console.WriteLine("MENU:");
            Console.WriteLine("1. Enter a recipe\n2. Display recipe\n3. Scale recipe\n4. Reset quanitities to original values\n5. Clear recipe\n6. Exit application");
            Console.WriteLine("*******************************");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("\nWhat would you like to do? Enter the corresponding number: ");
        }

        //Method 2:
        //This method is called when the caloriesExceed300 event is raised
        static void OnCaloriesExceed300(double? totalCalories)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Attention: The total calories of {totalCalories} for this recipe exceeds 300!");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

    }
}

//-------------------------------------------------------------------------------------
// FEEDBACK IMPLEMENTED FROM PART 1 
// I correctly implemented the scaling functionality such that the recipe is  scaled on the last value and not the original value
// I moved the methods in Program.cs after my Main[] method 
// I moved my methods to the Recipe class and simply call those methods in the Main[] method of Program.cs
// ---------------------------------------------------------------------------------------


//-------------------------------------------------------------------------------------
// FEEDBACK IMPLEMENTED FROM PART 2 
// I correctly implemented the display recipe functionality such that the calories for each ingredient is displayed 
// ---------------------------------------------------------------------------------------

