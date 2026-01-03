# Hướng dẫn thiết lập CI cho DOCTORLOAN

Tài liệu này hướng dẫn cách thiết lập và cấu hình CI (Continuous Integration) pipeline cho project DOCTORLOAN với chuẩn hóa code, clean code, security scanning, performance analysis và tự động tạo GitHub Issues.

## 📋 Tổng quan

Project sử dụng **GitHub Actions** để tự động kiểm tra code quality trên **TẤT CẢ CÁC BRANCH**:
- ✅ Build và test code
- ✅ Kiểm tra code formatting (EditorConfig)
- ✅ Kiểm tra code style (StyleCop)
- ✅ Code analysis (Roslyn Analyzers)
- ✅ **Security scanning** (NuGet vulnerabilities, hardcoded secrets, security code analysis)
- ✅ **Performance analysis** (code complexity, anti-patterns, optimization suggestions)
- ✅ **Tự động tạo GitHub Issues** với severity labels
- ✅ Code smells detection
- ✅ Comprehensive reporting

## 🚀 Workflow Structure

### `.github/workflows/ci.yml` - CI Pipeline

Workflow được chia thành các **parallel jobs** để tối ưu thời gian chạy:

1. **Code Quality & Formatting** (Job 1)
   - Code formatting verification
   - Build với warnings tracking
   - Code analysis với .NET Analyzers
   - Code smells detection

2. **Security Analysis** (Job 2) - Chạy song song với Job 1
   - NuGet vulnerability scanning
   - Hardcoded secrets detection
   - Security code analysis (OWASP, CWE checks)

3. **Performance Analysis** (Job 3) - Chạy song song
   - Code complexity analysis
   - Performance anti-patterns detection
   - Optimization suggestions

4. **Create GitHub Issues** (Job 4) - Chạy sau khi các jobs khác hoàn thành
   - Tự động tạo issues từ security findings
   - Tự động tạo issues từ performance findings
   - Phân loại severity: Critical, High, Medium, Low, Enhancement

5. **Generate Quality Report** (Job 5)
   - Tổng hợp báo cáo từ tất cả các jobs
   - Hiển thị summary trong GitHub Actions

### Trigger Events

- **Push** vào bất kỳ branch nào (`**`)
- **Pull Request** vào bất kỳ branch nào
- **Manual trigger** (workflow_dispatch)

## ⚙️ Cấu hình

### Files cấu hình

1. **`.editorconfig`** - Chuẩn hóa code formatting
   - C# coding conventions
   - Naming conventions
   - Formatting rules
   - Indentation preferences

2. **`stylecop.json`** - StyleCop configuration
   - Documentation rules
   - Naming rules
   - Layout rules
   - Ordering rules

3. **`DOCTORLOAN/code-analysis.ruleset`** - Code analysis rules
   - .NET Analyzers rules
   - StyleCop rules
   - Warning levels

4. **`Directory.Build.props`** - Shared build properties
   - Enable analyzers
   - Analysis level
   - Warning configuration

5. **`DOCTORLOAN/DOCTORLOAN.csproj`** - Project configuration
   - NuGet packages cho analyzers
   - Code analysis settings

### Scripts hỗ trợ

- `.github/scripts/create-github-issue.py` - Tạo GitHub Issues
- `.github/scripts/parse-security-issues.js` - Parse security findings
- `.github/scripts/parse-performance-issues.js` - Parse performance findings
- `.github/scripts/analyze-complexity.py` - Analyze code complexity
- `.github/scripts/export-secret-findings.py` - Export secret findings

### NuGet Packages

Project sử dụng các packages sau cho code analysis:
- `Microsoft.CodeAnalysis.NetAnalyzers` (v8.0.0) - .NET Code Analyzers
- `StyleCop.Analyzers` (v1.1.118) - StyleCop code style rules

### GitHub Permissions

Workflow cần các permissions sau:
- `contents: read` - Đọc code
- `issues: write` - Tạo issues
- `security-events: write` - Security scanning (optional)

**Lưu ý**: `GITHUB_TOKEN` được tự động cung cấp bởi GitHub Actions, không cần cấu hình thêm.

## 🔒 Security Scanning

### 1. NuGet Vulnerability Scanning

Pipeline tự động kiểm tra các package có lỗ hổng bảo mật:

```bash
dotnet list package --vulnerable --include-transitive
```

