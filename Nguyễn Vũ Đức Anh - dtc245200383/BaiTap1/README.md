# EduSubmit - Bai 1

Ung dung gom hai tien trinh .NET 10 giao tiep qua HTTP:

- `Submission.Api`: Minimal API cho danh sach bai tap va luot nop.
- `Submission.Client`: console client goi API bang `HttpClient`.

## Chay

Terminal 1:

```powershell
dotnet run --project src/Submission.Api --urls http://localhost:5000
```

Terminal 2:

```powershell
dotnet run --project src/Submission.Client -- http://localhost:5000
```

## Thu API

Mo `requests.http` bang VS Code REST Client va gui lan luot cac request. Request cuoi kiem tra truong hop 404; request doc theo ID lay ID tu response cua request `submit`.

## Nhat ky

Xem `docs/bai1.md`. Dien ho ten/lop, them anh chup man hinh va thay `USERNAME` bang tai khoan GitHub that truoc khi nop.
