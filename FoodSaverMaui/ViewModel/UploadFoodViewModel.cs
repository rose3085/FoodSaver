using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Model;
using FoodSaverMaui.Services.Food;
using FoodSaverMaui.Views;
using Microsoft.Maui.Storage;

namespace FoodSaverMaui.ViewModel
{
    public partial class UploadFoodViewModel : BaseViewModel
    {

        [ObservableProperty]
        string food;

        [ObservableProperty]
        string description;

        [ObservableProperty]
        double price;

        [ObservableProperty]
        string wardNumber;

        [ObservableProperty]
        string toleName;

        [ObservableProperty]
        string cityName;

        private double _stepperValue;

        public double StepperValue
        {
            get => _stepperValue;
            set
            {
                
                    _stepperValue = value;
                    OnPropertyChanged(nameof(StepperValue));  
                
            }
        }

        private ImageSource _pickedImage;

        public ImageSource PickedImage
        {
            get => _pickedImage;
            set
            {
                _pickedImage = value;
                OnPropertyChanged(nameof(PickedImage));

                OnPropertyChanged(nameof(IsImageSelected));
            }
        }

        private readonly FoodService _foodService;

        public Command OnFliePicked { get; }
        public Command IncrementCommand { get; }

        public Command OnPostTapped { get; }
        public Command DecrementCommand { get; }
        public UploadFoodViewModel(FoodService foodService)
        {
            _foodService = foodService;
            OnFliePicked = new Command(async() => await ImagePicked());
            IncrementCommand = new Command(OnIncrement);
            DecrementCommand = new Command(OnDecrement);
            OnPostTapped = new Command(async () => await PostButtonTapped());

        }
        public bool IsImageSelected => PickedImage != null;
        private byte[] _imageData;
        public string PickedImageName;



        public async Task PostButtonTapped()
        {
            try
            {
                IsBusy = true;
                if (!string.IsNullOrEmpty(food) && !string.IsNullOrEmpty(description) && !double.IsNaN(price)
                    && !double.IsNaN(_stepperValue) && !ImageSource.IsNullOrEmpty(_pickedImage) && !string.IsNullOrEmpty(PickedImageName) && !string.IsNullOrEmpty(wardNumber) && !string.IsNullOrEmpty(toleName)
                    && !string.IsNullOrEmpty(cityName)
                    )
                {
                    var result = await _foodService.PostFood(food, description, price, _stepperValue,wardNumber,toleName,cityName, _imageData, PickedImageName);
                    if (result != null)
                    {

                        //string resultError = "Enter Valid Credentials!!";
                      

                        if (result == "Product added successfully")
                        {

                            await Shell.Current.GoToAsync(nameof(PostSuccessfullPage));
                        }  
                        
                        var toast = Toast.Make($"{result}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                        await toast.Show();

                    }
                    else
                    {
                        string resultError = "Couldn't upload your post!!";
                        var toast = Toast.Make($"{resultError}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                        await toast.Show();

                    }
                }
                else
                {
                    string resultError = "Enter all fields!";
                    var toast = Toast.Make($"{resultError}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                    await toast.Show();

                }




            }
            catch
            {

                string resultError = "Couldn't upload your post!!";
                var toast = Toast.Make($"{resultError}", CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
                await toast.Show();
            }
            finally
            { 
                IsBusy = false;
            
            }
        
        }


        public async Task ImagePicked()
        {

            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Pick Image",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                PickedImageName = result.FileName;
                using (var stream = await result.OpenReadAsync())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        _imageData = memoryStream.ToArray();
                    }
                }
                PickedImage = ImageSource.FromStream(() => new MemoryStream(_imageData));
            }
        }
       

        private void OnIncrement()
        {
            StepperValue += 1; // Increment value by 1
        }

        // Method to decrement the value
        private void OnDecrement()
        {
            if (StepperValue > 0) // Avoid going below 0
            {
                StepperValue -= 1;
            }
        }

    }
}
