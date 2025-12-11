### Hi there 👋
# DOCTORLOAN

**DOCTORLOAN** is a clinic management web project, developed with **ASP.NET Core 7.0** and containerized using **Docker**.

---

## 🚀 Technologies Used
- **ASP.NET Core 7.0**
- **Docker**
- **LESS, CSS, JS**
- **External REST API** (backend data source)

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
│── Controllers/           # Controllers for handling logic
│   └── ProductsController.cs  # Public endpoints for products (no authentication required)
│── Models/                # Data models
│── Views/                 # Razor views
│── Helpers/               # Helper classes
│   └── LoadingStateHelper.cs  # Loading state management
│── wwwroot/               # Static files (CSS, JS, images...)
│── package.json           # LESS → CSS build configuration
│── docker-compose.yml     # Docker Compose configuration
│── Dockerfile             # Docker build configuration
└── README.md
```

---

## 🔌 Public Endpoints

### Products Controller
The `ProductsController` is configured as a **public endpoint** (no authentication required) using `[AllowAnonymous]` attribute.

#### Available Endpoints:

1. **GET /Products/Index**
   - **Description**: Displays the list of all products
   - **Authentication**: Not required (Public)
   - **Features**:
     - Loading state indicator while fetching data
     - Error handling with user-friendly messages
     - Server-side rendering (no jQuery AJAX)

2. **GET /Products/ProductDetail**
   - **Description**: Displays detailed information about a specific product
   - **Authentication**: Not required (Public)
   - **Parameters**:
     - `productId` (optional): Product ID
     - `categoryId` (optional): Category ID (if provided, shows first product in category)
   - **Features**:
     - Loading state indicator while fetching data
     - Product details with images, attributes, and related products
     - Error handling with user-friendly messages
     - Server-side rendering (no jQuery AJAX)

#### Example Usage:
```
# View all products
http://localhost:8081/Products/Index

# View product detail by ID
http://localhost:8081/Products/ProductDetail?productId=1

# View product detail by category
http://localhost:8081/Products/ProductDetail?categoryId=5
```

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

## 📝 Notes

- All Products endpoints are **public** and do not require authentication
- Data is fetched from external API: `http://localhost:49553/api/product-module`
- Loading states are automatically managed for better UX
- Server-side rendering is used instead of client-side AJAX calls

