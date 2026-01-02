# DOCTORLOAN

**DOCTORLOAN** is a clinic management web project, developed with **ASP.NET Core 7.0** and containerized using **Docker**.

---

## 🚀 Technologies Used

- **ASP.NET Core 7.0**
- **Docker**
- **LESS, CSS, JS**
- **External REST API** (backend data source)
- **Newtonsoft.Json** for JSON serialization
- **Bootstrap** for UI framework

---

## 🛠️ Installation & Run Guide

### 1. Clone the source code
```sh
git clone https://github.com/DOCTORLOAN/DOCTORLOAN.git
cd DOCTORLOAN
```

### 2. Build and run with Docker
```sh
# Clean Docker build
docker-compose build --no-cache

# Build and start containers
docker-compose up --build

# Stop and remove containers
docker-compose down

# Check status at: 
http://localhost:8081
```

### 3. Compile LESS to CSS
```sh
npm install -g less
npm run build-css
```

---

## 📂 Project Structure

```
DOCTORLOAN/
│── Controllers/           # Controllers for handling business logic
│   ├── ProductsController.cs     # Product listing and detail pages
│   ├── CartController.cs         # Shopping cart and payment processing
│   ├── HomeController.cs         # Home page and Payoo payment processing
│   ├── NewsController.cs         # News listing and detail pages
│   ├── ContactController.cs      # Contact form and booking
│   └── ...
│── Models/                # Data models
│   ├── Api/               # API response models
│   ├── Bookings/          # Booking models
│   ├── Orders/            # Order and payment models
│   ├── Products/          # Product view models
│   ├── Payoo/             # Payoo payment configuration
│   ├── NewsModal/         # News modal configuration
│   ├── Users/             # User models
│   └── VMAuth/            # Authentication view models
│── ViewComponents/        # Reusable UI components
│   ├── ProductCategoriesViewComponent.cs  # Product categories for header
│   └── NewsModalViewComponent.cs          # News modal for homepage
│── Services/              # Business services
│   └── PayooService.cs    # Payoo payment processing service
│── Helpers/               # Helper classes
│   └── LoadingStateHelper.cs     # Loading state management
│── Constants/             # Application constants
│   └── ApiConstants.cs    # Centralized API endpoints
│── Views/                 # Razor views
│── wwwroot/               # Static files (CSS, JS, images...)
│── appsettings.json       # Application configuration
│── docker-compose.yml     # Docker Compose configuration
│── Dockerfile             # Docker build configuration
└── README.md
```

---

## 🔌 Key Features

### Products Management
- **GET /Products/Index**: Display list of all products
- **GET /Products/ProductDetail**: Display detailed product information
- Server-side rendering with loading states
- Product categories navigation

### Shopping Cart & Payment
- Shopping cart management
- Payment processing via Payoo (server-side for security)
- Order creation and tracking

### News System
- News listing with categories
- News detail pages
- Auto-display news modal on homepage (server-side rendered, keyword configurable in appsettings.json)

### Contact & Booking
- Contact forms
- Medical registration
- Clinic booking system

---

## 🔐 Security Features

### Payoo Payment Integration
- **Server-side processing**: All payment credentials stored securely in `appsettings.json`
- **SHA-512 checksum**: Secure payment data validation
- **XML escaping**: Protection against XML injection attacks
- **Input validation**: Server-side validation for all payment data

### API Configuration
- Centralized API endpoints in `ApiConstants.cs`
- Configurable API base URLs
- Environment-specific configurations

---

## ⚙️ Configuration

### appsettings.json Structure

```json
{
  "Payoo": {
    "BaseUrl": "https://payoo.vn/v2/",
    "BusinessUsername": "...",
    "ShopID": "...",
    "ChecksumKey": "...",
    ...
  },
  "NewsModal": {
    "Keyword": "Kinh doanh"
  }
}
```

### API Constants

All API endpoints are centralized in `Constants/ApiConstants.cs`:
- Product Module APIs
- Order Module APIs
- Booking Module APIs
- News Module APIs

---

## ⚡ Loading State Management

The application implements a **LoadingState** system for better user experience:

### Features:
- **Loading Indicator**: Shows a spinner and message while data is being fetched
- **Error Handling**: Displays user-friendly error messages if data loading fails
- **Automatic Hide**: Loading indicator automatically hides when data is loaded

### Implementation:
- **Helper Class**: `LoadingStateHelper.cs` provides extension methods for controllers
- **Partial View**: `_LoadingState.cshtml` renders the loading UI
- **Usage**: Automatically applied to Products pages

### Example in Controller:
```csharp
// Set loading state
this.SetLoadingState(true, "Đang tải danh sách sản phẩm...");

// After data loaded
this.SetLoadingState(false);

// On error
this.SetErrorState(true, "Đã xảy ra lỗi khi tải dữ liệu");
```