**Severity Classification:**
- **High**: Vulnerable packages với CVSS score cao
- **Medium**: Vulnerable packages với CVSS score trung bình

**Gợi ý**: Update packages lên phiên bản mới nhất không có lỗ hổng.

### 2. Hardcoded Secrets Detection

Pipeline quét code để tìm hardcoded secrets:

- Passwords
- API keys
- Tokens
- Connection strings

**Severity Classification:**
- **Critical**: Hardcoded secrets trong production code

**Gợi ý**: 
- Sử dụng configuration files (appsettings.json, environment variables)
- Sử dụng Azure Key Vault hoặc similar services
- Không commit secrets vào repository

### 3. Security Code Analysis

Pipeline chạy security analyzers để phát hiện:

- SQL Injection vulnerabilities
- XSS vulnerabilities
- Insecure deserialization
- Weak cryptography
- Authentication/Authorization issues
- Input validation issues

**Severity Classification:**
- **Critical**: SQL injection, XSS, authentication bypass
- **High**: Weak crypto, insecure deserialization
- **Medium**: Missing validation, insecure configuration

## ⚡ Performance Analysis

### 1. Code Complexity Analysis

Pipeline phân tích:
- Files quá dài (>500 lines)
- Methods quá phức tạp
- Cyclomatic complexity

**Severity Classification:**
- **Enhancement**: Large files, high complexity

**Gợi ý**: 
- Chia nhỏ files thành các classes nhỏ hơn
- Extract methods để giảm complexity
- Apply SOLID principles

### 2. Performance Anti-patterns Detection

Pipeline phát hiện:
- **Blocking async operations** (`.Result`, `.Wait()`, `.GetAwaiter().GetResult()`)
- **String concatenation trong loops**
- **Missing async/await** trên I/O operations
- **Inefficient LINQ queries**
- **Large memory allocations**

**Severity Classification:**
- **Critical**: Blocking async operations, memory leaks
- **High**: Missing async, large allocations
- **Medium**: String concatenation, LINQ inefficiencies
- **Enhancement**: Minor optimizations

**Gợi ý**:
- Sử dụng `async/await` đúng cách
- Sử dụng `StringBuilder` cho string concatenation trong loops
- Sử dụng async I/O operations
- Optimize LINQ queries

## 📊 GitHub Issues Auto-Creation

### Severity Labels

Pipeline tự động tạo issues với các labels sau:

- **severity:critical** (🔴 Red) - Issues nghiêm trọng cần fix ngay
- **severity:high** (🟠 Orange) - Issues quan trọng nên fix sớm
- **severity:medium** (🟡 Yellow) - Issues nên được xem xét
- **severity:low** (🟢 Green) - Issues ít ưu tiên
- **severity:enhancement** (🔵 Blue) - Gợi ý cải thiện

### Type Labels

- **type:security** - Security issues
- **type:performance** - Performance issues
- **type:code_quality** - Code quality issues

### Issue Format

Mỗi issue tự động được tạo với:

- **Title**: Mô tả ngắn gọn vấn đề
- **Body**: 
  - Description chi tiết
  - Severity và Type
  - File và line number (nếu có)
  - Recommendation
  - Link đến workflow run
- **Labels**: Severity và Type labels

### Duplicate Prevention

Pipeline tự động kiểm tra và tránh tạo issues trùng lặp dựa trên:
- Title matching
- Same severity
- Same file location

## 📝 Sử dụng CI

### Tự động kiểm tra

1. **Push code lên bất kỳ branch nào**:
   ```bash
   git checkout your-branch
   git add .
   git commit -m "Your commit message"
   git push origin your-branch
   ```

2. **GitHub Actions tự động chạy**:
   - Tất cả các jobs chạy song song (code-quality, security-scan, performance-analysis)
   - Tự động tạo issues cho security và performance findings
   - Generate comprehensive report

3. **Xem kết quả**:
   - Vào tab **Actions** trong GitHub repository
   - Click vào workflow run để xem chi tiết
   - Xem **Summary** tab để xem comprehensive report
   - Xem **Issues** tab để xem các issues được tạo tự động
   - ✅ Xanh = Pass (code clean)
   - ❌ Đỏ = Fail (cần sửa lỗi)

### Manual trigger

1. Vào **Actions** tab trong GitHub repository
2. Chọn workflow "CI Pipeline - Code Quality, Security & Performance"
3. Click **Run workflow** → Chọn branch → **Run workflow**

