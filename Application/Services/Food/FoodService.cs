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


                    if (postFood.ImageFile?.Length > 1 * 1024 * 1024)
                    {
                        return new FoodServiceResponse()
                        {
                            IsSuccess = false,
                            Message = "File size shouldn't exist 1MB.",

                        };
                    }
                    string[] allowedFileExtentions = [".jpg", ".jpeg", ".png"];
                    string createdImageName = await SaveFileAsync(postFood.ImageFile, allowedFileExtentions);
                    var id = Guid.NewGuid().ToString();
                    var product = new FoodModel
                    {
                        Id = id,
                        FoodName = postFood.ProductName,
                        Description = postFood.Description,
                        PricePerKg = postFood.PricePerKg,
                        Quantity = postFood.Quantity,
                        ProductImage = createdImageName,
                        Users = new List<ApplicationUser> {userInfo },
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
                        s => s.Users,
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

                if (!seller.Users.Contains(userInfo))
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

        public async Task<IEnumerable<GetProductResponse>> GetProductsAsync()
        {
            var result = await _uow.AsyncRepositories<FoodModel>().GetRandomAsync();


             var baseUrl = $"https://0432-2405-acc0-1504-9a1f-7d83-813b-6a64-8e82.ngrok-free.app";
            // var baseUrl = $"https://localhost:7293";
            var productResult =  result
                .Where(product => product.IsBooked == false)
                .Select(product => new GetProductResponse
            {
               Id= product.Id,
               ProductName= product.FoodName,
               Description= product.Description,
               PricePerKg = product.PricePerKg,
               Quantity = product.Quantity,
               IsBooked = product.IsBooked,

               // image Url form ma return garne
                ImageUrl = $"{baseUrl}/Resources/{product.ProductImage}"
            }).ToList();
           // var getResult = _mapper.Map<IEnumerable<GetProductResponse>>(productResult);
            return productResult;
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