---

## 🐳 Docker Configuration

### Port Mapping:
- **Container Port**: 80
- **Host Port**: 8081
- **Access URL**: http://localhost:8081

### Environment Variables:
- `ASPNETCORE_ENVIRONMENT=Production`

### Volumes:
- `./wwwroot:/app/wwwroot` - Static files mounted for development

---

## 📝 Development Notes

### Server-Side Rendering
- Products, News, and Categories are loaded server-side for better performance and SEO
- ViewComponents are used for reusable UI components
- No client-side AJAX calls for initial page load

### Security Best Practices
- Sensitive credentials stored in `appsettings.json` (should use User Secrets in development)
- Payment processing entirely server-side
- Input validation on both client and server
- Anti-forgery tokens for form submissions

### API Integration
- All external API calls use `IHttpClientFactory`
- Centralized API endpoints for easy maintenance
- Consistent error handling across all API calls

---

## 🔄 Recent Updates

- ✅ Centralized API constants for better maintainability
- ✅ Server-side rendering for product categories and news modal
- ✅ Secure Payoo payment processing (server-side)
- ✅ News modal with configurable keywords (via appsettings.json)
- ✅ Loading state management for better UX
- ✅ Cleaned up unused files and empty folders (Filters, Repositories, Common, service, NewFolder)
- ✅ Removed unused SessionHelper and CartItem models

---

## 🚀 CI Pipeline

Project sử dụng **GitHub Actions** để tự động kiểm tra code quality, build và test.

### 📋 Tổng quan

CI pipeline được cấu hình cho nhánh **`coder2_dev`** với workflow **`ci.yml`**:

1. **Build và Test**
   - Restore dependencies
   - Build .NET application
   - Chạy tests (nếu có)

2. **Code Quality Checks**
   - Kiểm tra code formatting (dotnet format)
   - Kiểm tra build warnings
   - Đảm bảo code clean trước khi merge

### 🔄 Workflow Trigger

Pipeline tự động chạy khi:
- ✅ Push code lên nhánh `coder2_dev`
- ✅ Tạo Pull Request vào nhánh `coder2_dev`
- ✅ Manual trigger từ GitHub Actions tab

### 📝 Sử dụng CI

#### Tự động kiểm tra

1. **Push code lên nhánh `coder2_dev`**:
   ```bash
   git checkout coder2_dev
   git add .
   git commit -m "Your commit message"
   git push origin coder2_dev
   ```

2. **GitHub Actions tự động chạy**:
   - Build và test code
   - Kiểm tra code formatting
   - Kiểm tra build warnings

3. **Xem kết quả**:
   - Vào tab **Actions** trong GitHub repository
   - Click vào workflow run để xem chi tiết
   - ✅ Xanh = Pass (code clean)
   - ❌ Đỏ = Fail (cần sửa lỗi)

#### Manual trigger

1. Vào **Actions** tab trong GitHub repository
2. Chọn workflow "CI Pipeline"
3. Click **Run workflow** → Chọn nhánh `coder2_dev` → **Run workflow**

### ✅ Code Quality Checks

Pipeline sẽ kiểm tra:

- **Code Formatting**: Đảm bảo code tuân theo coding standards
  - Nếu fail: Chạy `dotnet format` để tự động fix
- **Build Warnings**: Cảnh báo nếu có warnings trong quá trình build
- **Tests**: Chạy unit tests (nếu có)

### 🔍 Monitoring

- **Xem logs**: Vào **Actions** tab → Click vào workflow run
- **Debug**: Kiểm tra logs từng step để tìm lỗi
- **Notifications**: GitHub sẽ gửi email khi workflow fail (nếu đã bật)

### 🛠️ Fix Code Issues

Nếu CI fail, bạn có thể:

1. **Format code tự động**:
   ```bash
   dotnet format ./DOCTORLOAN
   ```

2. **Fix build warnings**: Xem chi tiết trong build logs

3. **Fix test failures**: Chạy tests locally trước khi push:
   ```bash
   dotnet test ./DOCTORLOAN
   ```

---

## 📚 Additional Resources

- **API Documentation**: External API at `https://doctorloan-api.giathaidoctorloan.vn/api`
- **Payoo Documentation**: Payment gateway integration
- **Docker Documentation**: Container deployment guide
- **CI/CD Setup Guide**: Xem `CI-CD-SETUP.md` để biết chi tiết về thiết lập CI/CD

---

## 🤝 Contributing

When contributing to this project:
1. Follow the existing code structure
2. Use server-side rendering where possible
3. Maintain security best practices
4. Update documentation for new features
5. Test thoroughly before submitting

---

## 📄 License

[Add your license information here]

---

**Last Updated**: 2024
