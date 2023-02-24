using System;

namespace UI.Scripts.LoseWindow
{
    public class LoseWindowArguments : UIArgumentsForPanels
    {
        public Action ChefHelpAction => _chefHelpAction;
        private Action _chefHelpAction;

        public LoseWindowArguments(Action chefHelpAction)
        {
            _chefHelpAction = chefHelpAction;
        }
    }
}