## ✅ Code Quality Checks

### 1. Code Formatting

Pipeline sử dụng `dotnet format` để kiểm tra code formatting theo `.editorconfig`:

```bash
dotnet format --verify-no-changes
```

**Nếu fail:**
- Chạy `dotnet format DOCTORLOAN.sln` để tự động fix
- Commit và push lại

### 2. Build Warnings

Pipeline sẽ track và báo cáo warnings trong quá trình build:

```bash
dotnet build --configuration Release
```

**Nếu có warnings:**
- Xem chi tiết trong build logs
- Fix warnings trước khi merge
- Warnings được đếm và hiển thị trong report

### 3. Code Analysis

Pipeline chạy .NET Analyzers và StyleCop để phát hiện:
- Code style violations
- Potential bugs
- Performance issues
- Security vulnerabilities
- Best practice violations

## 🛠️ Fix Code Issues Locally

### Format code tự động

```bash
# Format toàn bộ solution
dotnet format DOCTORLOAN.sln

# Format project cụ thể
dotnet format DOCTORLOAN/DOCTORLOAN.csproj
```

### Build và kiểm tra warnings

```bash
cd DOCTORLOAN
dotnet build --configuration Release
```

### Chạy code analysis

```bash
dotnet build --configuration Release \
  /p:RunAnalyzersDuringBuild=true \
  /p:EnableNETAnalyzers=true \
  /p:AnalysisLevel=latest
```

### Kiểm tra NuGet vulnerabilities

```bash
dotnet list DOCTORLOAN.sln package --vulnerable --include-transitive
```

### Chạy tests

```bash
dotnet test DOCTORLOAN.sln --configuration Release
```

## 📊 Quality Report

Mỗi lần CI chạy, một **Comprehensive Code Quality Report** được tạo tự động với:

### Code Quality Section
- Build Errors và Warnings count
- Code analysis warnings
- Code smells (TODO/FIXME, commented code)

### Security Section
- Total security issues
- Critical và High severity issues count
- Vulnerable packages
- Hardcoded secrets

### Performance Section
- Total performance issues
- Complexity findings
- Anti-patterns detected

Xem report trong **Actions** → Workflow run → **Summary** tab.

## 🔍 Monitoring và Debugging

### Xem logs

- Vào **Actions** tab trong GitHub repository
- Click vào workflow run để xem chi tiết từng step
- Download artifacts để xem chi tiết logs:
  - `code-quality-logs` - Code quality analysis logs
  - `security-logs` - Security scanning logs
  - `performance-logs` - Performance analysis logs

### Xem GitHub Issues

- Vào **Issues** tab trong GitHub repository
- Filter theo labels:
  - `severity:critical` - Critical issues
  - `type:security` - Security issues
  - `type:performance` - Performance issues

### Common issues

1. **Build fails**:
   - Kiểm tra `.csproj` dependencies
   - Kiểm tra .NET version (phải là 7.0.x)
   - Xem build logs để tìm lỗi cụ thể

2. **Formatting fails**:
   - Chạy `dotnet format DOCTORLOAN.sln` để tự động fix
   - Commit và push lại

3. **Security warnings**:
   - Review hardcoded secrets - di chuyển sang configuration
   - Update vulnerable packages
   - Fix security code analysis warnings

4. **Performance warnings**:
   - Refactor large files
   - Fix blocking async operations
   - Optimize inefficient code

5. **Issues không được tạo**:
   - Kiểm tra GitHub permissions
   - Kiểm tra GITHUB_TOKEN có được set không
   - Xem logs trong create-issues job

## 📚 Best Practices

### Trước khi push code

1. **Format code**:
   ```bash
   dotnet format DOCTORLOAN.sln
   ```

2. **Build locally**:
   ```bash
   dotnet build DOCTORLOAN.sln --configuration Release
   ```

3. **Chạy code analysis**:
   ```bash
   dotnet build DOCTORLOAN.sln --configuration Release \
     /p:RunAnalyzersDuringBuild=true
   ```

4. **Kiểm tra security**:
   ```bash
   dotnet list DOCTORLOAN.sln package --vulnerable
   ```

5. **Chạy tests** (nếu có):
   ```bash
   dotnet test DOCTORLOAN.sln
   ```

