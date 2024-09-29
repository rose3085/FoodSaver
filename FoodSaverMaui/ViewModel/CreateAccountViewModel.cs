using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.ViewModel
{
   public partial class CreateAccountViewModel : BaseViewModel
    {
        private bool _isRadioButtonsVisible;

        public bool IsRadioButtonsVisible
        {
            get => _isRadioButtonsVisible;
            set
            {
                _isRadioButtonsVisible = value;
                OnPropertyChanged();
            }
        }

        public Command ToggleRadioButtonsCommand { get; }

        public CreateAccountViewModel()
        {
            ToggleRadioButtonsCommand = new Command(async () => await OnToggleRadioButtonsCommand());
        }


        public async Task OnToggleRadioButtonsCommand()
        {
            IsRadioButtonsVisible = !IsRadioButtonsVisible;

        }

    }
}
