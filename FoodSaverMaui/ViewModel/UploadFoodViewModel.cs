using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using FoodSaverMaui.Model;
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
        public Command OnFliePicked { get; }
        public Command IncrementCommand { get; }

        public Command OnPostTapped { get; }
        public Command DecrementCommand { get; }
        public UploadFoodViewModel()
        {
            OnFliePicked = new Command(async() => await ImagePicked());
            IncrementCommand = new Command(OnIncrement);
            DecrementCommand = new Command(OnDecrement);
            OnPostTapped = new Command(async () => await PostButtonTapped());

        }
        public bool IsImageSelected => PickedImage != null;
        private byte[] _imageData;



        public async Task PostButtonTapped()
        {
            try {
                if (!string.IsNullOrWhiteSpace(food) && !string.IsNullOrWhiteSpace(description) && !double.IsNaN(price)
                    && !double.IsNaN(_stepperValue) && !ImageSource.IsNullOrEmpty(_pickedImage) )
                    {

                    var requestModel = new PostFoodRequest()
                    {
                        ImageFile = _pickedImage,
                        ProductName = food,
                        Description = description,
                        Quantity = _stepperValue,
                        PricePerKg = price,

                    };


                }
            
            
            
            
            }
            catch { }
        
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
       

        //public void Reset()
        //{
        //    // Reset all the properties you want to initialize again
        //    PickedImage = null;
        //    _isInitialized = false;
        //}

        //public void Initialize()
        //{
        //    if (!_isInitialized)
        //    {
        //        // Initial setup when the page is first loaded
        //        // Perform any setup tasks
        //        _isInitialized = true;
        //    }
        //}
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
