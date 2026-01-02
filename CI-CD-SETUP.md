# Hướng dẫn thiết lập CI cho DOCTORLOAN

Tài liệu này hướng dẫn cách thiết lập và cấu hình CI (Continuous Integration) pipeline cho project DOCTORLOAN.

## 📋 Tổng quan

Project sử dụng **GitHub Actions** để tự động kiểm tra code quality:
- ✅ Build và test code
- ✅ Kiểm tra code formatting
- ✅ Kiểm tra build warnings
- ✅ Đảm bảo code clean trước khi merge

## 🚀 Workflow có sẵn

### `ci.yml` - CI Pipeline
- **Trigger**: Push/PR vào nhánh `coder2_dev`
- **Jobs**:
  - **Build and Test**: Restore dependencies, build .NET application, chạy tests
  - **Code Quality Checks**: Kiểm tra code formatting và build warnings

## ⚙️ Cấu hình

### Không cần GitHub Secrets

CI pipeline hiện tại **không cần** cấu hình secrets vì chỉ thực hiện:
- Build và test code
- Kiểm tra code quality
- Không có deploy hoặc push Docker images

### GitHub Environments

Không cần tạo environments vì không có deploy step.

## 🔄 Workflow Trigger

Pipeline tự động chạy khi:
- ✅ Push code lên nhánh `coder2_dev`
- ✅ Tạo Pull Request vào nhánh `coder2_dev`
- ✅ Manual trigger từ GitHub Actions tab

## 📝 Sử dụng CI

### Tự động kiểm tra

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

### Manual trigger

1. Vào **Actions** tab trong GitHub repository
2. Chọn workflow "CI Pipeline"
3. Click **Run workflow** → Chọn nhánh `coder2_dev` → **Run workflow**

## ✅ Code Quality Checks

### 1. Code Formatting

Pipeline sử dụng `dotnet format` để kiểm tra code formatting:

```bash
dotnet format --verify-no-changes
```

**Nếu fail:**
- Chạy `dotnet format ./DOCTORLOAN` để tự động fix
- Commit và push lại

### 2. Build Warnings

Pipeline sẽ cảnh báo nếu có warnings trong quá trình build:

```bash
dotnet build --configuration Release
```

**Nếu có warnings:**
- Xem chi tiết trong build logs
- Fix warnings trước khi merge

### 3. Tests

Pipeline sẽ chạy tests nếu có:

```bash
dotnet test --configuration Release
```

**Nếu tests fail:**
- Xem chi tiết trong test logs
- Fix tests trước khi merge

## 🛠️ Fix Code Issues Locally

### Format code tự động

```bash
cd DOCTORLOAN
dotnet format
```

### Build và kiểm tra warnings

```bash
cd DOCTORLOAN
dotnet build --configuration Release
```

### Chạy tests

```bash
cd DOCTORLOAN
dotnet test --configuration Release
```

## 🔍 Monitoring và Debugging

### Xem logs

- Vào **Actions** tab trong GitHub repository
- Click vào workflow run để xem chi tiết từng step

### Common issues

1. **Build fails**:
   - Kiểm tra `.csproj` dependencies
   - Kiểm tra .NET version (phải là 7.0.x)
   - Xem build logs để tìm lỗi cụ thể

2. **Formatting fails**:
   - Chạy `dotnet format ./DOCTORLOAN` để tự động fix
   - Commit và push lại

3. **Tests fail**:
   - Chạy tests locally: `dotnet test ./DOCTORLOAN`
   - Fix tests trước khi push

4. **Build warnings**:
   - Xem chi tiết warnings trong build logs
   - Fix warnings để code clean hơn

## 📚 Best Practices

### Trước khi push code

1. **Format code**:
   ```bash
   dotnet format ./DOCTORLOAN
   ```

2. **Build locally**:
   ```bash
   dotnet build ./DOCTORLOAN --configuration Release
   ```

3. **Chạy tests** (nếu có):
   ```bash
   dotnet test ./DOCTORLOAN
   ```

4. **Commit và push**:
   ```bash
   git add .
   git commit -m "Your commit message"
   git push origin coder2_dev
   ```

### Pull Request Workflow

1. Tạo branch từ `coder2_dev`
2. Code và commit changes
3. Push branch lên GitHub
4. Tạo Pull Request vào `coder2_dev`
5. CI sẽ tự động chạy và kiểm tra code
6. Chỉ merge khi CI pass ✅

## 🎯 Next Steps

1. ✅ Push code lên nhánh `coder2_dev` để test CI
2. ✅ Xem kết quả trong Actions tab
3. ✅ Fix các issues nếu có
4. ✅ Đảm bảo CI pass trước khi merge

---

**Lưu ý**: CI pipeline này chỉ kiểm tra code quality, không có deploy. Khi cần thêm CD (Continuous Deployment), có thể thêm deploy jobs vào workflow sau.
