using Application.DTO.Foods;
using Application.Interfaces.Data;
using Application.Interfaces.Food;
using Application.Response.Food;
using AutoMapper;
using System.Linq.Expressions;
using Domain.Entities.Foods;
using Domain.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;
using Domain.Entities.Location;
using System.Security.Claims;


namespace Application.Services.Food
{
    public class FoodService : IFoodService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _uow;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostEnvironment _environment;
        public FoodService(IUnitOfWork uow, IHostEnvironment environment,UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _uow = uow;
            _environment = environment;
        }
        public async Task<FoodServiceResponse> AddFood(PostFoodRequestDto postFood)
        {
            try
            {
                // string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


                //get user info from authorization token
                var userInfo = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
                if (userInfo != null)
                {
                    var userId = userInfo?.Id;
                  
                    var userRole = await _userManager.GetRolesAsync(userInfo);

                    if (!userRole.Contains("Seller"))
                    {

                        return new FoodServiceResponse()
                        {
                            IsSuccess = false,
                            Message = "You must be Seller to add food.",

                        };
                    }
                    if (userInfo.CanPost == false)
                    {
                        return new FoodServiceResponse()
                        {
                            IsSuccess = false,
                            Message = "Daily Limit reached! Please pay to continue posting.",

                        };

                    }


                    if (postFood.ImageFile?.Length > 5 * 1024 * 1024)
                    {
                        return new FoodServiceResponse()
                        {
                            IsSuccess = false,
                            Message = "File size shouldn't exist 5MB.",

                        };
                    }
                    string[] allowedFileExtentions = [".jpg", ".jpeg", ".png"];
                    string createdImageName = await SaveFileAsync(postFood.ImageFile, allowedFileExtentions);
                    var id = Guid.NewGuid().ToString();
                    var addressId = Guid.NewGuid().ToString();
                    var address = new AddressModel
                    { 
                        Id = addressId,
                        WardNumber = postFood.WardNumber,
                        ToleName = postFood.ToleName,
                        CityName = postFood.CityName,
                        Latitude = postFood.Latitude,
                        Longitude = postFood.Longitude,
                       
                    };
                    var product = new FoodModel
                    {
                        Id = id,
                        FoodName = postFood.ProductName,
                        Description = postFood.Description,
                        PricePerKg = postFood.PricePerKg,
                        Quantity = postFood.Quantity,
                        ProductImage = createdImageName,
                        Seller = userInfo,
                        Address = address,
                        Date = DateTime.Today,
                    };
                    var createdProduct = await _uow.AsyncRepositories<FoodModel>().AddAsync(product);
                   
                    _uow.save();
                    return new FoodServiceResponse()
                    {
                        IsSuccess = true,
                        Message = "Product added successfully",

                    };

                }
                else 
                {

                    return new FoodServiceResponse()
                    {
                        IsSuccess = false,
                        Message = "Login to your account first!",

                    };
                }
            }
            catch (Exception ex)
            {
                return new FoodServiceResponse()
                {
                    IsSuccess = false,
                    //Message = "File size shouldn't exist 1MB.",
                    Error = ex.Message,

                };

            }
        }

        public async Task<FoodServiceResponse> DeleteFood(string foodId)
        {
            try {
                var includes = new Expression<Func<FoodModel, object>>[]
                   {
                        s => s.Seller,
                   };
                var seller = await _uow.AsyncRepositories<FoodModel>().GetWithIncludeAndId(foodId,includes);
                if (seller == null)
                {
                    return new FoodServiceResponse()
                    {
                        IsSuccess = false,
                        Message = "select a valid food!",
                        

                    };

                }

                var userInfo = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);

                if (userInfo == null)
                {
                    return new FoodServiceResponse()
                    {
                        IsSuccess = false,
                        Message = "You are not authorized to delete the food",


                    };
                }

                var result = _uow.AsyncRepositories<FoodModel>().DeleteById(foodId);
                _uow.save();

                return new FoodServiceResponse()
                { 
                    IsSuccess = true,
                    Message = "Food deleted Sucessfully",
                
                };
            }
            catch (Exception ex)
            {
                return new FoodServiceResponse()
                {
                    IsSuccess = false,
                    //Message = "File size shouldn't exist 1MB.",
                    Error = ex.Message,

                };

            }

        }



        public async Task<string> CalculateTime(DateTime dateTime)
        {
            var currentTime = DateTime.Now;
            var timeDifference = currentTime - dateTime;
            if (timeDifference.TotalMinutes < 60)
            {
                return $"{(int)timeDifference.TotalMinutes}m";

            }
            else if (timeDifference.TotalHours < 24)
            {
                return $"{(int)timeDifference.TotalHours}h";

            }
            else 
            {
                return $"{(int)timeDifference.TotalDays}d";
            }

        }

        public async Task<IEnumerable<GetProductResponse>> GetProductsAsync()
        {
            var includes = new Expression<Func<FoodModel, object>>[]
                   {
                        s => s.Seller,
                        s => s.Address,
                   };
            var result = await _uow.AsyncRepositories<FoodModel>().GetRandomWithIncludeAsync(includes);


             var baseUrl = $"https://7112-2405-acc0-1504-cce4-9534-963d-17f3-f208.ngrok-free.app";
            // var baseUrl = $"https://localhost:7293";
            var productResult =  result
                .Where(product => product.IsBooked == false)
                .Select(async product => 
            {
                var timeDifference = await CalculateTime(product.Date);
                return new GetProductResponse
                {
                    Id = product.Id,
                    ProductName = product.FoodName,
                    Description = product.Description,
                    PricePerKg = product.PricePerKg,
                    Quantity = product.Quantity,
                    IsBooked = product.IsBooked,
                    UserName = product.Seller?.UserName,
                    CityName = product.Address?.CityName,
                    ToleName = product.Address?.ToleName,
                    Latitude = product.Address.Latitude,
                    Longitude = product.Address.Longitude,
                    Date = timeDifference,
                    // image Url form ma return garne
                    ImageUrl = $"{baseUrl}/Resources/{product.ProductImage}"
                };
            }).ToList();
            // var getResult = _mapper.Map<IEnumerable<GetProductResponse>>(productResult);
            var finalResult = await Task.WhenAll(productResult);
            return finalResult;
        }

        public async Task<IEnumerable<GetProductResponse>> GetUsersProduct()
        {
            try {

                var userInfo = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
                if (userInfo != null)
                {
                    var userNameContext = userInfo.UserName;
                    var getProduct = await GetProductsAsync();
                    if (getProduct != null)
                    {
                        var resultProduct = getProduct
                                    .Where(result => result.UserName == userNameContext).ToList();
                        if (resultProduct != null)
                        {
                            return resultProduct;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else {
                    return null;
                }
            }
            catch 
            {
                return null;
            }
        }

        public async Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions)
        {
            if (imageFile == null)
            {
                throw new ArgumentNullException(nameof(imageFile));
            }
            var contentPath = _environment.ContentRootPath;
            var path = Path.Combine(contentPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var ext = Path.GetExtension(imageFile.FileName);
            if (!allowedFileExtensions.Contains(ext))
            {
                throw new ArgumentException($"Only {string.Join(",", allowedFileExtensions)} are allowed.");
            }
            // generate a unique filename
            var fileName = $"{Guid.NewGuid().ToString()}{ext}";
            var fileNameWithPath = Path.Combine(path, fileName);
            using var stream = new FileStream(fileNameWithPath, FileMode.Create);
            await imageFile.CopyToAsync(stream);
          
            return fileName;
        }
    }
}
