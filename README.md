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

### 2.Build and run with Docker
```sh
#Clean Docker build
docker-compose build --no-cache

# Build and start containers
docker-compose up --build

# Stop and remove containers
docker-compose down

#Check status at: 
http://localhost:8080

### 3.Compile LESS to CSS
```sh
npm install -g less
npm run build-css

📂 Project Structure

DOCTORLOAN/
│── Controllers/           # Controllers for handling logic
│── Models/                # Data models
│── Views/                 # Razor views
│── wwwroot/               # Static files (CSS, JS, images...)
│── package.json    # LESS → CSS build configuration
│── docker-compose.yml     # Docker Compose configuration
│── Dockerfile             # Docker build configuration
└── README.md