6. **Review code smells**:
   - Fix TODO/FIXME comments
   - Xóa commented code
   - Extract constants

7. **Commit và push**:
   ```bash
   git add .
   git commit -m "Your commit message"
   git push origin your-branch
   ```

### Pull Request Workflow

1. Tạo branch từ main/develop
2. Code và commit changes
3. Push branch lên GitHub
4. Tạo Pull Request
5. CI sẽ tự động chạy và kiểm tra code
6. Review **Quality Report** trong PR
7. Review các **Issues** được tạo tự động
8. Fix issues nếu có
9. Chỉ merge khi CI pass ✅

### Code Quality Guidelines

1. **Naming Conventions**:
   - Classes, Methods, Properties: PascalCase
   - Parameters, Local variables: camelCase
   - Constants: PascalCase
   - Private fields: camelCase

2. **Code Style**:
   - Sử dụng `var` khi type rõ ràng
   - Prefer expression-bodied members cho simple properties
   - Sử dụng pattern matching
   - Null-checking với null-conditional operators

3. **Security Best Practices**:
   - Không hardcode secrets
   - Validate input
   - Use parameterized queries
   - Implement proper authentication/authorization
   - Keep packages updated

4. **Performance Best Practices**:
   - Use async/await cho I/O operations
   - Avoid blocking async operations
   - Use StringBuilder cho string concatenation trong loops
   - Optimize LINQ queries
   - Cache expensive operations

5. **Maintainability**:
   - Keep files < 500 lines
   - Keep methods focused và short
   - Apply SOLID principles
   - Write clear comments cho complex logic

## 🎯 Gợi ý Tối Ưu Hóa Code

CI pipeline tự động phát hiện và gợi ý các cải tiến:

### Performance Optimizations

- **String operations**: Sử dụng StringBuilder thay vì string concatenation
- **LINQ**: Sử dụng `.ToList()` hoặc `.ToArray()` khi cần enumerate nhiều lần
- **Async operations**: Sử dụng async/await cho I/O operations
- **Caching**: Cache expensive operations
- **Memory**: Avoid large allocations, use object pooling nếu cần

### Code Structure

- **Long methods**: Chia nhỏ methods > 50 lines
- **Long classes**: Chia nhỏ classes > 500 lines
- **Cyclomatic complexity**: Giảm complexity của methods
- **Duplication**: Extract common code vào methods/classes

### Security

- **Secrets management**: Sử dụng configuration hoặc secure storage
- **Input validation**: Validate tất cả user input
- **SQL injection**: Sử dụng parameterized queries
- **XSS**: Encode output
- **Package updates**: Keep packages updated để tránh vulnerabilities

### Maintainability

- **Documentation**: Thêm XML comments cho public APIs
- **Naming**: Sử dụng descriptive names
- **Comments**: Thêm comments cho complex logic
- **Tests**: Viết unit tests cho business logic

## 🔧 Tối ưu Workflow

Workflow được tối ưu với:

1. **Parallel Jobs**: Các jobs chạy song song để giảm thời gian
2. **Caching**: Cache NuGet packages để tăng tốc restore
3. **Artifacts**: Upload logs và findings để review sau
4. **Conditional Execution**: Một số steps chỉ chạy khi cần
5. **Timeout**: Set timeout cho mỗi job để tránh hang

## 🎯 Next Steps

1. ✅ Push code lên bất kỳ branch nào để test CI
2. ✅ Xem kết quả trong Actions tab
3. ✅ Review Quality Report
4. ✅ Review GitHub Issues được tạo tự động
5. ✅ Fix các issues theo severity
6. ✅ Đảm bảo CI pass trước khi merge
7. ✅ Cải thiện code quality dựa trên gợi ý

## 📖 Tài liệu tham khảo

- [.NET Code Analysis](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/overview)
- [StyleCop Analyzers](https://github.com/DotNetAnalyzers/StyleCopAnalyzers)
- [EditorConfig](https://editorconfig.org/)
- [GitHub Actions](https://docs.github.com/en/actions)
- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [CWE Top 25](https://cwe.mitre.org/top25/)

---

**Lưu ý**: CI pipeline này chạy trên **TẤT CẢ CÁC BRANCH** để đảm bảo code quality nhất quán trong toàn bộ project. Security và Performance issues sẽ được tự động tạo thành GitHub Issues với severity labels để dễ dàng tracking và fixing.
